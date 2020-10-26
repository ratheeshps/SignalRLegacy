using SignalRDemo.Hubs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SignalRDemo
{
    public class Products
    {
        public decimal ruleViolationId { get; set; }
        public decimal ruleId { get; set; }
        public decimal transactionId { get; set; }
        // public decimal QuantDecimal { get; set; }
    }
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [WebMethod]
        public static IEnumerable<Products> GetData()
        {

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DataBase"].ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(@"SELECT [ruleViolationId], [ruleId], [transactionId] FROM [dbo].[Messages_sample]", connection))
                {
                    // Make sure the command object does not already have
                    // a notification object associated with it.
                    command.Notification = null;
                    SqlDependency.Start(ConfigurationManager.ConnectionStrings["DataBase"].ConnectionString);
                    SqlDependency dependency = new SqlDependency(command);
                    dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        return reader.Cast<IDataRecord>()
                                                    .Select(x => new Products()
                                                    {
                                                        ruleViolationId = x.GetDecimal(0),
                                                        ruleId = x.GetDecimal(1),
                                                        transactionId = x.GetDecimal(2),
                                                    }).ToList();

                    }



                }
            }
        }

        public static void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                MessageHub.Show();
            }
        }
    }
}