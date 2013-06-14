namespace IA
{
    partial class HomeIA
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HomeIA));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this._id = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._connected = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._etatComm = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._etatRobot = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._posX = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._posY = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CB_Xbee = new System.Windows.Forms.CheckBox();
            this.btn_ActualiserListePortSerie = new System.Windows.Forms.Button();
            this.ctlListePorts = new System.Windows.Forms.ComboBox();
            this.btn_connection = new System.Windows.Forms.Button();
            this.lbl_portSerie = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.RTB_log = new System.Windows.Forms.RichTextBox();
            this.btn_test = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1278, 732);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btn_test);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.listView1);
            this.tabPage1.Controls.Add(this.CB_Xbee);
            this.tabPage1.Controls.Add(this.btn_ActualiserListePortSerie);
            this.tabPage1.Controls.Add(this.ctlListePorts);
            this.tabPage1.Controls.Add(this.btn_connection);
            this.tabPage1.Controls.Add(this.lbl_portSerie);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1270, 706);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "IA";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Robots :";
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._id,
            this._connected,
            this._etatComm,
            this._etatRobot,
            this._posX,
            this._posY});
            this.listView1.Location = new System.Drawing.Point(8, 53);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(650, 87);
            this.listView1.TabIndex = 14;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // _id
            // 
            this._id.Tag = "_id";
            this._id.Text = "ID";
            // 
            // _connected
            // 
            this._connected.Tag = "_connected";
            this._connected.Text = "Connecté";
            this._connected.Width = 101;
            // 
            // _etatComm
            // 
            this._etatComm.Tag = "_etatComm";
            this._etatComm.Text = "EtatComm";
            this._etatComm.Width = 154;
            // 
            // _etatRobot
            // 
            this._etatRobot.Tag = "_etatRobot";
            this._etatRobot.Text = "EtatRobot";
            this._etatRobot.Width = 168;
            // 
            // _posX
            // 
            this._posX.Tag = "_posX";
            this._posX.Text = "posX";
            this._posX.Width = 76;
            // 
            // _posY
            // 
            this._posY.Tag = "_posY";
            this._posY.Text = "posY";
            this._posY.Width = 81;
            // 
            // CB_Xbee
            // 
            this.CB_Xbee.AutoSize = true;
            this.CB_Xbee.Checked = true;
            this.CB_Xbee.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CB_Xbee.Location = new System.Drawing.Point(246, 8);
            this.CB_Xbee.Name = "CB_Xbee";
            this.CB_Xbee.Size = new System.Drawing.Size(68, 17);
            this.CB_Xbee.TabIndex = 13;
            this.CB_Xbee.Text = "XbeeAPI";
            this.CB_Xbee.UseVisualStyleBackColor = true;
            this.CB_Xbee.CheckedChanged += new System.EventHandler(this.CB_Xbee_CheckedChanged);
            // 
            // btn_ActualiserListePortSerie
            // 
            this.btn_ActualiserListePortSerie.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_ActualiserListePortSerie.Image = ((System.Drawing.Image)(resources.GetObject("btn_ActualiserListePortSerie.Image")));
            this.btn_ActualiserListePortSerie.Location = new System.Drawing.Point(71, 5);
            this.btn_ActualiserListePortSerie.Name = "btn_ActualiserListePortSerie";
            this.btn_ActualiserListePortSerie.Size = new System.Drawing.Size(29, 23);
            this.btn_ActualiserListePortSerie.TabIndex = 12;
            this.btn_ActualiserListePortSerie.UseVisualStyleBackColor = true;
            this.btn_ActualiserListePortSerie.Click += new System.EventHandler(this.btn_ActualiserListePortSerie_Click);
            // 
            // ctlListePorts
            // 
            this.ctlListePorts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ctlListePorts.FormattingEnabled = true;
            this.ctlListePorts.Items.AddRange(new object[] {
            "aze",
            "zae",
            "rfdqs"});
            this.ctlListePorts.Location = new System.Drawing.Point(106, 6);
            this.ctlListePorts.Name = "ctlListePorts";
            this.ctlListePorts.Size = new System.Drawing.Size(134, 21);
            this.ctlListePorts.TabIndex = 10;
            // 
            // btn_connection
            // 
            this.btn_connection.Location = new System.Drawing.Point(320, 6);
            this.btn_connection.Name = "btn_connection";
            this.btn_connection.Size = new System.Drawing.Size(75, 23);
            this.btn_connection.TabIndex = 9;
            this.btn_connection.Text = "Connection";
            this.btn_connection.UseVisualStyleBackColor = true;
            this.btn_connection.Click += new System.EventHandler(this.btn_connection_Click);
            // 
            // lbl_portSerie
            // 
            this.lbl_portSerie.AutoSize = true;
            this.lbl_portSerie.Location = new System.Drawing.Point(6, 10);
            this.lbl_portSerie.Name = "lbl_portSerie";
            this.lbl_portSerie.Size = new System.Drawing.Size(59, 13);
            this.lbl_portSerie.TabIndex = 11;
            this.lbl_portSerie.Text = "Port Série :";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.RTB_log);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1270, 706);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Log";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // RTB_log
            // 
            this.RTB_log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RTB_log.Location = new System.Drawing.Point(3, 3);
            this.RTB_log.Name = "RTB_log";
            this.RTB_log.Size = new System.Drawing.Size(1264, 700);
            this.RTB_log.TabIndex = 0;
            this.RTB_log.Text = "";
            // 
            // btn_test
            // 
            this.btn_test.Location = new System.Drawing.Point(583, 6);
            this.btn_test.Name = "btn_test";
            this.btn_test.Size = new System.Drawing.Size(75, 23);
            this.btn_test.TabIndex = 16;
            this.btn_test.Text = "TEST";
            this.btn_test.UseVisualStyleBackColor = true;
            this.btn_test.Click += new System.EventHandler(this.btn_test_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.pictureBox1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1270, 706);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1264, 700);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 18;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_Click);
            // 
            // HomeIA
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1278, 732);
            this.Controls.Add(this.tabControl1);
            this.Name = "HomeIA";
            this.Text = "Robot Color IA";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader _id;
        private System.Windows.Forms.CheckBox CB_Xbee;
        private System.Windows.Forms.Button btn_ActualiserListePortSerie;
        private System.Windows.Forms.ComboBox ctlListePorts;
        private System.Windows.Forms.Button btn_connection;
        private System.Windows.Forms.Label lbl_portSerie;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ColumnHeader _connected;
        private System.Windows.Forms.ColumnHeader _etatComm;
        private System.Windows.Forms.ColumnHeader _etatRobot;
        private System.Windows.Forms.ColumnHeader _posX;
        private System.Windows.Forms.ColumnHeader _posY;
        private System.Windows.Forms.RichTextBox RTB_log;
        private System.Windows.Forms.Button btn_test;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.PictureBox pictureBox1;

    }
}

