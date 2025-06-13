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
            btnGoToEnd = new Button();
            lblAmudBeis = new Label();
            lblAmudAleph = new Label();
            btnUseNextSet = new Button();
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
            txtTimestamps.Click += TxtTimestamps_Click;
            txtTimestamps.KeyDown += TxtTimestamps_KeyDown;
            txtTimestamps.KeyUp += TxtTimestamps_KeyUp;
            // 
            // btnGenerate
            // 
            btnGenerate.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnGenerate.Location = new Point(10, 400);
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
            txtLastDaf.TabIndex = 4;
            txtLastDaf.TextChanged += TxtLastDaf_TextChanged;
            // 
            // btnClearTimes
            // 
            btnClearTimes.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnClearTimes.Location = new Point(99, 400);
            btnClearTimes.Margin = new Padding(3, 2, 3, 2);
            btnClearTimes.Name = "btnClearTimes";
            btnClearTimes.Size = new Size(82, 22);
            btnClearTimes.TabIndex = 2;
            btnClearTimes.Text = "&Clear Times";
            btnClearTimes.UseVisualStyleBackColor = true;
            btnClearTimes.Click += BtnClearTimes_Click;
            // 
            // btnFindMissingTimes
            // 
            btnFindMissingTimes.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnFindMissingTimes.Location = new Point(10, 426);
            btnFindMissingTimes.Margin = new Padding(3, 2, 3, 2);
            btnFindMissingTimes.Name = "btnFindMissingTimes";
            btnFindMissingTimes.Size = new Size(170, 22);
            btnFindMissingTimes.TabIndex = 5;
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
            btnPrefill.TabIndex = 6;
            btnPrefill.Text = "&Prefill";
            btnPrefill.UseVisualStyleBackColor = true;
            btnPrefill.Click += BtnPrefill_Click;
            // 
            // btnGoToEnd
            // 
            btnGoToEnd.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnGoToEnd.Location = new Point(187, 400);
            btnGoToEnd.Margin = new Padding(3, 2, 3, 2);
            btnGoToEnd.Name = "btnGoToEnd";
            btnGoToEnd.Size = new Size(141, 22);
            btnGoToEnd.TabIndex = 3;
            btnGoToEnd.Text = "G&o To 0:00:00";
            btnGoToEnd.UseVisualStyleBackColor = true;
            btnGoToEnd.Click += BtnGoToEnd_Click;
            // 
            // lblAmudBeis
            // 
            lblAmudBeis.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblAmudBeis.AutoSize = true;
            lblAmudBeis.Location = new Point(10, 454);
            lblAmudBeis.Name = "lblAmudBeis";
            lblAmudBeis.Size = new Size(95, 15);
            lblAmudBeis.TabIndex = 7;
            lblAmudBeis.Text = "Amud Beis (Left)";
            // 
            // lblAmudAleph
            // 
            lblAmudAleph.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblAmudAleph.AutoSize = true;
            lblAmudAleph.Location = new Point(350, 454);
            lblAmudAleph.Name = "lblAmudAleph";
            lblAmudAleph.Size = new Size(113, 15);
            lblAmudAleph.TabIndex = 8;
            lblAmudAleph.Text = "Amud Aleph (Right)";
            // 
            // btnUseNextSet
            // 
            btnUseNextSet.Location = new Point(303, 429);
            btnUseNextSet.Name = "btnUseNextSet";
            btnUseNextSet.Size = new Size(82, 23);
            btnUseNextSet.TabIndex = 9;
            btnUseNextSet.Text = "Use &Next Set";
            btnUseNextSet.UseVisualStyleBackColor = true;
            btnUseNextSet.Click += BtnUseNextSet_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(484, 476);
            Controls.Add(btnUseNextSet);
            Controls.Add(lblAmudAleph);
            Controls.Add(lblAmudBeis);
            Controls.Add(btnGoToEnd);
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
        private Button btnGoToEnd;
        private Label lblAmudBeis;
        private Label lblAmudAleph;
        private Button btnUseNextSet;
    }
}
