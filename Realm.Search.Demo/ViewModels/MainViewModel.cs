using Acr.UserDialogs;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Realm.Search.Demo.Services;

namespace Realm.Search.Demo
{
    public partial class MainViewModel : ObservableObject
    {
        public MainViewModel()
        {
            _ = InitializeSearch();
        }

        private static async Task InitializeSearch()
        {
            try
            {
                using var loadingDialog = UserDialogs.Instance.Loading("Initializing Realm service");

                loadingDialog.Show();
                await SearchService.Initialize();
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert($"An error occurred while initializing the Realm app: {ex}", "Initialization failed");
            }
        }

        [RelayCommand]
        private async Task Navigate(string destination)
        {
            await Shell.Current.GoToAsync(destination);
        }
    }
}