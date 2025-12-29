using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Conect;
using PDF;


namespace WindowsFormsApplication1
{

    public partial class Form1 : Form
    {
       
        public Form1()
        {
            InitializeComponent();
        }     
        


        private void fillcombobox(ComboBox target, string query)
        {
            List<ListViewItem> Companies = simpleaccesconect.ConnectToAccess(query, true);
            target.Items.Clear();
            foreach (ListViewItem Company in Companies)
            {
                Console.WriteLine(Company.Text);
                target.Items.Add(Company.Text);
            }
        }

        private void filllist(CheckedListBox target, string query)
        {
            List<ListViewItem> Companies = simpleaccesconect.ConnectToAccess(query, true);
            target.Items.Clear();
            foreach (ListViewItem Company in Companies)
            {
                Console.WriteLine(Company.Text);
                target.Items.Add(Company.Text);
            }
        }



        private void getOrigins()
        {
            string wheres = "";
            foreach (string w in CLBCompany.CheckedItems)
            {
                wheres += " Company ='" + w + "' or ";
            }
            wheres += " 1=2";
            List<ListViewItem> Origenes = simpleaccesconect.ConnectToAccess("select Place from Origen WHERE " + wheres , true);
            CLBFrom.Items.Clear();
            foreach (ListViewItem Origen in Origenes)
            {
                Console.WriteLine(Origen.Text);
                CLBFrom.Items.Add(Origen.Text);
            }
            if (CLBFrom.Items.Count == 1)
            {
                CLBFrom.SetItemChecked(0, true);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string froms = "";
            foreach (string from in CLBFrom.CheckedItems)
            {
                froms+=from+"\n";
            }

            string companies = "";
            foreach (string company in CLBCompany.CheckedItems)
            {
                companies += company + "\n";
            }

            string tos = "";
            foreach (string t in CLBTo.CheckedItems)
            {
                tos += t + "\n";
            }

            DGV.Rows.Add(new String[] { DTPdate.Text, companies, froms,  tos,TBDispatch.Text,TBEmtys.Text, TBFB.Text,TBAmount.Text});
            decimal subtotal = 0;
            foreach (DataGridViewRow row in DGV.Rows)
            {
               subtotal=subtotal +Convert.ToDecimal(row.Cells[7].Value);
            }
            TBSubtotal.Text = subtotal.ToString();
            TBTotal.Text = (subtotal - Convert.ToDecimal(TBAdelanto.Text)).ToString();
            calculateTotal();
            cleanrow();
        }

        private void calculateTotal()
        {
            decimal subtotal = 0;
            foreach (DataGridViewRow row in DGV.Rows)
            {
                subtotal = subtotal + Decimal.Parse(row.Cells[7].Value.ToString());
            }
            TBSubtotal.Text = subtotal.ToString();
            TBTotal.Text = (subtotal - Decimal.Parse(TBAdelanto.Text)).ToString();
        }


        private void button3_Click(object sender, EventArgs e)
        {
            if (CBName.Text == "")
            {
                MessageBox.Show("Name is empty");
            }
            else
            {
                string query = "SELECT id FROM Costumers WHERE Name = '" + CBName.Text + "'";
                List<ListViewItem> exists = simpleaccesconect.ConnectToAccess(query, true);
                if (exists.Count > 0)
                {
                    query = "UPDATE Costumers SET Address='" + TBAddress.Text + "', City='" + TBCity.Text + "', State='" + TBState.Text + "', Phone='" + TBPhone.Text + "' WHERE Name='" + CBName.Text + "'";
                }
                else
                {
                    query = "INSERT INTO Costumers (Name, Address, City, State, Phone) VALUES ('" + CBName.Text + "','" + TBAddress.Text + "','" + TBCity.Text + "','" + TBState.Text + "','" + TBPhone.Text + "')";

                }
                Conect.simpleaccesconect.ConnectToAccess(query, false);
                fillcombobox(CBName, "select DISTINCT Name from Costumers");
            }
           
        }

        private void TBAdelanto_ValueChanged(object sender, EventArgs e)
        {
            calculateTotal();
        }

        private void TBAmount_ValueChanged(object sender, EventArgs e)
        {
            if (TBAmount.Text == "")
            {
                TBAmount.Text = "0";
            }
        }

        public void makePDF(Dictionary<string, string> invoicedata, List<List<String>> rowdata)
        {
            PDFMaker.GenInvoice(invoicedata, rowdata);
        }


        private void button2_Click(object sender, EventArgs e)
        {
            Dictionary<String, String> invoicedata = new Dictionary<string, string>();
            invoicedata.Add("invoice", NUDInvoice.Value.ToString());
            invoicedata.Add("date", DTPDateInv.Text);
            invoicedata.Add("check", TBCheck.Text);
            invoicedata.Add("Name", CBName.Text);
            invoicedata.Add("Adress", TBAddress.Text);
            invoicedata.Add("city", TBCity.Text);
            invoicedata.Add("state", TBState.Text);
            invoicedata.Add("phone", TBPhone.Text);
            invoicedata.Add("subtotal", TBSubtotal.Text);
            invoicedata.Add("advance", TBAdelanto.Value.ToString());
            invoicedata.Add("total", TBTotal.Text);
            List<List<String>> rowdata = new List<List<String>>();

            //List<ListViewItem> data = Conect.simpleaccesconect.ConnectToAccess("Select * From invoices where [No] = "+ NUDInvoice.Value, true);
            //if (data.Count > 0)
            //{
            //    MessageBox.Show("Duplicated Invoice No");
            //}
            //else
            //{

                Conect.simpleaccesconect.ConnectToAccess("Insert into Invoices ([No], D, [Check], Costumer, [Subtotal], [Advance]) values ('" + NUDInvoice.Value + "','" + DTPDateInv.Text + "', '" + TBCheck.Text + "', '" + CBName.Text + "' , '" + TBSubtotal.Text + "','" + TBAdelanto.Value + "')", false);
                Conect.simpleaccesconect.ConnectToAccess("Update Costumers set invoice=" + NUDInvoice.Value + " where Name = '" + CBName.Text + "'", false);

                foreach (DataGridViewRow datarow in DGV.Rows)
                {
                    List<String> row = new List<String>();
                    string values = "'" + NUDInvoice.Value + "'";
                    foreach (DataGridViewCell cell in datarow.Cells)
                    {
                        row.Add(cell.Value.ToString());
                        values = values + " , '" + cell.Value.ToString().Replace("\n", ",") + "'";
                    }
                    rowdata.Add(row);
                    Conect.simpleaccesconect.ConnectToAccess("Insert into Detail ([Invoice], [Date], [Company], [From], [To], [Dispatch], [FB],[Emtys], [Amount]) values (" + values + ")", false);
                    string query = "SELECT id FROM Precios WHERE Company = '" + datarow.Cells[1].Value.ToString().Replace("\n", ",") + "' and From ='" + datarow.Cells[2].Value.ToString().Replace("\n", ",") + "' and To ='" + datarow.Cells[3].Value.ToString().Replace("\n", ",") + "'";
                    List<ListViewItem> exists = Conect.simpleaccesconect.ConnectToAccess(query, true);
                    if (exists.Count > 0)
                    {
                        query = "UPDATE Precios SET [Amount]='" + datarow.Cells[7].Value.ToString() + "' WHERE [Company] = '" + datarow.Cells[1].Value.ToString().Replace("\n", ",") + "' and [From] ='" + datarow.Cells[2].Value.ToString().Replace("\n", ",") + "' and [To] ='" + datarow.Cells[3].Value.ToString().Replace("\n", ",") + "'";
                    }
                    else
                    {
                        query = "INSERT INTO Precios ([Company], [From], [To], [Amount]) VALUES ('" + datarow.Cells[1].Value.ToString().Replace("\n", ",") + "','" + datarow.Cells[2].Value.ToString().Replace("\n", ",") + "','" + datarow.Cells[3].Value.ToString().Replace("\n", ",") + "','" + datarow.Cells[7].Value.ToString() + "')";
                    }
                    //MessageBox.Show(query);

                Conect.simpleaccesconect.ConnectToAccess(query, false);
                }

                Form4 frm4 = new Form4(this, invoicedata, rowdata);
                frm4.ShowDialog();
            //}
            
        }

		private void DGV_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			calculateTotal();
		}

		private void DGV_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            calculateTotal();
        }

        private void BTNDeleteSH_Click(object sender, EventArgs e)
        {
            Conect.simpleaccesconect.ConnectToAccess("Delete from costumers where Name='" + CBName.Text + "' and Phone='" + TBPhone.Text + "' and Address='"+ TBAddress.Text +"'", false);
            fillcombobox(CBName, "select DISTINCT Name from Costumers");
            CBName.Text = "";
            TBAddress.Text = "";
            TBCity.Text = "";
            TBPhone.Text = "";
            TBState.Text="";
        }




        private void CBCompanies_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            getOrigins();
        }

        private void CBName_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            List<ListViewItem> Costumer = Conect.simpleaccesconect.ConnectToAccess("SELECT Address, City, State, Phone, Invoice FROM Costumers where name='" + CBName.Text + "'", true);
            TBAddress.Text = Costumer[0].SubItems[0].Text;
            TBCity.Text = Costumer[0].SubItems[1].Text;
            TBState.Text = Costumer[0].SubItems[2].Text;
            TBPhone.Text = Costumer[0].SubItems[3].Text;
            NUDInvoice.Value = Convert.ToInt32(Costumer[0].SubItems[4].Text)+1;
            this.Text = CBName.Text;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Form2 frm2 = new Form2(this);
            frm2.ShowDialog();
        }



        public void reload()
        {
            TBAddress.Text = "";            
            CBName.Text = "";
            for (int i = 0; i < CLBTo.Items.Count; i++)
            {
                CLBTo.SetItemChecked(i, false);
            }
            TBCity.Text = "";            
            TBState.Text = "";
            TBPhone.Text = "";
            TBCheck.Text = "";
            TBTotal.Text = "0.00";
            TBSubtotal.Text = "0.00";
            cleanrow();
            DGV.Rows.Clear();
            List<ListViewItem> Invoice = simpleaccesconect.ConnectToAccess("SELECT max([No]) FROM Invoices", true);
            NUDInvoice.Value = Convert.ToDecimal(Invoice[0].SubItems[0].Text) + 1;
            
        }

        private void cleanrow() {
            TBDispatch.Text = "";
            TBFB.Text = "";
            CLBFrom.Items.Clear();
            for (int i = 0; i < CLBCompany.Items.Count; i++)
            {
                CLBCompany.SetItemChecked(i, false);
            }
            for (int i = 0; i < CLBTo.Items.Count; i++)
            {
                CLBTo.SetItemChecked(i, false);
            }
            TBAmount.Value = 0;

        }

        public void reloaded(int invoice){
            List<ListViewItem> Invoice = simpleaccesconect.ConnectToAccess("SELECT [No],[D],[Check],[Costumer],[Subtotal],[Advance]  FROM Invoices Where [No]=" + invoice + " ", true);
            NUDInvoice.Value = Convert.ToDecimal(Invoice[0].SubItems[0].Text);
            DTPDateInv.Value = Convert.ToDateTime(Invoice[0].SubItems[1].Text);
            TBCheck.Text = Invoice[0].SubItems[2].Text;
            CBName.Text = Invoice[0].SubItems[3].Text;
            TBSubtotal.Text = Invoice[0].SubItems[4].Text;
            TBAdelanto.Text = Invoice[0].SubItems[5].Text;
            DGV.Rows.Clear();
            List<ListViewItem> Details = simpleaccesconect.ConnectToAccess("SELECT [Date],[Company],[From],[To],[Dispatch],[Emtys],[FB],[Amount]  FROM Detail Where [Invoice]='" + invoice + "'", true);
            foreach (ListViewItem data in Details)
            {
                String[] r = new String[8];
                int i = 0;
                foreach (ListViewItem.ListViewSubItem d in data.SubItems)
                {
                    r[i] = d.Text.Replace(",", "\n");
                    i++;
                }
                DGV.Rows.Add(r);
            
            }
            calculateTotal();
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            //this.MinimumSize = new Size(1900, 1080);
            //this.MaximumSize = new Size(1900,1080);
            DTPDateInv.Value = DateTime.Now;
            DTPdate.Value = DateTime.Now;
            //this.DGV.DefaultCellStyle.Font = new Font("Tahoma", 15);
            filllist(CLBCompany, "SELECT o.Company, max(o.orden) as used FROM Origen o GROUP BY o.Company ORDER BY  max(o.orden) DESC");
            filllist(CLBTo, "select Place from Destiny");
            fillcombobox(CBName, "select DISTINCT Name from Costumers");
            List<ListViewItem> Invoice = simpleaccesconect.ConnectToAccess("SELECT max([No]) FROM Invoices", true);
            if (Invoice.Count > 0)
            {
                try
                {
                    NUDInvoice.Value = Convert.ToDecimal(Invoice[0].SubItems[0].Text) + 1;
                }
                catch
                {
                    NUDInvoice.Value = 1;
                }

            }
        }

        private void CLBCompany_MouseUp(object sender, EventArgs e)
        {
            getOrigins();
        }

        private void CLBTo_MouseUp(object sender, EventArgs e)
        {
            string froms = "";
            foreach (string from in CLBFrom.CheckedItems)
            {
                froms += from + ",";
            }

            string companies = "";
            foreach (string company in CLBCompany.CheckedItems)
            {
                companies += company + ",";
            }

            string tos = "";
            foreach (string t in CLBTo.CheckedItems)
            {
                tos += t + ",";
            }

            List<ListViewItem> Amount = simpleaccesconect.ConnectToAccess("SELECT [Amount] FROM Precios where  [Company] = '" + companies + "' and [From] ='" + froms + "' and [To] ='" + tos + "'", true);

            if (Amount.Count > 0)
            {
                TBAmount.Value = Convert.ToDecimal(Amount[0].SubItems[0].Text);
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Form3 frm3 = new Form3(this);
            frm3.ShowDialog();
        }

        public void updatecities() {
            filllist(CLBCompany, "SELECT o.Company, max(o.orden) as used FROM Origen o GROUP BY o.Company ORDER BY  max(o.orden) DESC");
            filllist(CLBTo, "select Place from Destiny");

        }

		private void label11_Click(object sender, EventArgs e)
		{

		}

		private void DGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{

		}
	}
}
