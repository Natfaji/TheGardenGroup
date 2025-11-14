using CommunityToolkit.Mvvm.Input;
using GardenGroup;
using Model.Models;
using System;
using System.Windows.Input;
using UI.Stores;

namespace UI.ViewModel
{
	public class AppLayoutViewModel : BaseLayoutViewModel
	{
		#region Commands
		public ICommand NavigateDashboardCommand { get; }
		public ICommand NavigateTicketsCommand { get; }
		public ICommand NavigateEmployeesCommand { get; }
		public ICommand LogoutCommand { get; }
		#endregion Commands

		public PartialUser LoggedInUser { get; } = App.SessionService.CurrentUser;

		public string LoggedInAs => $"Welcome {LoggedInUser.Name}! You are a {LoggedInUser.Role}";

		public AppLayoutViewModel(NavigationStore parentNavigationStore) : base(parentNavigationStore)
		{
			// Initialize dashboard as default
			_layoutNavigationStore.CurrentViewModel = new DashboardViewModel();

			// Initialize navigation commands
			NavigateDashboardCommand = new ViewModelCommand(_ => _layoutNavigationStore.CurrentViewModel = new DashboardViewModel());
			NavigateTicketsCommand = new ViewModelCommand(_ => _layoutNavigationStore.CurrentViewModel = new TicketsOverviewViewModel(_layoutNavigationStore));
			NavigateEmployeesCommand = new ViewModelCommand(_ => _layoutNavigationStore.CurrentViewModel = new UserManagementViewModel(_layoutNavigationStore));
			LogoutCommand = new ViewModelCommand(_ => Logout());
		}

		private void Logout()
		{
			App.SessionService.CurrentUser = null;
			_parentNavigationStore.CurrentViewModel = new LoginLayoutViewModel(_parentNavigationStore);
		}
	}
}
