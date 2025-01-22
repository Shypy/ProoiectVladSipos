using ProoiectVladSipos.Models;
using System;

namespace ProoiectVladSipos.Views;

public partial class UserDetailPage : ContentPage
{
    public UserDetailPage()
    {
        InitializeComponent();
    }

    private async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var user = (User)BindingContext;
        await App.Database.SaveUserAsync(user);

        await Navigation.PopAsync();
    }

    private async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var user = (User)BindingContext;
        if (user.ID != 0)
        {
            bool confirm = await DisplayAlert("Confirm Delete",
                                              "Are you sure you want to delete this user?",
                                              "Yes", "No");
            if (confirm)
            {
                await App.Database.DeleteUserAsync(user);
                await Navigation.PopAsync();
            }
        }
    }
}
