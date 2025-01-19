using ProoiectVladSipos.Models;

namespace ProoiectVladSipos
{
    public partial class CreditDetailPage : ContentPage
    {
        public CreditDetailPage()
        {
            InitializeComponent();
        }

        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            var credit = (Credits)BindingContext;
            await App.Database.SaveCreditAsync(credit);

            await Navigation.PopAsync();
        }

        private async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var credit = (Credits)BindingContext;
            if (credit.ID != 0)
            {
                bool confirm = await DisplayAlert("Confirm Delete",
                                                  "Are you sure you want to delete this credit?",
                                                  "Yes",
                                                  "No");
                if (confirm)
                {
                    await App.Database.DeleteCreditAsync(credit);
                    await Navigation.PopAsync();
                }
            }
        }
    }
}
