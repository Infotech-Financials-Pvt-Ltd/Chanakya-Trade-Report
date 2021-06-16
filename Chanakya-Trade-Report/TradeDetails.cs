using System;
using System.Windows.Forms;

namespace Chanakya_Trade_Report
{
    /// <summary>
    /// Class <c>TradeDetails</c> Decodes received trade details and saves them into object in Packet structure with respect to market.
    /// </summary>
    public class TradeDetails
    {
        //Packet structure
        public int seq_no;
        public string trade_date;
        public string market_type;
        public string trade_no;
        public string trade_time;
        public int token;
        public int trade_qty;
        public int trade_price;
        public string buy_sell_flag;
        public string order_no;
        public string branch_no;
        public string user_id;
        public string client_type;
        public string client_code;
        public string custodial_participant_id;
        public string remarks;
        public short activity_type;
        public short trans_code;
        public string order_time;
        public short book_type;
        public string auction_number;//CM
        public string trade_settlement_type;//CM
        public string opp_broker_id;
        public int trade_trigger_price;
        public string ctcl_id;
        public string order_institution;
        public string sec_identifier;
        public string member_code;
        public string status;
        public string symbol;
        public string series;
        public string sec_name;
        public string instr;
        public string expiry_date;
        public string strike_price;
        public string option_type;
        //Packet structure ends

        public string contract_details;
        public string book_type_name;

        /// <summary>
        /// This method will be used to read Trade detaild file when app starts to 
        /// read and save price and qty details for total calculations.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="market"></param>
        /// <param name="initial_load"></param>
        public TradeDetails(string line, string market, bool initial_load)
        {
            try
            {
                if (market == "FO" || market == "CD")
                {
                    buy_sell_flag = line.Split(',')[13];
                    trade_qty = int.Parse(line.Split(',')[14]);
                    trade_price = (int)(double.Parse(line.Split(',')[15]) * 100);
                }
                else if (market == "CM")
                {
                    buy_sell_flag = line.Split(',')[10];
                    trade_qty = int.Parse(line.Split(',')[11]);
                    trade_price = (int)(double.Parse(line.Split(',')[12]) * 100);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error reading trade file. Line:" + line);
                TradeReportWindow.fp_applog("Error reading trade file. Line:" + line + " Exception:" + e);
            }
        }

        /// <summary>
        /// This method will be used to read and decode trade details from packet received on network
        /// and save in class object.
        /// </summary>
        /// <param name="line"></param>
        /// <param name="market"></param>
        public TradeDetails(string line, string market)
        {
            try
            {
                seq_no = int.Parse(line.Split(',')[0]);
                market_type = line.Split(',')[1];
                trade_no = line.Split(',')[2];
                trade_time = line.Split(',')[3];
                token = int.Parse(line.Split(',')[4]);
                trade_qty = int.Parse(line.Split(',')[5]);
                trade_price = int.Parse(line.Split(',')[6]);
                buy_sell_flag = line.Split(',')[7];
                order_no = line.Split(',')[8];
                branch_no = line.Split(',')[9];
                user_id = line.Split(',')[10];
                client_type = line.Split(',')[11];
                client_code = line.Split(',')[12];
                custodial_participant_id = line.Split(',')[13];
                remarks = line.Split(',')[14];
                activity_type = short.Parse(line.Split(',')[15]);
                trans_code = short.Parse(line.Split(',')[16]);
                order_time = line.Split(',')[17];
                book_type = short.Parse(line.Split(',')[18]);

                if (market == "FO" || market == "CD")
                {
                    opp_broker_id = line.Split(',')[19];
                    ctcl_id = line.Split(',')[20];
                    member_code = line.Split(',')[21];
                    status = line.Split(',')[22];
                    symbol = line.Split(',')[23];
                    series = line.Split(',')[24];
                    instr = line.Split(',')[25];
                    expiry_date = line.Split(',')[26];
                    strike_price = (float.Parse(line.Split(',')[27]) / 100.00).ToString("F2");
                    option_type = line.Split(',')[28];

                    string temp_expiry = DateTimeFunctions.UnixTimeStampToDateTime1980(double.Parse(line.Split(',')[26])).ToString("ddMMMyyyy");
                    expiry_date = temp_expiry.Split(' ')[0].ToUpper();

                    int day = int.Parse(DateTimeFunctions.UnixTimeStampToDateTime1980(double.Parse(line.Split(',')[26])).ToString("dd"));
                    int month = int.Parse(DateTimeFunctions.UnixTimeStampToDateTime1980(double.Parse(line.Split(',')[26])).ToString("MM"));
                    int year = int.Parse(DateTimeFunctions.UnixTimeStampToDateTime1980(double.Parse(line.Split(',')[26])).ToString("yyyy"));
                    DateTime d = new DateTime(year, month, day);
                    var last = DateTimeFunctions.GetLastThursdayOfTheMonth(d);

                    TimeSpan diff = last.Subtract(d);

                    if (diff.Days == 0)
                    {
                        temp_expiry = DateTimeFunctions.UnixTimeStampToDateTime1980(double.Parse(line.Split(',')[26])).ToString("yMMM");
                        temp_expiry = temp_expiry.Split(' ')[0].ToUpper();
                        int temp_strike = int.Parse(line.Split(',')[27]) / 100;

                        if (instr.Contains("OPT"))
                        {
                            contract_details = symbol + temp_expiry + temp_strike + option_type;
                        }
                        else if (instr.Contains("FUT"))
                        {
                            strike_price = " ";
                            option_type = " ";
                            contract_details = symbol + temp_expiry + "FUT";
                        }
                    }
                    else if (diff.Days >= 7)
                    {
                        temp_expiry = DateTimeFunctions.UnixTimeStampToDateTime1980(double.Parse(line.Split(',')[26])).ToString("yMdd");
                        temp_expiry = temp_expiry.Split(' ')[0].ToUpper();
                        int temp_strike = int.Parse(line.Split(',')[27]) / 100;

                        if (instr.Contains("OPT"))
                        {
                            contract_details = symbol + temp_expiry + temp_strike + option_type;
                        }
                        else if (instr.Contains("FUT"))
                        {
                            contract_details = symbol + temp_expiry + "FUT";
                        }
                    }


                }
                else if (market == "CM")
                {
                    auction_number = line.Split(',')[19];
                    trade_settlement_type = line.Split(',')[20];
                    opp_broker_id = line.Split(',')[21];
                    trade_trigger_price = int.Parse(line.Split(',')[22]);
                    ctcl_id = line.Split(',')[23];
                    order_institution = line.Split(',')[24];
                    sec_identifier = line.Split(',')[25];
                    symbol = line.Split(',')[26];
                    series = line.Split(',')[27];
                    sec_name = line.Split(',')[28];
                    instr = line.Split(',')[29];
                    member_code = line.Split(',')[30];

                    trade_no = trade_no.Substring(8);

                }


                trade_time = DateTimeFunctions.getDateTimeFromJiffy(long.Parse(line.Split(',')[3])).ToUpper();
                order_time = DateTimeFunctions.UnixTimeStampToDateTime1980(double.Parse(line.Split(',')[17])).ToString("dd MMM yyyy HH:mm:ss").ToUpper();

                if (trans_code == 6001)
                    status = "11";
                else if (trans_code == 5445)
                    status = "12";
                else if (trans_code == 5440)
                    status = "13";
                else if (trans_code == 5530)
                    status = "16";

                if (book_type == 1)
                    book_type_name = "RL";
                else if (book_type == 2)
                    book_type_name = "ST";
                else if (book_type == 3)
                    book_type_name = "SL";
                else if (book_type == 4)
                    book_type_name = "NT";
                else if (book_type == 5)
                    book_type_name = "OL";
                else if (book_type == 6)
                    book_type_name = "Spot";
                else if (book_type == 7)
                    book_type_name = "Auction";
                else if (book_type == 11)
                    book_type_name = "Call Auction 1";
                else if (book_type == 12)
                    book_type_name = "Call Auction 2";

            }
            catch (Exception e)
            {
                MessageBox.Show("Error while decoding trade details:" + e.GetType().FullName);
                TradeReportWindow.fp_applog("Error while decoding trade details:" + e);
            }
        }

    }

    /// <summary>
    /// All date and time related functions are defined here.
    /// </summary>
    public class DateTimeFunctions
    {
        /// <summary>
        /// This Function provides Unix timestamp in seconds past epoch from 1970.
        /// </summary>
        /// <param name="unixTimeStamp"></param>
        /// <returns></returns>
        public static DateTime UnixTimeStampToDateTime1970(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch from 1970
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);

            dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        /// <summary>
        /// This Function provides Unix timestamp in seconds past epoch from 1980.
        /// NSE Epoch Time are from 1980.
        /// </summary>
        /// <param name="unixTimeStamp"></param>
        /// <returns></returns>
        public static DateTime UnixTimeStampToDateTime1980(double unixTimeStamp)
        {

            // Unix timestamp is seconds past epoch from 1980
            System.DateTime dtDateTime = new DateTime(1980, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Local);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        /**This method is used to convert the Jiffy format date to readable format
	 * @param dtVal
	 * @return
	 */
        public static string getDateTimeFromJiffy(long dtVal)
        {
            long retDate = (int)(dtVal / 65536L) + 315513000L;
            var datetime = retDate * 1000;

            string dt = UnixTimeStampToDateTime1970(datetime).ToString("dd MMM yyyy HH:mm:ss");

            return dt;
        }

        /// <summary>
        /// To get the expiry date of last thursday of the month.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime GetLastThursdayOfTheMonth(DateTime date)
        {
            var lastDayOfMonth = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));

            while (lastDayOfMonth.DayOfWeek != DayOfWeek.Thursday)
                lastDayOfMonth = lastDayOfMonth.AddDays(-1);

            return lastDayOfMonth;
        }
    }
}
