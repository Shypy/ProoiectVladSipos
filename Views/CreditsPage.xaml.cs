using ProoiectVladSipos.Models;
using ProoiectVladSipos.GroupModels; 
using System.Collections.ObjectModel;

namespace ProoiectVladSipos.Views
{
    public partial class CreditsPage : ContentPage
    {
        // Colecția care va alimenta ListView cu date grupate
        public ObservableCollection<CreditsGroup> GroupedCredits { get; set; }
            = new ObservableCollection<CreditsGroup>();

        public CreditsPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Pas 1: obținem toți utilizatorii
            var users = await App.Database.GetUsersAsync();
            // Pas 2: obținem toate creditele
            var allCredits = await App.Database.GetCreditsAsync();

            GroupedCredits.Clear(); // Curăță înainte de reîncărcare

            // Pas 3: Pentru fiecare utilizator, luăm creditele asociate
            foreach (var user in users)
            {
                // Filtrăm creditele după UserID
                var userCredits = allCredits.Where(c => c.UserID == user.ID).ToList();

                // Dacă utilizatorul are credite, construim un group
                if (userCredits.Any())
                {
                    var group = new CreditsGroup(user, userCredits);
                    GroupedCredits.Add(group);
                }
            }

            // Pas 4: Legăm colecția grupată la ListView
            CreditsListView.ItemsSource = GroupedCredits;
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
            if (e.SelectedItem is Credits selectedCredit)
            {
                await Navigation.PushAsync(new CreditDetailPage
                {
                    BindingContext = selectedCredit
                });
            }
        }
    }
}
