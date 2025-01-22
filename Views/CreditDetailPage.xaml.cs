using ProoiectVladSipos.Models;
using System.ComponentModel;

namespace ProoiectVladSipos.Views
{
    public partial class CreditDetailPage : ContentPage
    {
        private List<User> _users;
        private List<LoanType> _loanTypes;

        public CreditDetailPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // 1. Obținem obiectul de tip Credits care e în BindingContext
            var credit = BindingContext as Credits;
            if (credit == null) return;

            // 2. Încărcăm date
            _users = await App.Database.GetUsersAsync();
            _loanTypes = await App.Database.GetLoanTypesAsync();

            // 3. Populăm pickerele
            UserPicker.ItemsSource = _users;
            LoanTypePicker.ItemsSource = _loanTypes;

            // 4. Setăm user-ul selectat
            if (credit.UserID > 0)
            {
                var user = _users.FirstOrDefault(u => u.ID == credit.UserID);
                UserPicker.SelectedItem = user;
            }

            // 5. Setăm loan type-ul selectat
            if (credit.LoanTypeID > 0)
            {
                var loanType = _loanTypes.FirstOrDefault(lt => lt.ID == credit.LoanTypeID);
                LoanTypePicker.SelectedItem = loanType;

                // Dacă creditul există (ID != 0), blocăm modificarea tipului
                if (credit.ID != 0)
                {
                    LoanTypePicker.IsEnabled = false;
                }
            }

            // Dacă e un credit nou (ID == 0), permitem alegerea tipului
            if (credit.ID == 0)
            {
                LoanTypePicker.SelectedIndexChanged += OnLoanTypePickerChanged;
            }
        }

        private void OnLoanTypePickerChanged(object sender, EventArgs e)
        {
            var credit = BindingContext as Credits;
            if (credit == null) return;

            // Când se schimbă tipul de credit (pentru un credit nou),
            // preluăm DefaultInterest și DefaultDurationMonths
            if (LoanTypePicker.SelectedItem is LoanType selectedType)
            {
                credit.AnualInterest = selectedType.DefaultInterest;
                credit.LoanMonths = selectedType.DefaultDurationMonths;
            }
        }

        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            var credit = BindingContext as Credits;
            if (credit == null) return;

            // Validări
            if (UserPicker.SelectedItem == null)
            {
                await DisplayAlert("Validation Error", "Please select a user.", "OK");
                return;
            }
            credit.UserID = ((User)UserPicker.SelectedItem).ID;

            if (LoanTypePicker.SelectedItem == null)
            {
                await DisplayAlert("Validation Error", "Please select a loan type.", "OK");
                return;
            }
            credit.LoanTypeID = ((LoanType)LoanTypePicker.SelectedItem).ID;

            if (credit.LoanedAmount <= 0)
            {
                await DisplayAlert("Validation Error", "Loaned amount must be greater than 0.", "OK");
                return;
            }
            if (credit.AnualInterest <= 0)
            {
                await DisplayAlert("Validation Error", "Annual interest must be greater than 0.", "OK");
                return;
            }
            if (credit.LoanMonths <= 0)
            {
                await DisplayAlert("Validation Error", "Loan duration must be greater than 0.", "OK");
                return;
            }

            // Salvăm în baza de date
            await App.Database.SaveCreditAsync(credit);
            await Navigation.PopAsync();
        }

        private async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var credit = BindingContext as Credits;
            if (credit == null) return;

            if (credit.ID > 0)
            {
                bool confirm = await DisplayAlert("Confirm Delete",
                    "Are you sure you want to delete this credit?",
                    "Yes", "No");
                if (confirm)
                {
                    await App.Database.DeleteCreditAsync(credit);
                    await Navigation.PopAsync();
                }
            }
        }
    }
}
