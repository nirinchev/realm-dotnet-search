using Microsoft.Maui.Maps;
using Realm.Search.Demo.ViewModels;

using Map = Microsoft.Maui.Controls.Maps.Map;

namespace Realm.Search.Demo.Pages;

public partial class CompoundPage : ContentPage
{
    private readonly CompoundViewModel _viewModel = new();
	public CompoundPage()
	{
		InitializeComponent();

        BindingContext = _viewModel;

        SearchMap.MoveToRegion(_viewModel.MapSpan);
        
        SearchMap.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(Map.VisibleRegion) && SearchMap.VisibleRegion != null)
            {
                _viewModel.MapSpan = SearchMap.VisibleRegion;
            }
        };
    }
}
