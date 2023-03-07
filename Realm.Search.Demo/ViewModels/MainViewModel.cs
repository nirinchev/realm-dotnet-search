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

		private async Task InitializeSearch()
		{
			using var loadingDialog = UserDialogs.Instance.Loading("Initializing Realm service");

            try
			{
				loadingDialog.Show();
                await SearchService.Initialize();
            }
			catch (Exception ex)
			{
				UserDialogs.Instance.Alert($"An error occurred while initializing the Realm app: {ex}", "Initialization failed");
			}
			finally
			{
				loadingDialog.Hide();
			}
        }

		[RelayCommand]
		private async Task Navigate(string destination)
		{
			await Shell.Current.GoToAsync(destination);
		}
	}
}