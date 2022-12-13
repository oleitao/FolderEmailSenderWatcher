using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace FolderEmailSenderWatcher
{
    public partial class FormContacts : Form
    {
        private Connection _conn;

        int ID = -1;

        public FormContacts(Connection conn)
        {
            this._conn = conn;

            InitializeComponent();

            BindData();
        }

        public Connection Conn { get => _conn; set => _conn = value; }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            MySqlConnection connection = Conn.OpenConnection();
            if (connection.State == ConnectionState.Open)
            {
                if (Conn.InsertContactoRow(txtEmail1.Text, txtEmail2.Text, txtPhone1.Text, txtPhone2.Text))
                {
                    MessageBox.Show("Inserted Contacts Successfully");
                    BindData();
                    ClearControls();
                }
            }

            Conn.CloseConnection();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ID != 0)
            {
                MySqlConnection connection = Conn.OpenConnection();
                if (connection.State == ConnectionState.Open)
                {
                    if (Conn.DeleteContactoRow(ID))
                    {
                        MessageBox.Show("Deleted Contancts Successfully!");
                        BindData();
                        ClearControls();
                    }
                }

                Conn.CloseConnection();
            }
            else
            {
                MessageBox.Show("Please select record to delete");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (ID != -1)
            {
                MySqlConnection connection = Conn.OpenConnection();
                if (connection.State == ConnectionState.Open)
                {
                    if (Conn.UpdateContactoRow(txtEmail1.Text, txtEmail2.Text, txtPhone1.Text, txtPhone2.Text, ID))
                    {
                        MessageBox.Show("Updated Contacts Successfully");
                        BindData();
                        ClearControls();
                    }
                }

                Conn.CloseConnection();
            }
            else
            {
                MessageBox.Show("Please enter mandatory details!");
            }
        }

        private void ClearControls()
        {
            txtEmail1.Text = "";
            txtEmail2.Text = "";
            txtPhone1.Text = "";
            txtPhone2.Text = "";
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            ID = Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString());
            txtEmail1.Text = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[1].Value.ToString();
            txtEmail2.Text = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[2].Value.ToString();
            txtPhone1.Text = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[3].Value.ToString();
            txtPhone2.Text = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[4].Value.ToString();
        }

        private void BindData()
        {
            dataGridView1.DataSource = Conn.PopulateContactsData();
        }
    }
}
