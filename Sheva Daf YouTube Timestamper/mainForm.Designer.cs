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
            txtTimestamps.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtTimestamps.Location = new Point(10, 9);
            txtTimestamps.Margin = new Padding(3, 2, 3, 2);
            txtTimestamps.MaxLength = 0;
            txtTimestamps.Multiline = true;
            txtTimestamps.Name = "txtTimestamps";
            txtTimestamps.ScrollBars = ScrollBars.Vertical;
            txtTimestamps.Size = new Size(463, 388);
            txtTimestamps.TabIndex = 0;
            // 
            // btnGenerate
            // 
            btnGenerate.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnGenerate.Location = new Point(10, 401);
            btnGenerate.Margin = new Padding(3, 2, 3, 2);
            btnGenerate.Name = "btnGenerate";
            btnGenerate.Size = new Size(82, 22);
            btnGenerate.TabIndex = 1;
            btnGenerate.Text = "&Generate";
            btnGenerate.UseVisualStyleBackColor = true;
            btnGenerate.Click += BtnGenerate_Click;
            // 
            // txtLastDaf
            // 
            txtLastDaf.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            txtLastDaf.Location = new Point(334, 402);
            txtLastDaf.Margin = new Padding(3, 2, 3, 2);
            txtLastDaf.Name = "txtLastDaf";
            txtLastDaf.PlaceholderText = "&Last Daf of prior set";
            txtLastDaf.RightToLeft = RightToLeft.Yes;
            txtLastDaf.Size = new Size(139, 23);
            txtLastDaf.TabIndex = 2;
            txtLastDaf.TextChanged += TxtLastDaf_TextChanged;
            // 
            // btnClearTimes
            // 
            btnClearTimes.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnClearTimes.Location = new Point(98, 401);
            btnClearTimes.Margin = new Padding(3, 2, 3, 2);
            btnClearTimes.Name = "btnClearTimes";
            btnClearTimes.Size = new Size(82, 22);
            btnClearTimes.TabIndex = 3;
            btnClearTimes.Text = "&Clear Times";
            btnClearTimes.UseVisualStyleBackColor = true;
            btnClearTimes.Click += BtnClearTimes_Click;
            // 
            // btnFindMissingTimes
            // 
            btnFindMissingTimes.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnFindMissingTimes.Location = new Point(10, 427);
            btnFindMissingTimes.Margin = new Padding(3, 2, 3, 2);
            btnFindMissingTimes.Name = "btnFindMissingTimes";
            btnFindMissingTimes.Size = new Size(170, 22);
            btnFindMissingTimes.TabIndex = 4;
            btnFindMissingTimes.Text = "&Find Missing Times";
            btnFindMissingTimes.UseVisualStyleBackColor = true;
            btnFindMissingTimes.Click += BtnFindMissingTimes_Click;
            // 
            // btnPrefill
            // 
            btnPrefill.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnPrefill.Location = new Point(391, 429);
            btnPrefill.Margin = new Padding(3, 2, 3, 2);
            btnPrefill.Name = "btnPrefill";
            btnPrefill.Size = new Size(82, 22);
            btnPrefill.TabIndex = 5;
            btnPrefill.Text = "&Prefill";
            btnPrefill.UseVisualStyleBackColor = true;
            btnPrefill.Click += BtnPrefill_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(484, 461);
            Controls.Add(btnPrefill);
            Controls.Add(btnFindMissingTimes);
            Controls.Add(btnClearTimes);
            Controls.Add(txtLastDaf);
            Controls.Add(btnGenerate);
            Controls.Add(txtTimestamps);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 2, 3, 2);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Sheva Daf YouTube Timestamper";
            Load += MainForm_Load;
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
