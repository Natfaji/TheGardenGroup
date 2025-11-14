using UI.Stores;

namespace UI.ViewModel
{
	public class BaseLayoutViewModel : ViewModelBase
	{
		protected readonly NavigationStore _layoutNavigationStore;
		protected readonly NavigationStore _parentNavigationStore;

		public ViewModelBase CurrentViewModel => _layoutNavigationStore.CurrentViewModel;

		public BaseLayoutViewModel(NavigationStore parentNavigationStore)
		{
			_parentNavigationStore = parentNavigationStore;
			_layoutNavigationStore = new NavigationStore();

			_layoutNavigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
		}

		private void OnCurrentViewModelChanged()
		{
			OnPropertyChanged(nameof(CurrentViewModel));
		}

		public override void Dispose()
		{
			_layoutNavigationStore.CurrentViewModelChanged -= OnCurrentViewModelChanged;
			_layoutNavigationStore.CurrentViewModel?.Dispose();
			base.Dispose();
		}
	}
}
