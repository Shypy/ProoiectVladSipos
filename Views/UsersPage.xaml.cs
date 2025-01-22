using ProoiectVladSipos.Models;
using System;
using System.Collections.ObjectModel;

namespace ProoiectVladSipos.Views
{
    public partial class UsersPage : ContentPage
    {
        public ObservableCollection<User> Users { get; set; }

        public UsersPage()
        {
            InitializeComponent();
            Users = new ObservableCollection<User>();
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var users = await App.Database.GetUsersAsync();
            Users.Clear();
            foreach (var user in users)
            {
                Users.Add(user);
            }
        }

        private async void OnAddUserClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UserDetailPage
            {
                BindingContext = new User()
            });
        }

        private async void OnUserSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is User selectedUser)
            {
                await Navigation.PushAsync(new UserDetailPage
                {
                    BindingContext = selectedUser
                });
            }
        }
    }
}
