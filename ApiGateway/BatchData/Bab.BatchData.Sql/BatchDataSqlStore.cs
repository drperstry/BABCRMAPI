using Bab.BatchData.Models;
using Bab.BatchData.Sql.Data;
using Microsoft.EntityFrameworkCore;

namespace Bab.BatchData.Sql
{
    public class BatchDataSqlStore : IBatchDataSqlStore
    {
        private readonly BabContext _dbContext;

        public BatchDataSqlStore(BabContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Contact> GetContactByCIF(int cif)
        {
            var contact = await _dbContext.Contacts.FirstOrDefaultAsync(c => c.Cif == cif);

            return contact;
        }

        public async Task<IEnumerable<Contact>> ExecuteContactQuery(string queryCondition, params object[] parameters)
        {
            var query = @"SELECT Contact.* FROM Contact " + queryCondition;

            if (parameters != null)
            {
                return await _dbContext.Contacts.FromSqlRaw(query, parameters).ToListAsync();
            }
            else
            {
                return await _dbContext.Contacts.FromSqlRaw(query).ToListAsync();
            }
        }

        public async Task<IEnumerable<Contact>> GetContacts(ContactsFilter contactsFilter)
        {
            var contacts = await _dbContext.Contacts
                .Where(c => string.IsNullOrWhiteSpace(contactsFilter.SamaClass) || c.SamaClass == contactsFilter.SamaClass)
                .Where(c => string.IsNullOrWhiteSpace(contactsFilter.Nationality) || c.Nationality == contactsFilter.Nationality)
                .Where(c => string.IsNullOrWhiteSpace(contactsFilter.MaritalStatus) || c.MaritalStatus == contactsFilter.MaritalStatus)
                .Where(c => contactsFilter.IdType == null || c.IdType == contactsFilter.IdType)
                .Where(c => string.IsNullOrWhiteSpace(contactsFilter.Gender) || c.Gender == contactsFilter.Gender)
                .Where(c => contactsFilter.PreferredLanguage == null || c.PreferredLanguage == contactsFilter.PreferredLanguage)
                .Where(c => string.IsNullOrWhiteSpace(contactsFilter.LegalStatusCode) || c.LegalStatusCode == contactsFilter.LegalStatusCode)
                .Where(c => contactsFilter.Number == null || c.MobileNumber == contactsFilter.Number)
                .Where(c => contactsFilter.Segment == null || c.Segment == contactsFilter.Segment)
                .Where(c => string.IsNullOrWhiteSpace(contactsFilter.City) || c.City == contactsFilter.City).ToListAsync();

            return contacts;
        }

        public async Task<IEnumerable<FinancialAccountDetails>> GetFinancialAccountsByCif(int cif)
        {
            var query = from account in _dbContext.FinancialAccounts
                        join contact in _dbContext.Contacts
                        on account.Cif equals contact.Cif
                        where account.Cif == cif
                        select new FinancialAccountDetails
                        {
                            FinancialAccount = account,
                            Email = contact.Email,
                            MobileNumber = contact.MobileNumber,
                            FirstName = contact.FirstName,
                            MiddleName = contact.MiddleName,
                            GrandFatherName = contact.GrandfatherName,
                            LastName = contact.FamilyName
                        };

            var result = await query.ToListAsync();

            return result;
        }

        public async Task<FinancialAccountDetails> GetFinancialAccountByAccountNumber(string accountNumber)
        {
            var query = from account in _dbContext.FinancialAccounts
                        join contact in _dbContext.Contacts
                        on account.Cif equals contact.Cif
                        where account.AccountNumber == accountNumber
                        select new FinancialAccountDetails
                        {
                            FinancialAccount = account,
                            Email = contact.Email,
                            MobileNumber = contact.MobileNumber,
                            FirstName = contact.FirstName,
                            MiddleName = contact.MiddleName,
                            GrandFatherName = contact.GrandfatherName,
                            LastName = contact.FamilyName
                        };

            var result = await query.FirstOrDefaultAsync();

            return result;
        }

        public async Task<IEnumerable<CustomFinancialAccount>> ExecuteFinancialAccountQuery(string queryCondition, params object[] parameters)
        {
            var query = @"SELECT [FinancialAccount].*, Contact.SQL_Email, Contact.SQL_MobileNumber, Contact.SQL_FirstName,
Contact.SQL_MiddleName,Contact.SQL_GrandfatherName,Contact.SQL_FamilyName
                          FROM [FinancialAccount] INNER JOIN
                          Contact ON [FinancialAccount].SQL_CIF = Contact.SQL_CIF " + queryCondition;


            if (parameters != null)
            {
                return await _dbContext.CustomFinancialAccount.FromSqlRaw(query, parameters).ToListAsync();
            }
            else
            {
                return await _dbContext.CustomFinancialAccount.FromSqlRaw(query).ToListAsync();
            }
        }

        
        public async Task<IEnumerable<CustomFinancePaymentHistory>> GetFinancePaymentHistoryByCIF(int cif)
        {
            var query = @"SELECT [FinancePaymentHistory].*, Contact.SQL_Email, Contact.SQL_MobileNumber
                          FROM [FinancePaymentHistory] INNER JOIN
                          Contact ON [FinancePaymentHistory].SQL_CIF = Contact.SQL_CIF
                          WHERE ([FinancePaymentHistory].SQL_CIF = @p0)";

            return await _dbContext.CustomFinancePaymentHistories.FromSqlRaw(query, cif).ToListAsync();
        }

        public async Task<IEnumerable<CustomFinancePaymentHistory>> ExecuteFinancePaymentHistoryQuery(string queryCondition, params object[] parameters)
        {
            var query = @"SELECT [FinancePaymentHistory].*, Contact.SQL_Email, Contact.SQL_MobileNumber
                          FROM [FinancePaymentHistory] INNER JOIN
                          Contact ON [FinancePaymentHistory].SQL_CIF = Contact.SQL_CIF " + queryCondition;


            if (parameters != null)
            {
                return await _dbContext.CustomFinancePaymentHistories.FromSqlRaw(query, parameters).ToListAsync();
            }
            else
            {
                return await _dbContext.CustomFinancePaymentHistories.FromSqlRaw(query).ToListAsync();
            }
        }

        public async Task<IEnumerable<CustomLoan>> GetLoansByCIF(int cif)
        {
            var query = @"SELECT Loan.*, Contact.SQL_Email, Contact.SQL_MobileNumber,Contact.SQL_FirstName,
                        Contact.SQL_MiddleName,Contact.SQL_GrandfatherName,
                        Contact.SQL_FamilyName
                          FROM Loan INNER JOIN
                          Contact ON Loan.SQL_CIF = Contact.SQL_CIF
                          WHERE (Loan.SQL_CIF = @p0)";

            return await _dbContext.CustomLoans.FromSqlRaw(query, cif).ToListAsync();
        }

        public async Task<CustomLoan> GetLoanById(string loanId)
        {
            var query = @"SELECT Loan.*, Contact.SQL_Email, Contact.SQL_MobileNumber,Contact.SQL_FirstName,
Contact.SQL_MiddleName,Contact.SQL_GrandfatherName,Contact.SQL_FamilyName
                          FROM Loan INNER JOIN
                          Contact ON Loan.SQL_CIF = Contact.SQL_CIF
                          WHERE (Loan.SQL_LoanId = @p0)";

            return await _dbContext.CustomLoans.FromSqlRaw(query, loanId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<CustomLoan>> ExecuteLoansQuery(string queryCondition, params object[] parameters)
        {
            var query = @"SELECT Loan.*, Contact.SQL_Email, Contact.SQL_MobileNumber,Contact.SQL_FirstName,
Contact.SQL_MiddleName,Contact.SQL_GrandfatherName,Contact.SQL_FamilyName
                          FROM Loan INNER JOIN
                          Contact ON Loan.SQL_CIF = Contact.SQL_CIF " + queryCondition;


            if (parameters != null)
            {
                return await _dbContext.CustomLoans.FromSqlRaw(query, parameters).ToListAsync();
            }
            else
            {
                return await _dbContext.CustomLoans.FromSqlRaw(query).ToListAsync();
            }
        }

        public async Task<IEnumerable<CustomCreditCard>> GetCreditCardsByCIF(int cif)
        {
            var query = @"SELECT [CreditCard].*, Contact.SQL_Email, Contact.SQL_MobileNumber,Contact.SQL_FirstName,
Contact.SQL_MiddleName,Contact.SQL_GrandfatherName,Contact.SQL_FamilyName
                          FROM [CreditCard] INNER JOIN
                          Contact ON [CreditCard].SQL_CIF = Contact.SQL_CIF
                          WHERE ([CreditCard].SQL_CIF = @p0)";

            return await _dbContext.CustomCreditCards.FromSqlRaw(query, cif).ToListAsync();
        }

        public async Task<CustomCreditCard> GetCreditCardByCMS(long cmsReplacementNumber)
        {
            var query = @"SELECT [CreditCard].*, Contact.SQL_Email, Contact.SQL_MobileNumber,Contact.SQL_FirstName,
Contact.SQL_MiddleName,Contact.SQL_GrandfatherName,Contact.SQL_FamilyName
                          FROM [CreditCard] INNER JOIN
                          Contact ON [CreditCard].SQL_CIF = Contact.SQL_CIF
                          WHERE ([CreditCard].SQL_CMSReplacementNumber = @p0)";

            return await _dbContext.CustomCreditCards.FromSqlRaw(query, cmsReplacementNumber).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<CustomCreditCard>> ExecuteCreditCardsQuery(string queryCondition, params object[] parameters)
        {
            var query = @"SELECT [CreditCard].*, Contact.SQL_Email, Contact.SQL_MobileNumber,Contact.SQL_FirstName,
Contact.SQL_MiddleName,Contact.SQL_GrandfatherName,Contact.SQL_FamilyName
                          FROM [CreditCard] INNER JOIN
                          Contact ON [CreditCard].SQL_CIF = Contact.SQL_CIF " + queryCondition;


            if (parameters != null)
            {
                return await _dbContext.CustomCreditCards.FromSqlRaw(query, parameters).ToListAsync();
            }
            else
            {
                return await _dbContext.CustomCreditCards.FromSqlRaw(query).ToListAsync();
            }
        }

        public async Task<IEnumerable<CustomDebitCard>> GetDebitCardsByCIF(int cif)
        {
            var query = @"SELECT [DebitCard].*, Contact.SQL_Email, Contact.SQL_MobileNumber,Contact.SQL_FirstName,
Contact.SQL_MiddleName,Contact.SQL_GrandfatherName,Contact.SQL_FamilyName
                          FROM [DebitCard] INNER JOIN
                          Contact ON [DebitCard].SQL_CIF = Contact.SQL_CIF
                          WHERE ([DebitCard].SQL_CIF = @p0)";

            return await _dbContext.CustomDebitCards.FromSqlRaw(query, cif).ToListAsync();
        }

        public async Task<CustomDebitCard> GetDebitCardByCMS(long cmsReplacementNumber)
        {
            var query = @"SELECT [DebitCard].*, Contact.SQL_Email, Contact.SQL_MobileNumber, Contact.SQL_FirstName,
Contact.SQL_MiddleName,Contact.SQL_GrandfatherName,Contact.SQL_FamilyName
                          FROM [DebitCard] INNER JOIN
                          Contact ON [DebitCard].SQL_CIF = Contact.SQL_CIF
                          WHERE ([DebitCard].SQL_CMSReplacementNumber = @p0)";

            return await _dbContext.CustomDebitCards.FromSqlRaw(query, cmsReplacementNumber).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<CustomDebitCard>> ExecuteDebitCardsQuery(string queryCondition, params object[] parameters)
        {
            var query = @"SELECT [DebitCard].*, Contact.SQL_Email, Contact.SQL_MobileNumber,Contact.SQL_FirstName,
Contact.SQL_MiddleName,Contact.SQL_GrandfatherName,Contact.SQL_FamilyName
                          FROM [DebitCard] INNER JOIN
                          Contact ON [DebitCard].SQL_CIF = Contact.SQL_CIF " + queryCondition;


            if (parameters != null)
            {
                return await _dbContext.CustomDebitCards.FromSqlRaw(query, parameters).ToListAsync();
            }
            else
            {
                return await _dbContext.CustomDebitCards.FromSqlRaw(query).ToListAsync();
            }
        }

        public async Task<IEnumerable<Branch>> GetBranches()
        {
            var branches = await _dbContext.Branches.ToListAsync();

            return branches;
        }

        public async Task<IEnumerable<object>> GetProperties(string entityName)
        {
            var propertiesEntity =  _dbContext
           .GetType()
           .GetProperties()
           .Where(p =>
               p.PropertyType.IsGenericType &&
               p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
           .Select(p => p.PropertyType.GetGenericArguments()[0])
           .Where(t => t.Name.ToUpper() ==entityName.ToUpper())
           .SelectMany(t => t.GetProperties())
           .ToArray();

            var properties = propertiesEntity.Select(a => new { Name = a.Name, Type = a.PropertyType.Name!= "Nullable`1" ? a.PropertyType.Name : a.PropertyType.FullName.Split('.')[2].Split(',')[0] });

            return properties;
        }



        public async Task<IEnumerable<Atm>> GetAtms()
        {
            var atms = await _dbContext.Atms.ToListAsync();

            return atms;
        }

        public async Task<IEnumerable<Sector>> GetSectors()
        {
            var sectors = await _dbContext.Sectors.ToListAsync();

            return sectors;
        }

        public async Task<IEnumerable<Department>> GetDepartments()
        {
            var departments = await _dbContext.Departments.ToListAsync();

            return departments;
        }
    }
}