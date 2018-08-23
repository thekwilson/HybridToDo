

using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System;
using System.Data.SqlClient;
using System.Text;


namespace OBowFunctions2
{
    public static class OBowFunctions2
    {
        [FunctionName("PullRecords")]
        public static IActionResult PullRun([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("Pull Records function triggered");
            string updatemessage ="";
 
            try
            {

                //UPDATE WITH YOUR OWN Connection String and/or Values
                var cstr = "Server=tcp:YOURSERVER.database.windows.net,1433;Initial Catalog=YOURDB;Persist Security Info=False;User ID=YOURUSER;Password=YOURPASSWORD;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

                using (SqlConnection sconn = new SqlConnection(cstr))
                {
                    sconn.Open();
                    log.Info("Opened DB Connection...");
                    //We query for all records returned as JSON since Azure SQL has this support
                    string cmdtext = $"SELECT * FROM ClientLog FOR JSON AUTO";
                    SqlCommand cmd = new SqlCommand(cmdtext, sconn);

                    StringBuilder jsonResult = new StringBuilder();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (!reader.HasRows)
                    {
                        updatemessage = $"No rows found in the table";
                        log.Info(updatemessage);
                        jsonResult.Append("[No rows found in the table]");

                    }
                    else
                    {
                        while (reader.Read())
                        {
                            jsonResult.Append(reader.GetValue(0).ToString());
                        }
                        
                        updatemessage = jsonResult.ToString();
                        log.Info(updatemessage);
                    }


                    
                }


            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }

            return (ActionResult)new OkObjectResult(updatemessage);
               
        }


        [FunctionName("PersistRecord")]
        public static IActionResult PersistRun([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");
            string serialNumber = req.Query["SerialNumber"];
            string updatemessage = "";

            if (serialNumber == null)
            {
                string requestBody = new StreamReader(req.Body).ReadToEnd();
                dynamic data = JsonConvert.DeserializeObject(requestBody);
                serialNumber = serialNumber ?? data?.serialNumber;
            }

            try
            {
                //UPDATE WITH YOUR OWN Connection String and/or Values
                var cstr = "Server=tcp:YOURSERVER.database.windows.net,1433;Initial Catalog=YOURDB;Persist Security Info=False;User ID=YOURUSER;Password=YOURPASSWORD;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

                using (SqlConnection sconn = new SqlConnection(cstr))
                {
                    sconn.Open();
                    log.Info("Opened DB Connection...");
                    string actdate = DateTime.Now.ToString("yyyy-MM-dd"); 

                    //Runs A Simple UPDATE to set the activcation date to today for all records that matched the serial number passed in on the query string                   
                    var cmdtext = $"UPDATE ClientLog SET ActivationDate = '{actdate}' WHERE SerialNumber='{serialNumber}'";

                    using (SqlCommand cmd = new SqlCommand(cmdtext, sconn))
                    {
                        var rows = cmd.ExecuteNonQuery();
                        log.Info($"{rows} rows were updated");
                        if (rows == 0)
                        {
                            updatemessage = $"No rows updated for serial number: {serialNumber}";
                        }
                        else
                        {
                            updatemessage = $"{rows} rows updated for serial number: {serialNumber}";
                        }

                    }
                }


            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }

            return serialNumber != null
                ? (ActionResult)new OkObjectResult(updatemessage)
                : new BadRequestObjectResult("Please pass a serialNumber on the query string or in the request body");
        }

    }
}
