namespace Sheva_Daf_YouTube_Timestamper
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            txtTimestamps = new TextBox();
            btnGenerate = new Button();
            txtLastDaf = new TextBox();
            btnClearTimes = new Button();
            btnFindMissingTimes = new Button();
            btnPrefill = new Button();
            SuspendLayout();
            // 
            // txtTimestamps
            // 
            txtTimestamps.Location = new Point(12, 12);
            txtTimestamps.MaxLength = 0;
            txtTimestamps.Multiline = true;
            txtTimestamps.Name = "txtTimestamps";
            txtTimestamps.ScrollBars = ScrollBars.Vertical;
            txtTimestamps.Size = new Size(458, 368);
            txtTimestamps.TabIndex = 0;
            // 
            // btnGenerate
            // 
            btnGenerate.Location = new Point(12, 386);
            btnGenerate.Name = "btnGenerate";
            btnGenerate.Size = new Size(94, 29);
            btnGenerate.TabIndex = 1;
            btnGenerate.Text = "&Generate";
            btnGenerate.UseVisualStyleBackColor = true;
            btnGenerate.Click += BtnGenerate_Click;
            // 
            // txtLastDaf
            // 
            txtLastDaf.Location = new Point(312, 388);
            txtLastDaf.Name = "txtLastDaf";
            txtLastDaf.PlaceholderText = "&Last Daf of prior set";
            txtLastDaf.RightToLeft = RightToLeft.Yes;
            txtLastDaf.Size = new Size(158, 27);
            txtLastDaf.TabIndex = 2;
            // 
            // btnClearTimes
            // 
            btnClearTimes.Location = new Point(112, 386);
            btnClearTimes.Name = "btnClearTimes";
            btnClearTimes.Size = new Size(94, 29);
            btnClearTimes.TabIndex = 3;
            btnClearTimes.Text = "&Clear Times";
            btnClearTimes.UseVisualStyleBackColor = true;
            btnClearTimes.Click += BtnClearTimes_Click;
            // 
            // btnFindMissingTimes
            // 
            btnFindMissingTimes.Location = new Point(12, 421);
            btnFindMissingTimes.Name = "btnFindMissingTimes";
            btnFindMissingTimes.Size = new Size(194, 29);
            btnFindMissingTimes.TabIndex = 4;
            btnFindMissingTimes.Text = "&Find Missing Times";
            btnFindMissingTimes.UseVisualStyleBackColor = true;
            btnFindMissingTimes.Click += BtnFindMissingTimes_Click;
            // 
            // btnPrefill
            // 
            btnPrefill.Location = new Point(376, 421);
            btnPrefill.Name = "btnPrefill";
            btnPrefill.Size = new Size(94, 29);
            btnPrefill.TabIndex = 5;
            btnPrefill.Text = "&Prefill";
            btnPrefill.UseVisualStyleBackColor = true;
            btnPrefill.Click += BtnPrefill_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(482, 453);
            Controls.Add(btnPrefill);
            Controls.Add(btnFindMissingTimes);
            Controls.Add(btnClearTimes);
            Controls.Add(txtLastDaf);
            Controls.Add(btnGenerate);
            Controls.Add(txtTimestamps);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Sheva Daf YouTube Timestamper";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtTimestamps;
        private Button btnGenerate;
        private TextBox txtLastDaf;
        private Button btnClearTimes;
        private Button btnFindMissingTimes;
        private Button btnPrefill;
    }
}
