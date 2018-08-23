using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;



namespace webapphybridconn.Pages
{
    public class IndexModel : PageModel
    {

        private static HttpClient client = new HttpClient();
        public string Records { get; set; }
        public List<RecordsViewModel> DRecords { get; set; }

        public string apiURL { get; set; }
        public string apiURL2 { get; set; }

        [FromForm]
        public string SerialNumber { get; set; }

        
        public async Task<IActionResult> OnPostSerialNumber()
        {
            
            
            apiURL = $"http://YOURAPPSERVICE.azurewebsites.net/api/values/devicerecords";

            apiURL2 = $"http://YOURAPPSERVICE.azurewebsites.net/api/values/logsn/" + SerialNumber;

            var opurl = new Uri(apiURL2);
            var res = await client.GetAsync(opurl);
            string textResult = await res.Content.ReadAsStringAsync();
            //ViewData["Message"] = textResult;
            //Records = textResult;

            ProcessRecords();
            Records = textResult;
            apiURL = apiURL2;

            return Page();
        }

        public IActionResult OnGet()
        {

            //List<RecordsViewModel> objList = GetRecords();

            ProcessRecords();

            //Task<string> pr = Task.Run<string>(async () => await PullRecords());
            //Records = pr.Result;
            //DRecords = new List<RecordsViewModel>();

            //if (Records.StartsWith("Error:") == false)
            //{
            //    JArray jsonVal = JArray.Parse(Records) as JArray;
            //    //dynamic CRecords = jsonVal;

            //    foreach (JObject CRecord in jsonVal)
            //    {
            //        RecordsViewModel temp = CRecord.ToObject<RecordsViewModel>();
            //        DRecords.Add(temp);
            //    }
            //}

            return Page();
        }

        private void ProcessRecords()
        {
            Task<string> pr = Task.Run<string>(async () => await PullRecords());
            Records = pr.Result;
            DRecords = new List<RecordsViewModel>();

            if (Records.StartsWith("Error:") == false)
            {
                JArray jsonVal = JArray.Parse(Records) as JArray;
                //dynamic CRecords = jsonVal;

                foreach (JObject CRecord in jsonVal)
                {
                    RecordsViewModel temp = CRecord.ToObject<RecordsViewModel>();
                    DRecords.Add(temp);
                }
            }

        }

        private async System.Threading.Tasks.Task<string> PullRecords()
        {
            apiURL = $"http://YOURAPPSERVICE.azurewebsites.net/api/values/devicerecords";
       
#if DEBUG
            //apiURL = $"http://localhost:7071/api/PullRecords";
#endif
            try
            {
                string response = await client.GetStringAsync(apiURL);
                return response as string;
            }
            catch (System.Exception ex)
            {
                return ("Error: " + ex.Message) as string;
            }
        }
    }

    public class RecordsViewModel
    {
        public string SerialNumber { set; get; }
        public string Comments { set; get; }
        public string ActivationDate { set; get; }
        public string SentTime { set; get; }

    }
}
