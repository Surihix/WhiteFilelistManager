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


        #region Filelist tools
        private void UnpkJSONButton_Click(object sender, EventArgs e)
        {

        }

        private void RpkJSONButton_Click(object sender, EventArgs e)
        {

        }

        private void UnpkTXTButton_Click(object sender, EventArgs e)
        {

        }

        private void RpkTxtButton_Click(object sender, EventArgs e)
        {

        }
        #endregion


        #region Path Generator tools
        private void GenerateJSONButton_Click(object sender, EventArgs e)
        {

        }

        private void GenerateTXTButton_Click(object sender, EventArgs e)
        {

        }

        private void ClearOutputButton_Click(object sender, EventArgs e)
        {

        }
        #endregion
    }
}