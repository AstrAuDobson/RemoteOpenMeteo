namespace ASCOM.RemoteOpenMeteo
{
    partial class SetupDialogForm
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
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.labelTitre = new System.Windows.Forms.Label();
            this.labelPortSerie = new System.Windows.Forms.Label();
            this.chkTrace = new System.Windows.Forms.CheckBox();
            this.comboBoxComPort = new System.Windows.Forms.ComboBox();
            this.labelVersion = new System.Windows.Forms.Label();
            this.labelAuthor = new System.Windows.Forms.Label();
            this.groupBoxParametres = new System.Windows.Forms.GroupBox();
            this.pictureBoxROM = new System.Windows.Forms.PictureBox();
            this.groupBoxParametres.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxROM)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(181, 194);
            this.cmdOK.Margin = new System.Windows.Forms.Padding(4);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(79, 30);
            this.cmdOK.TabIndex = 1;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(268, 194);
            this.cmdCancel.Margin = new System.Windows.Forms.Padding(4);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(79, 31);
            this.cmdCancel.TabIndex = 2;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // labelTitre
            // 
            this.labelTitre.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitre.Location = new System.Drawing.Point(13, 9);
            this.labelTitre.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTitre.Name = "labelTitre";
            this.labelTitre.Size = new System.Drawing.Size(353, 26);
            this.labelTitre.TabIndex = 2;
            this.labelTitre.Text = "Remote Open Meteo ASCOM Driver";
            // 
            // labelPortSerie
            // 
            this.labelPortSerie.AutoSize = true;
            this.labelPortSerie.Location = new System.Drawing.Point(89, 32);
            this.labelPortSerie.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPortSerie.Name = "labelPortSerie";
            this.labelPortSerie.Size = new System.Drawing.Size(64, 16);
            this.labelPortSerie.TabIndex = 5;
            this.labelPortSerie.Text = "Port série";
            // 
            // chkTrace
            // 
            this.chkTrace.AutoSize = true;
            this.chkTrace.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkTrace.Location = new System.Drawing.Point(45, 62);
            this.chkTrace.Margin = new System.Windows.Forms.Padding(4);
            this.chkTrace.Name = "chkTrace";
            this.chkTrace.Size = new System.Drawing.Size(136, 20);
            this.chkTrace.TabIndex = 1;
            this.chkTrace.Text = "Mode trace activé";
            this.chkTrace.UseVisualStyleBackColor = true;
            // 
            // comboBoxComPort
            // 
            this.comboBoxComPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxComPort.FormattingEnabled = true;
            this.comboBoxComPort.Location = new System.Drawing.Point(161, 30);
            this.comboBoxComPort.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxComPort.Name = "comboBoxComPort";
            this.comboBoxComPort.Size = new System.Drawing.Size(161, 24);
            this.comboBoxComPort.TabIndex = 0;
            // 
            // labelVersion
            // 
            this.labelVersion.Location = new System.Drawing.Point(13, 34);
            this.labelVersion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(309, 18);
            this.labelVersion.TabIndex = 8;
            this.labelVersion.Text = "Version BETA 6.6.0.1";
            // 
            // labelAuthor
            // 
            this.labelAuthor.Location = new System.Drawing.Point(13, 54);
            this.labelAuthor.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelAuthor.Name = "labelAuthor";
            this.labelAuthor.Size = new System.Drawing.Size(309, 18);
            this.labelAuthor.TabIndex = 9;
            this.labelAuthor.Text = "Le Zav\' et Juanito del Pepito";
            // 
            // groupBoxParametres
            // 
            this.groupBoxParametres.Controls.Add(this.comboBoxComPort);
            this.groupBoxParametres.Controls.Add(this.labelPortSerie);
            this.groupBoxParametres.Controls.Add(this.chkTrace);
            this.groupBoxParametres.Location = new System.Drawing.Point(14, 88);
            this.groupBoxParametres.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxParametres.Name = "groupBoxParametres";
            this.groupBoxParametres.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBoxParametres.Size = new System.Drawing.Size(333, 100);
            this.groupBoxParametres.TabIndex = 0;
            this.groupBoxParametres.TabStop = false;
            this.groupBoxParametres.Text = "Paramètres du périphérique ROM";
            // 
            // pictureBoxROM
            // 
            this.pictureBoxROM.Image = global::ASCOM.RemoteOpenMeteo.Properties.Resources.ROM;
            this.pictureBoxROM.Location = new System.Drawing.Point(353, 36);
            this.pictureBoxROM.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBoxROM.Name = "pictureBoxROM";
            this.pictureBoxROM.Size = new System.Drawing.Size(160, 148);
            this.pictureBoxROM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxROM.TabIndex = 11;
            this.pictureBoxROM.TabStop = false;
            // 
            // SetupDialogForm
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(525, 238);
            this.Controls.Add(this.pictureBoxROM);
            this.Controls.Add(this.groupBoxParametres);
            this.Controls.Add(this.labelAuthor);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.labelTitre);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupDialogForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Remote Open Meteo";
            this.groupBoxParametres.ResumeLayout(false);
            this.groupBoxParametres.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxROM)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Label labelTitre;
        private System.Windows.Forms.Label labelPortSerie;
        private System.Windows.Forms.CheckBox chkTrace;
        private System.Windows.Forms.ComboBox comboBoxComPort;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.Label labelAuthor;
        private System.Windows.Forms.GroupBox groupBoxParametres;
        private System.Windows.Forms.PictureBox pictureBoxROM;
    }
}