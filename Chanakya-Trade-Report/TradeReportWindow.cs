using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Chanakya_Trade_Report
{
    public partial class TradeReportWindow : Form
    {
        //List to save trade entries which will be read and displayed in table.
        public static ObservableCollection<TradeDetails> TradeDetailsList = new ObservableCollection<TradeDetails>();

        public HttpClient client;
        DataTable table;
        DataSet dataSet = null;
        Thread ReportFileReadThread = null;
        public Thread TradeDownloadThread = null;
        static object fileAccessLock = new object();
        public String WindowStat;

        static int total_buy_qty = 0;
        static double total_buy_price = 0;
        static int total_sell_qty = 0;
        static double total_sell_price = 0;

        public bool windowInvokerNotcalled = false;
        public static string market = "";

        public int maxReadSeqNo = 0;
        public bool LoadExistingFile = false;
        public bool READ_COMPLETE = false;
        public bool WRITE_COMPLETE = false;
        public bool INITIAL_LOAD_COMPLETE = false;
        int counter = 0;
        bool DEBUG = false;
        string debug_logs = "";
        string today = DateTime.Now.ToString("ddMMyyyy");
        int msgIdCounter = 0;
        public bool OLD_FORMAT = true;
        public bool NEW_FORMAT = false;

        public TradeReportWindow(string marketId)
        {

            InitializeComponent();

            try
            {
                market = marketId;

                //Sets window title text.
                this.Text = "Chanakya Trade Report : " + market + " Member Code: " + Properties.Settings.Default.member_code;

                DEBUG = Properties.Settings.Default.DEBUG;
                debug_logs = "log-debug-" + market + "-" + today + ".txt";
                //To get previous position of window.
                LoadWindowPosition();

                table = new DataTable();
                dataSet = new DataSet();
                bindingSource_main.DataSource = dataSet;
                dataGridView1.AutoGenerateColumns = true;

                dataGridView1.DataSource = bindingSource_main;

                //Creates blank table with columns to display the table.
                BlankRowTable();
                dataGridView1.AllowUserToOrderColumns = true;

                //To store table column positions of a session.
                string fileName = "ReportDataGrid.xml";
                LoadDataGridOrderFromFile(fileName, dataGridView1.Columns);
                dataGridView1.AutoGenerateColumns = false;

                //To load column position from file stored in previous session.
                LoadColumnPosition();

                //Reading Trade file if exists and getting max received sequence number.
                maxReadSeqNo = getMaxSeqNoFromFile();
                if (maxReadSeqNo > 0)
                {
                    //Loading data from existing trade file.
                    setReportDataInitial();
                }
                else
                {
                    msgIdCounter = 0;
                    INITIAL_LOAD_COMPLETE = true;
                }


                //Starting Thread to login into exchange and receive access token and trade download.
                TradeDownloadThread = new Thread(StartAuthRequestThread);
                TradeDownloadThread.Start();

                //Creates a thread to read trade download file.
                ReadTradeFileThread();

            }
            catch (Exception e)
            {
                fp_applog("TradeReportWindow:" + e);
                MessageBox.Show("Error while starting the application. " + e.GetType().FullName);

            }
        }
        public void StartAuthRequestThread()
        {
            SendAuthRequest(market);
        }

        static int tableCount = 0;
        /// <summary>
        /// Assigns thread function to thread object.
        /// </summary>
        public void ReadTradeFileThread()
        {
            try
            {
                fp_applog("ThreadCreation method called------");
                tableCount = table.Rows.Count;

                ReportFileReadThread = new Thread(setReportDataThread);
                ReportFileReadThread.Start();
                fp_applog("ThreadCreation method executed------");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Received error while setting thread");
                fp_applog("ThreadCreation method call" + ex);
            }

        }

        bool ReportCloseFlag = false;
        /// <summary>
        /// Function to start loop of the thread at particular interval.
        /// </summary>
        public void setReportDataThread()
        {
            try
            {

                while (true)
                {


                    try
                    {
                        if (INITIAL_LOAD_COMPLETE)
                        {
                            //Function to read the trade download file and display into  table.
                            setReportData();
                        }

                    }
                    catch (Exception ex)
                    {
                        fp_applog("Report thread method exception :" + ex);
                    }

                    if (ReportCloseFlag == true)
                    {
                        fp_applog("ReportCloseFlag to true");
                        return;
                    }
                    Thread.Sleep(Properties.Settings.Default.trade_interval * 1000);
                }
            }
            catch (ThreadAbortException ex)
            {
                fp_applog("Thread - caught ThreadAbortException - resetting. " + ex);

            }
            catch (Exception ex)
            {
                fp_applog("ex:" + ex);
            }
        }

        /// <summary>
        /// This methd will be used to make an initial readinng of file to 
        /// read the trade details for prices and qty for total calculations.
        /// </summary>
        public void setReportDataInitial()
        {
            string date1 = DateTime.Now.ToString("ddMMyyyy");
            string path = Properties.Settings.Default.csv_folder + "\\" + "Trade" + market + "_" + date1 + ".txt";
            fp_applog($"trade file:{path}");

            try
            {
                READ_COMPLETE = false;
                //Reading the trade download file.
                using (var reader = new StreamReader(path))
                {

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var count = line.Split(',').Count();
                        counter++;


                        TradeDetails td = new TradeDetails(line, market, true);

                        //Set total calculation here
                        if (td.buy_sell_flag == "1")
                        {
                            total_buy_price += (td.trade_price * td.trade_qty) / 100.00;
                            total_buy_qty += td.trade_qty;
                        }
                        else if (td.buy_sell_flag == "2")
                        {
                            total_sell_price += (td.trade_price * td.trade_qty) / 100.00;
                            total_sell_qty += td.trade_qty;
                        }

                        int limit = Properties.Settings.Default.record_limit;
                        if (TradeDetailsList.Count > limit)
                        {
                            TradeDetailsList.RemoveAt(0);
                        }


                    }
                    fp_applog("Done reading file");
                    reader.Close();
                    countValue.Text = counter.ToString();
                }
                READ_COMPLETE = true;

                TotalBuyValueTextBox.Text = total_buy_price.ToString("#0.0000");
                TotalSellValueTextBox.Text = total_sell_price.ToString("#0.0000");
                TotalTradeValueTextBox.Text = (total_buy_price + total_sell_price).ToString("#0.0000");
                TotalTradesTextBox.Text = (total_buy_qty + total_sell_qty).ToString();

            }
            catch (Exception e)
            {
                MessageBox.Show("Error while updating the table. " + e.GetType().FullName);
                fp_applog("Error while updating table: setReportDataInitial:" + e);

            }
            INITIAL_LOAD_COMPLETE = true;
        }

        /// <summary>
        /// This function will read trade download file and display its contents in a table in the window.
        /// </summary>
        public void setReportData()
        {
            fp_applog("in setReportData");
            if (!WRITE_COMPLETE)
            {
                READ_COMPLETE = true;
                return;
            }

            //This condition will do total calculation and set text of the same if previously it is not calculated.
            if (windowInvokerNotcalled == true)
            {

                if (this.IsHandleCreated)//If form loaded 
                {

                    /* Use of invoker necessary when updating gui components from a thread function.
                     * This invoker have to used if setting value in form controls
                     */
                    this.Invoke(new MethodInvoker(delegate
                    {

                        TotalBuyValueTextBox.Text = total_buy_price.ToString("#0.0000");
                        TotalSellValueTextBox.Text = total_sell_price.ToString("#0.0000");
                        TotalTradeValueTextBox.Text = (total_buy_price + total_sell_price).ToString("#0.0000");
                        TotalTradesTextBox.Text = (total_buy_qty + total_sell_qty).ToString();


                    }));
                    windowInvokerNotcalled = false;

                }
                else
                {
                    windowInvokerNotcalled = true;

                }
            }

            string date1 = DateTime.Now.ToString("ddMMyyyy");

            string path = Properties.Settings.Default.csv_folder + "Trade" + market + "_" + date1 + ".txt";
            fp_applog($"trade file:{path}");

            READ_COMPLETE = false;

            if (dataGridView1.IsHandleCreated)
            {
                dataGridView1.Invoke(new MethodInvoker(delegate
                {
                    dataGridView1.ClearSelection();
                    dataGridView1.Enabled = false;

                }));

                dataGridView1.Invoke(new MethodInvoker(delegate
                {
                    bindingSource_main.SuspendBinding();
                }));
            }

            try
            {
                lock (table)
                {
                    for (int i = 0; i < TradeDetailsList.Count; i++)
                    {
                        TradeDetails td = TradeDetailsList[i];

                        dataGridView1.Invoke(new MethodInvoker(delegate
                        {
                            var row = table.NewRow();
                            row["Seq No"] = td.seq_no;
                            row["Market"] = td.market_type;
                            row["Trade No"] = td.trade_no;
                            row["Trade Time"] = td.trade_time;
                            row["Token"] = td.token;
                            row["Trade Qty"] = td.trade_qty;
                            row["Trade Price"] = ((float)(td.trade_price) / 100.00).ToString("F2"); //td.trade_price;
                            row["B/S"] = td.buy_sell_flag;
                            row["Order No"] = td.order_no;
                            row["Branch No"] = td.branch_no;
                            row["User Id"] = td.user_id;
                            row["Client Type"] = td.client_type;
                            row["Client Code"] = td.client_code;
                            row["Custodial Participant Id"] = td.custodial_participant_id;
                            row["Remarks"] = td.remarks;
                            row["Activity Type"] = td.activity_type;
                            row["Trans Code"] = td.trans_code;
                            row["Order Time"] = td.order_time;
                            row["Book Type"] = td.book_type;
                            row["Opp Broker Id"] = td.opp_broker_id;
                            row["Ctcl Id"] = td.ctcl_id;
                            row["Member Code"] = td.member_code;
                            row["Status"] = td.status;
                            row["Symbol"] = td.symbol;
                            row["Series"] = td.series;
                            row["Instrument"] = td.instr;
                            row["Expiry Date"] = td.expiry_date;
                            row["Strike Price"] = td.strike_price;
                            row["Option Type"] = td.option_type;

                            table.Rows.InsertAt(row, 0);

                            int l = 100;
                            if (table.Rows.Count > l)
                            {
                                if (table.Rows[l]["Symbol"].ToString() != null)
                                {
                                    table.Rows.RemoveAt(l);
                                }
                            }
                        }));
                        dataGridView1.Invoke(new MethodInvoker(delegate
                        {

                            dataGridView1.ClearSelection();
                            dataGridView1.Rows[0].Selected = true;


                        }));

                    }
                }
            }
            catch (Exception e)
            {
                fp_applog("Error loading data into table. " + e);
                MessageBox.Show("Error loading data into table. " + e.GetType().FullName);
            }

            table.AcceptChanges();

            if (dataGridView1.IsHandleCreated)
            {

                dataGridView1.Invoke(new MethodInvoker(delegate
                {
                    bindingSource_main.ResumeBinding();
                }));

                if (dataGridView1.RowCount != 0)
                {
                    dataGridView1.Invoke(new MethodInvoker(delegate
                    {
                        dataGridView1.PerformLayout();

                        dataGridView1.Enabled = true;

                        dataGridView1.Rows[0].Selected = true;

                        if (dataGridView1.RowCount != 0)
                        {
                            dataGridView1.FirstDisplayedScrollingRowIndex = 0;
                        }

                    }));
                }
            }

            //To calculate the total buy and sell values.
            if (this.IsHandleCreated)//PG:if form loaded 
            {

                //This invoker have to used if setting value in form controls
                this.Invoke(new MethodInvoker(delegate
                {

                    TotalBuyValueTextBox.Text = total_buy_price.ToString("#0.0000");
                    TotalSellValueTextBox.Text = total_sell_price.ToString("#0.0000");
                    TotalTradeValueTextBox.Text = (total_buy_price + total_sell_price).ToString("#0.0000");
                    TotalTradesTextBox.Text = (total_buy_qty + total_sell_qty).ToString();

                }));
                windowInvokerNotcalled = false;

            }
            else
            {
                windowInvokerNotcalled = true;

            }


            READ_COMPLETE = true;

        }


        /// <summary>
        /// Creates a blank row table with columns to load and set the datagridview table handle in window.
        /// Alignment and formatting of column data is also decalred in this method.
        /// </summary>
        public void BlankRowTable()
        {
            try
            {
                table = dataSet.Tables.Add("TableTest");

                table.Columns.Add("Seq No", typeof(string));
                table.Columns.Add("Market", typeof(string));
                table.Columns.Add("Trade No", typeof(string));
                table.Columns.Add("Trade Time", typeof(string));
                table.Columns.Add("Token", typeof(string));
                table.Columns.Add("Trade Qty", typeof(string));
                table.Columns.Add("Trade Price", typeof(string));
                table.Columns.Add("B/S", typeof(string));
                table.Columns.Add("Order No", typeof(string));
                table.Columns.Add("Branch No", typeof(string));
                table.Columns.Add("User Id", typeof(string));
                table.Columns.Add("Client Type", typeof(string));
                table.Columns.Add("Client Code", typeof(string));
                table.Columns.Add("Custodial Participant Id", typeof(string));
                table.Columns.Add("Remarks", typeof(string));
                table.Columns.Add("Activity Type", typeof(string));
                table.Columns.Add("Trans Code", typeof(string));
                table.Columns.Add("Order Time", typeof(string));
                table.Columns.Add("Book Type", typeof(string));
                table.Columns.Add("Opp Broker Id", typeof(string));
                table.Columns.Add("Ctcl Id", typeof(string));
                table.Columns.Add("Member Code", typeof(string));
                table.Columns.Add("Status", typeof(string));
                table.Columns.Add("Symbol", typeof(string));
                table.Columns.Add("Series", typeof(string));
                table.Columns.Add("Instrument", typeof(string));
                table.Columns.Add("Expiry Date", typeof(string));
                table.Columns.Add("Strike Price", typeof(string));
                table.Columns.Add("Option Type", typeof(string));

                bindingSource_main.DataMember = table.TableName;

                dataGridView1.Columns["Trade Price"].DefaultCellStyle.Format = "n2";

                dataGridView1.Columns["Seq No"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView1.Columns["Market"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView1.Columns["Trade No"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView1.Columns["Trade Time"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView1.Columns["Token"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView1.Columns["Trade Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView1.Columns["Trade Price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView1.Columns["B/S"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView1.Columns["Order No"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView1.Columns["Branch No"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView1.Columns["User Id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView1.Columns["Client Type"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridView1.Columns["Client Code"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridView1.Columns["Custodial Participant Id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView1.Columns["Remarks"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridView1.Columns["Activity Type"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridView1.Columns["Trans Code"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView1.Columns["Order Time"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridView1.Columns["Book Type"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridView1.Columns["Opp Broker Id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridView1.Columns["Ctcl Id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView1.Columns["Member Code"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridView1.Columns["Status"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridView1.Columns["Symbol"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridView1.Columns["Series"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridView1.Columns["Instrument"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridView1.Columns["Expiry Date"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridView1.Columns["Strike Price"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView1.Columns["Option Type"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                table.Rows.Add();

            }
            catch (Exception ex)
            {
                fp_applog("Error BlankRowTable method call" + ex);
                MessageBox.Show("Error while creating table." + ex.GetType().FullName);
            }

        }



        /// <summary>
        /// Loads previously saved window position.
        /// </summary>
        public void LoadWindowPosition()
        {
            try
            {
                WindowState = Properties.Settings.Default.WindowState;
                Location = Properties.Settings.Default.WindowLocation;
                //Size = Properties.Settings.Default.WindowSize;
                fp_applog("restore location to:" + this.Location);
                fp_applog("restore size to :" + this.Size);
                fp_applog("restore state to:" + this.WindowState);

                //fp_applog();fp_applog();Console.WriteLine("this.state:" + this.WindowState);
            }
            catch (Exception ex)
            {
                fp_applog("restorePosition method call" + ex);
            }

        }

        /// <summary>
        /// Saves window position coordinates on closing the window.
        /// </summary>
        public void SaveWindowPosition()
        {
            try
            {
                if (WindowState == FormWindowState.Maximized || WindowState == FormWindowState.Normal)
                {
                    Properties.Settings.Default.WindowLocation = Location;
                    Properties.Settings.Default.WindowSize = Size;
                    Properties.Settings.Default.WindowState = WindowState;
                    fp_applog("window state condition :" + WindowState);
                }
                else
                {
                    Properties.Settings.Default.WindowLocation = RestoreBounds.Location;
                    Properties.Settings.Default.WindowSize = RestoreBounds.Size;
                    if (WindowStat.Equals("Normal"))
                    {
                        Properties.Settings.Default.WindowState = FormWindowState.Normal;

                    }
                    else if (WindowStat.Equals("Max"))
                    {
                        Properties.Settings.Default.WindowState = FormWindowState.Maximized;

                    }
                    fp_applog("window state condition :" + WindowState);

                }

                fp_applog("save size is :" + Properties.Settings.Default.WindowSize);
                fp_applog("save location is :" + Properties.Settings.Default.WindowLocation);
                fp_applog("this.location:" + this.Location);
                fp_applog("this.size :" + this.Size);
                fp_applog("this.state:" + this.WindowState);
                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                fp_applog("savePosition method call :" + ex);
            }

        }

        /// <summary>
        /// Loads previously saved column position from a file.
        /// </summary>
        public void LoadColumnPosition()
        {

        }

        /// <summary>
        /// This method will read an xml file and load the table column order saved in previous session.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="colCollection"></param>
        private static void LoadDataGridOrderFromFile(string path, DataGridViewColumnCollection colCollection)
        {
            try
            {
                if (File.Exists(path))
                {
                    lock (fileAccessLock)
                        using (FileStream fs = new FileStream(path, FileMode.Open))
                        {
                            XmlSerializer xmlSerializer = new XmlSerializer(typeof(DataGridViewColumnCollectionProxy));
                            DataGridViewColumnCollectionProxy proxy = (DataGridViewColumnCollectionProxy)xmlSerializer.Deserialize(fs);
                            proxy.SetColumnOrder(colCollection);
                        }
                }
            }
            catch (Exception ex)
            {
                fp_applog("LoadDataGridOrderFromFile method call :" + ex);
            }
        }

        private static ReaderWriterLockSlim fp_readWriteLock = new ReaderWriterLockSlim();
        private static StreamWriter fp;//LOG FILE pointer
        private static string log_file_name = "log-" + market + "-" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
        /// <summary>
        /// Method used to create logs.
        /// </summary>
        /// <param name="logMessage"></param>
        public static void fp_applog(string logMessage)
        {
            // Set Status to Locked
            fp_readWriteLock.EnterWriteLock();
            try
            {
                log_file_name = "log-" + market + "-" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";

                using (fp = File.AppendText(log_file_name))
                {
                    applog(fp, logMessage);
                }
            }
            finally
            {
                // Release lock
                fp_readWriteLock.ExitWriteLock();
            }
        }
        /// <summary>
        /// Log method which writes to file with time.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="logMessage"></param>
        public static void applog(TextWriter file, string logMessage)
        {
            file.Write($"{DateTime.Now.ToString("HH:mm:ss")}");
            file.WriteLine($": {logMessage}");
        }

        private void TradeReportWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            fp_applog("closing window");

            try
            {
                client.Dispose();
                SaveWindowPosition();
                string fileName = "ReportDataGrid.xml";
                SaveDataGridOrderToFile(fileName, dataGridView1.Columns);

                base.OnClosed(e);

                Environment.Exit(0);
            }
            catch (ThreadAbortException ex)
            {
                fp_applog("thread aborted: " + ex);
            }
            catch (Exception ex)
            {
                fp_applog("exception while closing window :" + ex);
            }
        }

        /// <summary>
        /// This method will be used to write the column order index value to xml file.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="colCollection"></param>
        private static void SaveDataGridOrderToFile(string path, DataGridViewColumnCollection colCollection)
        {
            try
            {
                lock (fileAccessLock)
                    using (FileStream fs = new FileStream(path, FileMode.Create))
                    {
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(DataGridViewColumnCollectionProxy));
                        xmlSerializer.Serialize(fs, new DataGridViewColumnCollectionProxy(colCollection));
                    }
            }
            catch (Exception ex)
            {
                fp_applog("SaveDataGridOrderToFile method call :" + ex);
            }

        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsWindow settings = new SettingsWindow(market, true);
            settings.Show();
        }

        /// <summary>
        /// This function will request for access token and trade doownload to the exchange and receive trade response and save it into trade file.
        /// </summary>
        /// <param name="market"></param>
        /// <returns></returns>
        public async Task SendAuthRequest(string market)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                var cons_key = "";
                var cons_secret = "";
                var login_url = "";
                string downloadType = "";
                var download_url = "";
                string inquiryType = "";

                //Reading settings value as per market.
                if (market == "FO")
                {
                    cons_key = Properties.Settings.Default.key_fo;
                    cons_secret = Properties.Settings.Default.secret_fo;
                    login_url = Properties.Settings.Default.login_url_fo;

                    downloadType = Properties.Settings.Default.downloadType_fo;
                    if (downloadType == "TRADES")
                    {
                        download_url = Properties.Settings.Default.trade_url_fo;
                    }
                    else if (downloadType == "ACTIONS")
                    {
                        download_url = Properties.Settings.Default.actions_url_fo;
                    }

                    inquiryType = Properties.Settings.Default.inquiryType_fo;
                }
                else if (market == "CM")
                {
                    cons_key = Properties.Settings.Default.key_cm;
                    cons_secret = Properties.Settings.Default.secret_cm;
                    login_url = Properties.Settings.Default.login_url_cm;
                    downloadType = Properties.Settings.Default.downloadType_cm;
                    if (downloadType == "TRADES")
                    {
                        download_url = Properties.Settings.Default.trade_url_cm;
                    }
                    else if (downloadType == "ACTIONS")
                    {
                        download_url = Properties.Settings.Default.actions_url_cm;
                    }
                    inquiryType = Properties.Settings.Default.inquiryType_cm;

                }
                else if (market == "CD")
                {
                    cons_key = Properties.Settings.Default.key_cd;
                    cons_secret = Properties.Settings.Default.secret_cd;
                    login_url = Properties.Settings.Default.login_url_cd;
                    downloadType = Properties.Settings.Default.downloadType_cd;
                    if (downloadType == "TRADES")
                    {
                        download_url = Properties.Settings.Default.trade_url_cd;
                    }
                    else if (downloadType == "ACTIONS")
                    {
                        download_url = Properties.Settings.Default.actions_url_cd;
                    }
                    inquiryType = Properties.Settings.Default.inquiryType_cd;

                }

                fp_applog("Login URL:  " + login_url);
                fp_applog("Download URL: " + download_url);
                fp_applog("Download Type: " + downloadType);
                fp_applog("Inquiry Type: " + inquiryType);

                fp_applog("key:" + cons_key + " secret:" + cons_secret + " url:" + login_url);


                client = new HttpClient();

                //Creating a base64 encoding of cons_key and cons_secrect.
                byte[] authToken = Encoding.ASCII.GetBytes($"{cons_key}:{cons_secret}");

                //Setting Authorization header.
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                        Convert.ToBase64String(authToken));

                fp_applog("authToken:" + authToken);

                //Creating nonce header.
                var datetime = DateTime.Now.ToString("ddMMyyyyHHmmss");
                datetime += "000";
                fp_applog("datetime:" + datetime);

                var num = "123456";

                //Creating base64 encoding for nonce header.
                var authNonce = Encoding.ASCII.GetBytes($"{datetime}:{num}");
                fp_applog("authNonce:" + authNonce.ToString());

                client.DefaultRequestHeaders.Add("nonce", Convert.ToBase64String(authNonce));

                string _ContentType = "application/x-www-form-urlencoded";
                fp_applog("Content type:" + _ContentType);

                var body = "grant_type=client_credentials";
                fp_applog("body:" + body);
                var data = new StringContent(body, Encoding.UTF8, _ContentType);


                fp_applog("client:" + client);
                fp_applog("client headers" + client.DefaultRequestHeaders);

                Invoke(new MethodInvoker(delegate
                {
                    statusDisplay.Text = "Requesting Token";
                }));

                HttpResponseMessage result = await client.PostAsync(login_url, data);

                string content = await result.Content.ReadAsStringAsync();

                fp_applog("" + result);

                fp_applog($"Response Received = {content}");
                Invoke(new MethodInvoker(delegate
                {
                    statusDisplay.Text = "Token Received";
                }));

                MessageBox.Show("Login Response Received ");

                ResponseToken rt = JsonConvert.DeserializeObject<ResponseToken>(content);

                fp_applog($"Respnse: access_token:{rt.access_token} token_type:{rt.token_type} expires_in:{rt.expires_in} scope:{rt.scope}");

                if (market == "FO")
                    Properties.Settings.Default.access_token_fo = rt.access_token;
                else if (market == "CM")
                    Properties.Settings.Default.access_token_cm = rt.access_token;
                else if (market == "CD")
                    Properties.Settings.Default.access_token_cd = rt.access_token;

                Properties.Settings.Default.Save();

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", rt.access_token);

                string member_code = Properties.Settings.Default.member_code;
                string date = DateTime.Now.ToString("yyyyMMdd");

                string tradeInquiryName = "";
                string actionsInquiryName = "";
                if (inquiryType == "TM")
                {
                    tradeInquiryName = "TMTRADES";
                    actionsInquiryName = "TMACTIONS";
                }
                else
                {
                    tradeInquiryName = "ALL";
                    actionsInquiryName = "ALL";
                }

                string date1 = DateTime.Now.ToString("ddMMyyyy");
                string path = "";
                string path2 = "";
                string inquiryDetails = "";
                if (downloadType == "TRADES")
                {
                    inquiryDetails = tradeInquiryName;
                    path = Properties.Settings.Default.csv_folder + "\\" + "Trade" + market + "_" + date1 + ".txt";
                    path2 = Properties.Settings.Default.csv_folder + "\\" + "Trade" + market + "_" + date1 + "_OLD.txt";
                }
                else if (downloadType == "ACTIONS")
                {
                    inquiryDetails = actionsInquiryName;
                    path = Properties.Settings.Default.csv_folder + "\\" + "Action" + market + "_" + date1 + ".txt";
                }


                while (true) //dowwnload loop
                {
                    int seq_no = maxReadSeqNo;


                    string inquiry_string = "";
                    if (seq_no <= 0)
                    {
                        seq_no = 1;
                        inquiry_string = "0," + inquiryDetails + ",,";

                    }
                    else
                    {
                        inquiry_string = seq_no + "," + inquiryDetails + ",,";
                    }

                    msgIdCounter++;
                    string msgId = member_code + date + msgIdCounter.ToString().PadLeft(7, '0');

                    fp_applog("msgId:" + msgId + " inquiry:" + inquiry_string);

                    var json_payload = "";

                    if (downloadType == "TRADES")
                    {
                        json_payload = @"{ ""version"":""1.0"",
                                   ""data"":{
                                   ""msgId"":""" + msgId + @""",
                                   ""dataFormat"":""CSV:CSV"",
                                   ""tradesInquiry"":""" + inquiry_string + @"""
                                   }
                                 }";


                    }
                    else if (downloadType == "ACTIONS")
                    {
                        json_payload = @"{ ""version"":""1.0"",
                                   ""data"":{
                                   ""msgId"":""" + msgId + @""",
                                   ""dataFormat"":""CSV:CSV"",
                                   ""actionsInquiry"":""" + inquiry_string + @"""
                                   }
                                 }";


                    }
                    fp_applog("jsonpayload:" + json_payload);

                    if (DEBUG)
                    {
                        File.AppendAllText(debug_logs, DateTime.Now.ToString("HH:mm:ss: ") + json_payload);
                    }


                    _ContentType = "application/json";
                    data = new StringContent(json_payload, Encoding.UTF8, _ContentType);

                    fp_applog("client: " + client);
                    fp_applog("client headers: " + client.DefaultRequestHeaders);

                    statusDisplay.Text = "Request Sent";
                    result = await client.PostAsync(download_url, data);

                    content = await result.Content.ReadAsStringAsync();


                    if (DEBUG)
                    {
                        File.AppendAllText(debug_logs, DateTime.Now.ToString("HH:mm:ss: ") + "content:" + content);
                    }


                    fp_applog("Download Response Received");
                    statusDisplay.Text = "Download Response Received";

                    if (result.StatusCode == System.Net.HttpStatusCode.OK)
                    {

                        TradeResponse tradeResponse = new TradeResponse();
                        tradeResponse.data = new TData();
                        tradeResponse = JsonConvert.DeserializeObject<TradeResponse>(content);



                        if (tradeResponse.messages.code != "01010000")
                        {
                            fp_applog("Error could not receive trade data." + tradeResponse.messages.code);
                            MessageBox.Show("Error could not receive trade data. Error Code:" + tradeResponse.messages.code);
                        }
                        else
                        {
                            while (!READ_COMPLETE)
                            {
                                //wait

                            }
                            using (StreamWriter sw = File.AppendText(path))
                            {
                                WRITE_COMPLETE = false;
                                string tradeDataDetails = tradeResponse.data.tradesInquiry.Substring(0, tradeResponse.data.tradesInquiry.IndexOf("^"));
                                string output = tradeResponse.data.tradesInquiry.Substring(tradeResponse.data.tradesInquiry.IndexOf('^') + 1);

                                long maxSeqNoInResponse = long.Parse(tradeDataDetails.Split(',')[4]);
                                int noOfRec = int.Parse(tradeDataDetails.Split(',')[5]);

                                fp_applog("Max received seq no in response : " + maxSeqNoInResponse + " No. of records: " + noOfRec);

                                string csv_data = output.Replace('^', '\n');

                                string line = "";



                                var sr = new StringReader(csv_data);

                                while ((line = sr.ReadLine()) != null)
                                {
                                    if (downloadType == "TRADES")
                                    {
                                        TradeDetails td = new TradeDetails(line, market);

                                        maxReadSeqNo = td.seq_no;

                                        if (OLD_FORMAT == false)
                                        {
                                            //This will save trade detailsin file with format specified in API.
                                            sw.WriteLine(line);
                                        }
                                        else
                                        {
                                            if (OLD_FORMAT && NEW_FORMAT)
                                            {
                                                File.AppendAllText(path2, line + Environment.NewLine);
                                            }

                                            string trade_entry = "";
                                            if (market == "FO" || market == "CD")
                                            {
                                                trade_entry = td.trade_no + "," + td.status + "," + td.instr + "," + td.symbol + "," +
                                                                td.expiry_date + "," + td.strike_price + "," + td.option_type + "," + td.contract_details + "," +
                                                                td.book_type + "," + td.book_type_name + "," + td.market_type + "," + td.user_id + "," + td.branch_no + "," + td.buy_sell_flag + "," +
                                                                td.trade_qty + "," + ((double)(td.trade_price / 100.00)).ToString("F2") + "," + td.client_type + "," + td.client_code + "," + td.custodial_participant_id + "," +
                                                                "OPEN," + "UNCOVER," + td.trade_time + "," + td.trade_time + "," + td.order_no + ",NIL," + td.order_time + "," +
                                                                td.ctcl_id;
                                            }
                                            else if (market == "CM")
                                            {
                                                trade_entry = td.trade_no + "," + td.status + "," + td.symbol + "," + td.instr + "," +
                                                              td.sec_name + ",0," +
                                                              td.book_type + "," + td.market_type + "," + td.user_id + "," + td.branch_no + "," + td.buy_sell_flag + "," +
                                                                td.trade_qty + "," + ((double)(td.trade_price / 100.00)).ToString("F2") + "," + td.client_type + "," + td.client_code + "," + td.custodial_participant_id + "," +
                                                                td.trade_settlement_type + "," + td.auction_number + ",7," + td.trade_time + "," + td.trade_time + "," + td.order_no + ",NIL," + td.order_time + "," +
                                                                td.ctcl_id;
                                            }
                                            sw.WriteLine(trade_entry);
                                        }


                                        TradeDetailsList.Add(td);
                                        counter++;

                                        //set total calculation 
                                        if (td.buy_sell_flag == "1")
                                        {
                                            total_buy_price += (td.trade_price * td.trade_qty) / 100.00;
                                            total_buy_qty += td.trade_qty;
                                        }
                                        else if (td.buy_sell_flag == "2")
                                        {
                                            total_sell_price += (td.trade_price * td.trade_qty) / 100.00;
                                            total_sell_qty += td.trade_qty;
                                        }


                                        int limit = Properties.Settings.Default.record_limit;

                                        /* Maintain array of x size 
                                         * Insert element  in list, check size if greater than required to be shown remove last element.
                                         */
                                        if (TradeDetailsList.Count > limit)
                                        {
                                            TradeDetailsList.RemoveAt(0);

                                        }


                                    }
                                    else if (downloadType == "ACTIONS")
                                    {
                                        sw.WriteLine(line);
                                        maxReadSeqNo = int.Parse(line.Split(',')[1]);
                                    }
                                }

                                /* Saving last-segment-seq-no-date.txt file as per market.
                                 * This file will be read on app restart to fetch last received sequence number and msgId.
                                 */
                                string file_name = "last-" + market + "-" + "seq-no-" + date1 + ".txt";
                                string last_recv_seq_no_msgId = maxReadSeqNo.ToString() + "," + msgIdCounter.ToString();
                                fp_applog("Last received sequence number: " + maxReadSeqNo.ToString() + " Total Count:" + counter);
                                File.WriteAllText(file_name, last_recv_seq_no_msgId);

                                lastUpdatedTime.Text = DateTime.Now.ToString();
                                sw.Flush();
                                sw.Close();
                            }
                            countValue.Text = counter.ToString();
                            WRITE_COMPLETE = true;
                        }
                    }
                    else
                    {
                        fp_applog("Received error for trade download request.Status Code:" + result);
                        MessageBox.Show("Received error for trade download request. Status Code:" + result.StatusCode);
                    }
                    int sleeptime = Properties.Settings.Default.trade_interval * 1000;
                    Thread.Sleep(sleeptime);

                }
            }
            catch (Exception e)
            {
                fp_applog("SendAuthRequest: " + e);
                MessageBox.Show("Error recevied: " + e.GetType().FullName);
            }
        }

        /// <summary>
        /// This method will read the last received trade sequence number so that 
        /// it can be used in manking new trade dwnload request.
        /// </summary>
        /// <returns></returns>
        public int getMaxSeqNoFromFile()
        {
            try
            {
                string date1 = DateTime.Now.ToString("ddMMyyyy");


                string path = "last-" + market + "-" + "seq-no-" + date1 + ".txt";

                if (File.Exists(path))
                {
                    var line = File.ReadLines(path).Last();
                    if (line.Trim() != "")
                    {

                        int max_seq_no = int.Parse(line.Split(',')[0]);
                        msgIdCounter = int.Parse(line.Split(',')[1]);
                        return max_seq_no;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    fp_applog("Trade file not found. Max sequence number zero.");
                    return 0;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error while reading max sequence number from file. " + e.GetType().FullName);
                fp_applog("Error while reading max sequence number from file. " + e);
            }
            return 0;
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            //Calling function refresh table data.
            setReportData();
        }
    }

    public class ResponseToken
    {
        public string access_token;
        public string token_type;
        public string expires_in;
        public string scope;

    }

    public class TradeResponse
    {
        public string status;
        public TMessages messages;
        public TData data;
    }

    public struct TMessages
    {
        public string code;
    }

    public struct TData
    {
        public string msgId;
        public string tradesInquiry;
    }

    [Serializable]
    public class DataGridViewColumnProxy
    {
        string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        int _index;

        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }

        public DataGridViewColumnProxy(DataGridViewColumn column)
        {
            this._name = column.DataPropertyName;
            this._index = column.DisplayIndex;
        }
        public DataGridViewColumnProxy()
        {
        }
    }
    [Serializable]
    public class DataGridViewColumnCollectionProxy
    {
        List<DataGridViewColumnProxy> _columns = new List<DataGridViewColumnProxy>();
        public List<DataGridViewColumnProxy> Columns
        {
            get { return _columns; }
            set { _columns = value; }
        }
        public DataGridViewColumnCollectionProxy(DataGridViewColumnCollection columnCollection)
        {
            foreach (var col in columnCollection)
            {
                if (col is DataGridViewColumn)
                    _columns.Add(new DataGridViewColumnProxy((DataGridViewColumn)col));
            }
        }
        public DataGridViewColumnCollectionProxy()
        {
        }

        /// <summary>
        /// This method will be used to set column order from the collectio created by reading column order file.
        /// </summary>
        /// <param name="columnCollection"></param>
        public void SetColumnOrder(DataGridViewColumnCollection columnCollection)
        {

            for (int i = 0; i < columnCollection.Count; i++)
            {
                foreach (var col in columnCollection)
                {

                    if (col is DataGridViewColumn)
                    {

                        DataGridViewColumn column = (DataGridViewColumn)col;
                        DataGridViewColumnProxy proxy = this._columns.FirstOrDefault(p => p.Name == column.DataPropertyName);
                        if (proxy != null)
                        {
                            if (i == proxy.Index)
                            {

                                column.DisplayIndex = proxy.Index;
                            }

                        }
                    }
                }

            }
        }
    }

}
