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
    public class LineRepository : Repository, ILineRepository
    {
         private static IDbConnection db = null;

        public LineRepository(string strConn)
        {
            db = new SqlConnection(strConn);
        } 
        public async Task<List<LineItem>> GetLineAll()
        {
           List<LineItem> resCall = await db.QueryAsync<Call>(sql:"dbo.spSelLine", commandType: CommandType.StoredProcedure ) as List<LineItem>;
           return resCall;  
        }

        public async Task<List<LineItem>> GetLineDetailByID(string strID)
        {
            
           DynamicParameters dp = new DynamicParameters();
           dp.Add("ID", strID);
           List<LineItem> resLine = await db.QueryAsync<Call>(sql:"dbo.spSelLine",  param: dp,commandType: CommandType.StoredProcedure ) as List<LineItem>;
           return resLine;  
        }
        public bool InsertLines(List<LineItem> lstLines)
        {
            //Match DB STG_Line
            List<Line>  lstLinesDB = new List<Line>();

            foreach (LineItem lineItem in lstLines) {

                Line line = new Line();
                line.ID = lineItem.ID;
                line.TrackingNumber = lineItem.TrackingNumber;
                line.RingTo = lineItem.RingTo;
                line.TrackingLineName = lineItem.TrackingLineName;
                line.Type = lineItem.Type;
                line.Year = lineItem.Year;
                line.AccountNumber = lineItem.AccountNumber;
                line.Market = lineItem.Market;
                line.Heading = lineItem.Heading;
                line.UDAC = lineItem.UDAC;
                line.PubDate = lineItem.PubDate;
                line.EmailAddress = lineItem.EmailAddress;                
                line.CreatedDate = lineItem.CreatedDate;

                lstLinesDB.Add(line);
            }

             return BulkCopy<Line>(db as SqlConnection, "STG_Line", lstLinesDB);
        }

        public bool TruncateTable()
        {
           return TruncateTable(db as SqlConnection,"STG_Line");
        }

        public int MergeLine()
        {           
           return db.Execute(sql:"dbo.spMergeLine", commandType: CommandType.StoredProcedure);        
        }
     }
}
