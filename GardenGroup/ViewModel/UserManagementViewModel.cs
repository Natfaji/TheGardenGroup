using Logic;
using Model.Models;
using Model.Models.Request;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using UI.Stores;

namespace UI.ViewModel
{
	public class UserManagementViewModel : ViewModelBase
	{
		#region Fields
		private ObservableCollection<PartialUser> _users;
		private PartialUser _selectedUser;
		#endregion

		#region Properties
		public ObservableCollection<PartialUser> Users
		{
			get => _users;
			set
			{
				_users = value;
				OnPropertyChanged(nameof(Users));
			}
		}
		public PartialUser SelectedUser
		{
			get => _selectedUser;
			set
			{
				_selectedUser = value;
				OnPropertyChanged(nameof(SelectedUser));
			}
		}
		#endregion

		#region Commands
		public ICommand CreateUserCommand { get; }
		public ICommand EditUserCommand { get; }
		public ICommand DeleteUserCommand { get; }
		#endregion

		#region Pagination Commands
		public ICommand NextPageCommand { get; }
		public ICommand PreviousPageCommand { get; }
		public ICommand ChangeItemsPerPageCommand { get; }
		#endregion

		public RequestParameters RequestParameters { get; } = new RequestParameters();
		private readonly NavigationStore _navigationStore;

		private UserLogic userLogic;

		public UserManagementViewModel(NavigationStore navigationStore)
		{
			userLogic = new UserLogic();
			this._navigationStore = navigationStore;

			CreateUserCommand = new ViewModelCommand(_ => CreateUser());
			EditUserCommand = new ViewModelCommand(_ => EditUser());
			DeleteUserCommand = new ViewModelCommand(_ => DeleteUser());

			NextPageCommand = new ViewModelCommand(_ => NextPage());
			PreviousPageCommand = new ViewModelCommand(_ => PreviousPage());
			ChangeItemsPerPageCommand = new ViewModelCommand(param => ChangeItemsPerPage((int)param));

			LoadUsers();
		}

		private void CreateUser()
		{

		}

		private void EditUser()
		{

		}

		private void DeleteUser()
		{

		}

		private async Task LoadUsers()
		{
			RequestParameters requestParameters = new RequestParameters();
			Users = new ObservableCollection<PartialUser>(await userLogic.ReadAll(requestParameters));
		}

		private void NextPage()
		{
			RequestParameters.NextPage();
			OnPropertyChanged(nameof(RequestParameters));

			loadItems();
		}

		private void PreviousPage()
		{
			RequestParameters.PreviousPage();
			OnPropertyChanged(nameof(RequestParameters));

			loadItems();
		}

		private void ChangeItemsPerPage(int param)
		{
			RequestParameters.ItemsPerPage = param;
			OnPropertyChanged(nameof(RequestParameters));

			loadItems();
		}

		private async void loadItems()
		{
			//var userList = await userLogic.Search(SearchQuery, RequestParameters); //, partialUser
			//Users = new ObservableCollection<PartialUser>(userList);
		}
	}
}
