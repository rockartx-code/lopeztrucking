namespace WindowsFormsApplication1
{
    partial class Form1
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.DTPdate = new System.Windows.Forms.DateTimePicker();
			this.label1 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.TBDispatch = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.TBFB = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.TBSubtotal = new System.Windows.Forms.TextBox();
			this.TBTotal = new System.Windows.Forms.TextBox();
			this.label12 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.BTNDeleteSH = new System.Windows.Forms.Button();
			this.TBState = new System.Windows.Forms.ComboBox();
			this.CBName = new System.Windows.Forms.ComboBox();
			this.button3 = new System.Windows.Forms.Button();
			this.TBPhone = new System.Windows.Forms.TextBox();
			this.TBCity = new System.Windows.Forms.TextBox();
			this.TBAddress = new System.Windows.Forms.TextBox();
			this.label19 = new System.Windows.Forms.Label();
			this.label18 = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.DGV = new System.Windows.Forms.DataGridView();
			this.Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Company = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.From = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.To = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Dispatch = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Emtys = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.FB = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Amount = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.TBAmount = new System.Windows.Forms.NumericUpDown();
			this.TBAdelanto = new System.Windows.Forms.NumericUpDown();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.NUDInvoice = new System.Windows.Forms.NumericUpDown();
			this.TBCheck = new System.Windows.Forms.TextBox();
			this.DTPDateInv = new System.Windows.Forms.DateTimePicker();
			this.label9 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.CLBCompany = new System.Windows.Forms.CheckedListBox();
			this.CLBFrom = new System.Windows.Forms.CheckedListBox();
			this.label10 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.CLBTo = new System.Windows.Forms.CheckedListBox();
			this.label11 = new System.Windows.Forms.Label();
			this.TBEmtys = new System.Windows.Forms.TextBox();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.DGV)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.TBAmount)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.TBAdelanto)).BeginInit();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.NUDInvoice)).BeginInit();
			this.groupBox3.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// DTPdate
			// 
			this.DTPdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.DTPdate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.DTPdate.Location = new System.Drawing.Point(68, 212);
			this.DTPdate.Name = "DTPdate";
			this.DTPdate.Size = new System.Drawing.Size(129, 30);
			this.DTPdate.TabIndex = 8;
			this.DTPdate.Value = new System.DateTime(2019, 2, 2, 0, 0, 0, 0);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(10, 217);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(53, 25);
			this.label1.TabIndex = 1;
			this.label1.Text = "Date";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(496, 202);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(36, 25);
			this.label3.TabIndex = 4;
			this.label3.Text = "To";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(706, 198);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(88, 25);
			this.label4.TabIndex = 6;
			this.label4.Text = "Dispatch";
			// 
			// TBDispatch
			// 
			this.TBDispatch.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TBDispatch.Location = new System.Drawing.Point(710, 224);
			this.TBDispatch.Multiline = true;
			this.TBDispatch.Name = "TBDispatch";
			this.TBDispatch.Size = new System.Drawing.Size(187, 51);
			this.TBDispatch.TabIndex = 12;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(903, 201);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(47, 25);
			this.label5.TabIndex = 8;
			this.label5.Text = "F.B.";
			// 
			// TBFB
			// 
			this.TBFB.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TBFB.Location = new System.Drawing.Point(907, 224);
			this.TBFB.Multiline = true;
			this.TBFB.Name = "TBFB";
			this.TBFB.Size = new System.Drawing.Size(211, 151);
			this.TBFB.TabIndex = 13;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(1124, 265);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(80, 25);
			this.label6.TabIndex = 10;
			this.label6.Text = "Amount";
			// 
			// button1
			// 
			this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.button1.Location = new System.Drawing.Point(1128, 340);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(164, 35);
			this.button1.TabIndex = 22;
			this.button1.Text = "Add";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.button2.Location = new System.Drawing.Point(32, 657);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(98, 37);
			this.button2.TabIndex = 24;
			this.button2.Text = "Save";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// TBSubtotal
			// 
			this.TBSubtotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TBSubtotal.Location = new System.Drawing.Point(1129, 627);
			this.TBSubtotal.Name = "TBSubtotal";
			this.TBSubtotal.Size = new System.Drawing.Size(173, 30);
			this.TBSubtotal.TabIndex = 25;
			// 
			// TBTotal
			// 
			this.TBTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TBTotal.Location = new System.Drawing.Point(1078, 695);
			this.TBTotal.Name = "TBTotal";
			this.TBTotal.Size = new System.Drawing.Size(173, 30);
			this.TBTotal.TabIndex = 27;
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label12.Location = new System.Drawing.Point(1028, 667);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(84, 25);
			this.label12.TabIndex = 28;
			this.label12.Text = "Subtotal";
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label13.Location = new System.Drawing.Point(1028, 632);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(90, 25);
			this.label13.TabIndex = 29;
			this.label13.Text = "Adelanto";
			// 
			// label14
			// 
			this.label14.AutoSize = true;
			this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label14.Location = new System.Drawing.Point(1016, 700);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(56, 25);
			this.label14.TabIndex = 30;
			this.label14.Text = "Total";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.BTNDeleteSH);
			this.groupBox1.Controls.Add(this.TBState);
			this.groupBox1.Controls.Add(this.CBName);
			this.groupBox1.Controls.Add(this.button3);
			this.groupBox1.Controls.Add(this.TBPhone);
			this.groupBox1.Controls.Add(this.TBCity);
			this.groupBox1.Controls.Add(this.TBAddress);
			this.groupBox1.Controls.Add(this.label19);
			this.groupBox1.Controls.Add(this.label18);
			this.groupBox1.Controls.Add(this.label17);
			this.groupBox1.Controls.Add(this.label16);
			this.groupBox1.Controls.Add(this.label15);
			this.groupBox1.Location = new System.Drawing.Point(14, 28);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(610, 178);
			this.groupBox1.TabIndex = 31;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Subhauler";
			// 
			// BTNDeleteSH
			// 
			this.BTNDeleteSH.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.BTNDeleteSH.Location = new System.Drawing.Point(326, 133);
			this.BTNDeleteSH.Name = "BTNDeleteSH";
			this.BTNDeleteSH.Size = new System.Drawing.Size(128, 33);
			this.BTNDeleteSH.TabIndex = 13;
			this.BTNDeleteSH.Text = "Delete Subhauler";
			this.BTNDeleteSH.UseVisualStyleBackColor = true;
			this.BTNDeleteSH.Click += new System.EventHandler(this.BTNDeleteSH_Click);
			// 
			// TBState
			// 
			this.TBState.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TBState.FormattingEnabled = true;
			this.TBState.Items.AddRange(new object[] {
            "Alabama",
            "Alaska",
            "Arizona",
            "Arkansas",
            "California",
            "Colorado",
            "Connecticut",
            "Delaware",
            "Florida",
            "Georgia",
            "Hawaii",
            "Idaho",
            "Illinois",
            "Indiana",
            "Iowa",
            "Kansas",
            "Kentucky",
            "Louisiana",
            "Maine",
            "Maryland",
            "Massachusetts",
            "Michigan",
            "Minnesota",
            "Mississippi",
            "Missouri",
            "Montana",
            "Nebraska",
            "Nevada",
            "New Hampshire",
            "New Jersey",
            "New Mexico",
            "New York",
            "North Carolina",
            "North Dakota",
            "Ohio",
            "Oklahoma",
            "Oregon",
            "Pennsylvania",
            "Rhode Island",
            "South Carolina",
            "South Dakota",
            "Tennessee",
            "Texas",
            "Utah",
            "Vermont",
            "Virginia",
            "Washington",
            "West Virginia",
            "Wisconsin",
            "Wyoming"});
			this.TBState.Location = new System.Drawing.Point(432, 94);
			this.TBState.Name = "TBState";
			this.TBState.Size = new System.Drawing.Size(157, 33);
			this.TBState.TabIndex = 3;
			// 
			// CBName
			// 
			this.CBName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.CBName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.CBName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.CBName.FormattingEnabled = true;
			this.CBName.Location = new System.Drawing.Point(88, 19);
			this.CBName.Name = "CBName";
			this.CBName.Size = new System.Drawing.Size(501, 33);
			this.CBName.TabIndex = 0;
			this.CBName.SelectedIndexChanged += new System.EventHandler(this.CBName_SelectedIndexChanged_1);
			// 
			// button3
			// 
			this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.button3.Location = new System.Drawing.Point(463, 133);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(126, 31);
			this.button3.TabIndex = 10;
			this.button3.Text = "Save Subhauler";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// TBPhone
			// 
			this.TBPhone.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TBPhone.Location = new System.Drawing.Point(64, 135);
			this.TBPhone.Name = "TBPhone";
			this.TBPhone.Size = new System.Drawing.Size(186, 30);
			this.TBPhone.TabIndex = 4;
			// 
			// TBCity
			// 
			this.TBCity.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TBCity.Location = new System.Drawing.Point(66, 94);
			this.TBCity.Name = "TBCity";
			this.TBCity.Size = new System.Drawing.Size(298, 30);
			this.TBCity.TabIndex = 2;
			// 
			// TBAddress
			// 
			this.TBAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TBAddress.Location = new System.Drawing.Point(105, 58);
			this.TBAddress.Name = "TBAddress";
			this.TBAddress.Size = new System.Drawing.Size(484, 30);
			this.TBAddress.TabIndex = 1;
			// 
			// label19
			// 
			this.label19.AutoSize = true;
			this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label19.Location = new System.Drawing.Point(14, 141);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(42, 25);
			this.label19.TabIndex = 4;
			this.label19.Text = "ZIP";
			// 
			// label18
			// 
			this.label18.AutoSize = true;
			this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label18.Location = new System.Drawing.Point(368, 96);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(58, 25);
			this.label18.TabIndex = 3;
			this.label18.Text = "State";
			// 
			// label17
			// 
			this.label17.AutoSize = true;
			this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label17.Location = new System.Drawing.Point(14, 99);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(46, 25);
			this.label17.TabIndex = 2;
			this.label17.Text = "City";
			// 
			// label16
			// 
			this.label16.AutoSize = true;
			this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label16.Location = new System.Drawing.Point(14, 63);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(85, 25);
			this.label16.TabIndex = 1;
			this.label16.Text = "Address";
			// 
			// label15
			// 
			this.label15.AutoSize = true;
			this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label15.Location = new System.Drawing.Point(13, 23);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(64, 25);
			this.label15.TabIndex = 0;
			this.label15.Text = "Name";
			// 
			// DGV
			// 
			this.DGV.AllowUserToAddRows = false;
			this.DGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.DGV.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.DGV.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
			this.DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.DGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Date,
            this.Company,
            this.From,
            this.To,
            this.Dispatch,
            this.Emtys,
            this.FB,
            this.Amount});
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.DGV.DefaultCellStyle = dataGridViewCellStyle4;
			this.DGV.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
			this.DGV.Location = new System.Drawing.Point(11, 399);
			this.DGV.Name = "DGV";
			this.DGV.RowHeadersWidth = 62;
			this.DGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.DGV.Size = new System.Drawing.Size(1290, 226);
			this.DGV.TabIndex = 37;
			this.DGV.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_CellContentClick);
			this.DGV.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_CellEndEdit);
			this.DGV.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.DGV_RowsRemoved);
			// 
			// Date
			// 
			this.Date.HeaderText = "Date";
			this.Date.MinimumWidth = 8;
			this.Date.Name = "Date";
			// 
			// Company
			// 
			this.Company.HeaderText = "Company";
			this.Company.MinimumWidth = 8;
			this.Company.Name = "Company";
			// 
			// From
			// 
			this.From.HeaderText = "From";
			this.From.MinimumWidth = 8;
			this.From.Name = "From";
			// 
			// To
			// 
			this.To.HeaderText = "To";
			this.To.MinimumWidth = 8;
			this.To.Name = "To";
			// 
			// Dispatch
			// 
			this.Dispatch.HeaderText = "Dispatch";
			this.Dispatch.MinimumWidth = 8;
			this.Dispatch.Name = "Dispatch";
			// 
			// Emtys
			// 
			this.Emtys.HeaderText = "Emtys";
			this.Emtys.Name = "Emtys";
			// 
			// FB
			// 
			this.FB.HeaderText = "F.B.";
			this.FB.MinimumWidth = 8;
			this.FB.Name = "FB";
			// 
			// Amount
			// 
			this.Amount.HeaderText = "Amount";
			this.Amount.MinimumWidth = 8;
			this.Amount.Name = "Amount";
			// 
			// TBAmount
			// 
			this.TBAmount.DecimalPlaces = 2;
			this.TBAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TBAmount.Increment = new decimal(new int[] {
            10,
            0,
            0,
            131072});
			this.TBAmount.Location = new System.Drawing.Point(1128, 293);
			this.TBAmount.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
			this.TBAmount.Name = "TBAmount";
			this.TBAmount.Size = new System.Drawing.Size(164, 30);
			this.TBAmount.TabIndex = 14;
			this.TBAmount.ValueChanged += new System.EventHandler(this.TBAmount_ValueChanged);
			// 
			// TBAdelanto
			// 
			this.TBAdelanto.DecimalPlaces = 2;
			this.TBAdelanto.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TBAdelanto.Increment = new decimal(new int[] {
            10,
            0,
            0,
            131072});
			this.TBAdelanto.Location = new System.Drawing.Point(1129, 661);
			this.TBAdelanto.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
			this.TBAdelanto.Name = "TBAdelanto";
			this.TBAdelanto.Size = new System.Drawing.Size(173, 30);
			this.TBAdelanto.TabIndex = 40;
			this.TBAdelanto.ValueChanged += new System.EventHandler(this.TBAdelanto_ValueChanged);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.NUDInvoice);
			this.groupBox2.Controls.Add(this.TBCheck);
			this.groupBox2.Controls.Add(this.label9);
			this.groupBox2.Controls.Add(this.label7);
			this.groupBox2.Location = new System.Drawing.Point(645, 31);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(414, 146);
			this.groupBox2.TabIndex = 45;
			this.groupBox2.TabStop = false;
			// 
			// NUDInvoice
			// 
			this.NUDInvoice.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.NUDInvoice.Location = new System.Drawing.Point(185, 34);
			this.NUDInvoice.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
			this.NUDInvoice.Name = "NUDInvoice";
			this.NUDInvoice.Size = new System.Drawing.Size(163, 30);
			this.NUDInvoice.TabIndex = 5;
			this.NUDInvoice.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// TBCheck
			// 
			this.TBCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TBCheck.Location = new System.Drawing.Point(185, 88);
			this.TBCheck.Name = "TBCheck";
			this.TBCheck.Size = new System.Drawing.Size(163, 30);
			this.TBCheck.TabIndex = 7;
			// 
			// DTPDateInv
			// 
			this.DTPDateInv.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.DTPDateInv.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			this.DTPDateInv.Location = new System.Drawing.Point(1128, 232);
			this.DTPDateInv.Name = "DTPDateInv";
			this.DTPDateInv.Size = new System.Drawing.Size(164, 30);
			this.DTPDateInv.TabIndex = 6;
			this.DTPDateInv.Value = new System.DateTime(2019, 2, 2, 0, 0, 0, 0);
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label9.Location = new System.Drawing.Point(45, 91);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(69, 25);
			this.label9.TabIndex = 39;
			this.label9.Text = "Check";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label8.Location = new System.Drawing.Point(1124, 202);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(53, 25);
			this.label8.TabIndex = 38;
			this.label8.Text = "Date";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label7.Location = new System.Drawing.Point(45, 39);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(104, 25);
			this.label7.TabIndex = 37;
			this.label7.Text = "Invoice No";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.CLBCompany);
			this.groupBox3.Controls.Add(this.CLBFrom);
			this.groupBox3.Controls.Add(this.label10);
			this.groupBox3.Controls.Add(this.label2);
			this.groupBox3.Location = new System.Drawing.Point(8, 244);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(482, 131);
			this.groupBox3.TabIndex = 46;
			this.groupBox3.TabStop = false;
			// 
			// CLBCompany
			// 
			this.CLBCompany.CheckOnClick = true;
			this.CLBCompany.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.CLBCompany.FormattingEnabled = true;
			this.CLBCompany.Location = new System.Drawing.Point(6, 39);
			this.CLBCompany.Name = "CLBCompany";
			this.CLBCompany.Size = new System.Drawing.Size(247, 79);
			this.CLBCompany.TabIndex = 9;
			this.CLBCompany.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CLBCompany_MouseUp);
			// 
			// CLBFrom
			// 
			this.CLBFrom.CheckOnClick = true;
			this.CLBFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.CLBFrom.FormattingEnabled = true;
			this.CLBFrom.Location = new System.Drawing.Point(271, 39);
			this.CLBFrom.Name = "CLBFrom";
			this.CLBFrom.Size = new System.Drawing.Size(189, 79);
			this.CLBFrom.TabIndex = 70;
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label10.Location = new System.Drawing.Point(6, 16);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(97, 25);
			this.label10.TabIndex = 46;
			this.label10.Text = "Company";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(267, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(57, 25);
			this.label2.TabIndex = 45;
			this.label2.Text = "From";
			// 
			// toolStrip1
			// 
			this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
			this.toolStrip1.Size = new System.Drawing.Size(1316, 28);
			this.toolStrip1.TabIndex = 47;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// toolStripButton1
			// 
			this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripButton1.Font = new System.Drawing.Font("Segoe UI", 12F);
			this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
			this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton1.Name = "toolStripButton1";
			this.toolStripButton1.Size = new System.Drawing.Size(40, 25);
			this.toolStripButton1.Text = "Log";
			this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
			// 
			// toolStripButton2
			// 
			this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripButton2.Font = new System.Drawing.Font("Segoe UI", 12F);
			this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
			this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton2.Name = "toolStripButton2";
			this.toolStripButton2.Size = new System.Drawing.Size(42, 25);
			this.toolStripButton2.Text = "Add";
			this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(1082, 34);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(210, 158);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 48;
			this.pictureBox1.TabStop = false;
			// 
			// CLBTo
			// 
			this.CLBTo.CheckOnClick = true;
			this.CLBTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.CLBTo.FormattingEnabled = true;
			this.CLBTo.Location = new System.Drawing.Point(500, 225);
			this.CLBTo.Name = "CLBTo";
			this.CLBTo.Size = new System.Drawing.Size(200, 154);
			this.CLBTo.TabIndex = 11;
			this.CLBTo.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CLBTo_MouseUp);
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label11.Location = new System.Drawing.Point(706, 295);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(66, 25);
			this.label11.TabIndex = 49;
			this.label11.Text = "Emtys";
			this.label11.Click += new System.EventHandler(this.label11_Click);
			// 
			// TBEmtys
			// 
			this.TBEmtys.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TBEmtys.Location = new System.Drawing.Point(711, 324);
			this.TBEmtys.Multiline = true;
			this.TBEmtys.Name = "TBEmtys";
			this.TBEmtys.Size = new System.Drawing.Size(187, 51);
			this.TBEmtys.TabIndex = 50;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.ClientSize = new System.Drawing.Size(1316, 701);
			this.Controls.Add(this.TBEmtys);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.DTPDateInv);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.CLBTo);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.TBAdelanto);
			this.Controls.Add(this.TBAmount);
			this.Controls.Add(this.DGV);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label14);
			this.Controls.Add(this.label13);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.TBTotal);
			this.Controls.Add(this.TBSubtotal);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.TBFB);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.TBDispatch);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.DTPdate);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Form1";
			this.Text = "INVOICE";
			this.Load += new System.EventHandler(this.Form1_Load_1);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.DGV)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.TBAmount)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.TBAdelanto)).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.NUDInvoice)).EndInit();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker DTPdate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TBDispatch;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TBFB;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox TBSubtotal;
        private System.Windows.Forms.TextBox TBTotal;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox TBPhone;
        private System.Windows.Forms.TextBox TBCity;
        private System.Windows.Forms.TextBox TBAddress;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ComboBox CBName;
        private System.Windows.Forms.DataGridView DGV;
        private System.Windows.Forms.ComboBox TBState;
        private System.Windows.Forms.NumericUpDown TBAmount;
        private System.Windows.Forms.NumericUpDown TBAdelanto;
        private System.Windows.Forms.Button BTNDeleteSH;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown NUDInvoice;
        private System.Windows.Forms.TextBox TBCheck;
        private System.Windows.Forms.DateTimePicker DTPDateInv;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckedListBox CLBFrom;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.CheckedListBox CLBTo;
        private System.Windows.Forms.CheckedListBox CLBCompany;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.TextBox TBEmtys;
		private System.Windows.Forms.DataGridViewTextBoxColumn Date;
		private System.Windows.Forms.DataGridViewTextBoxColumn Company;
		private System.Windows.Forms.DataGridViewTextBoxColumn From;
		private System.Windows.Forms.DataGridViewTextBoxColumn To;
		private System.Windows.Forms.DataGridViewTextBoxColumn Dispatch;
		private System.Windows.Forms.DataGridViewTextBoxColumn Emtys;
		private System.Windows.Forms.DataGridViewTextBoxColumn FB;
		private System.Windows.Forms.DataGridViewTextBoxColumn Amount;
	}
}

