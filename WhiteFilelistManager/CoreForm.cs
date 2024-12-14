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
        private void UnpkJSONButton_Click(object sender, EventArgs e)
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
                    MessageBox.Show($"An exception has occured\n{ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                finally
                {
                    BeginInvoke(new Action(() => EnableDisableGUI(true)));
                }
            });
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
                    MessageBox.Show($"An exception has occured\n{ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                finally
                {
                    BeginInvoke(new Action(() => EnableDisableGUI(true)));
                }
            });
        }

        private void UnpkTXTButton_Click(object sender, EventArgs e)
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
                    MessageBox.Show($"An exception has occured\n{ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                finally
                {
                    BeginInvoke(new Action(() => EnableDisableGUI(true)));
                }
            });
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
                    MessageBox.Show($"An exception has occured\n{ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    MessageBox.Show($"An exception has occured\n{ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    MessageBox.Show($"An exception has occured\n{ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
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