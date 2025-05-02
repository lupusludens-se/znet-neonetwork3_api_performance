using SE.Neo.EmailTemplates.Models;

namespace SE.Neo.Infrastructure.Services.Interfaces
{
    public interface ISendgridService
    {
        Task<List<UndeliveredData>> GetDataFromSendgrid(string undeliveredMailSubject);

    }
}