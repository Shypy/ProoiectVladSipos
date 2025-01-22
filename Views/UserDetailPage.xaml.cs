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
            // Afișează un mesaj de confirmare
            bool confirm = await DisplayAlert(
                "Confirm Delete",
                "Are you sure you want to delete this user and all their credits?",
                "Yes", "No"
            );

            if (confirm)
            {
                // Apelează metoda nouă din baza de date
                await App.Database.DeleteUserAsync(user);
                await Navigation.PopAsync();
            }
        }
    }

}
