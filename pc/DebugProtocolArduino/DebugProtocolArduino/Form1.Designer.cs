namespace DebugProtocolArduino
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.gPortSerie = new System.IO.Ports.SerialPort(this.components);
            this.Tabs = new System.Windows.Forms.TabControl();
            this.TabControle = new System.Windows.Forms.TabPage();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_ActualiserListePortSerie = new System.Windows.Forms.Button();
            this.liste_portSerie = new System.Windows.Forms.ComboBox();
            this.lbl_IdDest = new System.Windows.Forms.Label();
            this.txt_idSrc = new System.Windows.Forms.TextBox();
            this.btn_connection = new System.Windows.Forms.Button();
            this.txt_idDst = new System.Windows.Forms.TextBox();
            this.lbl_IdSrc = new System.Windows.Forms.Label();
            this.lbl_portSerie = new System.Windows.Forms.Label();
            this.TabLogs = new System.Windows.Forms.TabPage();
            this.txtbox_debug_log = new System.Windows.Forms.RichTextBox();
            this.btn_up = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_left = new System.Windows.Forms.Button();
            this.btn_down = new System.Windows.Forms.Button();
            this.btn_right = new System.Windows.Forms.Button();
            this.btn_pince_haut = new System.Windows.Forms.Button();
            this.btn_pince_bas = new System.Windows.Forms.Button();
            this.btn_pince_open = new System.Windows.Forms.Button();
            this.btn_pince_close = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_IR_Sensor = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lbl_UltraSon = new System.Windows.Forms.Label();
            this.proBar_IrSensor = new System.Windows.Forms.ProgressBar();
            this.proBar_UltraSon = new System.Windows.Forms.ProgressBar();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.pictBoxEtatConn = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Tabs.SuspendLayout();
            this.TabControle.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.TabLogs.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxEtatConn)).BeginInit();
            this.SuspendLayout();
            // 
            // gPortSerie
            // 
            this.gPortSerie.BaudRate = 115200;
            // 
            // Tabs
            // 
            this.Tabs.Controls.Add(this.TabControle);
            this.Tabs.Controls.Add(this.TabLogs);
            this.Tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Tabs.Location = new System.Drawing.Point(0, 0);
            this.Tabs.Name = "Tabs";
            this.Tabs.SelectedIndex = 0;
            this.Tabs.Size = new System.Drawing.Size(560, 330);
            this.Tabs.TabIndex = 0;
            // 
            // TabControle
            // 
            this.TabControle.Controls.Add(this.groupBox4);
            this.TabControle.Controls.Add(this.groupBox3);
            this.TabControle.Controls.Add(this.groupBox2);
            this.TabControle.Controls.Add(this.button2);
            this.TabControle.Controls.Add(this.button1);
            this.TabControle.Controls.Add(this.groupBox1);
            this.TabControle.Location = new System.Drawing.Point(4, 22);
            this.TabControle.Name = "TabControle";
            this.TabControle.Padding = new System.Windows.Forms.Padding(3);
            this.TabControle.Size = new System.Drawing.Size(552, 304);
            this.TabControle.TabIndex = 0;
            this.TabControle.Text = "Controle";
            this.TabControle.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(424, 35);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(424, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_ActualiserListePortSerie);
            this.groupBox1.Controls.Add(this.liste_portSerie);
            this.groupBox1.Controls.Add(this.lbl_IdDest);
            this.groupBox1.Controls.Add(this.txt_idSrc);
            this.groupBox1.Controls.Add(this.btn_connection);
            this.groupBox1.Controls.Add(this.txt_idDst);
            this.groupBox1.Controls.Add(this.lbl_IdSrc);
            this.groupBox1.Controls.Add(this.lbl_portSerie);
            this.groupBox1.Location = new System.Drawing.Point(8, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(326, 109);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Configuration";
            // 
            // btn_ActualiserListePortSerie
            // 
            this.btn_ActualiserListePortSerie.BackgroundImage = global::DebugProtocolArduino.Properties.Resources.actualiser_discret;
            this.btn_ActualiserListePortSerie.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_ActualiserListePortSerie.Location = new System.Drawing.Point(66, 18);
            this.btn_ActualiserListePortSerie.Name = "btn_ActualiserListePortSerie";
            this.btn_ActualiserListePortSerie.Size = new System.Drawing.Size(29, 23);
            this.btn_ActualiserListePortSerie.TabIndex = 7;
            this.btn_ActualiserListePortSerie.UseVisualStyleBackColor = true;
            this.btn_ActualiserListePortSerie.Click += new System.EventHandler(this.btn_ActualiserListePortSerie_Click);
            // 
            // liste_portSerie
            // 
            this.liste_portSerie.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.liste_portSerie.FormattingEnabled = true;
            this.liste_portSerie.Items.AddRange(new object[] {
            "aze",
            "zae",
            "rfdqs"});
            this.liste_portSerie.Location = new System.Drawing.Point(101, 19);
            this.liste_portSerie.Name = "liste_portSerie";
            this.liste_portSerie.Size = new System.Drawing.Size(134, 21);
            this.liste_portSerie.TabIndex = 1;
            this.liste_portSerie.SelectedIndexChanged += new System.EventHandler(this.liste_portSerie_SelectedIndexChanged);
            // 
            // lbl_IdDest
            // 
            this.lbl_IdDest.AutoSize = true;
            this.lbl_IdDest.Location = new System.Drawing.Point(13, 76);
            this.lbl_IdDest.Name = "lbl_IdDest";
            this.lbl_IdDest.Size = new System.Drawing.Size(47, 13);
            this.lbl_IdDest.TabIndex = 6;
            this.lbl_IdDest.Text = "ID dest :";
            // 
            // txt_idSrc
            // 
            this.txt_idSrc.Location = new System.Drawing.Point(66, 47);
            this.txt_idSrc.Name = "txt_idSrc";
            this.txt_idSrc.Size = new System.Drawing.Size(100, 20);
            this.txt_idSrc.TabIndex = 3;
            this.txt_idSrc.Text = "254";
            // 
            // btn_connection
            // 
            this.btn_connection.Location = new System.Drawing.Point(241, 18);
            this.btn_connection.Name = "btn_connection";
            this.btn_connection.Size = new System.Drawing.Size(75, 23);
            this.btn_connection.TabIndex = 0;
            this.btn_connection.Text = "Connection";
            this.btn_connection.UseVisualStyleBackColor = true;
            this.btn_connection.Click += new System.EventHandler(this.btn_connection_Click);
            // 
            // txt_idDst
            // 
            this.txt_idDst.Location = new System.Drawing.Point(66, 73);
            this.txt_idDst.Name = "txt_idDst";
            this.txt_idDst.Size = new System.Drawing.Size(100, 20);
            this.txt_idDst.TabIndex = 4;
            this.txt_idDst.Text = "255";
            // 
            // lbl_IdSrc
            // 
            this.lbl_IdSrc.AutoSize = true;
            this.lbl_IdSrc.Location = new System.Drawing.Point(19, 50);
            this.lbl_IdSrc.Name = "lbl_IdSrc";
            this.lbl_IdSrc.Size = new System.Drawing.Size(41, 13);
            this.lbl_IdSrc.TabIndex = 5;
            this.lbl_IdSrc.Text = "ID src :";
            // 
            // lbl_portSerie
            // 
            this.lbl_portSerie.AutoSize = true;
            this.lbl_portSerie.Location = new System.Drawing.Point(1, 23);
            this.lbl_portSerie.Name = "lbl_portSerie";
            this.lbl_portSerie.Size = new System.Drawing.Size(59, 13);
            this.lbl_portSerie.TabIndex = 2;
            this.lbl_portSerie.Text = "Port Série :";
            // 
            // TabLogs
            // 
            this.TabLogs.Controls.Add(this.txtbox_debug_log);
            this.TabLogs.Location = new System.Drawing.Point(4, 22);
            this.TabLogs.Name = "TabLogs";
            this.TabLogs.Padding = new System.Windows.Forms.Padding(3);
            this.TabLogs.Size = new System.Drawing.Size(552, 304);
            this.TabLogs.TabIndex = 1;
            this.TabLogs.Text = "Logs";
            this.TabLogs.UseVisualStyleBackColor = true;
            // 
            // txtbox_debug_log
            // 
            this.txtbox_debug_log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtbox_debug_log.Location = new System.Drawing.Point(3, 3);
            this.txtbox_debug_log.Name = "txtbox_debug_log";
            this.txtbox_debug_log.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.txtbox_debug_log.Size = new System.Drawing.Size(546, 298);
            this.txtbox_debug_log.TabIndex = 0;
            this.txtbox_debug_log.Text = "";
            // 
            // btn_up
            // 
            this.btn_up.Location = new System.Drawing.Point(123, 29);
            this.btn_up.Name = "btn_up";
            this.btn_up.Size = new System.Drawing.Size(75, 23);
            this.btn_up.TabIndex = 10;
            this.btn_up.Text = "^";
            this.btn_up.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btn_pince_open);
            this.groupBox2.Controls.Add(this.btn_pince_close);
            this.groupBox2.Controls.Add(this.btn_pince_bas);
            this.groupBox2.Controls.Add(this.btn_pince_haut);
            this.groupBox2.Controls.Add(this.btn_right);
            this.groupBox2.Controls.Add(this.btn_down);
            this.groupBox2.Controls.Add(this.btn_left);
            this.groupBox2.Controls.Add(this.btn_up);
            this.groupBox2.Location = new System.Drawing.Point(8, 131);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(326, 160);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Mouvement";
            // 
            // btn_left
            // 
            this.btn_left.Location = new System.Drawing.Point(42, 29);
            this.btn_left.Name = "btn_left";
            this.btn_left.Size = new System.Drawing.Size(75, 23);
            this.btn_left.TabIndex = 11;
            this.btn_left.Text = "<=";
            this.btn_left.UseVisualStyleBackColor = true;
            this.btn_left.Click += new System.EventHandler(this.btn_left_Click);
            // 
            // btn_down
            // 
            this.btn_down.Location = new System.Drawing.Point(123, 58);
            this.btn_down.Name = "btn_down";
            this.btn_down.Size = new System.Drawing.Size(75, 23);
            this.btn_down.TabIndex = 12;
            this.btn_down.Text = "v";
            this.btn_down.UseVisualStyleBackColor = true;
            // 
            // btn_right
            // 
            this.btn_right.Location = new System.Drawing.Point(204, 29);
            this.btn_right.Name = "btn_right";
            this.btn_right.Size = new System.Drawing.Size(75, 23);
            this.btn_right.TabIndex = 13;
            this.btn_right.Text = "=>";
            this.btn_right.UseVisualStyleBackColor = true;
            // 
            // btn_pince_haut
            // 
            this.btn_pince_haut.Location = new System.Drawing.Point(42, 90);
            this.btn_pince_haut.Name = "btn_pince_haut";
            this.btn_pince_haut.Size = new System.Drawing.Size(75, 23);
            this.btn_pince_haut.TabIndex = 14;
            this.btn_pince_haut.Text = "Pince Haut";
            this.btn_pince_haut.UseVisualStyleBackColor = true;
            // 
            // btn_pince_bas
            // 
            this.btn_pince_bas.Location = new System.Drawing.Point(42, 119);
            this.btn_pince_bas.Name = "btn_pince_bas";
            this.btn_pince_bas.Size = new System.Drawing.Size(75, 23);
            this.btn_pince_bas.TabIndex = 15;
            this.btn_pince_bas.Text = "Pince Bas";
            this.btn_pince_bas.UseVisualStyleBackColor = true;
            // 
            // btn_pince_open
            // 
            this.btn_pince_open.Location = new System.Drawing.Point(204, 119);
            this.btn_pince_open.Name = "btn_pince_open";
            this.btn_pince_open.Size = new System.Drawing.Size(75, 23);
            this.btn_pince_open.TabIndex = 17;
            this.btn_pince_open.Text = "Ouvre Pince";
            this.btn_pince_open.UseVisualStyleBackColor = true;
            // 
            // btn_pince_close
            // 
            this.btn_pince_close.Location = new System.Drawing.Point(204, 90);
            this.btn_pince_close.Name = "btn_pince_close";
            this.btn_pince_close.Size = new System.Drawing.Size(75, 23);
            this.btn_pince_close.TabIndex = 16;
            this.btn_pince_close.Text = "Ferme Pince";
            this.btn_pince_close.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.proBar_UltraSon);
            this.groupBox3.Controls.Add(this.proBar_IrSensor);
            this.groupBox3.Controls.Add(this.lbl_UltraSon);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.lbl_IR_Sensor);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Location = new System.Drawing.Point(340, 131);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(204, 81);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Capteurs";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "IR Sensor :";
            // 
            // lbl_IR_Sensor
            // 
            this.lbl_IR_Sensor.AutoSize = true;
            this.lbl_IR_Sensor.Location = new System.Drawing.Point(81, 29);
            this.lbl_IR_Sensor.Name = "lbl_IR_Sensor";
            this.lbl_IR_Sensor.Size = new System.Drawing.Size(13, 13);
            this.lbl_IR_Sensor.TabIndex = 1;
            this.lbl_IR_Sensor.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Ultra Son:";
            // 
            // lbl_UltraSon
            // 
            this.lbl_UltraSon.AutoSize = true;
            this.lbl_UltraSon.Location = new System.Drawing.Point(81, 58);
            this.lbl_UltraSon.Name = "lbl_UltraSon";
            this.lbl_UltraSon.Size = new System.Drawing.Size(13, 13);
            this.lbl_UltraSon.TabIndex = 3;
            this.lbl_UltraSon.Text = "0";
            // 
            // proBar_IrSensor
            // 
            this.proBar_IrSensor.Location = new System.Drawing.Point(100, 26);
            this.proBar_IrSensor.Name = "proBar_IrSensor";
            this.proBar_IrSensor.Size = new System.Drawing.Size(98, 18);
            this.proBar_IrSensor.TabIndex = 4;
            // 
            // proBar_UltraSon
            // 
            this.proBar_UltraSon.Location = new System.Drawing.Point(100, 55);
            this.proBar_UltraSon.Name = "proBar_UltraSon";
            this.proBar_UltraSon.Size = new System.Drawing.Size(98, 18);
            this.proBar_UltraSon.TabIndex = 5;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.pictBoxEtatConn);
            this.groupBox4.Location = new System.Drawing.Point(340, 218);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(204, 73);
            this.groupBox4.TabIndex = 13;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Etat Robot";
            // 
            // pictBoxEtatConn
            // 
            this.pictBoxEtatConn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictBoxEtatConn.Location = new System.Drawing.Point(98, 13);
            this.pictBoxEtatConn.Name = "pictBoxEtatConn";
            this.pictBoxEtatConn.Size = new System.Drawing.Size(100, 50);
            this.pictBoxEtatConn.TabIndex = 0;
            this.pictBoxEtatConn.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Etat Connection :";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 330);
            this.Controls.Add(this.Tabs);
            this.Name = "Form1";
            this.Text = "Debug Arduino Bots";
            this.Tabs.ResumeLayout(false);
            this.TabControle.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.TabLogs.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictBoxEtatConn)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.IO.Ports.SerialPort gPortSerie;
        private System.Windows.Forms.TabControl Tabs;
        private System.Windows.Forms.TabPage TabControle;
        private System.Windows.Forms.TabPage TabLogs;
        private System.Windows.Forms.RichTextBox txtbox_debug_log;
        private System.Windows.Forms.ComboBox liste_portSerie;
        private System.Windows.Forms.Button btn_connection;
        private System.Windows.Forms.Label lbl_portSerie;
        private System.Windows.Forms.Label lbl_IdDest;
        private System.Windows.Forms.Label lbl_IdSrc;
        private System.Windows.Forms.TextBox txt_idDst;
        private System.Windows.Forms.TextBox txt_idSrc;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_ActualiserListePortSerie;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_right;
        private System.Windows.Forms.Button btn_down;
        private System.Windows.Forms.Button btn_left;
        private System.Windows.Forms.Button btn_up;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ProgressBar proBar_UltraSon;
        private System.Windows.Forms.ProgressBar proBar_IrSensor;
        private System.Windows.Forms.Label lbl_UltraSon;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbl_IR_Sensor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_pince_open;
        private System.Windows.Forms.Button btn_pince_close;
        private System.Windows.Forms.Button btn_pince_bas;
        private System.Windows.Forms.Button btn_pince_haut;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictBoxEtatConn;
    }
}

