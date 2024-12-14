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
            radioButton3 = new RadioButton();
            radioButton2 = new RadioButton();
            radioButton1 = new RadioButton();
            UnpkJSONButton = new Button();
            FilelistToolsGroupBox = new GroupBox();
            RpkTxtButton = new Button();
            RpkJSONButton = new Button();
            UnpkTXTButton = new Button();
            PathGenToolsGroupBox = new GroupBox();
            VPathTiplabel = new Label();
            ClearOutputButton = new Button();
            GenerateTXTButton = new Button();
            GenerateJSONButton = new Button();
            OutputLabel = new Label();
            VPathLabel = new Label();
            OutputTxtBox = new TextBox();
            VPathTxtBox = new TextBox();
            GameSelectGroupBox.SuspendLayout();
            FilelistToolsGroupBox.SuspendLayout();
            PathGenToolsGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // GameSelectGroupBox
            // 
            GameSelectGroupBox.Controls.Add(radioButton3);
            GameSelectGroupBox.Controls.Add(radioButton2);
            GameSelectGroupBox.Controls.Add(radioButton1);
            GameSelectGroupBox.Location = new Point(12, 12);
            GameSelectGroupBox.Name = "GameSelectGroupBox";
            GameSelectGroupBox.Size = new Size(189, 52);
            GameSelectGroupBox.TabIndex = 0;
            GameSelectGroupBox.TabStop = false;
            GameSelectGroupBox.Text = "Select Game:";
            // 
            // radioButton3
            // 
            radioButton3.AutoSize = true;
            radioButton3.Location = new Point(122, 18);
            radioButton3.Name = "radioButton3";
            radioButton3.Size = new Size(59, 19);
            radioButton3.TabIndex = 2;
            radioButton3.Text = "XIII-LR";
            radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.Location = new Point(64, 18);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new Size(52, 19);
            radioButton2.TabIndex = 1;
            radioButton2.Text = "XIII-2";
            radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Checked = true;
            radioButton1.Location = new Point(6, 18);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(52, 19);
            radioButton1.TabIndex = 0;
            radioButton1.TabStop = true;
            radioButton1.Text = "XIII-1";
            radioButton1.UseVisualStyleBackColor = true;
            // 
            // UnpkJSONButton
            // 
            UnpkJSONButton.Location = new Point(15, 22);
            UnpkJSONButton.Name = "UnpkJSONButton";
            UnpkJSONButton.Size = new Size(107, 23);
            UnpkJSONButton.TabIndex = 1;
            UnpkJSONButton.Text = "Unpack as JSON";
            UnpkJSONButton.UseVisualStyleBackColor = true;
            UnpkJSONButton.Click += UnpkJSONButton_Click;
            // 
            // FilelistToolsGroupBox
            // 
            FilelistToolsGroupBox.Controls.Add(RpkTxtButton);
            FilelistToolsGroupBox.Controls.Add(RpkJSONButton);
            FilelistToolsGroupBox.Controls.Add(UnpkTXTButton);
            FilelistToolsGroupBox.Controls.Add(UnpkJSONButton);
            FilelistToolsGroupBox.Location = new Point(328, 12);
            FilelistToolsGroupBox.Name = "FilelistToolsGroupBox";
            FilelistToolsGroupBox.Size = new Size(261, 90);
            FilelistToolsGroupBox.TabIndex = 5;
            FilelistToolsGroupBox.TabStop = false;
            FilelistToolsGroupBox.Text = "Filelist tools:";
            // 
            // RpkTxtButton
            // 
            RpkTxtButton.Location = new Point(137, 51);
            RpkTxtButton.Name = "RpkTxtButton";
            RpkTxtButton.Size = new Size(107, 23);
            RpkTxtButton.TabIndex = 7;
            RpkTxtButton.Text = "Repack TXT(s)";
            RpkTxtButton.UseVisualStyleBackColor = true;
            RpkTxtButton.Click += RpkTxtButton_Click;
            // 
            // RpkJSONButton
            // 
            RpkJSONButton.Location = new Point(137, 22);
            RpkJSONButton.Name = "RpkJSONButton";
            RpkJSONButton.Size = new Size(107, 23);
            RpkJSONButton.TabIndex = 5;
            RpkJSONButton.Text = "Repack JSON";
            RpkJSONButton.UseVisualStyleBackColor = true;
            RpkJSONButton.Click += RpkJSONButton_Click;
            // 
            // UnpkTXTButton
            // 
            UnpkTXTButton.Location = new Point(15, 51);
            UnpkTXTButton.Name = "UnpkTXTButton";
            UnpkTXTButton.Size = new Size(107, 23);
            UnpkTXTButton.TabIndex = 6;
            UnpkTXTButton.Text = "Unpack as TXT(s)";
            UnpkTXTButton.UseVisualStyleBackColor = true;
            UnpkTXTButton.Click += UnpkTXTButton_Click;
            // 
            // PathGenToolsGroupBox
            // 
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
            PathGenToolsGroupBox.Size = new Size(577, 306);
            PathGenToolsGroupBox.TabIndex = 6;
            PathGenToolsGroupBox.TabStop = false;
            PathGenToolsGroupBox.Text = "Path Generator tools:";
            // 
            // VPathTiplabel
            // 
            VPathTiplabel.AutoSize = true;
            VPathTiplabel.Location = new Point(168, 31);
            VPathTiplabel.Name = "VPathTiplabel";
            VPathTiplabel.Size = new Size(226, 15);
            VPathTiplabel.TabIndex = 7;
            VPathTiplabel.Text = "Path should be separated with / character";
            // 
            // ClearOutputButton
            // 
            ClearOutputButton.Location = new Point(232, 270);
            ClearOutputButton.Name = "ClearOutputButton";
            ClearOutputButton.Size = new Size(113, 23);
            ClearOutputButton.TabIndex = 6;
            ClearOutputButton.Text = "Clear Output";
            ClearOutputButton.UseVisualStyleBackColor = true;
            ClearOutputButton.Click += ClearOutputButton_Click;
            // 
            // GenerateTXTButton
            // 
            GenerateTXTButton.Location = new Point(298, 109);
            GenerateTXTButton.Name = "GenerateTXTButton";
            GenerateTXTButton.Size = new Size(107, 23);
            GenerateTXTButton.TabIndex = 5;
            GenerateTXTButton.Text = "Generate for TXT";
            GenerateTXTButton.UseVisualStyleBackColor = true;
            GenerateTXTButton.Click += GenerateTXTButton_Click;
            // 
            // GenerateJSONButton
            // 
            GenerateJSONButton.Location = new Point(160, 109);
            GenerateJSONButton.Name = "GenerateJSONButton";
            GenerateJSONButton.Size = new Size(113, 23);
            GenerateJSONButton.TabIndex = 4;
            GenerateJSONButton.Text = "Generate for JSON";
            GenerateJSONButton.UseVisualStyleBackColor = true;
            GenerateJSONButton.Click += GenerateJSONButton_Click;
            // 
            // OutputLabel
            // 
            OutputLabel.AutoSize = true;
            OutputLabel.Location = new Point(15, 143);
            OutputLabel.Name = "OutputLabel";
            OutputLabel.Size = new Size(48, 15);
            OutputLabel.TabIndex = 3;
            OutputLabel.Text = "Output:";
            // 
            // VPathLabel
            // 
            VPathLabel.AutoSize = true;
            VPathLabel.Location = new Point(15, 54);
            VPathLabel.Name = "VPathLabel";
            VPathLabel.Size = new Size(71, 15);
            VPathLabel.TabIndex = 2;
            VPathLabel.Text = "Virtual Path:";
            // 
            // OutputTxtBox
            // 
            OutputTxtBox.Location = new Point(15, 161);
            OutputTxtBox.Multiline = true;
            OutputTxtBox.Name = "OutputTxtBox";
            OutputTxtBox.Size = new Size(546, 98);
            OutputTxtBox.TabIndex = 1;
            // 
            // VPathTxtBox
            // 
            VPathTxtBox.Location = new Point(15, 72);
            VPathTxtBox.Name = "VPathTxtBox";
            VPathTxtBox.Size = new Size(546, 23);
            VPathTxtBox.TabIndex = 0;
            // 
            // CoreForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(601, 440);
            Controls.Add(PathGenToolsGroupBox);
            Controls.Add(FilelistToolsGroupBox);
            Controls.Add(GameSelectGroupBox);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "CoreForm";
            Text = "White Filelist Manager";
            GameSelectGroupBox.ResumeLayout(false);
            GameSelectGroupBox.PerformLayout();
            FilelistToolsGroupBox.ResumeLayout(false);
            PathGenToolsGroupBox.ResumeLayout(false);
            PathGenToolsGroupBox.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox GameSelectGroupBox;
        private RadioButton radioButton3;
        private RadioButton radioButton2;
        private RadioButton radioButton1;
        private Button UnpkJSONButton;
        private GroupBox FilelistToolsGroupBox;
        private Button RpkJSONButton;
        private Button RpkTxtButton;
        private Button UnpkTXTButton;
        private GroupBox PathGenToolsGroupBox;
        private TextBox OutputTxtBox;
        private TextBox VPathTxtBox;
        private Label OutputLabel;
        private Label VPathLabel;
        private Button GenerateTXTButton;
        private Button GenerateJSONButton;
        private Button ClearOutputButton;
        private Label VPathTiplabel;
    }
}
