using UI.Stores;

namespace UI.ViewModel
{
	public class LoginLayoutViewModel : BaseLayoutViewModel
	{
		public LoginLayoutViewModel(NavigationStore parentNavigationStore) : base(parentNavigationStore)
		{
			_layoutNavigationStore.CurrentViewModel = new LoginViewModel(_layoutNavigationStore);
		}
	}
}
