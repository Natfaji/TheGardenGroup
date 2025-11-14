//using System;
//using UI.Stores;
//using UI.ViewModel;

//namespace UI.Services
//{
//	public class LayoutNavigationService
//	{
//		private readonly NavigationStore _navigationStore;
//		private readonly ViewModelBase _createViewModel;

//		public LayoutNavigationService(NavigationStore navigationStore,
//			ViewModelBase createViewModel)
//		{
//			_navigationStore = navigationStore;
//			_createViewModel = createViewModel;
//		}

//		public void Navigate()
//		{
//			_navigationStore.CurrentViewModel = new AppLayoutViewModel(_navigationStore);
//		}
//	}
//}
