using WhiteFilelistManager.FilelistTools;
using static WhiteFilelistManager.Support.SharedEnums;

namespace WhiteFilelistManager
{
    public partial class CoreForm : Form
    {
        public static CoreForm CoreFormInstance { get; set; }

        public CoreForm()
        {
            InitializeComponent();
            OutputTxtBox.BackColor = SystemColors.Window;
            AppStatusStripLabel.Text = "Tool opened!";
        }


        private void CoreForm_Shown(object sender, EventArgs e)
        {
            CoreFormInstance = (CoreForm)Application.OpenForms["CoreForm"];
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
                AppStatusStripLabel.Text = "Unpacking data to JSON file....";

                var gameCode = GetGameCode();
                EnableDisableGUI(false);

                Task.Run(() =>
                {
                    bool success = true;

                    try
                    {
                        FilelistToolsCore.UnpackFilelist(gameCode, FilelistSelect.FileName, ParseType.json);
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
            var jsonFileSelect = new OpenFileDialog
            {
                Title = "Select JSON file",
                Filter = "JSON files (*.json)|*.json"
            };

            if (jsonFileSelect.ShowDialog() == DialogResult.OK)
            {
                AppStatusStripLabel.Text = "Repacking data from JSON file....";

                var gameCode = GetGameCode();
                EnableDisableGUI(false);

                Task.Run(() =>
                {
                    bool success = true;

                    try
                    {
                        FilelistToolsCore.RepackFilelist(ParseType.json, jsonFileSelect.FileName, gameCode);
                        CoreFormInstance.Invoke(new Action(() => MessageBox.Show("Selected JSON file is now repacked as a filelist", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)));
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
                            BeginInvoke(new Action(() => AppStatusStripLabel.Text = "Finished repacking data from JSON file!"));
                        }
                        else
                        {
                            BeginInvoke(new Action(() => AppStatusStripLabel.Text = "Failed to repack data from JSON file!"));
                        }

                        BeginInvoke(new Action(() => EnableDisableGUI(true)));
                    }
                });
            }
        }

        private void UnpkAsTXTButton_Click(object sender, EventArgs e)
        {
            if (FilelistSelect.ShowDialog() == DialogResult.OK)
            {
                AppStatusStripLabel.Text = "Unpacking data to text files....";

                var gameCode = GetGameCode();
                EnableDisableGUI(false);

                Task.Run(() =>
                {
                    bool success = true;

                    try
                    {
                        FilelistToolsCore.UnpackFilelist(gameCode, FilelistSelect.FileName, ParseType.txt);
                        CoreFormInstance.Invoke(new Action(() => MessageBox.Show("Selected filelist is now unpacked as text files", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)));
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
                            BeginInvoke(new Action(() => AppStatusStripLabel.Text = "Finished unpacking data to text files!"));
                        }
                        else
                        {
                            BeginInvoke(new Action(() => AppStatusStripLabel.Text = "Failed to unpack data to text files!"));
                        }

                        BeginInvoke(new Action(() => EnableDisableGUI(true)));
                    }
                });
            }
        }

        private void RpkTxtButton_Click(object sender, EventArgs e)
        {
            var txtFilesDir = new FolderBrowserDialog
            {
                Description = "Select unpacked filelist folder",
                UseDescriptionForTitle = true,
                AutoUpgradeEnabled = true
            };

            if (txtFilesDir.ShowDialog() == DialogResult.OK)
            {
                AppStatusStripLabel.Text = "Repacking data from text files....";

                var gameCode = GetGameCode();
                EnableDisableGUI(false);

                Task.Run(() =>
                {
                    bool success = true;

                    try
                    {
                        FilelistToolsCore.RepackFilelist(ParseType.txt, txtFilesDir.SelectedPath, gameCode);
                        CoreFormInstance.Invoke(new Action(() => MessageBox.Show("Selected directory with text files is now repacked as a filelist", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)));
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
                            BeginInvoke(new Action(() => AppStatusStripLabel.Text = "Finished repacking data from text files!"));
                        }
                        else
                        {
                            BeginInvoke(new Action(() => AppStatusStripLabel.Text = "Failed to repack data from text files!"));
                        }

                        BeginInvoke(new Action(() => EnableDisableGUI(true)));
                    }
                });
            }
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