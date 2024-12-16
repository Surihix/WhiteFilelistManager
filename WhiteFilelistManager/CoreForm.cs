using WhiteFilelistManager.FilelistHelpers;

namespace WhiteFilelistManager
{
    public partial class CoreForm : Form
    {
        private static readonly CoreForm CoreFormInstance = (CoreForm)Application.OpenForms["CoreForm"];

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


        private static void ShowException(Exception ex)
        {
            if (ex.Message.ToString() != "Handled")
            {
                CoreFormInstance.Invoke(new Action(() => MessageBox.Show($"An exception has occured\n{ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)));                
            }
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
                    bool success = true;

                    try
                    {
                        FilelistFunctions.UnpackFilelist(FilelistFunctions.ParseType.json, gameCode, FilelistSelect.FileName);
                        CoreFormInstance.Invoke(new Action(() => MessageBox.Show("Selected filelist is now unpacked as JSON file", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)));
                    }
                    catch (Exception ex)
                    {
                        success = false;
                        ShowException(ex);
                    }
                    finally
                    {
                        if (success)
                        {
                            BeginInvoke(new Action(() => AppStatusStripLabel.Text = "Finished unpacking data to JSON file!"));
                        }
                        else
                        {
                            BeginInvoke(new Action(() => AppStatusStripLabel.Text = "Failed to unpack data to JSON file!"));
                        }

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
                bool success = true;

                try
                {

                }
                catch (Exception ex)
                {
                    success = false;
                    ShowException(ex);
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
                    bool success = true;

                    try
                    {
                        FilelistFunctions.UnpackFilelist(FilelistFunctions.ParseType.txt, gameCode, FilelistSelect.FileName);
                        CoreFormInstance.Invoke(new Action(() => MessageBox.Show("Selected filelist is now unpacked as text file(s)", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)));
                    }
                    catch (Exception ex)
                    {
                        success = false;
                        ShowException(ex);
                    }
                    finally
                    {
                        if (success)
                        {
                            BeginInvoke(new Action(() => AppStatusStripLabel.Text = "Finished unpacking data to TXT file(s)!"));
                        }
                        else
                        {
                            BeginInvoke(new Action(() => AppStatusStripLabel.Text = "Failed to unpack data to TXT file(s)!"));
                        }

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
                bool success = true;

                try
                {

                }
                catch (Exception ex)
                {
                    success = false;
                    ShowException(ex);
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
                bool success = true;

                try
                {

                }
                catch (Exception ex)
                {
                    success = false;
                    ShowException(ex);
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
                bool success = true;

                try
                {

                }
                catch (Exception ex)
                {
                    success = false;
                    ShowException(ex);
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