using Realm.Search.Demo.Pages;

namespace Realm.Search.Demo;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute("autocomplete", typeof(AutocompletePage));
    }
}

