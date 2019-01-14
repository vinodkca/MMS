using System.Collections.Generic;
using System.Threading.Tasks;
using MMS.Model;

namespace MMS.Api.Interface
{
    public interface ICallRepository
    {
        Task<List<Call>> GetCallsAll();
        Task<List<Call>> GetCallsDateRange (string dtStart, string dtEnd);
    }
}