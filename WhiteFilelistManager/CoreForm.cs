using WhiteFilelistManager.FilelistTools;
using WhiteFilelistManager.PathGenTools;
using static WhiteFilelistManager.Support.SharedEnums;

namespace WhiteFilelistManager
{
    public partial class CoreForm : Form
    {
        public static TextBox OutputTxtBoxInstance { get; set; }
        public CoreForm()
        {
            InitializeComponent();
            OutputTxtBox.BackColor = SystemColors.Window;
            AppStatusStripLabel.Text = "Tool opened!";
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
                MessageBox.Show($"An exception has occured\n{ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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


        private GameID GetGameID()
        {
            if (FF131RadioButton.Checked)
            {
                return GameID.xiii;
            }
            else if (FF132RadioButton.Checked)
            {
                return GameID.xiii2;
            }
            else
            {
                return GameID.xiii3;
            }
        }


        private void SetOutputTxtBox()
        {
            if (OutputTxtBoxInstance == null)
            {
                OutputTxtBoxInstance = OutputTxtBox;
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
                        MessageBox.Show("Selected filelist is now unpacked as JSON file", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        MessageBox.Show("Selected JSON file is now repacked as a filelist", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        MessageBox.Show("Selected filelist is now unpacked as text files", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        MessageBox.Show("Selected directory with text files is now repacked as a filelist", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            SetOutputTxtBox();

            var virtualPath = VPathTxtBox.Text;

            AppStatusStripLabel.Text = "Generating JSON output....";

            var gameID = GetGameID();
            EnableDisableGUI(false);

            Task.Run(() =>
            {
                bool success = true;

                try
                {
                    var jsonOutput = PathGenCore.GenerateOutput(virtualPath, ParseType.json, gameID);
                    BeginInvoke(new Action(() => OutputTxtBoxInstance.Text = jsonOutput));
                    MessageBox.Show("Generated JSON output for the specified virtual path", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        BeginInvoke(new Action(() => AppStatusStripLabel.Text = "Finished generating JSON output!"));
                    }
                    else
                    {
                        BeginInvoke(new Action(() => AppStatusStripLabel.Text = "Failed to generate JSON output!"));
                    }

                    BeginInvoke(new Action(() => EnableDisableGUI(true)));
                }
            });
        }

        private void GenerateTXTButton_Click(object sender, EventArgs e)
        {
            SetOutputTxtBox();

            var virtualPath = VPathTxtBox.Text;

            AppStatusStripLabel.Text = "Generating TXT output....";

            var gameID = GetGameID();
            EnableDisableGUI(false);

            Task.Run(() =>
            {
                bool success = true;

                try
                {
                    var txtOutput = PathGenCore.GenerateOutput(virtualPath, ParseType.txt, gameID);
                    BeginInvoke(new Action(() => OutputTxtBoxInstance.Text = txtOutput));
                    MessageBox.Show("Generated TXT output for the specified virtual path", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        BeginInvoke(new Action(() => AppStatusStripLabel.Text = "Finished generating TXT output!"));
                    }
                    else
                    {
                        BeginInvoke(new Action(() => AppStatusStripLabel.Text = "Failed to generate TXT output!"));
                    }

                    BeginInvoke(new Action(() => EnableDisableGUI(true)));
                }
            });
        }

        private void ClearOutputButton_Click(object sender, EventArgs e)
        {
            OutputTxtBox.Clear();
        }

        private static readonly FolderBrowserDialog DirectorySelect = new()
        {
            Description = "Select a directory",
            UseDescriptionForTitle = true,
            AutoUpgradeEnabled = true
        };

        private void GenerateJSONDirButton_Click(object sender, EventArgs e)
        {
            if (DirectorySelect.ShowDialog() == DialogResult.OK)
            {
                AppStatusStripLabel.Text = "Generating JSON for directory....";

                var gameID = GetGameID();
                EnableDisableGUI(false);

                Task.Run(() =>
                {
                    bool success = true;

                    try
                    {
                        PathGenCore.GenerateForDir(ParseType.json, DirectorySelect.SelectedPath, gameID);
                        MessageBox.Show("Generated JSON for the specified directory", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                            BeginInvoke(new Action(() => AppStatusStripLabel.Text = "Finished generating JSON for the directory!"));
                        }
                        else
                        {
                            BeginInvoke(new Action(() => AppStatusStripLabel.Text = "Failed to generate JSON for the directory!"));
                        }

                        BeginInvoke(new Action(() => EnableDisableGUI(true)));
                    }
                });
            }
        }

        private void GenerateTXTDirButton_Click(object sender, EventArgs e)
        {
            if (DirectorySelect.ShowDialog() == DialogResult.OK)
            {
                AppStatusStripLabel.Text = "Generating TXT for directory....";

                var gameID = GetGameID();
                EnableDisableGUI(false);

                Task.Run(() =>
                {
                    bool success = true;

                    try
                    {
                        PathGenCore.GenerateForDir(ParseType.txt, DirectorySelect.SelectedPath, gameID);
                        MessageBox.Show("Generated TXT for the specified directory", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                            BeginInvoke(new Action(() => AppStatusStripLabel.Text = "Finished generating TXT for the directory!"));
                        }
                        else
                        {
                            BeginInvoke(new Action(() => AppStatusStripLabel.Text = "Failed to generate TXT for the directory!"));
                        }

                        BeginInvoke(new Action(() => EnableDisableGUI(true)));
                    }
                });
            }
        }
        #endregion
    }
}