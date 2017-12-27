using System.Text;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class NoConnectionController : Controller
    {
        // GET: General

        [HttpPost]
        public ActionResult IsUnique(Models.UniqueViewModel model)
        {
            bool result = true;

            // create sql statement
            StringBuilder sql = new StringBuilder("Select Count(0) From " + model.tablename + " Where");
            // for update
            if (model.id != null)
                sql.Append(" Name <> " + model.id + " And");
            // for child rows
            if (model.parentColumn != null)
                sql.Append(" " + model.parentColumn + " = " + model.parentId + " And");
            // basic filter columns
            for (var i = 0; i < model.columns.Length; i++)
            {
                if (i != 0) sql.Append(" And");
                sql.Append(" " + model.columns[i] + " = '" + model.values[i] + "'");
            }

            System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["HrContext"].ConnectionString);
            try
            {
                conn.Open();
                System.Data.SqlClient.SqlCommand comm = new System.Data.SqlClient.SqlCommand(sql.ToString(), conn);
                System.Data.SqlClient.SqlDataReader reader = comm.ExecuteReader(System.Data.CommandBehavior.SingleResult);
                if (reader.Read())
                    result = reader.GetValue(0).ToString() == "0";
                reader.Close();
                comm.Dispose();
            }
            catch (System.Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}