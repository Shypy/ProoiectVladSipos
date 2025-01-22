using ProoiectVladSipos.Models;

namespace ProoiectVladSipos.Views
{
    public partial class RepaymentPlanPage : ContentPage
    {
        private int _creditId;

        public RepaymentPlanPage(int creditId)
        {
            InitializeComponent();
            _creditId = creditId;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var plan = await App.Database.GetRepaymentPlanByCreditIdAsync(_creditId);
            RepaymentPlanListView.ItemsSource = plan;

        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            // Poți face ceva la selectarea unei rate, ex. detalii
            // De regulă, deselectezi direct
            if (e.SelectedItem != null)
            {
                RepaymentPlanListView.SelectedItem = null;
            }
        }
    }
}
