using System;
using System.Windows.Forms;

namespace FolderEmailSenderWatcher
{
    public partial class FormDBSettingConnection : Form
    {
        public FormDBSettingConnection()
        {
            InitializeComponent();

            txtConnectionString.Text = Properties.Settings.Default.ConnectionString;

            txtSmtpAddress.Text = Properties.Settings.Default.SMTPAddress;
            ckSSL.Checked = Properties.Settings.Default.EnableSSL;
            txtPort.Text = Properties.Settings.Default.PortNumber.ToString();
            txtFromEmail.Text = Properties.Settings.Default.FromEmail;
            txtPassword.Text = Properties.Settings.Default.Password;
            txtBody.Text = Properties.Settings.Default.Body;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.ConnectionString = txtConnectionString.Text;

            Properties.Settings.Default.SMTPAddress = txtSmtpAddress.Text;
            Properties.Settings.Default.Password = txtPassword.Text;
            Properties.Settings.Default.EnableSSL = ckSSL.Checked;
            int.TryParse(txtPort.Text, out int port);
            Properties.Settings.Default.PortNumber = port;
            Properties.Settings.Default.FromEmail = txtFromEmail.Text;
            Properties.Settings.Default.Body = txtBody.Text;

            Properties.Settings.Default.Save();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
