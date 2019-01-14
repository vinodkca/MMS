using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MMS.Api.DAL;
using MMS.Model;

namespace MMS.Api.BAL
{
    public class CallService
    {
        // connection string to my SQL Server
        CallRepository callRepo = null;
        
        public CallService()
        {
           callRepo = new CallRepository(AppSetting.SQL_DIAD);
        }
        
        public async Task<List<Call>>  GetCallsAll()
        {
            return await callRepo.GetCallsAll();
        }

         public async Task<List<Call>> GetCallsAllByID(string strID)
        {
            return await callRepo.GetCallsAllByID(strID);
        }

        public async Task<List<Call>> GetCallsDateRange(string dtStart, string dtEnd)
        {
            return await callRepo.GetCallsDateRange(dtStart, dtEnd);
        } 

        public bool InsertCalls(List<Call> lstCall)
        {
            return callRepo.InsertCalls(lstCall);       
        }

        public bool TruncateTable(){
            return callRepo.TruncateTable();
        }

        public int MergeCall()
        {
            return callRepo.MergeCall() /2;       
        }
    }
}
