using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Conect;

namespace WindowsFormsApplication1
{
    public partial class Form3 : Form
    {
        Form1 father;

        public Form3(Form1 dad)
        {
            InitializeComponent();
            father = dad;
            
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'invoiceDataSet1.Destiny' table. You can move, or remove it, as needed.
            this.destinyTableAdapter.Fill(this.invoiceDataSet1.Destiny);
            this.origenTableAdapter.Fill(this.invoiceDataSet.Origen);
            fillcombobox(CBCompanies, "select DISTINCT Company from Origen");
        }



        private void fillcombobox(ComboBox target, string query)
        {
            List<ListViewItem> Companies = Conect.simpleaccesconect.ConnectToAccess(query, true);
            target.Items.Clear();
            foreach (ListViewItem Company in Companies)
            {
                Console.WriteLine(Company.Text);
                target.Items.Add(Company.Text);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (TBDestiny.Text != "")
            {
                string query = "SELECT id FROM Destiny WHERE Place = '" + TBDestiny.Text + "'";
                List<ListViewItem> exists = Conect.simpleaccesconect.ConnectToAccess(query, true);
                if (exists.Count == 0)
                {
                    query = "INSERT INTO Destiny (Place) VALUES ('" + TBDestiny.Text + "')";
                    Conect.simpleaccesconect.ConnectToAccess(query, false);
                    father.updatecities();                    
                }
                MessageBox.Show("Destiny has been successfully added");
            }
            else
            {
                MessageBox.Show("Destiny is empty");
            }
            this.destinyTableAdapter.Fill(this.invoiceDataSet1.Destiny);
            this.dataGridView2.Refresh();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (CBCompanies.Text != "" && TBOrigen.Text != "")
            {
                string query = "SELECT id FROM Origen WHERE Place = '" + TBOrigen.Text + "' and Company='" + CBCompanies.Text + "'";
                List<ListViewItem> exists = Conect.simpleaccesconect.ConnectToAccess(query, true);
                if (exists.Count == 0)
                {
                    query = "INSERT INTO Origen (Place, Company) VALUES ('" + TBOrigen.Text + "','" + CBCompanies.Text + "')";
                    Conect.simpleaccesconect.ConnectToAccess(query, false);
                    father.updatecities();
                }
                MessageBox.Show("Origin has been successfully added");
            }
            else
            {
                MessageBox.Show("Company or Origin is empty");
            }
            this.origenTableAdapter.Fill(this.invoiceDataSet.Origen);
            this.dataGridView1.Refresh();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            DataSet changes = this.invoiceDataSet.GetChanges();
            if (changes != null)
            {
                int updatedRows = this.origenTableAdapter.Update(invoiceDataSet);
                this.invoiceDataSet.AcceptChanges();
            }

            DataSet ochanges = this.invoiceDataSet1.GetChanges();
            if (ochanges != null)
            {
                int updatedRows = this.destinyTableAdapter.Update(invoiceDataSet1);
                this.invoiceDataSet1.AcceptChanges();
            }
            father.updatecities();
            this.Close();
        }


    }
}
