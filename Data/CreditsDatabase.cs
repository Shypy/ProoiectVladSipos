using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProoiectVladSipos.Models;

namespace ProoiectVladSipos.Data
{
    public class CreditsDatabase
    {
        private readonly SQLiteAsyncConnection _database;

        public CreditsDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Credits>().Wait();
            _database.CreateTableAsync<User>().Wait();
            //_database.CreateTableAsync<LoanType>().Wait();
        }

        // Obține toate creditele
        public Task<List<Credits>> GetCreditsAsync()
        {
            return _database.Table<Credits>().ToListAsync();
        }

        // Obține un credit după ID
        public Task<Credits> GetCreditByIdAsync(int id)
        {
            return _database.Table<Credits>()
                            .Where(c => c.ID == id)
                            .FirstOrDefaultAsync();
        }

        // Salvează un credit (inserează sau actualizează)
        public Task<int> SaveCreditAsync(Credits credit)
        {
            if (credit.ID != 0)
            {
                return _database.UpdateAsync(credit);
            }
            else
            {
                return _database.InsertAsync(credit);
            }
        }

        // Șterge un credit
        public Task<int> DeleteCreditAsync(Credits credit)
        {
            return _database.DeleteAsync(credit);
        }

       public Task<List<User>> GetUsersAsync()
        {
            return _database.Table<User>().ToListAsync();
        }

        public Task<int> SaveUserAsync(User user)
        {
            if (user.ID != 0)
            {
                return _database.UpdateAsync(user);
            }
            else
            {
                return _database.InsertAsync(user);
            }
        }

        public Task<int> DeleteUserAsync(User user)
        {
            return _database.DeleteAsync(user);
        }

        // Operații suplimentare pentru tipurile de credite
        /*public Task<List<LoanType>> GetLoanTypesAsync()
        {
            return _database.Table<LoanType>().ToListAsync();
        }

        public Task<int> SaveLoanTypeAsync(LoanType loanType)
        {
            if (loanType.ID != 0)
            {
                return _database.UpdateAsync(loanType);
            }
            else
            {
                return _database.InsertAsync(loanType);
            }
        }

        public Task<int> DeleteLoanTypeAsync(LoanType loanType)
        {
            return _database.DeleteAsync(loanType);
        }*/
    }
}
