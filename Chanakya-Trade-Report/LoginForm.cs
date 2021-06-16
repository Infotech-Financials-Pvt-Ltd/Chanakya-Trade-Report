using System;
using System.Windows.Forms;

namespace Chanakya_Trade_Report
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void confirmButton_Click(object sender, EventArgs e)
        {
            ValidateLogin();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            base.OnClosed(e);

            Environment.Exit(0);
        }

        public void ValidateLogin()
        {
            if (marketComboBox.Text == "")
            {
                MessageBox.Show("Please select proper Market Id");
                return;
            }

            if (memberIdTextBox.Text != "" && userTextBox.Text != "" && passwordTextBox.Text != "")
            {
                if (userTextBox.Text != "TEST" || passwordTextBox.Text != "abc@123")
                {
                    MessageBox.Show("Incorrect username or password.");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Please enter proper details.");
                return;
            }

            Properties.Settings.Default.member_code = memberIdTextBox.Text;
            Properties.Settings.Default.Save();

            SettingsWindow settings = new SettingsWindow(marketComboBox.Text.Trim(), false);
            settings.Show();

            this.Hide();
        }

        private void LoginForm_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                ValidateLogin();
            }
        }
    }
}
