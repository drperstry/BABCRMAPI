

using Bab.BatchData.Models;

namespace Bab.BatchData
{
    public interface IBatchDataSqlStore
    {
        public Task<Contact> GetContactByCIF(int Cif);

        public Task<IEnumerable<Contact>> ExecuteContactQuery(string queryCondition, params object[] parameters);

        public Task<IEnumerable<Contact>> GetContacts(ContactsFilter contactsFilter);

        public Task<IEnumerable<FinancialAccountDetails>> GetFinancialAccountsByCif(int cif);

        public Task<FinancialAccountDetails> GetFinancialAccountByAccountNumber(string accountNumber);

        public Task<IEnumerable<CustomFinancialAccount>> ExecuteFinancialAccountQuery(string queryCondition, params object[] parameters);

        public Task<IEnumerable<CustomFinancePaymentHistory>> GetFinancePaymentHistoryByCIF(int cif);

        public Task<IEnumerable<CustomFinancePaymentHistory>> ExecuteFinancePaymentHistoryQuery(string queryCondition, params object[] parameters);

        public Task<IEnumerable<CustomLoan>> GetLoansByCIF(int cif);

        public Task<CustomLoan> GetLoanById(string id);

        public Task<IEnumerable<CustomLoan>> ExecuteLoansQuery(string queryCondition, params object[] parameters);

        public Task<IEnumerable<CustomCreditCard>> GetCreditCardsByCIF(int cif);

        public Task<CustomCreditCard> GetCreditCardByCMS(long cmsReplacementNumber);

        public Task<IEnumerable<CustomCreditCard>> ExecuteCreditCardsQuery(string queryCondition, params object[] parameters);

        public Task<IEnumerable<CustomDebitCard>> GetDebitCardsByCIF(int cif);

        public Task<CustomDebitCard> GetDebitCardByCMS(long CMSReplacementNumber);

        public Task<IEnumerable<CustomDebitCard>> ExecuteDebitCardsQuery(string queryCondition, params object[] parameters);

        public Task<IEnumerable<Branch>> GetBranches();

        public  Task<IEnumerable<Atm>> GetAtms();

        public Task<IEnumerable<Sector>> GetSectors();

        public Task<IEnumerable<Department>> GetDepartments();

        public Task<IEnumerable<object>> GetProperties(string entityName);
    }
}
