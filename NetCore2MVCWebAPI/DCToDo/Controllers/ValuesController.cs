using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;


namespace TodoApi.Controllers
{
    [Route("api/[controller]")] 
    public class ValuesController : Controller
    {

        private static HttpClient client = new HttpClient();

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        // GET api/values/DeviceRecords
        [HttpGet("DeviceRecords", Name = "GetDeviceRecords")]
        public async System.Threading.Tasks.Task<string> GetDeviceRecords(string serialNumber)
        {

                try
                {
                   //Set this value to the on premise host you are making a hybrid connection to
                   string onpremurl = $"http://YOURLOCALHOST:7071/api/PullRecords";
#if DEBUG
                 //onpremurl = $"http://localhost:7071/api/PullRecords";
#endif
                    string response = await client.GetStringAsync(onpremurl);
                    return response as string;
                }
                catch (System.Exception ex)
                {
                    return ("Error: " + ex.Message) as string;
                }
            

        }

        // GET api/values/serialNumber
        [HttpGet("logsn/{serialNumber}", Name = "LogSerialNumber")]
        public async System.Threading.Tasks.Task<string> LogSerialNumberAsync(string serialNumber)
        {


            if (serialNumber != null)
            {
                try
                {
                    string onpremurl = $"http://YOURLOCALHOSTNAME:7071/api/PersistRecord?serialNumber={serialNumber}";
#if DEBUG
                    onpremurl = $"http://localhost:7071/api/PersistRecord?serialNumber={serialNumber}";
#endif
                    string response = await client.GetStringAsync(onpremurl);
                    return response;
                }
                catch (System.Exception ex)
                {
                    return ("Error: " + ex.Message);
                }
            }
            else
            {
                return ("This API operation requires a serialNumber in the path");
            }

        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
