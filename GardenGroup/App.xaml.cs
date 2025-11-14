using System.Windows;
using System.Windows.Controls;
using UI;
using UI.Services;
using UI.Stores;
using UI.ViewModel;

namespace GardenGroup
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public static readonly SessionService SessionService = new SessionService();
		private readonly NavigationStore _navigationStore = new NavigationStore();

		protected override void OnStartup(StartupEventArgs e)
		{
			_navigationStore.CurrentViewModel = new LoginViewModel(_navigationStore);
			MainWindow = new MainWindow()
			{
				DataContext = new MainViewModel(_navigationStore)
			};
			MainWindow.Show();

			base.OnStartup(e);
		}
	}
}
