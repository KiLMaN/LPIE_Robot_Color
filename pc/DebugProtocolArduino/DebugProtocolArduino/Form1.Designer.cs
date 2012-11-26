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
            this.button1 = new System.Windows.Forms.Button();
            this.Tabs.SuspendLayout();
            this.TabControle.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.TabLogs.SuspendLayout();
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
            this.Tabs.Size = new System.Drawing.Size(560, 407);
            this.Tabs.TabIndex = 0;
            // 
            // TabControle
            // 
            this.TabControle.Controls.Add(this.groupBox1);
            this.TabControle.Location = new System.Drawing.Point(4, 22);
            this.TabControle.Name = "TabControle";
            this.TabControle.Padding = new System.Windows.Forms.Padding(3);
            this.TabControle.Size = new System.Drawing.Size(552, 381);
            this.TabControle.TabIndex = 0;
            this.TabControle.Text = "Controle";
            this.TabControle.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
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
            this.TabLogs.Size = new System.Drawing.Size(552, 381);
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
            this.txtbox_debug_log.Size = new System.Drawing.Size(546, 375);
            this.txtbox_debug_log.TabIndex = 0;
            this.txtbox_debug_log.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(241, 66);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 407);
            this.Controls.Add(this.Tabs);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Tabs.ResumeLayout(false);
            this.TabControle.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.TabLogs.ResumeLayout(false);
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
    }
}

