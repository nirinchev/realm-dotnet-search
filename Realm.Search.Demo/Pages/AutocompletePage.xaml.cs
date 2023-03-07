using Realm.Search.Demo.ViewModels;

namespace Realm.Search.Demo.Pages;

public partial class AutocompletePage : ContentPage
{
	public AutocompletePage()
	{
		try
		{
			InitializeComponent();

			BindingContext = new AutocompleteViewModel();
		}
		catch (Exception eX)
		{
			Console.WriteLine(eX);
		}
    }
}
