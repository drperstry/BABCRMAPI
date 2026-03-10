using BabCrm.Core;
using BabCrm.Service.ArchiveDataModels;

namespace BabCrm.Service
{
    public interface IArchiveSqlStore
    {
        Task<IEnumerable<CaseRequest>> GetArchivedCaseRequests(string cif, DateTime? startDate, DateTime? endDate, string requestNumber = "");

        Task<IEnumerable<CaseRequest>> GetArchivedCases(string cif, DateTime? startDate, DateTime? endDate, string mobileNumber);

        Task<SubmissionResponse> GetArchivedData(string tableName, CommonFilter filter);
    }

}
