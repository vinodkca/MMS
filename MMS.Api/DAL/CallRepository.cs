using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using MMS.Api.Interface;
using MMS.Model;
using System.Threading.Tasks;

namespace MMS.Api.DAL
{
    public class CallRepository : Repository, ICallRepository
    {
         private static IDbConnection db = null;

        public CallRepository(string strConn)
        {
            db = new SqlConnection(strConn);
        } 

        public async Task<List<Call>> GetCallsAll()
        {
           List<Call> resCall = await db.QueryAsync<Call>(sql:"dbo.spSelCall", commandType: CommandType.StoredProcedure ) as List<Call>;
           return resCall;  
        }

        public async Task<List<Call>> GetCallsAllByID(string strID)
        {
            
           DynamicParameters dp = new DynamicParameters();
           dp.Add("ID", strID);
           List<Call> resCall = await db.QueryAsync<Call>(sql:"dbo.spSelCall",  param: dp,commandType: CommandType.StoredProcedure ) as List<Call>;
           return resCall;  
        }
         public async Task<List<Call>> GetCallsDateRange(string dtStart, string dtEnd)
         {
            DynamicParameters dp = new DynamicParameters();
            dp.Add("stDate", Convert.ToDateTime(dtStart));
            dp.Add("endDate", Convert.ToDateTime(dtStart));
            List<Call> resCall = await db.QueryAsync<Call>(sql:"dbo.spSelCallByDate",  param: dp,commandType: CommandType.StoredProcedure ) as List<Call>;
            return resCall;             
         }
         
         public bool InsertCalls(List<Call> lstCall)
         {
             return BulkCopy<Call>(db as SqlConnection, "STG_Call", lstCall);
         }

        public bool TruncateTable()
        {
           return TruncateTable(db as SqlConnection,"STG_Call");
        }

        public int MergeCall()
        {           
           return db.Execute(sql:"dbo.spMergeCall", commandType: CommandType.StoredProcedure);        
        }
     }
}
