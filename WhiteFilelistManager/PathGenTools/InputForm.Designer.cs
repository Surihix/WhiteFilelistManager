namespace WhiteFilelistManager
{
    partial class InputForm
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
            RangeTxtLabel = new Label();
            InputOkBtn = new Button();
            RangeNumericUpDown = new NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)RangeNumericUpDown).BeginInit();
            SuspendLayout();
            // 
            // RangeTxtLabel
            // 
            RangeTxtLabel.AutoSize = true;
            RangeTxtLabel.Location = new Point(81, 24);
            RangeTxtLabel.Name = "RangeTxtLabel";
            RangeTxtLabel.Size = new Size(61, 15);
            RangeTxtLabel.TabIndex = 0;
            RangeTxtLabel.Text = "RangeText";
            // 
            // InputOkBtn
            // 
            InputOkBtn.Location = new Point(74, 101);
            InputOkBtn.Name = "InputOkBtn";
            InputOkBtn.Size = new Size(75, 23);
            InputOkBtn.TabIndex = 1;
            InputOkBtn.Text = "OK";
            InputOkBtn.UseVisualStyleBackColor = true;
            InputOkBtn.Click += InputOkBtn_Click;
            // 
            // RangeNumericUpDown
            // 
            RangeNumericUpDown.Location = new Point(77, 58);
            RangeNumericUpDown.Name = "RangeNumericUpDown";
            RangeNumericUpDown.Size = new Size(68, 23);
            RangeNumericUpDown.TabIndex = 2;
            // 
            // InputForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(222, 144);
            Controls.Add(RangeNumericUpDown);
            Controls.Add(InputOkBtn);
            Controls.Add(RangeTxtLabel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "InputForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Input";
            ((System.ComponentModel.ISupportInitialize)RangeNumericUpDown).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label RangeTxtLabel;
        private Button InputOkBtn;
        private NumericUpDown RangeNumericUpDown;
    }
}