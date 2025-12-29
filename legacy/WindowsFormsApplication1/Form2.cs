using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form2 : Form
    {
        Form1 father;
        
        public Form2(Form1 dad)
        {
            father = dad;
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            DataGridView target = dataGridView1;
            DataGridView DGV = dataGridView1;
            List<ListViewItem> data = Conect.simpleaccesconect.ConnectToAccess("Select [No], [D], [Check], [Costumer], [Subtotal], [Advance] From invoices order by [No] desc", true);
            target.Rows.Clear();
            foreach (ListViewItem d in data)
            {
                String[] r=new String[6];
                int i = 0;
                foreach (ListViewItem.ListViewSubItem dato in d.SubItems)
                {
                    r[i] = dato.Text;
                    i++;
                }

                DGV.Rows.Add(r);
           
            }
            fillcombobox(CBName, "select DISTINCT Name from Costumers");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            father.reloaded(Convert.ToInt16(dataGridView1.SelectedRows[0].Cells[0].Value));
            this.Close();
        }

        private void DTPDateInv_ValueChanged(object sender, EventArgs e)
        {
            DataGridView target = dataGridView1;
            DataGridView DGV = dataGridView1;
            List<ListViewItem> data = Conect.simpleaccesconect.ConnectToAccess("Select [No], [D], [Check], [Costumer], [Subtotal], [Advance] From invoices Where [D]='"+DTPDateInv.Text+"' order by [No] desc", true);
            target.Rows.Clear();
            foreach (ListViewItem d in data)
            {
                String[] r = new String[6];
                int i = 0;
                foreach (ListViewItem.ListViewSubItem dato in d.SubItems)
                {
                    r[i] = dato.Text;
                    i++;
                }

                DGV.Rows.Add(r);

            }
        }

        private void CBName_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataGridView target = dataGridView1;
            DataGridView DGV = dataGridView1;
            List<ListViewItem> data = Conect.simpleaccesconect.ConnectToAccess("Select [No], [D], [Check], [Costumer], [Subtotal], [Advance] From invoices Where [Costumer]='" + CBName.Text + "' order by [No] desc", true);
            target.Rows.Clear();
            foreach (ListViewItem d in data)
            {
                String[] r = new String[6];
                int i = 0;
                foreach (ListViewItem.ListViewSubItem dato in d.SubItems)
                {
                    r[i] = dato.Text;
                    i++;
                }

                DGV.Rows.Add(r);

            }
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

        private void button2_Click(object sender, EventArgs e)
        {
            DataGridView target = dataGridView1;
            DataGridView DGV = dataGridView1;
            List<ListViewItem> data = Conect.simpleaccesconect.ConnectToAccess("Select [No], [D], [Check], [Costumer], [Subtotal], [Advance] From invoices order by [No] desc", true);
            target.Rows.Clear();
            foreach (ListViewItem d in data)
            {
                String[] r = new String[6];
                int i = 0;
                foreach (ListViewItem.ListViewSubItem dato in d.SubItems)
                {
                    r[i] = dato.Text;
                    i++;
                }

                DGV.Rows.Add(r);

            }
        }
    }
}
