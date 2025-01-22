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

            // Creăm cele trei tabele (dacă nu există deja)
            _database.CreateTableAsync<Credits>().Wait();
            _database.CreateTableAsync<User>().Wait();
            _database.CreateTableAsync<LoanType>().Wait();

            // Inițializare (seed) tipuri de credit
            //SeedLoanTypesAsync().Wait();
        }

        // Metodă de seed pentru LoanType
        private async Task SeedLoanTypesAsync()
        {
            var count =1;
            try
            {
                 count = await _database.Table<LoanType>().CountAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            if (count == 0)
                {
                    var loanTypes = new List<LoanType>
                    {
                        new LoanType { Name = "Ipotecar", DefaultInterest = 5m, DefaultDurationMonths = 360 },
                        new LoanType { Name = "Nevoi Personale", DefaultInterest = 10m, DefaultDurationMonths = 60 },
                        new LoanType { Name = "Credit Mașină", DefaultInterest = 7m, DefaultDurationMonths = 84 }
                    };
                try
                {
                    await _database.InsertAllAsync(loanTypes);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

            }
        }

        // ----------------------------------
        // Operații CRUD pentru Credits
        // ----------------------------------

        public Task<List<Credits>> GetCreditsAsync()
        {
            return _database.Table<Credits>().ToListAsync();
        }

        public Task<Credits> GetCreditByIdAsync(int id)
        {
            return _database.Table<Credits>()
                            .Where(c => c.ID == id)
                            .FirstOrDefaultAsync();
        }

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

        public Task<int> DeleteCreditAsync(Credits credit)
        {
            return _database.DeleteAsync(credit);
        }

        // ----------------------------------
        // Operații CRUD pentru User
        // ----------------------------------

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

        // ----------------------------------
        // Operații CRUD pentru LoanType
        // ----------------------------------

        public Task<List<LoanType>> GetLoanTypesAsync()
        {
            return _database.Table<LoanType>().ToListAsync();
        }

        public Task<LoanType> GetLoanTypeByIdAsync(int id)
        {
            return _database.Table<LoanType>()
                            .Where(lt => lt.ID == id)
                            .FirstOrDefaultAsync();
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

        public async Task<int> DeleteLoanTypeAsync(LoanType loanType)
        {
            // Verificăm dacă există credite pentru acest LoanType
            int associatedCreditsCount = await _database.Table<Credits>()
                .Where(c => c.LoanTypeID == loanType.ID)
                .CountAsync();

            if (associatedCreditsCount > 0)
            {
                // Aruncăm excepție – UI-ul o va prinde și va afișa un mesaj
                throw new InvalidOperationException(
                    "Cannot delete this LoanType because it is referenced by existing Credits."
                );
            }

            // Dacă nu există, putem șterge liniștiți
            return await _database.DeleteAsync(loanType);
        }

    }
}
