using ProoiectVladSipos.Models;

namespace ProoiectVladSipos;

public partial class CreditsPage : ContentPage
{
	public CreditsPage()
	{
		InitializeComponent();
	}
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            CreditsListView.ItemsSource = await App.Database.GetCreditsAsync();
        }

        private async void OnAddCreditClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreditDetailPage
            {
                BindingContext = new Credits()
            });
        }

        private async void OnCreditSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                await Navigation.PushAsync(new CreditDetailPage
                {
                    BindingContext = e.SelectedItem as Credits
                });
            }
        }
}