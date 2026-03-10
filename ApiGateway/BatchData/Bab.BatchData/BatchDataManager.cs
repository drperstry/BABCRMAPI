using Bab.BatchData.Models;

namespace Bab.BatchData
{
    public class BatchDataManager
    {
        private readonly IBatchDataSqlStore _batchDataSqlStore;

        public BatchDataManager(IBatchDataSqlStore batchDataSqlStore)
        {
          this._batchDataSqlStore = batchDataSqlStore;
        }

        public async Task<Contact> GetContactByCif(int cif)
        {
            return await _batchDataSqlStore.GetContactByCIF(cif);
        }

        public async Task<IEnumerable<Contact>> GetContacts(string queryCondition, params object[] parameters)
        {
            return await _batchDataSqlStore.ExecuteContactQuery(queryCondition, parameters);
        }

        public async Task<IEnumerable<Contact>> GetContacts(ContactsFilter contactsFilter)
        {
            return await _batchDataSqlStore.GetContacts(contactsFilter);
        }

        public async Task<IEnumerable<FinancialAccountDetails>> GetFinancialAccountsByCif(int cif, bool isDistinctByCif)
        {
            if (isDistinctByCif)
            {
                var result = await _batchDataSqlStore.GetFinancialAccountsByCif(cif);
                return result.Take(1);
            }
            else
            {
                return await _batchDataSqlStore.GetFinancialAccountsByCif(cif);
            }

        }

        public async Task<FinancialAccountDetails> GetFinancialAccountByAccountNumber(string accountNumber)
        {
            return await _batchDataSqlStore.GetFinancialAccountByAccountNumber(accountNumber);
        }

        public async Task<IEnumerable<CustomFinancialAccount>> GetFinancialAccounts(string queryCondition, bool isDistinctByCif, params object[] parameters)
        {
            if (isDistinctByCif)
            {
                var result = await _batchDataSqlStore.ExecuteFinancialAccountQuery(queryCondition, parameters);
                return result.DistinctBy(x => x.Cif);
            }
            else
            {
                return await _batchDataSqlStore.ExecuteFinancialAccountQuery(queryCondition, parameters);
            }
        }

        public async Task<IEnumerable<CustomFinancePaymentHistory>> GetFinancePaymentHistoryByCIF(int cif)
        {
            return await _batchDataSqlStore.GetFinancePaymentHistoryByCIF(cif);
        }

        public async Task<IEnumerable<CustomFinancePaymentHistory>> GetFinancePaymentHistory(string queryCondition, params object[] parameters)
        {
            return await _batchDataSqlStore.ExecuteFinancePaymentHistoryQuery(queryCondition, parameters);
        }

        public async Task<IEnumerable<CustomLoan>> GetLoansByCIF(int cif, bool isDistinctByCif)
        {
            if (isDistinctByCif)
            {
                var result = await _batchDataSqlStore.GetLoansByCIF(cif);
                return result.Take(1);
            }
            else
            {
                return await _batchDataSqlStore.GetLoansByCIF(cif);
            }
        }

        public async Task<CustomLoan> GetLoanById(string loanId)
        {
            return await _batchDataSqlStore.GetLoanById(loanId);
        }

        public async Task<IEnumerable<CustomLoan>> GetLoans(string queryCondition, bool isDistinctByCif, params object[] parameters)
        {
            if (isDistinctByCif)
            {
                var result = await _batchDataSqlStore.ExecuteLoansQuery(queryCondition, parameters);
                return result.DistinctBy(x => x.Cif);
            }
            else
            {
                return await _batchDataSqlStore.ExecuteLoansQuery(queryCondition, parameters);
            }

        }

        public async Task<IEnumerable<CustomCreditCard>> GetCreditCardsByCIF(int cif, bool isDistinctByCif)
        {
            if (isDistinctByCif)
            {
                var result = await _batchDataSqlStore.GetCreditCardsByCIF(cif);
                return result.Take(1);
            }
            else
            {
                return await _batchDataSqlStore.GetCreditCardsByCIF(cif);
            }
        }

        public async Task<CustomCreditCard> GetCreditCardByCMS(long cmsReplacementNumber)
        {
            return await _batchDataSqlStore.GetCreditCardByCMS(cmsReplacementNumber);
        }

        public async Task<IEnumerable<CustomCreditCard>> GetCreditCards(string queryCondition, bool isDistinctByCif, params object[] parameters)
        {
            if (isDistinctByCif)
            {
                var result = await _batchDataSqlStore.ExecuteCreditCardsQuery(queryCondition, parameters);
                return result.DistinctBy(x => x.Cif);
            }
            else
            {
                return await _batchDataSqlStore.ExecuteCreditCardsQuery(queryCondition, parameters);
            }
        }

        public async Task<IEnumerable<CustomDebitCard>> GetDebitCardsByCIF(int cif, bool isDistinctByCif)
        {
            if (isDistinctByCif)
            {
                var result = await _batchDataSqlStore.GetDebitCardsByCIF(cif);
                return result.Take(1);
            }
            else
            {
                return await _batchDataSqlStore.GetDebitCardsByCIF(cif);
            }

        }

        public async Task<CustomDebitCard> GetDebitCardByCMS(long cmsReplacementNumber)
        {
            return await _batchDataSqlStore.GetDebitCardByCMS(cmsReplacementNumber);
        }

        public async Task<IEnumerable<CustomDebitCard>> GetDebitCards(string queryCondition, bool isDistinctByCif, params object[] parameters)
        {
            if (isDistinctByCif)
            {
                var result = await _batchDataSqlStore.ExecuteDebitCardsQuery(queryCondition, parameters);
                return result.DistinctBy(x => x.Cif);
            }
            else
            {
                return await _batchDataSqlStore.ExecuteDebitCardsQuery(queryCondition, parameters);
            }
        }

        public async Task<IEnumerable<Branch>> GetBranches()
        {
            return await _batchDataSqlStore.GetBranches();
        }

        public async Task<IEnumerable<object>> GetProperties( string entityName)
        {
            return await _batchDataSqlStore.GetProperties(entityName);
        }

        public async Task<IEnumerable<Atm>> GetAtms()
        {
            return await _batchDataSqlStore.GetAtms();
        }

        public async Task<IEnumerable<Sector>> GetSectors()
        {
            return await _batchDataSqlStore.GetSectors();
        }

        public async Task<IEnumerable<Department>> GetDepartmentsFromDb()
        {
            return await _batchDataSqlStore.GetDepartments();
        }
    }
}
