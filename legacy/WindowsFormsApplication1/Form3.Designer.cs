namespace WindowsFormsApplication1
{
    partial class Form3
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			this.button5 = new System.Windows.Forms.Button();
			this.TBOrigen = new System.Windows.Forms.TextBox();
			this.button6 = new System.Windows.Forms.Button();
			this.CBCompanies = new System.Windows.Forms.ComboBox();
			this.TBDestiny = new System.Windows.Forms.TextBox();
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.button1 = new System.Windows.Forms.Button();
			this.dataGridView2 = new System.Windows.Forms.DataGridView();
			this.idDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.placeDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.destinyBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.invoiceDataSet1 = new WindowsFormsApplication1.InvoiceDataSet1();
			this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.companyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.placeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.origenBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.invoiceDataSet = new WindowsFormsApplication1.InvoiceDataSet();
			this.origenTableAdapter = new WindowsFormsApplication1.InvoiceDataSetTableAdapters.OrigenTableAdapter();
			this.destinyTableAdapter = new WindowsFormsApplication1.InvoiceDataSet1TableAdapters.DestinyTableAdapter();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.destinyBindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.invoiceDataSet1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.origenBindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.invoiceDataSet)).BeginInit();
			this.SuspendLayout();
			// 
			// button5
			// 
			this.button5.Location = new System.Drawing.Point(139, 12);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(90, 23);
			this.button5.TabIndex = 43;
			this.button5.Text = "Save Destiny";
			this.button5.UseVisualStyleBackColor = true;
			this.button5.Click += new System.EventHandler(this.button5_Click);
			// 
			// TBOrigen
			// 
			this.TBOrigen.Location = new System.Drawing.Point(586, 15);
			this.TBOrigen.Name = "TBOrigen";
			this.TBOrigen.Size = new System.Drawing.Size(125, 20);
			this.TBOrigen.TabIndex = 53;
			// 
			// button6
			// 
			this.button6.Location = new System.Drawing.Point(717, 12);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size(121, 23);
			this.button6.TabIndex = 52;
			this.button6.Text = "Add New Origin";
			this.button6.UseVisualStyleBackColor = true;
			this.button6.Click += new System.EventHandler(this.button6_Click);
			// 
			// CBCompanies
			// 
			this.CBCompanies.FormattingEnabled = true;
			this.CBCompanies.Location = new System.Drawing.Point(459, 15);
			this.CBCompanies.Name = "CBCompanies";
			this.CBCompanies.Size = new System.Drawing.Size(121, 21);
			this.CBCompanies.TabIndex = 51;
			// 
			// TBDestiny
			// 
			this.TBDestiny.Location = new System.Drawing.Point(12, 16);
			this.TBDestiny.Name = "TBDestiny";
			this.TBDestiny.Size = new System.Drawing.Size(121, 20);
			this.TBDestiny.TabIndex = 54;
			// 
			// dataGridView1
			// 
			this.dataGridView1.AutoGenerateColumns = false;
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.companyDataGridViewTextBoxColumn,
            this.placeDataGridViewTextBoxColumn});
			this.dataGridView1.DataSource = this.origenBindingSource;
			this.dataGridView1.Location = new System.Drawing.Point(459, 59);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.Size = new System.Drawing.Size(348, 148);
			this.dataGridView1.TabIndex = 55;
			this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(329, 294);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 56;
			this.button1.Text = "Ok";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// dataGridView2
			// 
			this.dataGridView2.AllowDrop = true;
			this.dataGridView2.AllowUserToAddRows = false;
			this.dataGridView2.AllowUserToDeleteRows = false;
			this.dataGridView2.AutoGenerateColumns = false;
			this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn1,
            this.placeDataGridViewTextBoxColumn1});
			this.dataGridView2.DataSource = this.destinyBindingSource;
			this.dataGridView2.Location = new System.Drawing.Point(12, 59);
			this.dataGridView2.MultiSelect = false;
			this.dataGridView2.Name = "dataGridView2";
			this.dataGridView2.Size = new System.Drawing.Size(258, 154);
			this.dataGridView2.TabIndex = 57;
			// 
			// idDataGridViewTextBoxColumn1
			// 
			this.idDataGridViewTextBoxColumn1.DataPropertyName = "Id";
			this.idDataGridViewTextBoxColumn1.HeaderText = "Id";
			this.idDataGridViewTextBoxColumn1.Name = "idDataGridViewTextBoxColumn1";
			// 
			// placeDataGridViewTextBoxColumn1
			// 
			this.placeDataGridViewTextBoxColumn1.DataPropertyName = "Place";
			this.placeDataGridViewTextBoxColumn1.HeaderText = "Place";
			this.placeDataGridViewTextBoxColumn1.Name = "placeDataGridViewTextBoxColumn1";
			// 
			// destinyBindingSource
			// 
			this.destinyBindingSource.DataMember = "Destiny";
			this.destinyBindingSource.DataSource = this.invoiceDataSet1;
			// 
			// invoiceDataSet1
			// 
			this.invoiceDataSet1.DataSetName = "InvoiceDataSet1";
			this.invoiceDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// idDataGridViewTextBoxColumn
			// 
			this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
			this.idDataGridViewTextBoxColumn.HeaderText = "Id";
			this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
			// 
			// companyDataGridViewTextBoxColumn
			// 
			this.companyDataGridViewTextBoxColumn.DataPropertyName = "Company";
			this.companyDataGridViewTextBoxColumn.HeaderText = "Company";
			this.companyDataGridViewTextBoxColumn.Name = "companyDataGridViewTextBoxColumn";
			// 
			// placeDataGridViewTextBoxColumn
			// 
			this.placeDataGridViewTextBoxColumn.DataPropertyName = "Place";
			this.placeDataGridViewTextBoxColumn.HeaderText = "Place";
			this.placeDataGridViewTextBoxColumn.Name = "placeDataGridViewTextBoxColumn";
			// 
			// origenBindingSource
			// 
			this.origenBindingSource.DataMember = "Origen";
			this.origenBindingSource.DataSource = this.invoiceDataSet;
			// 
			// invoiceDataSet
			// 
			this.invoiceDataSet.DataSetName = "InvoiceDataSet";
			this.invoiceDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// origenTableAdapter
			// 
			this.origenTableAdapter.ClearBeforeFill = true;
			// 
			// destinyTableAdapter
			// 
			this.destinyTableAdapter.ClearBeforeFill = true;
			// 
			// Form3
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(850, 338);
			this.Controls.Add(this.dataGridView2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.dataGridView1);
			this.Controls.Add(this.TBDestiny);
			this.Controls.Add(this.TBOrigen);
			this.Controls.Add(this.button6);
			this.Controls.Add(this.CBCompanies);
			this.Controls.Add(this.button5);
			this.Name = "Form3";
			this.Text = "Form3";
			this.Load += new System.EventHandler(this.Form3_Load);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.destinyBindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.invoiceDataSet1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.origenBindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.invoiceDataSet)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TextBox TBOrigen;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.ComboBox CBCompanies;
        private System.Windows.Forms.TextBox TBDestiny;
        private System.Windows.Forms.DataGridView dataGridView1;
        private InvoiceDataSet invoiceDataSet;
        private System.Windows.Forms.BindingSource origenBindingSource;
        private InvoiceDataSetTableAdapters.OrigenTableAdapter origenTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn companyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn placeDataGridViewTextBoxColumn;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dataGridView2;
        private InvoiceDataSet1 invoiceDataSet1;
        private System.Windows.Forms.BindingSource destinyBindingSource;
        private InvoiceDataSet1TableAdapters.DestinyTableAdapter destinyTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn placeDataGridViewTextBoxColumn1;
    }
}