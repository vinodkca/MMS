using System.Collections.Generic;
using System.Threading.Tasks;
using MMS.Model;

namespace MMS.Api.Interface
{
    public interface ILineRepository
    {
        Task<List<LineItem>> GetLineAll();
        Task<List<LineItem>> GetLineDetailByID (string ID);
    }
}