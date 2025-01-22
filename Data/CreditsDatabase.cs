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
            _database.CreateTableAsync<RepaymentPlan>().Wait();
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

        public async Task<int> DeleteCreditAsync(Credits credit)
        {
            // 1. Șterge toate ratele din RepaymentPlan pentru acest Credit
            await _database.Table<RepaymentPlan>()
                           .Where(rp => rp.CreditID == credit.ID)
                           .DeleteAsync();

            // 2. Șterge creditul propriu-zis
            return await _database.DeleteAsync(credit);
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

        public async Task<int> DeleteUserAsync(User user)
        {
            // 1. Obținem toate creditele asociate acestui user
            var userCredits = await _database.Table<Credits>()
                                             .Where(c => c.UserID == user.ID)
                                             .ToListAsync();

            // 2. Dacă există credite, le ștergem pe toate
            if (userCredits.Any())
            {
                foreach (var credit in userCredits)
                {
                    await _database.DeleteAsync(credit);
                }
            }

            // 3. În final, ștergem și userul
            return await _database.DeleteAsync(user);
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
        public async Task GenerateRepaymentPlanAsync(Credits credit)
        {
            // 1. Ștergem planul existent (dacă există)
            await _database.Table<RepaymentPlan>()
                           .Where(rp => rp.CreditID == credit.ID)
                           .DeleteAsync();

            // 2. Calculăm ratele folosind formula clasică
            //    M = P * i / (1 - (1+i)^(-n))
            //    unde:
            //      P = suma împrumutată (LoanedAmount)
            //      i = dobânda lunară (annualInterest / 12 / 100)
            //      n = număr de luni (LoanMonths)
            //    Apoi, pentru fiecare lună:
            //      interest = sold * i
            //      principal = M - interest
            //      sold = sold - principal
            //    PaymentDate poate fi data creditului + i luni, sau altă logică

            decimal annualRate = credit.AnualInterest / 100; // ex. 10% => 0.10
            decimal monthlyRate = annualRate / 12;           // ex. 0.10/12 => 0.008333..
            int totalMonths = credit.LoanMonths;
            decimal principalAmount = credit.LoanedAmount;

            // Dacă dobânda e 0, e un caz special, dar aici presupunem > 0
            decimal monthlyPayment = CalculateMonthlyPayment(principalAmount, monthlyRate, totalMonths);

            // soldul curent
            decimal remainingBalance = principalAmount;

            var repaymentPlans = new List<RepaymentPlan>();

            for (int installment = 1; installment <= totalMonths; installment++)
            {
                // Calcul interest/principal pentru luna curentă
                decimal interest = remainingBalance * monthlyRate;
                decimal principal = monthlyPayment - interest;

                // În caz de rotunjiri, e posibil să ajustezi ultima rată
                // Ca exemplu simplu, facem:
                if (installment == totalMonths)
                {
                    // Ca să evităm solduri negative (cauzate de rotunjiri)
                    principal = remainingBalance;
                    monthlyPayment = principal + interest;
                }

                // Actualizăm soldul rămas
                remainingBalance -= principal;

                // Data scadentă: exemplu = credit.Date + (installment) luni
                DateTime paymentDate = credit.Date.AddMonths(installment);

                // Formăm obiectul
                var plan = new RepaymentPlan
                {
                    CreditID = credit.ID,
                    InstallmentNumber = installment,
                    PaymentAmount = decimal.Round(monthlyPayment, 2),
                    Principal = decimal.Round(principal, 2),
                    Interest = decimal.Round(interest, 2),
                    RemainingBalance = decimal.Round(remainingBalance, 2),
                    PaymentDate = paymentDate
                };
                repaymentPlans.Add(plan);
            }

            // 3. Inserăm toată lista în DB
            await _database.InsertAllAsync(repaymentPlans);
        }

        // Funcție ajutătoare pentru calculul ratei lunare
        private decimal CalculateMonthlyPayment(decimal principal, decimal monthlyRate, int months)
        {
            if (monthlyRate == 0)
            {
                // Dobândă 0 => plătește principal/număr de luni
                return months == 0 ? principal : principal / months;
            }
            // Formula: P * i / (1 - (1+i)^(-n))
            double p = (double)principal;
            double i = (double)monthlyRate;
            double n = (double)months;

            double payment = p * i / (1 - Math.Pow(1 + i, -n));
            return (decimal)payment;
        }
        public async Task<List<RepaymentPlan>> GetRepaymentPlanByCreditIdAsync(int creditId)
        {
            return await _database.Table<RepaymentPlan>()
                                  .Where(rp => rp.CreditID == creditId)
                                  .OrderBy(rp => rp.InstallmentNumber)
                                  .ToListAsync();
        }


    }
}
