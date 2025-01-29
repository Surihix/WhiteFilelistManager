namespace WhiteFilelistManager
{
    partial class CoreForm
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
            GameSelectGroupBox = new GroupBox();
            FF13LRRadioButton = new RadioButton();
            FF132RadioButton = new RadioButton();
            FF131RadioButton = new RadioButton();
            UnpkAsJSONButton = new Button();
            FilelistToolsGroupBox = new GroupBox();
            RpkTxtButton = new Button();
            RpkJSONButton = new Button();
            UnpkAsTXTButton = new Button();
            PathGenToolsGroupBox = new GroupBox();
            GenerateMoreTXTButton = new Button();
            GenerateMoreJSONButton = new Button();
            VPathTiplabel = new Label();
            ClearOutputButton = new Button();
            GenerateTXTButton = new Button();
            GenerateJSONButton = new Button();
            OutputLabel = new Label();
            VPathLabel = new Label();
            OutputTxtBox = new TextBox();
            VPathTxtBox = new TextBox();
            AppStatusStrip = new StatusStrip();
            AppStatusStripLabel = new ToolStripStatusLabel();
            GameSelectGroupBox.SuspendLayout();
            FilelistToolsGroupBox.SuspendLayout();
            PathGenToolsGroupBox.SuspendLayout();
            AppStatusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // GameSelectGroupBox
            // 
            GameSelectGroupBox.Controls.Add(FF13LRRadioButton);
            GameSelectGroupBox.Controls.Add(FF132RadioButton);
            GameSelectGroupBox.Controls.Add(FF131RadioButton);
            GameSelectGroupBox.Location = new Point(12, 12);
            GameSelectGroupBox.Name = "GameSelectGroupBox";
            GameSelectGroupBox.Size = new Size(189, 52);
            GameSelectGroupBox.TabIndex = 0;
            GameSelectGroupBox.TabStop = false;
            GameSelectGroupBox.Text = "Select Game:";
            // 
            // FF13LRRadioButton
            // 
            FF13LRRadioButton.AutoSize = true;
            FF13LRRadioButton.Location = new Point(122, 18);
            FF13LRRadioButton.Name = "FF13LRRadioButton";
            FF13LRRadioButton.Size = new Size(59, 19);
            FF13LRRadioButton.TabIndex = 2;
            FF13LRRadioButton.Text = "XIII-LR";
            FF13LRRadioButton.UseVisualStyleBackColor = true;
            // 
            // FF132RadioButton
            // 
            FF132RadioButton.AutoSize = true;
            FF132RadioButton.Location = new Point(64, 18);
            FF132RadioButton.Name = "FF132RadioButton";
            FF132RadioButton.Size = new Size(52, 19);
            FF132RadioButton.TabIndex = 1;
            FF132RadioButton.Text = "XIII-2";
            FF132RadioButton.UseVisualStyleBackColor = true;
            // 
            // FF131RadioButton
            // 
            FF131RadioButton.AutoSize = true;
            FF131RadioButton.Checked = true;
            FF131RadioButton.Location = new Point(6, 18);
            FF131RadioButton.Name = "FF131RadioButton";
            FF131RadioButton.Size = new Size(52, 19);
            FF131RadioButton.TabIndex = 0;
            FF131RadioButton.TabStop = true;
            FF131RadioButton.Text = "XIII-1";
            FF131RadioButton.UseVisualStyleBackColor = true;
            // 
            // UnpkAsJSONButton
            // 
            UnpkAsJSONButton.Location = new Point(15, 22);
            UnpkAsJSONButton.Name = "UnpkAsJSONButton";
            UnpkAsJSONButton.Size = new Size(107, 23);
            UnpkAsJSONButton.TabIndex = 0;
            UnpkAsJSONButton.Text = "Unpack as JSON";
            UnpkAsJSONButton.UseVisualStyleBackColor = true;
            UnpkAsJSONButton.Click += UnpkAsJSONButton_Click;
            // 
            // FilelistToolsGroupBox
            // 
            FilelistToolsGroupBox.Controls.Add(RpkTxtButton);
            FilelistToolsGroupBox.Controls.Add(RpkJSONButton);
            FilelistToolsGroupBox.Controls.Add(UnpkAsTXTButton);
            FilelistToolsGroupBox.Controls.Add(UnpkAsJSONButton);
            FilelistToolsGroupBox.Location = new Point(328, 12);
            FilelistToolsGroupBox.Name = "FilelistToolsGroupBox";
            FilelistToolsGroupBox.Size = new Size(261, 90);
            FilelistToolsGroupBox.TabIndex = 1;
            FilelistToolsGroupBox.TabStop = false;
            FilelistToolsGroupBox.Text = "Filelist tools:";
            // 
            // RpkTxtButton
            // 
            RpkTxtButton.Location = new Point(137, 51);
            RpkTxtButton.Name = "RpkTxtButton";
            RpkTxtButton.Size = new Size(107, 23);
            RpkTxtButton.TabIndex = 3;
            RpkTxtButton.Text = "Repack TXTs";
            RpkTxtButton.UseVisualStyleBackColor = true;
            RpkTxtButton.Click += RpkTxtButton_Click;
            // 
            // RpkJSONButton
            // 
            RpkJSONButton.Location = new Point(137, 22);
            RpkJSONButton.Name = "RpkJSONButton";
            RpkJSONButton.Size = new Size(107, 23);
            RpkJSONButton.TabIndex = 1;
            RpkJSONButton.Text = "Repack JSON";
            RpkJSONButton.UseVisualStyleBackColor = true;
            RpkJSONButton.Click += RpkJSONButton_Click;
            // 
            // UnpkAsTXTButton
            // 
            UnpkAsTXTButton.Location = new Point(15, 51);
            UnpkAsTXTButton.Name = "UnpkAsTXTButton";
            UnpkAsTXTButton.Size = new Size(107, 23);
            UnpkAsTXTButton.TabIndex = 2;
            UnpkAsTXTButton.Text = "Unpack as TXTs";
            UnpkAsTXTButton.UseVisualStyleBackColor = true;
            UnpkAsTXTButton.Click += UnpkAsTXTButton_Click;
            // 
            // PathGenToolsGroupBox
            // 
            PathGenToolsGroupBox.Controls.Add(GenerateMoreTXTButton);
            PathGenToolsGroupBox.Controls.Add(GenerateMoreJSONButton);
            PathGenToolsGroupBox.Controls.Add(VPathTiplabel);
            PathGenToolsGroupBox.Controls.Add(ClearOutputButton);
            PathGenToolsGroupBox.Controls.Add(GenerateTXTButton);
            PathGenToolsGroupBox.Controls.Add(GenerateJSONButton);
            PathGenToolsGroupBox.Controls.Add(OutputLabel);
            PathGenToolsGroupBox.Controls.Add(VPathLabel);
            PathGenToolsGroupBox.Controls.Add(OutputTxtBox);
            PathGenToolsGroupBox.Controls.Add(VPathTxtBox);
            PathGenToolsGroupBox.Location = new Point(12, 121);
            PathGenToolsGroupBox.Name = "PathGenToolsGroupBox";
            PathGenToolsGroupBox.Size = new Size(577, 363);
            PathGenToolsGroupBox.TabIndex = 2;
            PathGenToolsGroupBox.TabStop = false;
            PathGenToolsGroupBox.Text = "Path Generator tools:";
            // 
            // GenerateMoreTXTButton
            // 
            GenerateMoreTXTButton.Location = new Point(351, 317);
            GenerateMoreTXTButton.Name = "GenerateMoreTXTButton";
            GenerateMoreTXTButton.Size = new Size(190, 23);
            GenerateMoreTXTButton.TabIndex = 9;
            GenerateMoreTXTButton.Text = "Generate more TXT Paths";
            GenerateMoreTXTButton.UseVisualStyleBackColor = true;
            GenerateMoreTXTButton.Click += GenerateMoreTXTButton_Click;
            // 
            // GenerateMoreJSONButton
            // 
            GenerateMoreJSONButton.Location = new Point(38, 317);
            GenerateMoreJSONButton.Name = "GenerateMoreJSONButton";
            GenerateMoreJSONButton.Size = new Size(190, 23);
            GenerateMoreJSONButton.TabIndex = 8;
            GenerateMoreJSONButton.Text = "Generate more JSON Paths";
            GenerateMoreJSONButton.UseVisualStyleBackColor = true;
            GenerateMoreJSONButton.Click += GenerateMoreJSONButton_Click;
            // 
            // VPathTiplabel
            // 
            VPathTiplabel.AutoSize = true;
            VPathTiplabel.Location = new Point(168, 31);
            VPathTiplabel.Name = "VPathTiplabel";
            VPathTiplabel.Size = new Size(226, 15);
            VPathTiplabel.TabIndex = 0;
            VPathTiplabel.Text = "Path should be separated with / character";
            // 
            // ClearOutputButton
            // 
            ClearOutputButton.Location = new Point(232, 270);
            ClearOutputButton.Name = "ClearOutputButton";
            ClearOutputButton.Size = new Size(113, 23);
            ClearOutputButton.TabIndex = 7;
            ClearOutputButton.Text = "Clear Output";
            ClearOutputButton.UseVisualStyleBackColor = true;
            ClearOutputButton.Click += ClearOutputButton_Click;
            // 
            // GenerateTXTButton
            // 
            GenerateTXTButton.Location = new Point(298, 109);
            GenerateTXTButton.Name = "GenerateTXTButton";
            GenerateTXTButton.Size = new Size(126, 23);
            GenerateTXTButton.TabIndex = 4;
            GenerateTXTButton.Text = "Generate TXT Output";
            GenerateTXTButton.UseVisualStyleBackColor = true;
            GenerateTXTButton.Click += GenerateTXTButton_Click;
            // 
            // GenerateJSONButton
            // 
            GenerateJSONButton.Location = new Point(137, 109);
            GenerateJSONButton.Name = "GenerateJSONButton";
            GenerateJSONButton.Size = new Size(136, 23);
            GenerateJSONButton.TabIndex = 3;
            GenerateJSONButton.Text = "Generate JSON Output";
            GenerateJSONButton.UseVisualStyleBackColor = true;
            GenerateJSONButton.Click += GenerateJSONButton_Click;
            // 
            // OutputLabel
            // 
            OutputLabel.AutoSize = true;
            OutputLabel.Location = new Point(15, 143);
            OutputLabel.Name = "OutputLabel";
            OutputLabel.Size = new Size(48, 15);
            OutputLabel.TabIndex = 5;
            OutputLabel.Text = "Output:";
            // 
            // VPathLabel
            // 
            VPathLabel.AutoSize = true;
            VPathLabel.Location = new Point(15, 54);
            VPathLabel.Name = "VPathLabel";
            VPathLabel.Size = new Size(71, 15);
            VPathLabel.TabIndex = 1;
            VPathLabel.Text = "Virtual Path:";
            // 
            // OutputTxtBox
            // 
            OutputTxtBox.BackColor = SystemColors.Control;
            OutputTxtBox.BorderStyle = BorderStyle.FixedSingle;
            OutputTxtBox.Location = new Point(15, 161);
            OutputTxtBox.Multiline = true;
            OutputTxtBox.Name = "OutputTxtBox";
            OutputTxtBox.ReadOnly = true;
            OutputTxtBox.ScrollBars = ScrollBars.Both;
            OutputTxtBox.Size = new Size(546, 98);
            OutputTxtBox.TabIndex = 6;
            // 
            // VPathTxtBox
            // 
            VPathTxtBox.Location = new Point(15, 72);
            VPathTxtBox.Name = "VPathTxtBox";
            VPathTxtBox.Size = new Size(546, 23);
            VPathTxtBox.TabIndex = 2;
            // 
            // AppStatusStrip
            // 
            AppStatusStrip.Items.AddRange(new ToolStripItem[] { AppStatusStripLabel });
            AppStatusStrip.Location = new Point(0, 498);
            AppStatusStrip.Name = "AppStatusStrip";
            AppStatusStrip.Size = new Size(601, 22);
            AppStatusStrip.SizingGrip = false;
            AppStatusStrip.TabIndex = 3;
            AppStatusStrip.Text = "statusStrip1";
            // 
            // AppStatusStripLabel
            // 
            AppStatusStripLabel.Name = "AppStatusStripLabel";
            AppStatusStripLabel.Size = new Size(0, 17);
            // 
            // CoreForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(601, 520);
            Controls.Add(AppStatusStrip);
            Controls.Add(PathGenToolsGroupBox);
            Controls.Add(FilelistToolsGroupBox);
            Controls.Add(GameSelectGroupBox);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "CoreForm";
            Text = "White Filelist Manager";
            Shown += CoreForm_Shown;
            GameSelectGroupBox.ResumeLayout(false);
            GameSelectGroupBox.PerformLayout();
            FilelistToolsGroupBox.ResumeLayout(false);
            PathGenToolsGroupBox.ResumeLayout(false);
            PathGenToolsGroupBox.PerformLayout();
            AppStatusStrip.ResumeLayout(false);
            AppStatusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox GameSelectGroupBox;
        private RadioButton FF13LRRadioButton;
        private RadioButton FF132RadioButton;
        private RadioButton FF131RadioButton;
        private Button UnpkAsJSONButton;
        private GroupBox FilelistToolsGroupBox;
        private Button RpkJSONButton;
        private Button RpkTxtButton;
        private Button UnpkAsTXTButton;
        private GroupBox PathGenToolsGroupBox;
        private TextBox OutputTxtBox;
        private TextBox VPathTxtBox;
        private Label OutputLabel;
        private Label VPathLabel;
        private Button GenerateTXTButton;
        private Button GenerateJSONButton;
        private Button ClearOutputButton;
        private Label VPathTiplabel;
        private StatusStrip AppStatusStrip;
        private ToolStripStatusLabel AppStatusStripLabel;
        private Button GenerateMoreJSONButton;
        private Button GenerateMoreTXTButton;
    }
}
