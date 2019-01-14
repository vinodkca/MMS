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

namespace MMS.Api {
    class Program {
       //Creates one instances of connection and avaids exhaust of ssockets
       public static void Main (string[] args) {
            try
            {                           
                //HttpService.InitializeService().GetAwaiter().GetResult();
                HttpService.InitializeService();               
                List<LineItem> lstLines =  HttpService.GetLinesAll().GetAwaiter().GetResult(); 
                Console.WriteLine( $"Recieved total lines :  {lstLines.Count}");
               

                //await HttpService.InitializeService();
                //List<Line> lstLines =  await HttpService.GetLinesAll();                                 
                //ShowLines(lstLines);
               
            
               //Get All Calls
               if(AppSetting.CALL_ALL == 1)
               {                   
                   //List<Call> lstCalls =  HttpService.GetCallsAll().GetAwaiter().GetResult();                     
                   int iCount =  HttpService.GetCallsAllPage().GetAwaiter().GetResult();         
                   Console.WriteLine( $"Recieved total calls :  {iCount}");
               }
               else //Get Calls by Selected Date
               {
                   string dtStart = string.Empty;
                   string dtEnd = string.Empty;

                   if(AppSetting.CALL_DAYS == 0)
                   {    
                        dtStart = AppSetting.CALL_STARTDATE;
                        dtEnd = AppSetting.CALL_ENDDATE;
                   }
                   else
                   {
                       dtEnd = DateTime.Now.ToString("yyyy-MM-dd");
                       dtStart = DateTime.Now.AddDays(AppSetting.CALL_DAYS).ToString("yyyy-MM-dd");
                   }
                    //List<Call> lstCallsDate =  HttpService.GetCallsDateRange("2018-12-13","2018-12-15").GetAwaiter().GetResult();                     
                    //List<Call> lstCallsDate =  HttpService.GetCallsDateRange(AppSetting.CALL_STARTDATE,AppSetting.CALL_ENDDATE).GetAwaiter().GetResult(); 
                    int iCount = HttpService.GetCallsDateRangePage(dtStart, dtEnd).GetAwaiter().GetResult(); 
                    Console.WriteLine( $"Recieved total calls By Date :  {iCount}");
                   
               }

               //Insert into DB               
               //string strMergeCall = HttpService.callService.MergeCall() ? "Completed" : "Error";
               int iCallRows = HttpService.callService.MergeCall();
               Console.WriteLine( "Merged calls in DB : " + iCallRows);
               //Console.WriteLine( $"{strMergeCall} Merging calls");
             
               //string strMergeLine = HttpService.lineService.MergeLine() ? "Completed" : "Error";
               int iLineRows = HttpService.lineService.MergeLine(); 
               Console.WriteLine( "Merged lines in DB : " + iLineRows);
               //Console.WriteLine( $"{strMergeCall} Merging lines");
               Console.WriteLine( "Completed Successfully");

            }
            catch(Exception e)
            {
                throw e;
            }

        }

    

        // private static async Task InitializeService() {

        //      //Generate URL   
        //     client.BaseAddress = new Uri (AppSetting.API_URL);
        //     client.DefaultRequestHeaders.Clear ();
        //     client.DefaultRequestHeaders.Accept.Add (new MediaTypeWithQualityHeaderValue ("application/json"));

        //     //Generate token username:password
        //     var byteArray = Encoding.ASCII.GetBytes( AppSetting.USER_NAME +  ":" + AppSetting.PSWD);
        //     string authInfo = Convert.ToBase64String(byteArray);

        //     //Create Basic Authorization
        //     AuthenticationHeaderValue ahv = new AuthenticationHeaderValue("Basic", authInfo);
        //     client.DefaultRequestHeaders.Authorization = ahv;

        //     List<Line> lstLines =  await GetLinesAll(); 
        //     ShowLines(lstLines);
        //     Console.WriteLine( $"Recieved total lines :  {lstLines.Count}");
            
        //     //Console.WriteLine( $"Completed GetLines :  {ShowLineInfo().Result}");
        //     //Console.WriteLine(ShowLineInfo().GetAwaiter().GetResult()); //Console.WriteLine( $"{ShowLineInfo().Result}");
        // }

        //private static async Task<string> ShowLines ( List<Line> lstLines) {
      

    }
}