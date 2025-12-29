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
    public partial class Form4 : Form
    {

        Form1 father;
        Dictionary<string, string> invoicedata =new Dictionary<string, string>();
        List<List<String>> rowdata = new List<List<String>>();

        public Form4(Form1 dad, Dictionary<string, string> invoicedata, List<List<String>> rowdata)
        {
            father = dad;
            this.invoicedata = invoicedata;
            this.rowdata = rowdata;
            InitializeComponent();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (KeyValuePair<string, string> kvp in this.invoicedata)
            {
                sb.AppendLine(kvp.Key + ":" + kvp.Value);
            }
            foreach (List<String> l in rowdata)
            {
                sb.AppendLine();
                sb.AppendLine("-------------------------------------");
                foreach (String s in l)
                {
                    sb.Append(s+"      ");
                }                
                textBox1.Text = sb.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            father.reload();
            father.makePDF(invoicedata, rowdata);
            this.Close();
        }           
        
    }
}
