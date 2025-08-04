using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SAM2025
{
    public partial class TimePeriodAdd : System.Web.UI.Page
    {
        private string depId = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            depId = Request.QueryString["DepID"];
            string depName = Request.QueryString["DepName"];

            ViewState["DepID"] = depId;

            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(depId))
                {
                    //返回部門班段清單
                    LinkButton1.PostBackUrl = $"~/TimePeriod.aspx?DepID={depId}&DepName={depName}";
                    //部門總覽
                    LinkButton2.PostBackUrl = $"~/Default.aspx?DepID={depId}&DepName={depName}";

                    lblDepartment.Text = $"部門代碼：{depId}<br/>部門名稱：{depName}";

                    // ✅ 加條件判斷，避免重複加
                    if (SqlDataSource1.SelectParameters["DepIDFilter"] == null)
                    {
                        SqlDataSource1.SelectCommand += " WHERE PID LIKE @DepIDFilter";
                        SqlDataSource1.SelectParameters.Add("DepIDFilter", depId + "_%");
                    }
                }

                DetailsView1.DataBind();
            }
        }

        protected string ShowFullPid(object pidObj)
        {
            string suffix = pidObj?.ToString();
            string depId = ViewState["DepID"]?.ToString();

            // 若 PID 本身已經是完整格式就直接顯示
            if (suffix.Contains("_"))
                return suffix;

            if (!string.IsNullOrEmpty(depId))
                return depId + "_" + suffix;

            return suffix;
        }

        protected string GetFullPid(object pidObj)
        {
            string pid = pidObj?.ToString();
            if (string.IsNullOrEmpty(pid)) return "";

            // 若已包含 _ 就不重組
            if (pid.Contains("_")) return pid;

            string depId = ViewState["DepID"]?.ToString() ?? "";
            return depId + "_" + pid;
        }

        protected string GetPidSuffix(object pidObj)
        {
            string fullPid = pidObj?.ToString() ?? "";
            var parts = fullPid.Split('_');
            return parts.Length > 1 ? parts[1] : fullPid;
        }

        protected void DetailsView1_ItemCommand(object sender, DetailsViewCommandEventArgs e)
        {
            DBUtil db = new DBUtil();

            if (e.CommandName == "Insert")
            {
                //TextBox txbPID = (TextBox)DetailsView1.FindControl("txbPID");
                TextBox txbSuffix = (TextBox)DetailsView1.FindControl("txbSuffix");

                if (txbSuffix == null || string.IsNullOrWhiteSpace(txbSuffix.Text))
                {
                    Response.Write("<Script language='JavaScript'>alert('Error ，班別代碼不可空白');</Script>");
                    return;
                }

                // ✅ 自動組合 DepID + "_" + 輸入
                depId = Request.QueryString["DepID"];
                string fullPID = depId + "_" + txbSuffix.Text.Trim();

                string strCodeA01 = "";
                if (!string.IsNullOrEmpty(fullPID))
                {
                    DataTable dtA01 = db.queryDataTable(@"SELECT * FROM PCode WHERE PID = '" + fullPID + "'");
                    if (dtA01.Rows.Count != 0)
                    {
                        strCodeA01 = dtA01.Rows[0]["id"].ToString();
                        if (strCodeA01 != "")
                        {
                            Response.Write("<Script language='JavaScript'>alert('Error ，PID代號已經存在');</Script>");
                        }
                    }
                }
                else
                {
                    Response.Write("<Script language='JavaScript'>alert('Error ，PID代號不可空白');</Script>");
                }

                // ✅ 將組合後的 PID 放入 DetailsView 的資料列中，供 SQL 使用
                SqlDataSource1.InsertParameters["PID"].DefaultValue = fullPID;

                // 執行 Insert（也可用 DetailsView1.InsertItem()）
                DetailsView1.ChangeMode(DetailsViewMode.Insert);
            }
        }

        protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e)
        {
            string depId = ViewState["DepID"]?.ToString();
            TextBox txbSuffix = (TextBox)DetailsView1.FindControl("txbSuffix");

            if (txbSuffix != null && !string.IsNullOrWhiteSpace(txbSuffix.Text))
            {
                e.Values["PID"] = depId + "_" + txbSuffix.Text.Trim();
            }
        }

        protected void DetailsView1_ItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {
            string depId = ViewState["DepID"]?.ToString();
            TextBox txbSuffix = (TextBox)DetailsView1.FindControl("txbSuffix");

            if (txbSuffix != null && !string.IsNullOrWhiteSpace(txbSuffix.Text))
            {
                e.NewValues["PID"] = depId + "_" + txbSuffix.Text.Trim();
            }
        }

        protected void DetailsView1_ItemDeleting(object sender, DetailsViewDeleteEventArgs e)
        {
            string fullPid = e.Keys["PID"].ToString();
            string depId = ViewState["DepID"]?.ToString();

            if (!fullPid.StartsWith(depId + "_"))
            {
                e.Cancel = true;
                Response.Write("<script>alert('錯誤的 PID');</script>");
            }
        }

        protected void SqlDataSource1_Inserting(object sender, SqlDataSourceCommandEventArgs e)
        {
            if (ViewState["DepID"] != null)
            {
                e.Command.Parameters["@DepID"].Value = ViewState["DepID"].ToString();
            }
        }
    }
}