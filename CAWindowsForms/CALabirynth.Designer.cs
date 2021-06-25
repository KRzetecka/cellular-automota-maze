namespace CAWindowsForms
{
    partial class CALabirynth
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.MainPanel = new System.Windows.Forms.Panel();
            this.DataPanel = new System.Windows.Forms.Panel();
            this.ConsoleBox = new System.Windows.Forms.RichTextBox();
            this.ViewPanel = new System.Windows.Forms.Panel();
            this.showLabBox = new System.Windows.Forms.PictureBox();
            this.SettingsPanel = new System.Windows.Forms.Panel();
            this.chkPunishment = new System.Windows.Forms.CheckBox();
            this.btnDefault = new System.Windows.Forms.Button();
            this.btnValue = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.sclCell = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblLabirynthPick = new System.Windows.Forms.Label();
            this.sclLabirynth = new System.Windows.Forms.ComboBox();
            this.btnNext = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.chkShowValues = new System.Windows.Forms.CheckBox();
            this.MainPanel.SuspendLayout();
            this.DataPanel.SuspendLayout();
            this.ViewPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.showLabBox)).BeginInit();
            this.SettingsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainPanel
            // 
            this.MainPanel.Controls.Add(this.DataPanel);
            this.MainPanel.Controls.Add(this.ViewPanel);
            this.MainPanel.Controls.Add(this.SettingsPanel);
            this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainPanel.Location = new System.Drawing.Point(0, 0);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(809, 542);
            this.MainPanel.TabIndex = 0;
            // 
            // DataPanel
            // 
            this.DataPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DataPanel.Controls.Add(this.ConsoleBox);
            this.DataPanel.Location = new System.Drawing.Point(200, 412);
            this.DataPanel.Name = "DataPanel";
            this.DataPanel.Size = new System.Drawing.Size(609, 130);
            this.DataPanel.TabIndex = 2;
            // 
            // ConsoleBox
            // 
            this.ConsoleBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ConsoleBox.Location = new System.Drawing.Point(0, 0);
            this.ConsoleBox.Name = "ConsoleBox";
            this.ConsoleBox.ReadOnly = true;
            this.ConsoleBox.Size = new System.Drawing.Size(609, 130);
            this.ConsoleBox.TabIndex = 0;
            this.ConsoleBox.Text = "";
            // 
            // ViewPanel
            // 
            this.ViewPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ViewPanel.Controls.Add(this.showLabBox);
            this.ViewPanel.Location = new System.Drawing.Point(200, 0);
            this.ViewPanel.Name = "ViewPanel";
            this.ViewPanel.Size = new System.Drawing.Size(609, 412);
            this.ViewPanel.TabIndex = 1;
            // 
            // showLabBox
            // 
            this.showLabBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.showLabBox.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.showLabBox.Location = new System.Drawing.Point(6, 12);
            this.showLabBox.Name = "showLabBox";
            this.showLabBox.Size = new System.Drawing.Size(600, 394);
            this.showLabBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.showLabBox.TabIndex = 0;
            this.showLabBox.TabStop = false;
            // 
            // SettingsPanel
            // 
            this.SettingsPanel.Controls.Add(this.chkShowValues);
            this.SettingsPanel.Controls.Add(this.chkPunishment);
            this.SettingsPanel.Controls.Add(this.btnDefault);
            this.SettingsPanel.Controls.Add(this.btnValue);
            this.SettingsPanel.Controls.Add(this.btnRefresh);
            this.SettingsPanel.Controls.Add(this.btnLoad);
            this.SettingsPanel.Controls.Add(this.label4);
            this.SettingsPanel.Controls.Add(this.txtValue);
            this.SettingsPanel.Controls.Add(this.sclCell);
            this.SettingsPanel.Controls.Add(this.label3);
            this.SettingsPanel.Controls.Add(this.label2);
            this.SettingsPanel.Controls.Add(this.lblLabirynthPick);
            this.SettingsPanel.Controls.Add(this.sclLabirynth);
            this.SettingsPanel.Controls.Add(this.btnNext);
            this.SettingsPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.SettingsPanel.Location = new System.Drawing.Point(0, 0);
            this.SettingsPanel.Name = "SettingsPanel";
            this.SettingsPanel.Size = new System.Drawing.Size(200, 542);
            this.SettingsPanel.TabIndex = 0;
            // 
            // chkPunishment
            // 
            this.chkPunishment.AutoSize = true;
            this.chkPunishment.Location = new System.Drawing.Point(15, 231);
            this.chkPunishment.Name = "chkPunishment";
            this.chkPunishment.Size = new System.Drawing.Size(133, 17);
            this.chkPunishment.TabIndex = 16;
            this.chkPunishment.Text = "Punish robot for retreat";
            this.chkPunishment.UseVisualStyleBackColor = true;
            // 
            // btnDefault
            // 
            this.btnDefault.Location = new System.Drawing.Point(15, 188);
            this.btnDefault.Name = "btnDefault";
            this.btnDefault.Size = new System.Drawing.Size(88, 23);
            this.btnDefault.TabIndex = 1;
            this.btnDefault.Text = "Back to default";
            this.btnDefault.UseVisualStyleBackColor = true;
            this.btnDefault.Click += new System.EventHandler(this.btnDefault_Click);
            // 
            // btnValue
            // 
            this.btnValue.Location = new System.Drawing.Point(15, 159);
            this.btnValue.Name = "btnValue";
            this.btnValue.Size = new System.Drawing.Size(88, 23);
            this.btnValue.TabIndex = 15;
            this.btnValue.Text = "Change value";
            this.btnValue.UseVisualStyleBackColor = true;
            this.btnValue.Click += new System.EventHandler(this.btnValue_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(15, 51);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(88, 23);
            this.btnRefresh.TabIndex = 14;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(142, 26);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(54, 23);
            this.btnLoad.TabIndex = 13;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.BtnLoad_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 214);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(184, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "-----------------------------------------------------------";
            // 
            // txtValue
            // 
            this.txtValue.Location = new System.Drawing.Point(15, 133);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(100, 20);
            this.txtValue.TabIndex = 11;
            // 
            // sclCell
            // 
            this.sclCell.FormattingEnabled = true;
            this.sclCell.Location = new System.Drawing.Point(15, 106);
            this.sclCell.Name = "sclCell";
            this.sclCell.Size = new System.Drawing.Size(121, 21);
            this.sclCell.TabIndex = 10;
            this.sclCell.SelectedIndexChanged += new System.EventHandler(this.sclCell_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(114, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Change values of cells";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(184, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "-----------------------------------------------------------";
            // 
            // lblLabirynthPick
            // 
            this.lblLabirynthPick.AutoSize = true;
            this.lblLabirynthPick.Location = new System.Drawing.Point(12, 12);
            this.lblLabirynthPick.Name = "lblLabirynthPick";
            this.lblLabirynthPick.Size = new System.Drawing.Size(91, 13);
            this.lblLabirynthPick.TabIndex = 6;
            this.lblLabirynthPick.Text = "Pick the  labirynth";
            // 
            // sclLabirynth
            // 
            this.sclLabirynth.FormattingEnabled = true;
            this.sclLabirynth.Location = new System.Drawing.Point(15, 28);
            this.sclLabirynth.Name = "sclLabirynth";
            this.sclLabirynth.Size = new System.Drawing.Size(121, 21);
            this.sclLabirynth.TabIndex = 5;
            this.sclLabirynth.SelectedIndexChanged += new System.EventHandler(this.SclBox_SelectedIndexChanged);
            this.sclLabirynth.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.sclLabirynth_Format);
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNext.Location = new System.Drawing.Point(12, 463);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(179, 76);
            this.btnNext.TabIndex = 1;
            this.btnNext.Text = "Next step";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.BtnNext_Click);
            // 
            // chkShowValues
            // 
            this.chkShowValues.AutoSize = true;
            this.chkShowValues.Location = new System.Drawing.Point(15, 254);
            this.chkShowValues.Name = "chkShowValues";
            this.chkShowValues.Size = new System.Drawing.Size(106, 17);
            this.chkShowValues.TabIndex = 17;
            this.chkShowValues.Text = "Show cell values";
            this.chkShowValues.UseVisualStyleBackColor = true;
            // 
            // CALabirynth
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(809, 542);
            this.Controls.Add(this.MainPanel);
            this.MinimumSize = new System.Drawing.Size(816, 489);
            this.Name = "CALabirynth";
            this.Text = "Cellular Automata Labirynth";
            this.Resize += new System.EventHandler(this.CALabirynth_Resize);
            this.MainPanel.ResumeLayout(false);
            this.DataPanel.ResumeLayout(false);
            this.ViewPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.showLabBox)).EndInit();
            this.SettingsPanel.ResumeLayout(false);
            this.SettingsPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel MainPanel;
        private System.Windows.Forms.Panel DataPanel;
        private System.Windows.Forms.RichTextBox ConsoleBox;
        private System.Windows.Forms.Panel ViewPanel;
        private System.Windows.Forms.Panel SettingsPanel;
        private System.Windows.Forms.PictureBox showLabBox;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Label lblLabirynthPick;
        private System.Windows.Forms.ComboBox sclLabirynth;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox sclCell;
        private System.Windows.Forms.TextBox txtValue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnValue;
        private System.Windows.Forms.Button btnDefault;
        private System.Windows.Forms.CheckBox chkPunishment;
        private System.Windows.Forms.CheckBox chkShowValues;
    }
}

