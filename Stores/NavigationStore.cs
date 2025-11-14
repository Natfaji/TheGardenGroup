using System;
using System.Collections.Generic;
using UI.ViewModel;

namespace UI.Stores
{
	public class NavigationStore
	{
		private readonly Stack<ViewModelBase> _backStack = new();
		private readonly Stack<ViewModelBase> _forwardStack = new();

		private ViewModelBase _currentViewModel;

		public ViewModelBase CurrentViewModel
		{
			get => _currentViewModel;
			set
			{
				if (_currentViewModel != null && value != _currentViewModel)
				{
					_backStack.Push(_currentViewModel);
					_forwardStack.Clear(); // clear forward history on new navigation
				}

				_currentViewModel?.Dispose();
				_currentViewModel = value;
				CurrentViewModelChanged?.Invoke();
			}
		}

		public event Action CurrentViewModelChanged;

		public bool CanGoBack => _backStack.Count > 0;
		public bool CanGoForward => _forwardStack.Count > 0;

		public void GoBack()
		{
			if (!CanGoBack)
				return;

			_forwardStack.Push(_currentViewModel);
			_currentViewModel?.Dispose();

			_currentViewModel = _backStack.Pop();
			CurrentViewModelChanged?.Invoke();
		}

		public void GoForward()
		{
			if (!CanGoForward)
				return;

			_backStack.Push(_currentViewModel);
			_currentViewModel?.Dispose();

			_currentViewModel = _forwardStack.Pop();
			CurrentViewModelChanged?.Invoke();
		}
	}
}
