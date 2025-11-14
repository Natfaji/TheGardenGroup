using GardenGroup;
using Logic;
using Model.Models;
using System;
using System.Windows.Input;
using UI.Services;
using UI.Stores;
using UI.Views;

namespace UI.ViewModel
{
	public class LoginViewModel : ViewModelBase
	{
		#region Fields
		private string _username = "a";
		private string _password = "1234";
		private string _errorMessage;
		#endregion Fields

		#region Properties
		public string Username
		{
			get => _username;
			set
			{
				_username = value;
				OnPropertyChanged(nameof(Username));
			}
		}
		public string Password
		{
			get => _password;
			set
			{
				_password = value;
				OnPropertyChanged(nameof(Password));
			}
		}
		public string ErrorMessage
		{
			get => _errorMessage;
			set
			{
				_errorMessage = value;
				OnPropertyChanged(nameof(ErrorMessage));
			}
		}
		#endregion Properties

		#region Commands
		public ICommand LoginCommand { get; }
		public ICommand ResetPasswordCommand { get; }
		#endregion Commands

		private AuthLogic authLogic;

		public LoginViewModel(NavigationStore _navigationStore)
		{
			authLogic = new AuthLogic();

			this._navigationStore = _navigationStore;

			LoginCommand = new ViewModelCommand(_ => Login(), _ => CanLogin());
			ResetPasswordCommand = new ViewModelCommand(_ => ResetPassword());
		}

		private bool CanLogin()
		{
			if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
				return false;

			return true;
		}

		private void Login()
		{
			if (Username == "TEST" && Password == "TEST")
			{
				Tools.Seeder.Seed();
				return;
			}

			PartialUser user = authLogic.VerifyLogin(Username, Password);
			if (user == null)
			{
				ErrorMessage = "The username or password is not valid";
				return;
			}

			// Clear any previous error message
			ErrorMessage = string.Empty;

			// Set the current user in the session
			App.SessionService.CurrentUser = user;

			_navigationStore.CurrentViewModel = new AppLayoutViewModel(_navigationStore);
		}

		private void ResetPassword()
		{
		}
	}
}
