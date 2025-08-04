using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SAM2025
{
    public partial class _Default : Page
    {
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
                    
                    LinkButton1.PostBackUrl = $"~/TimePeriod.aspx?DepID={encodedDepId}&DepName={encodedDepName}"; //班段查詢
                    LinkButton2.PostBackUrl = $"~/TimeExcelDep.aspx?DepID={encodedDepId}&DepName={encodedDepName}"; //打卡記錄
                    LinkButton3.PostBackUrl = $"~/EmpPCode.aspx?DepID={encodedDepId}&DepName={encodedDepName}"; //人員班段設定
                }
            }
        }
    }
}