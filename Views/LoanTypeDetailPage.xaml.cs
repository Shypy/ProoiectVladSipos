using ProoiectVladSipos.Models;

namespace ProoiectVladSipos.Views
{
    public partial class LoanTypeDetailPage : ContentPage
    {
        public LoanTypeDetailPage()
        {
            InitializeComponent();
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            var loanType = (LoanType)BindingContext;
            if (loanType == null) return;

            // (Opțional) Poți adăuga validări, ex: nume gol, etc.

            await App.Database.SaveLoanTypeAsync(loanType);
            await Navigation.PopAsync(); // Înapoi la lista de LoanTypes
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            var loanType = (LoanType)BindingContext;
            if (loanType == null) return;

            // Confirmare ștergere
            bool confirm = await DisplayAlert("Confirm Delete",
                "Are you sure you want to delete this loan type?",
                "Yes", "No");

            if (confirm && loanType.ID != 0)
            {
                await App.Database.DeleteLoanTypeAsync(loanType);
                await Navigation.PopAsync();
            }
        }
    }
}
