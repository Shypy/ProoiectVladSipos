using ProoiectVladSipos.Models;
using System.ComponentModel;

namespace ProoiectVladSipos.Views
{
    public partial class CreditDetailPage : ContentPage
    {
        public List<User> Users { get; set; }
        public User SelectedUser { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public CreditDetailPage()
        {
            InitializeComponent();
            //BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            Users = await App.Database.GetUsersAsync();

            var credit = BindingContext as Credits;
            if (credit != null)
            {
                if (credit.UserID > 0)
                {
                    SelectedUser = Users.FirstOrDefault(u => u.ID == credit.UserID);
                }
                else
                {
                    SelectedUser = Users.FirstOrDefault(); 
                }
            }

            UserPicker.ItemsSource = Users;
            UserPicker.SelectedItem = SelectedUser;

            //OnPropertyChanged(nameof(Users));
            //OnPropertyChanged(nameof(SelectedUser));
        }


        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            var credit = BindingContext as Credits;
            if (credit == null) return;

            // Validare pentru UserID
            if (UserPicker.SelectedItem == null)
            {
                await DisplayAlert("Validation Error", "Please select a user.", "OK");
                return;
            }

            credit.UserID = ((User)UserPicker.SelectedItem).ID;

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

            await App.Database.SaveCreditAsync(credit);
            await Navigation.PopAsync();
        }


        private async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var credit = BindingContext as Credits;
            if (credit == null) return;

            if (credit.ID > 0)
            {
                bool confirm = await DisplayAlert("Confirm Delete", "Are you sure you want to delete this credit?", "Yes", "No");
                if (confirm)
                {
                    await App.Database.DeleteCreditAsync(credit);
                    await Navigation.PopAsync();
                }
            }
        }
    }
}
