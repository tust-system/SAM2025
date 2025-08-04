using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SAM2025
{
    public partial class TimeExcel : System.Web.UI.Page
    {
        int emp;

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
                ddlEmp();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            emp = 0;
            BindGrid();
        }

        private void ddlEmp()
        {
            DataTable dt = new DataTable();

            using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["SAMConnectionString"].ToString()))
            {
                string select = @"SELECT [SQID]
                                      ,[EmpName]
                                      ,[EmpID]
                                      ,[DepID]
                                  FROM [EmpPCode]
                                  WHERE [DepID] = @DepID";

                SqlCommand cmd = new SqlCommand(select, sqlConn);
                cmd.Parameters.AddWithValue("@DepID", Request.QueryString["DepID"].Trim());

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            ddl_Emp.DataSource= dt;
            ddl_Emp.DataTextField = "EmpName";
            ddl_Emp.DataValueField = "EmpID";
            ddl_Emp.DataBind();

            ddl_Emp.Items.Insert(0, new ListItem("-- 請選擇人員 --", ""));
        }

        protected void ddl_Emp_SelectedIndexChanged(object sender, EventArgs e)
        {
            emp = 1;
            string empId = ddl_Emp.SelectedValue;
            string empName = ddl_Emp.SelectedItem.Text;

            BindGrid();
        }

        private void BindGrid()
        {
            DataTable dt = new DataTable();

            using (SqlConnection sqlConn = new SqlConnection(ConfigurationManager.ConnectionStrings["SAMConnectionString"].ToString()))
            {
                if(emp == 0 | ddl_Emp.SelectedValue == "")
                {
                    string select = @"
                                    SELECT 
                                        E.[EmpID], E.[EmpName], S.[Date], P.PNAME, S.[InStatus], S.[OutStatus] 
                                    FROM [EmpPCode] AS E 
                                    INNER JOIN [PCode] AS P ON E.PCodeID = P.PID 
                                    INNER JOIN [EmpCodeStatus] AS S ON E.EmpID = S.EmpID 
                                      WHERE 
                                        E.[DepID] = @DepID 
                                        AND @StartDate <= S.[Date] 
                                        AND S.[Date] <= @EndDate 
                                      ORDER BY E.[EmpID], S.[Date]";

                    SqlCommand cmd = new SqlCommand(select, sqlConn);
                    cmd.Parameters.AddWithValue("@DepID", Request.QueryString["DepID"].Trim());
                    cmd.Parameters.AddWithValue("@StartDate", tbxStartDate.Text.Trim());
                    cmd.Parameters.AddWithValue("@EndDate", tbxEndDate.Text.Trim());

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
                else if(emp == 0 | ddl_Emp.SelectedValue != "")
                {
                    string select = @"
                                    SELECT 
                                        E.[EmpID], E.[EmpName], S.[Date], P.PNAME, S.[InStatus], S.[OutStatus] 
                                    FROM [EmpPCode] AS E 
                                    INNER JOIN [PCode] AS P ON E.PCodeID = P.PID 
                                    INNER JOIN [EmpCodeStatus] AS S ON E.EmpID = S.EmpID 
                                      WHERE 
                                        E.[DepID] = @DepID 
                                        AND @StartDate <= S.[Date] 
                                        AND S.[Date] <= @EndDate 
                                        AND E.[EmpID] = @EmpID
                                      ORDER BY E.[EmpID], S.[Date]";

                    SqlCommand cmd = new SqlCommand(select, sqlConn);
                    cmd.Parameters.AddWithValue("@DepID", Request.QueryString["DepID"].Trim());
                    cmd.Parameters.AddWithValue("@StartDate", tbxStartDate.Text.Trim());
                    cmd.Parameters.AddWithValue("@EndDate", tbxEndDate.Text.Trim());
                    cmd.Parameters.AddWithValue("@EmpID", ddl_Emp.SelectedValue.Trim());

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }

            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
    }
}