using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.NetworkInformation;

namespace SAM2025
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadDepTreeFromCodeStructure();
        }

        private void LoadDepTreeFromCodeStructure()
        {
            string connStr = ConfigurationManager.ConnectionStrings["TrustERPConnectionString"].ConnectionString;
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();

                //string sql = "SELECT DepID, DepName, UseFlag \r\nFROM Department \r\nWHERE (UseFlag = 1) AND (DepID LIKE '%' + '00000000' + '%') AND (DepID <> '000000000000') ORDER BY DepName";
                string sql = "SELECT DepID, DepName, UseFlag FROM Department WHERE UseFlag = 1 ORDER BY DepID";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                da.Fill(dt);
            }

            tvDepartments.Nodes.Clear();
            Dictionary<string, TreeNode> nodeMap = new Dictionary<string, TreeNode>();

            foreach (DataRow row in dt.Rows)
            {
                string depId = row["DepID"].ToString();
                string depName = row["DepName"].ToString();

                TreeNode node = new TreeNode(depName, depId);

                // ✅ 最後一層才設定 NavigateUrl
                if (IsLeafNode(depId))
                {
                    node.NavigateUrl = $"Default.aspx?DepID={Server.UrlEncode(depId)}&DepName={Server.UrlEncode(depName)}";
                    node.Target = "_self";
                }

                nodeMap[depId] = node;

                string parentId = GetParentDepID(depId);

                if (string.IsNullOrEmpty(parentId) || !nodeMap.ContainsKey(parentId))
                    tvDepartments.Nodes.Add(node);
                else
                    nodeMap[parentId].ChildNodes.Add(node);
            }
        }

        private string GetParentDepID(string depId)
        {
            if (depId.Substring(4, 8) == "00000000")
            {
                // 第一層：沒有父節點
                return null;
            }
            else if (depId.Substring(6, 6) == "000000")
            {
                // 第二層：父是第一層
                return depId.Substring(0, 4) + "00000000";
            }
            else if (depId.Substring(8, 4) == "0000")
            {
                // 第三層：父是第二層
                return depId.Substring(0, 6) + "000000";
            }
            else
            {
                // 第四層或更多：父是第三層
                return depId.Substring(0, 8) + "0000";
            }
        }

        private bool IsLeafNode(string depId)
        {
            // 假設結構為 4+2+2+4 共12碼，只有最底層沒有 0 結尾
            return !(depId.EndsWith("0000") || depId.EndsWith("000000") || depId.EndsWith("00000000"));
        }

        protected void tvDepartments_SelectedNodeChanged(object sender, EventArgs e)
        {
            string selectedDepId = tvDepartments.SelectedNode.Value; //DepId
            string selectedDepName = tvDepartments.SelectedNode.Text; //DepName

            string url = $"Default.aspx?DepID={Server.UrlEncode(selectedDepId)}&DepName={Server.UrlEncode(selectedDepName)}";
            Response.Redirect(url);

            //Response.Redirect("Default.aspx?DepID=" + Server.UrlEncode(selectedDepId));

            //SqlDataSource1.SelectParameters["EmpDep"].DefaultValue = selectedDepId;
            //GridView1.DataBind();
        }
    }
}