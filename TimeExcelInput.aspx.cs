using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;

namespace SAM2025
{
    public partial class TimeExcelInput : System.Web.UI.Page
    {
        private DateTime dtmNow = DateTime.Now;
        private DBUtil db = new DBUtil();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string depId = Request.QueryString["DepID"];
                string depName = Request.QueryString["DepName"];

                if (!string.IsNullOrEmpty(depId))
                {
                    lblDepartment.Text = $"部門代碼：{depId}<br/>部門名稱：{depName}";

                    // ✅ 帶入部門代號與名稱
                    string encodedDepId = Server.UrlEncode(depId);
                    string encodedDepName = Server.UrlEncode(depName);

                    //部門總覽
                    LinkButton1.PostBackUrl = $"~/Default.aspx?DepID={encodedDepId}&DepName={encodedDepName}";
                    //打卡記錄查詢
                    LinkButton2.PostBackUrl = $"~/TimeExcelDep.aspx?DepID={encodedDepId}&DepName={encodedDepName}";
                }
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            //列號
            int iExcelMN = 1;

            try
            {
                if (FileUpload1.HasFile)
                {
                    string guid = Guid.NewGuid().ToString();
                    string sourcefilename = this.FileUpload1.FileName;

                    //檔名解析
                    char[] delimiterChars = { '_' };

                    //string text = "one\ttwo three:four,five six seven";
                    //System.Console.WriteLine("Original text: '{0}'", text);
                    string[] words = sourcefilename.Split(delimiterChars);
                    //日期開始
                    string sSdate = words[2].ToString();
                    DateTime dSdate = DateTime.Now;
                    if (sSdate != "")
                    {
                        dSdate = (DateTime)Convert.ToDateTime(sSdate);
                    }
                    //日期結束
                    string sEdate = words[3].ToString();
                    DateTime dEdate = DateTime.Now;
                    if (sEdate != "")
                    {
                        dEdate = (DateTime)Convert.ToDateTime(sEdate);
                    }


                    // Difference in days, hours, and minutes.
                    TimeSpan ts = dEdate - dSdate;
                    // Difference in days.
                    int differenceInDays = ts.Days;

                    string filename = dtmNow.ToString("yyyyMMddHHmmss") + "_新北卡鐘資料_" + this.FileUpload1.FileName;

                    //存至Disk   
                    this.FileUpload1.SaveAs(Server.MapPath(string.Format(@"UpLoad\" + filename.ToString() + "", guid, Path.GetExtension(filename))));


                    DateTime dtNow = DateTime.Now;

                    //定義OleDb======================================================
                    //1.檔案位置    注意絕對路徑 -> 非 \  是 \\
                    //string FileName = @"D:\WebAP\SAM2\Upload\" + filename + "";

                    string uploadFolder = Server.MapPath("~/Upload/");
                    string filePath = Path.Combine(uploadFolder, filename);
                    string FileName = filePath;

                    string ProviderName = "Microsoft.ACE.OLEDB.12.0;";
                    //3.Excel版本，Excel 8.0 針對Excel2000及以上版本，Excel5.0 針對Excel97。
                    string ExtendedString = "'Excel 8.0;";
                    //4.第一行是否為標題
                    string Hdr = "Yes;";
                    //5.IMEX=1 通知驅動程序始終將「互混」數據列作為文本讀取
                    string IMEX = "0';";
                    //=============================================================

                    //連線字串
                    string cs =
                            "Data Source=" + FileName + ";" +
                            "Provider=" + ProviderName +
                            "Extended Properties=" + ExtendedString +
                            "HDR=" + Hdr +
                            "IMEX=" + IMEX;
                    //Excel 的工作表名稱 (Excel左下角有的分頁名稱)
                    //string SheetName = "考勤異常表";
                    string SheetName = "打卡記錄表";

                    //OleDbConnection conn = new OleDbConnection(cs);
                    //conn.Open();
                    //string qs = "select * from [" + SheetName + "$]";
                    //OleDbDataAdapter dr = new OleDbDataAdapter(qs, conn);
                    //DataTable dt = new DataTable();
                    //DataSet ds = new DataSet();
                    //dr.Fill(ds, "SelTableA");

                    using (OleDbConnection conn = new OleDbConnection(cs))
                    {
                        conn.Open();
                        string qs = "select * from [" + SheetName + "$]";
                        OleDbDataAdapter dr = new OleDbDataAdapter(qs, conn);
                        DataTable dt = new DataTable();
                        dr.Fill(dt);

                        // === 尋找實際資料 header ===
                        int headerRowIndex = -1;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Rows[i][0].ToString().Contains("姓名"))
                            {
                                headerRowIndex = i;
                                break;
                            }
                        }
                        if (headerRowIndex == -1)
                        {
                            labError.Text += "找不到標題列\n";
                            return;
                        }

                        // 取標題列之後的資料
                        DataTable cleanDt = dt.AsEnumerable()
                            .Skip(headerRowIndex + 1)
                            .CopyToDataTable();

                        // 第1欄: 姓名、第2欄: 員編、第4欄以後是打卡紀錄
                        for (int r = 0; r < cleanDt.Rows.Count; r++)
                        {
                            string name = cleanDt.Rows[r][0].ToString().Trim();
                            string empid = cleanDt.Rows[r][1].ToString().Trim();

                            for (int c = 3; c < cleanDt.Columns.Count; c++)
                            {
                                string raw = cleanDt.Rows[r][c].ToString().Trim();

                                if (string.IsNullOrEmpty(raw) || raw == "-") continue;

                                string[] times = raw.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                                List<TimeSpan> timeList = new List<TimeSpan>();

                                foreach (string t in times)
                                {
                                    if (TimeSpan.TryParse(t.Trim(), out TimeSpan tis))
                                        timeList.Add(tis);
                                }

                                if (timeList.Count == 0) continue;

                                TimeSpan minTime = timeList.Min();
                                TimeSpan maxTime = timeList.Max();

                                // 取得日期（從表頭）
                                //string rawDate = dt.Rows[headerRowIndex][c].ToString().Trim();

                                int dateStartColumn = 3; // 從第4欄開始為日期資料
                                int totalCols = dt.Columns.Count;

                                // 抓日期欄位（用 c）
                                string rawDate = dt.Columns[c].ColumnName.Trim();

                                Match match = Regex.Match(rawDate, @"\d{4}年\d{1,2}月[ _]?\d{1,2}日");
                                if (match.Success)
                                {
                                    string dateStr = match.Value
                                        .Replace("年", "-")
                                        .Replace("月_", "-")
                                        .Replace("日", "");

                                    if (DateTime.TryParseExact(dateStr, "yyyy-M-d", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                                    {
                                        using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["SAMConnectionString"].ToString()))
                                        {
                                            string insert = "INSERT INTO [EmpCodeUP] ([EmpID], [Date], [InTime], [OutTime]) " +
                                                            "VALUES (@EmpID, @Date, @InTime, @OutTime)";

                                            SqlCommand cmd = new SqlCommand(insert, sqlConn);
                                            cmd.Parameters.AddWithValue("@EmpID", empid);
                                            cmd.Parameters.AddWithValue("@Date", date.ToString("yyyy-MM-dd"));
                                            cmd.Parameters.AddWithValue("@InTime", minTime);
                                            cmd.Parameters.AddWithValue("@OutTime", maxTime);

                                            sqlConn.Open();
                                            cmd.ExecuteNonQuery();
                                            sqlConn.Close();
                                        }
                                    }
                                }
                            }
                        }
                    }

                    labError.Text += "上傳完成，資料已匯入。";

                    using (SqlConnection connSP = new SqlConnection(ConfigurationManager.ConnectionStrings["SAMConnectionString"].ToString()))
                    {
                        SqlCommand cmd = new SqlCommand("usp_UpdateEmpCodeStatus", connSP);
                        cmd.CommandType = CommandType.StoredProcedure;
                        connSP.Open();
                        cmd.ExecuteNonQuery();
                        connSP.Close();
                    }
                }

                labError.Text += "異常記錄完成。";
            }
            catch (Exception ex)
            {
                labError.Text += "列號：" + iExcelMN.ToString() + "  " + ex.Message.ToString();
            }
        }

        /// <summary>
        /// SQL存檔處理
        /// </summary>
        /// <param name="sEmpName">員工姓名</param>
        /// <param name="sEmpID">員工編號</param>
        /// <param name="sWorkKind">部門</param>
        /// <param name="sWorkDate">日期</param>
        /// <param name="sLeaveCode">假況代號</param>
        /// <param name="iNGAcc">假況時數</param>
        private void SQLProce(string sEmpName, string sEmpID, string sWorkKind, string sWorkDate, string sLeaveCode, double dNGAcc, string sid)
        {
            SqlConnection SqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["SAMConnectionString"].ConnectionString);

            //新增
            //先寫入order SUB 檔
            string strSQL = "INSERT INTO DayCardTime " +
                        "(" +
                        "[EmpName], " +
                        "[EmpID], " +
                        "[WorkKind], " +
                        "[WorkDate], " +
                        "[CardTimeCode], " +
                        "[CardTime]," +
                        "[CardTimeCodeEdit], " +
                        "[CardTimeEdit], " +
                        "[sid] " +
                        ") " +
                        " VALUES " +
                        "(" +
                        "@EmpName, " +
                        "@EmpID, " +
                        "@WorkKind, " +
                        "@WorkDate, " +
                        "@CardTimeCode, " +
                        "@CardTime," +
                        "@CardTimeCodeEdit, " +
                        "@CardTimeEdit, " +
                        "@sid " +
                        ")";

            SqlCommand cmdSQL = new SqlCommand(strSQL, SqlConn);


            cmdSQL.Parameters.AddWithValue("@EmpName", sEmpName);
            cmdSQL.Parameters.AddWithValue("@EmpID", sEmpID);
            cmdSQL.Parameters.AddWithValue("@WorkKind", sWorkKind);
            cmdSQL.Parameters.AddWithValue("@WorkDate", sWorkDate);
            cmdSQL.Parameters.AddWithValue("@CardTimeCode", sLeaveCode);
            cmdSQL.Parameters.AddWithValue("@CardTime", dNGAcc);
            cmdSQL.Parameters.AddWithValue("@CardTimeCodeEdit", sLeaveCode);
            cmdSQL.Parameters.AddWithValue("@CardTimeEdit", dNGAcc);
            cmdSQL.Parameters.AddWithValue("@sid", sid);
            try
            {
                SqlConn.Open();
                cmdSQL.ExecuteNonQuery();
            }
            catch (Exception E)
            {
                labError.Text += "匯入時有問題：" + E.Message;
            }
            finally
            {
                SqlConn.Close();
                SqlConn.Dispose();
            }
        }

        private void SQLDel(string sEmpID, string sWorkDate, string sid)
        {
            SqlConnection SqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["SAMConnectionString"].ConnectionString);
            string strSQLDel = "DELETE FROM [DayCardTime] " +
                        "WHERE [EmpID] ='" + sEmpID + "' " +
                        "AND [WorkDate] ='" + sWorkDate + "' " +
                        "AND [sid] ='" + sid + "' " +
                        "";

            SqlCommand cmdSQLDel = new SqlCommand(strSQLDel, SqlConn);
            try
            {
                SqlConn.Open();
                cmdSQLDel.ExecuteNonQuery();
            }
            catch (Exception E)
            {
                labError.Text += "刪除" + sEmpID + "時有問題：" + E.Message;
            }
            finally
            {
                SqlConn.Close();
                SqlConn.Dispose();
            }
        }

        //判斷指定字串內的指定位置是否為中文字
        private bool CheckChineseString(string strInputString, int intIndexNumber)
        {
            int intCode = 0;

            //中文範圍（0x4e00 - 0x9fff）轉換成int（intChineseFrom - intChineseEnd）
            int intChineseFrom = Convert.ToInt32("4e00", 16);
            int intChineseEnd = Convert.ToInt32("9fff", 16);
            if (strInputString != "")
            {
                //取得input字串中指定判斷的index字元的unicode碼
                intCode = Char.ConvertToUtf32(strInputString, intIndexNumber);

                if (intCode >= intChineseFrom && intCode <= intChineseEnd)
                {
                    return true;     //如果是範圍內的數值就回傳true
                }
                else
                {
                    return false;    //如果是範圍外的數值就回傳true
                }
            }
            return false;
        }


        //判斷字串裡面是否有數字
        public static bool isnumeric(string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] >= 48 && text[i] <= 57)
                {
                    return true;
                }
            }
            return false;
        }
    }
}