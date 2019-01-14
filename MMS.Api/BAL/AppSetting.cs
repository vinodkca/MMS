using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.FileExtensions;
using Microsoft.Extensions.Configuration.Json;
using System;

namespace MMS.Api.BAL
{
    public class AppSetting{
        // Adding JSON file into IConfiguration.
        public static readonly IConfiguration config; 
        public static readonly string SQL_DIAD;
        public static readonly string USER_NAME; 
        public static readonly string USER_PSWD;
        public static readonly int CALL_ALL; 
        public static readonly string CALL_STARTDATE; 
        public static readonly string CALL_ENDDATE;   
        public static readonly int CALL_DAYS;   
         public static readonly int CALL_LIMIT;   
      
        //Initialize all static variable  after const have been called. Static constructor is called only once
        static AppSetting(){
            config = new ConfigurationBuilder()
                        .AddJsonFile("AppConfig.json",  optional: true, reloadOnChange: true)
                        .Build();  
            SQL_DIAD = config["DBConnection:SQLDIAD"]; 
            USER_NAME = config["Credentials:UserName"]; 
            USER_PSWD = config["Credentials:Password"]; 
            CALL_ALL =  Convert.ToInt32(config["Call:All"]); 
            CALL_STARTDATE = config["Call:StartDate"]; 
            CALL_ENDDATE = config["Call:EndDate"]; 
            CALL_DAYS =  Convert.ToInt32(config["Call:Days"]); 
            CALL_LIMIT =  Convert.ToInt32(config["Call:Limit"]); 

        }

        //DB Connection
        //public const string connDIADSQL = @"Data Source=172.16.0.4;Initial Catalog=PROD-Profile;User ID=AppDBAccess;Password=appdbaccess;";
        //public static string connDIADSQL = AppSetting.config["SQLDIAD"];              
        //Credentials
        // public const string USER_NAME = "vypapi@myyp.com";
        // public const string PSWD = "vypapi";

        //CONST are called before static constructor
        //https://callcounts.com/data/Help/Api/GET-api-Calls
        public const string API_URL = "https://callcounts.com/data/api";

        //LINE DETAILS
        public static readonly string linesAll = $"{API_URL}/lines";
        public static readonly string linesDetail = $"{API_URL}/lines/{{0}}";

        //CALL DETAILS
        public static readonly string callsAll = $"{API_URL}/Calls";
        public static readonly string callsDetail = $"{API_URL}/Calls/{{0}}";
        public static readonly string callsDateRange = $"{API_URL}/Calls?startdate={{0}}&enddate={{1}}";
       
        public static readonly string callsAllPage = $"{API_URL}/Calls?page={{0}}&calls={{1}}";
        public static readonly string callsDateRangePage = $"{API_URL}/Calls?page={{0}}&calls={{1}}&startdate={{2}}&enddate={{3}}";
        
    }

}


    // private static void Example()
    //     {
    //         // Adding JSON file into IConfiguration.
    //         IConfiguration config = new ConfigurationBuilder()
    //             .AddJsonFile("AppConfig.json",  optional: true, reloadOnChange: true)
    //             .Build();

    //         // Read configuration
    //         string FirstName = config["FirstName"];
    //         string LastName = config["LastName"];
    //         Console.WriteLine($"Hello {FirstName} {LastName}!");

    //         var address = config.GetSection("Address");
    //         string street = address["Street"];
    //         string city = address["City"];
    //         string zip = address["Zip"];
    //         string state = address["State"];
    //         Console.WriteLine($"Your address is: {street}, {city}, {state} {zip}");
           

    //         //    {
    //         //     "FirstName": "John",
    //         //     "LastName": "Smith",
            
    //         //    "Address": {
    //         //     "Street": "100 Main St",
    //         //     "City": "Boston",
    //         //     "Zip": "02102",
    //         //     "State": "MA"
    //         //     }
    //         //    }
    //     }
