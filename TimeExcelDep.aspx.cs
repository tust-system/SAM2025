using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SAM2025
{
    public partial class TimeExcel : System.Web.UI.Page
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

                    //部門總覽
                    LinkButton1.PostBackUrl = $"~/Default.aspx?DepID={encodedDepId}&DepName={encodedDepName}";
                    //打卡記錄上傳
                    LinkButton2.PostBackUrl = $"~/TimeExcelInput.aspx?DepID={encodedDepId}&DepName={encodedDepName}";
                    
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {

        }
    }
}