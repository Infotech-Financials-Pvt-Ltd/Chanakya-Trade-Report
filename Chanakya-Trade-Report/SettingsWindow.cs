using System;
using System.IO;
using System.Windows.Forms;

namespace Chanakya_Trade_Report
{
    public partial class SettingsWindow : Form
    {
        public string marketId = "";
        public bool IsWindowOpen = false;
        public SettingsWindow(string market, bool isOpen)
        {
            InitializeComponent();
            marketId = market;
            this.Text = "Settings Window : " + market;

            IsWindowOpen = isOpen;
            memberTextBox.Text = Properties.Settings.Default.member_code;
            recordslimitTextBox.Text = Properties.Settings.Default.record_limit.ToString();

            //Loading data from settings file into the window.
            if (market == "FO")
            {
                keyTextBox.Text = Properties.Settings.Default.key_fo;
                secretTextBox.Text = Properties.Settings.Default.secret_fo;
                accesstokenTextBox.Text = Properties.Settings.Default.access_token_fo;
                loginUrlTextBox.Text = Properties.Settings.Default.login_url_fo;
                tradeUrlTextBox.Text = Properties.Settings.Default.trade_url_fo;
                folderTextBox.Text = Properties.Settings.Default.csv_folder;
                tradeIntervalTextbox.Text = Properties.Settings.Default.trade_interval.ToString();
                actionsUrlTextBox.Text = Properties.Settings.Default.actions_url_fo;
                if (Properties.Settings.Default.inquiryType_fo == "ALL")
                {
                    allCheckBox.Checked = true;
                }
                else if (Properties.Settings.Default.inquiryType_fo == "TM")
                {
                    tmCheckBox.Checked = true;
                }
                else
                {
                    allCheckBox.Checked = true;
                }

                if (Properties.Settings.Default.downloadType_fo == "TRADES")
                {
                    tradeCheckBox.Checked = true;
                }
                else if (Properties.Settings.Default.downloadType_fo == "ACTIONS")
                {
                    actionsCheckBox.Checked = true;
                }
                else
                {
                    tradeCheckBox.Checked = true;
                }
            }
            else if (market == "CM")
            {
                keyTextBox.Text = Properties.Settings.Default.key_cm;
                secretTextBox.Text = Properties.Settings.Default.secret_cm;
                accesstokenTextBox.Text = Properties.Settings.Default.access_token_cm;
                loginUrlTextBox.Text = Properties.Settings.Default.login_url_cm;
                tradeUrlTextBox.Text = Properties.Settings.Default.trade_url_cm;
                folderTextBox.Text = Properties.Settings.Default.csv_folder;
                tradeIntervalTextbox.Text = Properties.Settings.Default.trade_interval.ToString();
                actionsUrlTextBox.Text = Properties.Settings.Default.actions_url_cm;
                if (Properties.Settings.Default.inquiryType_fo == "ALL")
                {
                    allCheckBox.Checked = true;
                }
                else if (Properties.Settings.Default.inquiryType_cm == "TM")
                {
                    tmCheckBox.Checked = true;
                }
                else
                {
                    allCheckBox.Checked = true;
                }

                if (Properties.Settings.Default.downloadType_cm == "TRADES")
                {
                    tradeCheckBox.Checked = true;
                }
                else if (Properties.Settings.Default.downloadType_cm == "ACTIONS")
                {
                    actionsCheckBox.Checked = true;
                }
                else
                {
                    tradeCheckBox.Checked = true;
                }
            }
            else if (market == "CD")
            {
                keyTextBox.Text = Properties.Settings.Default.key_cd;
                secretTextBox.Text = Properties.Settings.Default.secret_cd;
                accesstokenTextBox.Text = Properties.Settings.Default.access_token_cd;
                loginUrlTextBox.Text = Properties.Settings.Default.login_url_cd;
                tradeUrlTextBox.Text = Properties.Settings.Default.trade_url_cd;
                folderTextBox.Text = Properties.Settings.Default.csv_folder;
                tradeIntervalTextbox.Text = Properties.Settings.Default.trade_interval.ToString();
                actionsUrlTextBox.Text = Properties.Settings.Default.actions_url_cd;
                if (Properties.Settings.Default.inquiryType_fo == "ALL")
                {
                    allCheckBox.Checked = true;
                }
                else if (Properties.Settings.Default.inquiryType_cd == "TM")
                {
                    tmCheckBox.Checked = true;
                }
                else
                {
                    allCheckBox.Checked = true;
                }

                if (Properties.Settings.Default.downloadType_cd == "TRADES")
                {
                    tradeCheckBox.Checked = true;
                }
                else if (Properties.Settings.Default.downloadType_cd == "ACTIONS")
                {
                    actionsCheckBox.Checked = true;
                }
                else
                {
                    tradeCheckBox.Checked = true;
                }
            }

            if (Properties.Settings.Default.DEBUG == true)
            {
                debugCheckBox.Checked = true;
            }
            else
            {
                debugCheckBox.Checked = false;
            }


        }
        private void submitButton_Click(object sender, EventArgs e)
        {
            updateSettings();
        }

        public void updateSettings()
        {
            try
            {
                //Saving updated setttings values in config file.
                if (marketId == "FO")
                {
                    Properties.Settings.Default.member_code = memberTextBox.Text;
                    Properties.Settings.Default.key_fo = keyTextBox.Text;
                    Properties.Settings.Default.secret_fo = secretTextBox.Text;
                    Properties.Settings.Default.access_token_fo = accesstokenTextBox.Text;
                    Properties.Settings.Default.login_url_fo = loginUrlTextBox.Text;
                    Properties.Settings.Default.trade_url_fo = tradeUrlTextBox.Text;
                    Properties.Settings.Default.csv_folder = folderTextBox.Text;
                    Properties.Settings.Default.actions_url_fo = actionsUrlTextBox.Text;

                    if (allCheckBox.Checked)
                    {
                        Properties.Settings.Default.inquiryType_fo = "ALL";
                    }
                    else if (tmCheckBox.Checked)
                    {
                        Properties.Settings.Default.inquiryType_fo = "TM";
                    }

                    if (tradeCheckBox.Checked)
                    {
                        Properties.Settings.Default.downloadType_fo = "TRADES";
                    }
                    else if (actionsCheckBox.Checked)
                    {
                        Properties.Settings.Default.downloadType_fo = "ACTIONS";
                    }
                }
                else if (marketId == "CM")
                {
                    Properties.Settings.Default.member_code = memberTextBox.Text;
                    Properties.Settings.Default.key_cm = keyTextBox.Text;
                    Properties.Settings.Default.secret_cm = secretTextBox.Text;
                    Properties.Settings.Default.access_token_cm = accesstokenTextBox.Text;
                    Properties.Settings.Default.login_url_cm = loginUrlTextBox.Text;
                    Properties.Settings.Default.trade_url_cm = tradeUrlTextBox.Text;
                    Properties.Settings.Default.csv_folder = folderTextBox.Text;
                    Properties.Settings.Default.actions_url_cm = actionsUrlTextBox.Text;

                    if (allCheckBox.Checked)
                    {
                        Properties.Settings.Default.inquiryType_cm = "ALL";
                    }
                    else if (tmCheckBox.Checked)
                    {
                        Properties.Settings.Default.inquiryType_cm = "TM";
                    }

                    if (tradeCheckBox.Checked)
                    {
                        Properties.Settings.Default.downloadType_cm = "TRADES";
                    }
                    else if (actionsCheckBox.Checked)
                    {
                        Properties.Settings.Default.downloadType_cm = "ACTIONS";
                    }
                }
                else if (marketId == "CD")
                {
                    Properties.Settings.Default.member_code = memberTextBox.Text;
                    Properties.Settings.Default.key_cd = keyTextBox.Text;
                    Properties.Settings.Default.secret_cd = secretTextBox.Text;
                    Properties.Settings.Default.access_token_cd = accesstokenTextBox.Text;
                    Properties.Settings.Default.login_url_cd = loginUrlTextBox.Text;
                    Properties.Settings.Default.trade_url_cd = tradeUrlTextBox.Text;
                    Properties.Settings.Default.csv_folder = folderTextBox.Text;
                    Properties.Settings.Default.actions_url_cd = actionsUrlTextBox.Text;

                    if (allCheckBox.Checked)
                    {
                        Properties.Settings.Default.inquiryType_cd = "ALL";
                    }
                    else if (tmCheckBox.Checked)
                    {
                        Properties.Settings.Default.inquiryType_cd = "TM";
                    }

                    if (tradeCheckBox.Checked)
                    {
                        Properties.Settings.Default.downloadType_cd = "TRADES";
                    }
                    else if (actionsCheckBox.Checked)
                    {
                        Properties.Settings.Default.downloadType_cd = "ACTIONS";
                    }
                }

                if (int.TryParse(tradeIntervalTextbox.Text, out int n))
                {
                    Properties.Settings.Default.trade_interval = int.Parse(tradeIntervalTextbox.Text);
                }
                else
                {
                    MessageBox.Show("Enter proper Records Limit value.");
                    return;
                }

                if (int.TryParse(recordslimitTextBox.Text, out n))
                {
                    Properties.Settings.Default.record_limit = int.Parse(recordslimitTextBox.Text);
                }
                else
                {
                    MessageBox.Show("Enter proper Records Limit value.");
                    return;
                }

                Properties.Settings.Default.Save();

                if (IsWindowOpen == false)
                {
                    TradeReportWindow reportWindow = new TradeReportWindow(marketId);
                    if (oldFormatCheckBox.Checked)
                    {
                        reportWindow.OLD_FORMAT = true;
                        reportWindow.NEW_FORMAT = false;
                    }

                    if (newFormatCheckBox.Checked)
                    {
                        reportWindow.NEW_FORMAT = true;
                        reportWindow.OLD_FORMAT = false;
                    }

                    if (newFormatCheckBox.Checked && oldFormatCheckBox.Checked)
                    {
                        reportWindow.NEW_FORMAT = true;
                        reportWindow.OLD_FORMAT = true;
                    }

                    Console.WriteLine("Showing window");
                    reportWindow.Show();
                    IsWindowOpen = true;
                }

                this.Hide();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error opening report window");
                File.AppendAllText("app-error.txt", "Error updating settings:" + e);
                this.Close();
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {

            if (IsWindowOpen == false)
            {
                base.OnClosed(e);
                Environment.Exit(0);
            }
            else
            {
                this.Hide();
            }
        }

        private void allCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (allCheckBox.Checked)
            {
                tmCheckBox.Checked = false;
            }
            else
            {
                tmCheckBox.Checked = true;
            }
        }

        private void tmCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (tmCheckBox.Checked)
            {
                allCheckBox.Checked = false;
            }
            else
            {
                allCheckBox.Checked = true;
            }
        }

        private void tradeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (tradeCheckBox.Checked)
            {
                actionsCheckBox.Checked = false;
            }
            else
            {
                actionsCheckBox.Checked = true;
            }
        }

        private void actionsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (actionsCheckBox.Checked)
            {
                tradeCheckBox.Checked = false;
            }
            else
            {
                tradeCheckBox.Checked = true;
            }
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.ShowNewFolderButton = true;
            // Show the FolderBrowserDialog.  
            DialogResult result = folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                folderTextBox.Text = folderDlg.SelectedPath;
                Environment.SpecialFolder root = folderDlg.RootFolder;
            }
        }

        private void SettingsWindow_KeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine("Settings: keydown:" + e.KeyCode);
            if (e.KeyCode == Keys.Enter)
            {
                updateSettings();
            }
        }

        private void debugCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (debugCheckBox.Checked == true)
            {
                Properties.Settings.Default.DEBUG = true;
            }
            else
            {
                Properties.Settings.Default.DEBUG = false;
            }
        }

        private void SettingsWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsWindowOpen == false)
            {
                base.OnClosed(e);
                Environment.Exit(0);
            }
            else
            {
                //Hide the window as using close will close the app.
                this.Hide();
            }
        }
    }
}
