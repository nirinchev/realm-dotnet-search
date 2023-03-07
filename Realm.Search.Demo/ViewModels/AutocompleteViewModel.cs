using Acr.UserDialogs;
using CommunityToolkit.Mvvm.ComponentModel;
using Realm.Search.Demo.Models;
using Realm.Search.Demo.Services;

namespace Realm.Search.Demo.ViewModels
{
    public partial class AutocompleteViewModel : ObservableObject
	{
        private readonly Action<string> _searchDebouncer;

        [ObservableProperty]
		[NotifyPropertyChangedFor(nameof(HasResults))]
		[NotifyPropertyChangedFor(nameof(IsSearching))]
        private Movie[] results = Array.Empty<Movie>();

        [ObservableProperty]
		[NotifyPropertyChangedFor(nameof(IsSearching))]
		private string searchQuery = string.Empty;

		public bool HasResults => Results.Any();

        public bool IsSearching => !HasResults && !string.IsNullOrEmpty(SearchQuery);

        public AutocompleteViewModel()
        {
            Action<string> search = (query) => _ = Search(query);

            _searchDebouncer = search.Debounce();
        }

        partial void OnSearchQueryChanged(string value)
        {
            Results = Array.Empty<Movie>();
            _searchDebouncer(value);
        }

        private async Task Search(string query)
        {
            try
            {
                var results = await SearchService.Autocomplete(query);
                if (query == SearchQuery)
                {
                    Results = results;
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Alert($"An error occurred while executing search: {ex}", "Failed to execute search");
            }
        }
    }
}
