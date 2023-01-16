﻿using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WEBPOS_RFIDSender
{
    internal class API_odoo
    {
        public class MyResponse
        {
            public string id { get; set; }
            public string name { get; set; }
            public string avatar { get; set; }
            public string department { get; set; }
            public string last_checkin { get; set; }
            public string last_checkout { get; set; }
            public string last_checkin_image { get; set; }
            public string last_checkout_image { get; set; }


        }
        public class InfoResponse
        {
            public string code { get; set; }
            public string name { get; set; }
            public string ID { get; set; } 
            public string department { get; set; } 
            public string avatar { get; set; }
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

        public async Task<string> APICheckin(string RFID, string image, string url_Odoo, string url_checkin, string dateTime_checkin)
        {
            string ret;
            HttpClient api_client = new HttpClient();
            api_client.BaseAddress = new Uri(url_Odoo);
            api_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string json = System.Text.Json.JsonSerializer.Serialize(new { rfid = RFID, image = image, checkin_time = dateTime_checkin });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            Console.WriteLine("check" + content);

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
    }

}

