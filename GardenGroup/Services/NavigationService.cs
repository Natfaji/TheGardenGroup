using System;
using System.Windows.Controls;
using UI.Stores;
using UI.ViewModel;

namespace UI.Services
{
	public class NavigationService
	{
		private readonly NavigationStore _navigationStore;
		private readonly ViewModelBase _createViewModel;

		public NavigationService(NavigationStore navigationStore, ViewModelBase createViewModel)
		{
			_navigationStore = navigationStore;
			_createViewModel = createViewModel;
		}

		public void Navigate()
		{
			_navigationStore.CurrentViewModel = _createViewModel;
		}
	}
}
