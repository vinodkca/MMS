using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MMS.Api.BAL;
using MMS.Model;
using Newtonsoft.Json;

namespace MMS.Api.BAL {
    public static class HttpService {
        private static HttpClient client;
        public static CallService callService;
        public static LineService lineService;

        static HttpService () {
            //Static constructor
            client = new HttpClient ();
            callService = new CallService ();
            lineService = new LineService ();
        }

        //public static async Task InitializeService () {
        public static void InitializeService () {
            //Generate URL   
            client.BaseAddress = new Uri (AppSetting.API_URL);
            client.DefaultRequestHeaders.Clear ();
            client.DefaultRequestHeaders.Accept.Add (new MediaTypeWithQualityHeaderValue ("application/json"));

            //Generate token username:password
            var byteArray = Encoding.ASCII.GetBytes (AppSetting.USER_NAME + ":" + AppSetting.USER_PSWD);
            string authInfo = Convert.ToBase64String (byteArray);

            //Create Basic Authorization
            AuthenticationHeaderValue ahv = new AuthenticationHeaderValue ("Basic", authInfo);
            client.DefaultRequestHeaders.Authorization = ahv;

            //List<Line> lstLines =  await GetLinesAll(); 
            //ShowLines(lstLines);
            //Console.WriteLine( $"Recieved total lines :  {lstLines.Count}");

            //Console.WriteLine( $"Completed GetLines :  {ShowLineInfo().Result}");
            //Console.WriteLine(ShowLineInfo().GetAwaiter().GetResult()); //Console.WriteLine( $"{ShowLineInfo().Result}");
        }

        #region API calls
        public static async Task<List<LineItem>> GetLinesAll () {

            List<LineItem> lstLines = null;

            string strLinesAllPath = AppSetting.linesAll;

            HttpResponseMessage resp = await client.GetAsync (strLinesAllPath);
            if (resp.IsSuccessStatusCode) {

                Stream received = await resp.Content.ReadAsStreamAsync ();
                StreamReader readStream = new StreamReader (received);
                string jsonString = readStream.ReadToEnd ();
                //Console.WriteLine ("{0}", jsonString);

                //dotnet "add" "c:\Projects\VS2017\MMS\MMS.Api\MMS.Api.csproj" "package" "json.net"
                lstLines = JsonConvert.DeserializeObject<List<LineItem>> (jsonString);

                foreach (LineItem line in lstLines) {
                    //Convert to DB line item
                    ConvertLineItem (line.Label01, line);
                    ConvertLineItem (line.Label02, line);
                    ConvertLineItem (line.Label03, line);
                    ConvertLineItem (line.Label04, line);
                    ConvertLineItem (line.Label05, line);
                    ConvertLineItem (line.Label06, line);
                    ConvertLineItem (line.Label07, line);
                    ConvertLineItem (line.Label08, line);
                    ConvertLineItem (line.Label09, line);
                }

                if (lineService.TruncateTable ()) {
                    string strInsertLines = lineService.InsertLines (lstLines) ? $"Inserted {lstLines.Count} in STG_Line table" : "Failed to insert calls in STG_Line table";
                    Console.WriteLine (strInsertLines);
                } else {
                    Console.WriteLine ("STG_Line table was not truncated");
                }
            }

            return lstLines;
        }

        public static async Task<List<Call>> GetCallsAll () {
            List<Call> lstCalls = null;
            string strCallsAllPath = AppSetting.callsAll;

            HttpResponseMessage resp = await client.GetAsync (strCallsAllPath);
            if (resp.IsSuccessStatusCode) {

                Stream received = await resp.Content.ReadAsStreamAsync ();
                StreamReader readStream = new StreamReader (received);
                string jsonString = readStream.ReadToEnd ();

                lstCalls = JsonConvert.DeserializeObject<List<Call>> (jsonString);

                if (callService.TruncateTable ()) {
                    string strInsertCalls = callService.InsertCalls (lstCalls) ? $"Inserted {lstCalls.Count} in STG_Call table" : "Failed to insert calls in STG_Call table";
                    Console.WriteLine (strInsertCalls);
                } else {
                    Console.WriteLine ("STG_Call table was not truncated");
                }
            }

            return lstCalls;
        }

        public static async Task<int> GetCallsAllPage () {                     
            return await InsertCallsPage("PageAll", string.Empty, string.Empty);
        }
        public static async Task<int> GetCallsDateRangePage (string dtStart, string dtEnd) {                        
            return await InsertCallsPage("PageDate", dtStart, dtEnd); 
        }
        public static async Task<int> InsertCallsPage(string type, string dtStart, string dtEnd)
        {
             int iCount = 0;
             string strPagePath = string.Empty;
            //Truncate staging table
            if (callService.TruncateTable ()) {

                for (int iPage = 1; iPage - 1 < iPage; iPage = iPage + 1) //Infinite loop
                {
                    List<Call> lstCalls = new List<Call> ();
                    if(type == "PageAll")
                        strPagePath = String.Format (AppSetting.callsAllPage, iPage, AppSetting.CALL_LIMIT); //page 1 and calls 1000   
                    else if(type == "PageDate")    
                        strPagePath = String.Format (AppSetting.callsDateRangePage, iPage, AppSetting.CALL_LIMIT, dtStart, dtEnd);  

                    try {
                        HttpResponseMessage resp = await client.GetAsync (strPagePath);
                        if (resp.StatusCode == HttpStatusCode.NotFound) //HTTP-404 Not Found
                        {
                            Console.WriteLine ("End of Response 404 - " + HttpStatusCode.NotFound);
                            break;
                        }

                        Console.WriteLine (strPagePath);
                        if (resp.IsSuccessStatusCode) {
                            Stream received = await resp.Content.ReadAsStreamAsync ();
                            StreamReader readStream = new StreamReader (received);
                            string jsonString = readStream.ReadToEnd ();

                            lstCalls = JsonConvert.DeserializeObject<List<Call>> (jsonString);

                            string strInsertCalls = callService.InsertCalls (lstCalls) ? $"Inserted {lstCalls.Count} in STG_Call table" : "Failed to insert calls in STG_Call table";
                            Console.WriteLine (strInsertCalls);
                            iCount += lstCalls.Count;
                        }

                    } catch (WebException ex) {
                        Console.WriteLine ("Exception Response " + ex.Message);
                    }
                }
            } else {
                Console.WriteLine ("STG_Call table was not truncated");
            }

            return iCount;
        }
        public static async Task<List<Call>> GetCallsDateRange (string dtStart, string dtEnd) {
            List<Call> lstCalls = null;
            string strCallsAllPath = String.Format (AppSetting.callsDateRange, dtStart, dtEnd);

            HttpResponseMessage resp = await client.GetAsync (strCallsAllPath);
            if (resp.IsSuccessStatusCode) {

                Stream received = await resp.Content.ReadAsStreamAsync ();
                StreamReader readStream = new StreamReader (received);
                string jsonString = readStream.ReadToEnd ();

                lstCalls = JsonConvert.DeserializeObject<List<Call>> (jsonString);
                if (callService.TruncateTable ()) {
                    string strInsertCalls = callService.InsertCalls (lstCalls) ? $"Inserted {lstCalls.Count} in STG_Call table" : "Failed to insert calls in STG_Call table";
                    Console.WriteLine (strInsertCalls);
                } else {
                    Console.WriteLine ("STG_Call table was not truncated");
                }

            }

            return lstCalls;
        }
        #endregion

        #region Helper functions
        private static void ConvertLineItem (string ColName, LineItem line) {
            switch (ColName) {
                case "Year":
                    line.Year = line.LabelValue01;
                    break;
                case "Account Number":
                    line.AccountNumber = line.LabelValue02;
                    break;
                case "Market":
                    line.Market = line.LabelValue03;
                    break;
                case "Heading":
                    line.Heading = line.LabelValue04;
                    break;
                case "UDAC":
                    line.UDAC = line.LabelValue05;
                    break;
                case "Pub Date":
                    line.PubDate = line.LabelValue06;
                    break;
                case "Email Address":
                    line.EmailAddress = line.LabelValue07;
                    break;
                case "Type":
                    line.PortType = line.LabelValue08;
                    break;
                case "Port Date":
                    line.PortDate = line.LabelValue09;
                    break;
                default:
                    break;
            }
        }
        private static void ShowLines (List<LineItem> lstLines) {

            foreach (LineItem line in lstLines) {

                Console.WriteLine (
                    $" ID : {line.ID} \t" +
                    $" TrackingNumber : {line.TrackingNumber} \t" +
                    $" RingTo : {line.RingTo} \t" +
                    $" TrackingLineName : {line.TrackingLineName} \t" +
                    $" Type : {line.Type} \t" +
                    $" Label01 -- 10 : {line.Label01},{line.Label02} ,{line.Label03} ,{line.Label04} ,{line.Label05} ,{line.Label06} ,{line.Label07} ,{line.Label08},{line.Label09} ,{line.Label10}  \t " +
                    $" LabelValue01--10 : {line.LabelValue01},{line.LabelValue02} ,{line.LabelValue03} ,{line.LabelValue04} ,{line.LabelValue05} ,{line.LabelValue06} ,{line.LabelValue07} ,{line.LabelValue08},{line.LabelValue09} ,{line.LabelValue10} \t" +
                    $" Year : {line.Year} \t" +
                    $" AccountNumber : {line.AccountNumber} \t" +
                    $" Market : {line.Market} \t" +
                    $" Heading : {line.Heading} \t" +
                    $" UDAC : {line.UDAC} \t" +
                    $" PubDate : {line.PubDate} \t" +
                    $" EmailAddress : {line.EmailAddress} \t" +
                    $" Type : {line.PortType} \t" +
                    $" PortDate : {line.PortDate} \t" 
                );
                return;
            }
            //string retLine = $" Start Date Time {DateTime.Now}";
            //+ $" {await GetLineInfo()} "  ;
            //return retLine;
        }
        #endregion
    }
}