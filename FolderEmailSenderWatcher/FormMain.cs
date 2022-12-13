using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Windows.Forms;

namespace FolderEmailSenderWatcher
{
    public partial class FormMain : Form
    {
        private MySqlConnection mConn;

        private StringBuilder m_Sb;
        private bool m_bDirty;
        private System.IO.FileSystemWatcher m_Watcher;
        private bool m_bIsWatching;

        private Connection _conn;
        public Connection Conn { get => _conn; set => _conn = value; }

        public FormMain()
        {
            InitializeComponent();
            m_Sb = new StringBuilder();
            m_bDirty = false;
            m_bIsWatching = false;

            _conn = new Connection(Properties.Settings.Default.ConnectionString);
        }

        private void btnWatchFile_Click(object sender, EventArgs e)
        {
            if (m_bIsWatching)
            {
                m_bIsWatching = false;
                m_Watcher.EnableRaisingEvents = false;
                m_Watcher.Dispose();
                btnWatchFile.BackColor = Color.LightSkyBlue;
                btnWatchFile.Text = "Start Watching";

            }
            else
            {
                m_bIsWatching = true;
                btnWatchFile.BackColor = Color.Red;
                btnWatchFile.Text = "Stop Watching";

                m_Watcher = new System.IO.FileSystemWatcher();
                if (rdbDir.Checked)
                {
                    m_Watcher.Filter = "*.*";
                    m_Watcher.Path = txtFile.Text + "\\";
                }
                else
                {
                    m_Watcher.Filter = txtFile.Text.Substring(txtFile.Text.LastIndexOf('\\') + 1);
                    m_Watcher.Path = txtFile.Text.Substring(0, txtFile.Text.Length - m_Watcher.Filter.Length);
                }

                if (chkSubFolder.Checked)
                {
                    m_Watcher.IncludeSubdirectories = true;
                }

                m_Watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                     | NotifyFilters.FileName | NotifyFilters.DirectoryName;

                m_Watcher.Created += new FileSystemEventHandler(OnChanged);
                m_Watcher.EnableRaisingEvents = true;
            }
        }

        public bool SendEmail(string attachPath, AppMail mailData)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(Properties.Settings.Default.FromEmail);

                    if(!string.IsNullOrEmpty(mailData.Email1))
                        mail.To.Add(mailData.Email1);

                    if (!string.IsNullOrEmpty(mailData.Email2))
                        mail.To.Add(mailData.Email2);

                    mail.Subject = string.Format("{0} - {1}", mailData.Company , mailData.Manager);
                    mail.Body = Properties.Settings.Default.Body;
                    mail.IsBodyHtml = true;
                    mail.Attachments.Add(new Attachment(attachPath));

                    using (SmtpClient smtp = new SmtpClient(Properties.Settings.Default.SMTPAddress, Properties.Settings.Default.PortNumber))
                    {
                        smtp.Credentials = new NetworkCredential(Properties.Settings.Default.FromEmail, Properties.Settings.Default.Password);
                        smtp.EnableSsl = Properties.Settings.Default.EnableSSL;
                        smtp.Send(mail);

                        return true;
                    }
                }
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (!m_bDirty)
            {
                AppMail email = Conn.ComposeEmail(e.Name);
                if (email != null)
                {
                    if (SendEmail(e.FullPath, email))
                    {
                        m_Sb.Remove(0, m_Sb.Length);
                        m_Sb.Append(e.FullPath);
                        m_Sb.Append(" ");
                        m_Sb.Append(e.ChangeType.ToString());
                        m_Sb.Append("    ");
                        m_Sb.Append(DateTime.Now.ToString());
                        m_bDirty = true;
                    }
                }
            }
        }

        private void tmrEditNotify_Tick(object sender, EventArgs e)
        {
            if (m_bDirty)
            {
                lstNotification.BeginUpdate();
                lstNotification.Items.Add(m_Sb.ToString());
                lstNotification.EndUpdate();
                m_bDirty = false;
            }
        }

        private void btnBrowseFile_Click(object sender, EventArgs e)
        {
            if (rdbDir.Checked)
            {
                DialogResult resDialog = dlgOpenDir.ShowDialog();
                if (resDialog.ToString() == "OK")
                {
                    txtFile.Text = dlgOpenDir.SelectedPath;
                }
            }
            else
            {
                DialogResult resDialog = dlgOpenFile.ShowDialog();
                if (resDialog.ToString() == "OK")
                {
                    txtFile.Text = dlgOpenFile.FileName;
                }
            }
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            DialogResult resDialog = dlgSaveFile.ShowDialog();
            if (resDialog.ToString() == "OK")
            {
                FileInfo fi = new FileInfo(dlgSaveFile.FileName);
                StreamWriter sw = fi.CreateText();
                foreach (string sItem in lstNotification.Items)
                {
                    sw.WriteLine(sItem);
                }
                sw.Close();
            }
        }

        private void rdbDir_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbDir.Checked == true)
            {
                chkSubFolder.Enabled = true;
            }
        }

        private void menuContacts_Click(object sender, EventArgs e)
        {
            FormContacts formContacts = new FormContacts(Conn);
            formContacts.ShowDialog();
        }

        private void menuDBConnection_Click(object sender, EventArgs e)
        {
            FormDBSettingConnection formDBSettingConnection = new FormDBSettingConnection();
            formDBSettingConnection.ShowDialog();
        }
    }
}