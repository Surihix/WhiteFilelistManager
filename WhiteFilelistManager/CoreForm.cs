using WhiteFilelistManager.FilelistHelpers;

namespace WhiteFilelistManager
{
    public partial class CoreForm : Form
    {
        public CoreForm()
        {
            InitializeComponent();
        }


        private void EnableDisableGUI(bool isEnabled)
        {
            GameSelectGroupBox.Enabled = isEnabled;
            FilelistToolsGroupBox.Enabled = isEnabled;
            PathGenToolsGroupBox.Enabled = isEnabled;
        }


        public enum GameCode
        {
            ff131,
            ff132
        }

        private GameCode GetGameCode()
        {
            if (FF131RadioButton.Checked)
            {
                return GameCode.ff131;
            }
            else
            {
                return GameCode.ff132;
            }
        }


        #region Filelist tools
        private static readonly OpenFileDialog FilelistSelect = new()
        {
            Title = "Select Filelist",
            Filter = "Filelist files (*.bin)|*.bin"
        };

        private void UnpkAsJSONButton_Click(object sender, EventArgs e)
        {
            if (FilelistSelect.ShowDialog() == DialogResult.OK)
            {
                AppStatusStripLabel.Text = "Unpacking to JSON....";

                var gameCode = GetGameCode();
                EnableDisableGUI(false);

                Task.Run(() =>
                {
                    try
                    {
                        FilelistFunctions.UnpackFilelist(FilelistFunctions.ParseType.json, gameCode, FilelistSelect.FileName);
                        BeginInvoke(new Action(() => MessageBox.Show("Selected filelist is now unpacked as JSON file", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)));
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.ToString() != "Handled")
                        {
                            

                            MessageBox.Show($"An exception has occured\n{ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    finally
                    {
                        BeginInvoke(new Action(() => AppStatusStripLabel.Text = "Finished unpacking data to JSON file!"));
                        BeginInvoke(new Action(() => EnableDisableGUI(true)));
                    }
                });
            }
        }

        private void RpkJSONButton_Click(object sender, EventArgs e)
        {
            var gameCode = GetGameCode();
            EnableDisableGUI(false);

            Task.Run(() =>
            {
                try
                {

                }
                catch (Exception ex)
                {
                    if (ex.Message.ToString() != "Handled")
                    {
                        MessageBox.Show($"An exception has occured\n{ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                finally
                {
                    BeginInvoke(new Action(() => EnableDisableGUI(true)));
                }
            });
        }

        private void UnpkAsTXTButton_Click(object sender, EventArgs e)
        {
            if (FilelistSelect.ShowDialog() == DialogResult.OK)
            {
                AppStatusStripLabel.Text = "Unpacking to TXT file(s)....";

                var gameCode = GetGameCode();
                EnableDisableGUI(false);

                Task.Run(() =>
                {
                    try
                    {
                        FilelistFunctions.UnpackFilelist(FilelistFunctions.ParseType.txt, gameCode, FilelistSelect.FileName);
                        BeginInvoke(new Action(() => MessageBox.Show("Selected filelist is now unpacked as text file(s)", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)));
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.ToString() != "Handled")
                        {
                            MessageBox.Show($"An exception has occured\n{ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    finally
                    {
                        BeginInvoke(new Action(() => AppStatusStripLabel.Text = "Finished unpacking data to TXT file(s)!"));
                        BeginInvoke(new Action(() => EnableDisableGUI(true)));
                    }
                });
            }
        }

        private void RpkTxtButton_Click(object sender, EventArgs e)
        {
            var gameCode = GetGameCode();
            EnableDisableGUI(false);

            Task.Run(() =>
            {
                try
                {

                }
                catch (Exception ex)
                {
                    if (ex.Message.ToString() != "Handled")
                    {
                        MessageBox.Show($"An exception has occured\n{ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                finally
                {
                    BeginInvoke(new Action(() => EnableDisableGUI(true)));
                }
            });
        }
        #endregion


        #region Path Generator tools
        private void GenerateJSONButton_Click(object sender, EventArgs e)
        {
            var gameCode = GetGameCode();
            EnableDisableGUI(false);

            Task.Run(() =>
            {
                try
                {

                }
                catch (Exception ex)
                {
                    if (ex.Message.ToString() != "Handled")
                    {
                        MessageBox.Show($"An exception has occured\n{ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                finally
                {
                    BeginInvoke(new Action(() => EnableDisableGUI(true)));
                }
            });
        }

        private void GenerateTXTButton_Click(object sender, EventArgs e)
        {
            var gameCode = GetGameCode();
            EnableDisableGUI(false);

            Task.Run(() =>
            {
                try
                {

                }
                catch (Exception ex)
                {
                    if (ex.Message.ToString() != "Handled")
                    {
                        MessageBox.Show($"An exception has occured\n{ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                finally
                {
                    BeginInvoke(new Action(() => EnableDisableGUI(true)));
                }
            });
        }

        private void ClearOutputButton_Click(object sender, EventArgs e)
        {
            OutputTxtBox.Clear();
        }
        #endregion
    }
}