using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MMS.Model;
using MMS.Api.DAL;

namespace MMS.Api.BAL
{
    public class LineService
    {
        // connection string to my SQL Server
        LineRepository lineRepo = null;
        
        public LineService()
        {
           lineRepo = new LineRepository(AppSetting.SQL_DIAD);
        }
        
        public async Task<List<LineItem>>  GetLineAll()
        {
            return await lineRepo.GetLineAll();
        }

         public async Task<List<LineItem>> GetLineAllByID(string strID)
        {
            return await lineRepo.GetLineDetailByID(strID);
        }
        public bool InsertLines(List<LineItem> lstLine)
        {            
            return lineRepo.InsertLines(lstLine);       
        }

        public bool TruncateTable()
        {
            return lineRepo.TruncateTable();
        }

        public int MergeLine()
        {
            return lineRepo.MergeLine() /2;       
        }
    }
}
