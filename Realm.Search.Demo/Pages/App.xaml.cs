using Realm.Search.Demo.Converters;

namespace Realm.Search.Demo;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		Resources.Add("SearchModelToFormattedStringConverter", new SearchModelToFormattedStringConverter());

		MainPage = new AppShell();
	}
}

