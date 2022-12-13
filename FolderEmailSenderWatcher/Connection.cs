using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace FolderEmailSenderWatcher
{
    public class Connection
    {
        private MySqlDataAdapter mAdapter;
        private DataSet mDataSet;
        private MySqlConnection mConn;
        private MySqlCommand command;
        public Connection(string connectionString)
        {
            //define o dataset
            mDataSet = new DataSet();
            mConn = new MySqlConnection(connectionString);
            command = new MySqlCommand();
        }

        public MySqlConnection OpenConnection()
        {
            try
            {
                if(mConn.State == System.Data.ConnectionState.Closed)
                    mConn.Open();

                return mConn;
            }
            catch
            {
                return null;
            }
        }

        public bool CloseConnection()
        {
            try
            {
                if (mConn.State == System.Data.ConnectionState.Open)
                    return true;

                return false;
            }
            catch
            {
                return false;
            }
        }

        public DataTable PopulateContactsData()
        {
            try
            {
                DataTable dt = new DataTable();
                mAdapter = new MySqlDataAdapter("select * from tbcontactos", mConn);
                mAdapter.Fill(dt);

                return dt;
            }
            catch
            {
                return null;
            }
        }

        public bool InsertContactoRow(string strEmail1, string strEmail2, string strTelemovel1, string strTelemovel2)
        {
            try
            {
                command = new MySqlCommand("insert into tbcontactos(EMAIL1,EMAIL2,TELEMOVEL1,TELEMOVEL2) values(@EMAIL1,@EMAIL2,@TELEMOVEL1,@TELEMOVEL2)", mConn);
                command.Parameters.AddWithValue("@EMAIL1", strEmail1);
                command.Parameters.AddWithValue("@EMAIL2", strEmail2);
                command.Parameters.AddWithValue("@TELEFONE1", strTelemovel1);
                command.Parameters.AddWithValue("@TELEFONE2", strTelemovel2);
                command.ExecuteNonQuery();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateContactoRow(string strEmail1, string strEmail2, string strTelemovel1, string strTelemovel2, int id)
        {
            try
            {
                command = new MySqlCommand("update tbcontactos set EMAIL1=@EMAIL1,EMAIL2=@EMAIL2,TELEMOVEL1=@TELEMOVEL1,TELEMOVEL2=@TELEMOVEL2 where ID=@ID", mConn);
                command.Parameters.AddWithValue("@ID", id);
                command.Parameters.AddWithValue("@EMAIL1", strEmail1);
                command.Parameters.AddWithValue("@EMAIL2", strEmail2);
                command.Parameters.AddWithValue("@TELEMOVEL1", strTelemovel1);
                command.Parameters.AddWithValue("@TELEMOVEL2", strTelemovel2);
                command.ExecuteNonQuery();

                return true;
            }
            catch
            {
                return false;
            }            
        }

        public bool DeleteContactoRow(int id)
        {
            try
            {
                command = new MySqlCommand("delete tbcontactos where ID=@ID", mConn);
                command.Parameters.AddWithValue("@ID", id);
                command.ExecuteNonQuery();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public AppMail ComposeEmail(string folder)
        {
            AppMail appMail = null;

            try
            {
                DataTable dt = new DataTable();
                mAdapter = new MySqlDataAdapter("SELECT cl.EMPRESA, cl.NOME, cl.PASTA, co.EMAIL1, co.email2 FROM tbclientes cl, tbcontactos co WHERE cl.ID=co.ID", mConn);
                mAdapter.Fill(dt);

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        appMail = new AppMail(Convert.ToInt32(dt.Rows[0].ToString()),
                                              dt.Rows[1].ToString(),
                                              dt.Rows[2].ToString(),
                                              dt.Rows[3].ToString(),
                                              dt.Rows[4].ToString(),
                                              dt.Rows[5].ToString());
                        
                    }
                }

                return appMail;
            }
            catch
            {
                return appMail;
            }
        }
    }
}
