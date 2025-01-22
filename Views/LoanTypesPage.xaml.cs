using ProoiectVladSipos.Models;
using System.Collections.ObjectModel;

namespace ProoiectVladSipos.Views
{
    public partial class LoanTypesPage : ContentPage
    {
        public ObservableCollection<LoanType> LoanTypes { get; set; }

        public LoanTypesPage()
        {
            InitializeComponent();
            LoanTypes = new ObservableCollection<LoanType>();
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Încărcăm lista de tipuri de credit din baza de date
            var loanTypesFromDb = await App.Database.GetLoanTypesAsync();
            LoanTypes.Clear();

            foreach (var lt in loanTypesFromDb)
            {
                LoanTypes.Add(lt);
            }
        }

        private async void OnAddLoanTypeClicked(object sender, EventArgs e)
        {
            // Navigăm la pagina de detalii pentru a adăuga un nou LoanType
            await Navigation.PushAsync(new LoanTypeDetailPage
            {
                BindingContext = new LoanType()
            });
        }

        private async void OnLoanTypeSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is LoanType selectedLoanType)
            {
                // Navigăm la pagina de detalii pentru a edita LoanType-ul selectat
                await Navigation.PushAsync(new LoanTypeDetailPage
                {
                    BindingContext = selectedLoanType
                });
            }
        }
    }
}
