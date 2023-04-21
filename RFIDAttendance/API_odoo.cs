using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace RFIDAttendance
{
    internal class API_odoo
    {
        public class MyResponse
        {
            public string id { get; set; }
            public string name { get; set; }
            public string avatar { get; set; }
            public string department { get; set; }
            public string gender { get; set; }
            public string last_checkin { get; set; }
            public string last_checkout { get; set; }
            public string last_checkin_image { get; set; }
            public string last_checkout_image { get; set; }
            public string checkin { get; set; }
            public string checkin_image { get; set; }
            public string checkout { get; set; }
            public string checkout_image { get; set; }
            


        }
        public class InfoResponse
        {

            public string code { get; set; }
            public string name { get; set; }
            public string ID { get; set; } 
            public string department { get; set; } 
            public string avatar { get; set; }
            public string phone { get; set; }

        }
        public class SignalResponse
        {
            public string signalexist { get; set; }
            public string signalcheck { get; set; }
        }
        public async Task<string> APIGetTimeCheckincutVideoAsync(string url_Odoo, string url_gettimecheckin, string rfid)
        {
            var client = new HttpClient();
            var content = await client.GetStringAsync(url_Odoo + url_gettimecheckin + rfid);
            return content;
        }
        public async Task<string> APIGetTimeCheckoutcutVideoAsync(string url_Odoo, string url_gettimecheckout, string rfid)
        {
            var client = new HttpClient();
            var content=await client.GetStringAsync(url_Odoo+url_gettimecheckout+rfid);

            return content;
        }
        public async Task<InfoResponse> APIGetInfoEmployeebyID(string url_Odoo, string url_showinfo, string id)
        {
            var client = new HttpClient();
            var content = await client.GetStringAsync(url_Odoo + url_showinfo + id);
            InfoResponse json = JsonConvert.DeserializeObject<InfoResponse>(content);
            return json;
        }

        public async Task<MyResponse> APIGetInfoEmployee(string url_Odoo, string url_api_Employee, string rfid)
        {
            var client = new HttpClient();
            var content = await client.GetStringAsync(url_Odoo + url_api_Employee + rfid);
            MyResponse json = JsonConvert.DeserializeObject<MyResponse>(content);
            return json;
        }        
        public async Task<SignalResponse> APIGetSignalCheckInCheckOut(string url_Odoo, string url_getsignalinout, string rfid)
        {
            var client = new HttpClient();
            var content = await client.GetStringAsync(url_Odoo + url_getsignalinout + rfid);
            SignalResponse json = JsonConvert.DeserializeObject<SignalResponse>(content);
            return json;
        }
        public async Task<string>APIUpdateCheckOut(string RFID,string image,string url_Odoo,string url_updatecheckout,string dateTime_checkout)
        {
                string ret;           
                HttpClient api_client = new HttpClient();
                api_client.BaseAddress = new Uri(url_Odoo);
                api_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string json = System.Text.Json.JsonSerializer.Serialize(new { rfid = RFID, image = image, checkout_time = dateTime_checkout });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var result = await api_client.PostAsync(url_updatecheckout, content);
                string resultContent = await result.Content.ReadAsStringAsync();
                JObject obj = JObject.Parse(resultContent);
                if (obj.ContainsKey("result"))
                {
                    ret = "success";
                }
                else
                {
                    string message = obj["error"]["data"]["message"].ToString();
                    ret = message;
                }
                
            return ret;
        }
        public static void WriteLogE(Exception exception)
        {
            using (TextWriter writer = new StreamWriter("Log_data.txt", true))  // true is for append mode
            {
                writer.WriteLine(
                    "=>{0} An Error occurred: {1}  Message: {2}{3}",
                    DateTime.Now,
                    exception.StackTrace,
                    exception.Message,
                    Environment.NewLine
                    );
            }
        }
        public async Task<string> APICheckin(string RFID, string image, string url_Odoo, string url_checkin, string dateTime_checkin)
        {
            string ret;
            HttpClient api_client = new HttpClient();
            api_client.BaseAddress = new Uri(url_Odoo);
            api_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string json = System.Text.Json.JsonSerializer.Serialize(new { rfid = RFID, image = image, checkin_time = dateTime_checkin });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await api_client.PostAsync(url_checkin, content);
            string resultContent = await result.Content.ReadAsStringAsync();
            JObject obj = JObject.Parse(resultContent);
            if (obj.ContainsKey("result"))
            {
                ret = "success";
            }
            else
            {
                string message = obj["error"]["data"]["message"].ToString();
                ret = message;
            }
            return ret;
        }

        public async Task<string> APICheckout(string RFID, string image, string url_Odoo, string url_checkout, string dateTime_checkout)
        {
            string ret;
            HttpClient api_client = new HttpClient();
            api_client.BaseAddress = new Uri(url_Odoo);
            api_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string json = System.Text.Json.JsonSerializer.Serialize(new { rfid = RFID, image = image, checkout_time = dateTime_checkout });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var result = await api_client.PostAsync(url_checkout, content);
            string resultContent = await result.Content.ReadAsStringAsync();
            JObject obj = JObject.Parse(resultContent);
            if (obj.ContainsKey("result"))
            {
                ret = "success";
            }
            else
            {
                string message = obj["error"]["data"]["message"].ToString();
                ret = message;
            }
            return ret;
        }

        public async Task<string> APIUpdateForgetCheckOut(string RFID,string url_Odoo, string url_updateforgetcheckout)
        {
            string ret;
            HttpClient api_client = new HttpClient();
            api_client.BaseAddress = new Uri(url_Odoo);
            api_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string json = System.Text.Json.JsonSerializer.Serialize(new { rfid = RFID});
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await api_client.PostAsync(url_updateforgetcheckout, content);
            string resultContent = await result.Content.ReadAsStringAsync();
            JObject obj = JObject.Parse(resultContent);
            if (obj.ContainsKey("result"))
            {
                ret = "success";
            }
            else
            {
                string message = obj["error"]["data"]["message"].ToString();
                ret = message;
            }

            return ret;
        }
        public async Task<string> APICreateNewRFIDEMployee(string id, string rfid,string url_Odoo, string url_createnew)
        {
            string ret;
            HttpClient api_client = new HttpClient();
            api_client.BaseAddress = new Uri(url_Odoo);
            api_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string json = System.Text.Json.JsonSerializer.Serialize(new { id = id, rfid = rfid });
            var content=new StringContent(json,Encoding.UTF8, "application/json");
            var result = await api_client.PostAsync(url_createnew, content);
            string resultContent=await result.Content.ReadAsStringAsync();
            JObject obj = JObject.Parse(resultContent);
            if (obj.ContainsKey("result"))
            {
                ret = obj["result"].ToString();
            }
            else
            {
                string message = obj["error"]["data"].ToString();
                ret = message;
            }
            return ret;
        }
        public async Task<string> API_PostBase64_Checkin(string rfid,string url_Odoo,string checkin_video, string url_createnew)
        {
            string ret;
            HttpClient api_client = new HttpClient();
            api_client.BaseAddress = new Uri(url_Odoo);
            api_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string json = System.Text.Json.JsonSerializer.Serialize(new { rfid = rfid, checkin_video = checkin_video });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await api_client.PostAsync(url_createnew, content);
            string resultContent = await result.Content.ReadAsStringAsync();
            JObject obj = JObject.Parse(resultContent);
            if (obj.ContainsKey("result"))
            {
                ret = obj["result"].ToString();
                Console.WriteLine("RESPONE: "+ ret);
            }
            else
            {
                string message = obj["error"]["data"].ToString();
                ret = message;
            }
            return ret;
        }

        public async Task <string> API_PostBase64_Checkout(string rfid, string url_Odoo, string checkout_video, string url_createnew)
        {
            string ret;
            HttpClient api_client = new HttpClient();
            api_client.BaseAddress = new Uri(url_Odoo);
            api_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string json = System.Text.Json.JsonSerializer.Serialize(new { rfid = rfid, checkout_video = checkout_video });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await api_client.PostAsync(url_createnew, content);
            string resultContent = await result.Content.ReadAsStringAsync();
            JObject obj = JObject.Parse(resultContent);
            if (obj.ContainsKey("result"))
            {
                ret = obj["result"].ToString();
                Console.WriteLine("RESPONE: " + ret);
            }
            else
            {
                string message = obj["error"]["data"].ToString();
                ret = message;
            }
            return ret;
        }
    }

}

