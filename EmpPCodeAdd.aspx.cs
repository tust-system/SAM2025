using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SAM2025
{
    public partial class EmpPCodeAdd : System.Web.UI.Page
    {
        private string depId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                depId = Request.QueryString["DepID"];
                string depName = Request.QueryString["DepName"];

                if (!string.IsNullOrEmpty(depId))
                {
                    lblDepartment.Text = $"部門代碼：{depId}<br/>部門名稱：{depName}";

                    // ✅ 帶入部門代號與名稱
                    string encodedDepId = Server.UrlEncode(depId);
                    string encodedDepName = Server.UrlEncode(depName);

                    //返回部門班段清單
                    LinkButton1.PostBackUrl = $"~/TimePeriod.aspx?DepID={depId}&DepName={depName}";
                    //部門總覽
                    LinkButton2.PostBackUrl = $"~/Default.aspx?DepID={depId}&DepName={depName}";
                    //人員班段清單
                    LinkButton3.PostBackUrl = $"~/EmpPCode.aspx?DepID={depId}&DepName={depName}";
                }

                BindGridView();
                GridView1.RowDataBound += new GridViewRowEventHandler(GridView1_RowDataBound);
            }
        }

        private void BindGridView()
        {
            string connStr = ConfigurationManager.ConnectionStrings["TrustERPConnectionString"].ConnectionString;
            string sql = @"SELECT [EmpID], [EmpChiName]
                       FROM [Employee] 
                       JOIN [Department] ON [Department].[DepID] = [Employee].[EmpDep]
                       JOIN [Code] ON [CodeID] = [EmpTitle]
                       WHERE [Department].[UseFlag] = 1 
                         AND [EmpDateOut] = '1900-01-01 12:00:00.000' 
                         AND [Department].[DepID] = @DepID";

            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@DepID", depId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // 取得這列的人員資料
                string empId = DataBinder.Eval(e.Row.DataItem, "EmpID").ToString();
                string empDep = Request.QueryString["DepID"]; // 或從資料綁定欄抓 EmpDep

                // 找出 DropDownList 控制項
                DropDownList ddlShift = (DropDownList)e.Row.FindControl("ddlShift");
                if (ddlShift != null)
                {
                    // 取得班段資料
                    DataTable dtShift = GetShiftList(empDep);
                    ddlShift.DataSource = dtShift;
                    ddlShift.DataTextField = "PID";   // 顯示欄
                    ddlShift.DataValueField = "PID";    // 實際值
                    ddlShift.DataBind();

                    // 可以加預設選項
                    ddlShift.Items.Insert(0, new ListItem("--請選擇--", ""));
                }
            }
        }

        private DataTable GetShiftList(string depId)
        {
            string connStr = ConfigurationManager.ConnectionStrings["SAMConnectionString"].ConnectionString;
            string sql = @"SELECT [id], [PID] FROM [PCode] WHERE [DepID] = @DepID";

            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@DepID", depId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in GridView1.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    string empId = row.Cells[0].Text.Trim(); // EmpID
                    string empName = row.Cells[1].Text.Trim(); // EmpID
                    DropDownList ddlShift = (DropDownList)row.FindControl("ddlShift");

                    if (ddlShift != null && !string.IsNullOrEmpty(ddlShift.SelectedValue))
                    {
                        string selectedShift = ddlShift.SelectedValue;

                        // 執行儲存，例如：更新 EmployeeShift 設定
                        SaveEmployeeShift(empId, empName, selectedShift);
                    }
                }
            }

            // ✅ 顯示提示並導頁
            ClientScript.RegisterStartupScript(this.GetType(), "msg",
                "alert('儲存完成！'); window.location='EmpPCode.aspx?DepID=" + Server.UrlEncode(depId) + "&DepName=" + Server.UrlEncode(Request.QueryString["DepName"]) + "';", true);
        }

        private void SaveEmployeeShift(string empId, string empName, string shiftId)
        {
            depId = Request.QueryString["DepID"];

            string connStr = ConfigurationManager.ConnectionStrings["SAMConnectionString"].ConnectionString;
            string sql = @"
                        IF EXISTS (SELECT 1 FROM [EmpPCode] WHERE [EmpID] = @EmpID AND [DepID] = @DepID)
                            UPDATE [EmpPCode]
                            SET [PCodeID] = @PCodeID
                            WHERE [EmpID] = @EmpID AND DepID = @DepID
                        ELSE
                            INSERT INTO [EmpPCode] ([EmpID], [EmpName], [DepID], [PCodeID])
                            VALUES (@EmpID, @EmpName, @DepID, @PCodeID)";

            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@EmpID", empId);
                cmd.Parameters.AddWithValue("@EmpName", empName);
                cmd.Parameters.AddWithValue("@DepID", depId);
                cmd.Parameters.AddWithValue("@PCodeID", shiftId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}