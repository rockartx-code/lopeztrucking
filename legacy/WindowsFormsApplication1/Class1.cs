using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Conect
{
    public static class simpleaccesconect
    {        
        public static List<ListViewItem> ConnectToAccess(string queryString, bool query)
        {
            List<ListViewItem> returned = new List<ListViewItem>();
            string basedir = AppDomain.CurrentDomain.BaseDirectory;
            OleDbConnection conn = new OleDbConnection();
            conn.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;" +
                @"Data source= " + basedir +
                @"\Invoice.accdb";

            OleDbCommand command = new OleDbCommand(queryString, conn);
            try
            {
                conn.Open();
                if (query)
                {
                    OleDbDataReader reader = command.ExecuteReader();
                    int lvindex = 0;
                    while (reader.Read())
                    {
                        Console.WriteLine(reader[0].ToString());
                        ListViewItem item = new ListViewItem(reader[0].ToString(), lvindex);
                        for (int i = 1; i < reader.FieldCount; i++)
                        {
                            Console.WriteLine(reader[i].ToString());
                            item.SubItems.Add(reader[i].ToString());
                        }
                        returned.Add(item);
                        lvindex++;
                    }

                    reader.Close();
                }
                else
                {
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to connect to data source" + ex.Message + ";" + queryString );
            }
            finally
            {
                conn.Close();
            }

            return returned;
        }
    }
}
