using GardenGroup;
using Logic;
using Model.Enums;
using Model.Models;
using Model.Models.Request;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using UI.Stores;

namespace UI.ViewModel
{
	public class CreateEditTicketViewModel : ViewModelBase
	{
		#region Fields
		private Ticket _ticket = new Ticket();
		private string _searchText = "";
		private IEnumerable<PartialUser> _assignedToList;
		private bool _assignedToPopupIsOpen = false;
		private bool _isDropDownOpen = false;
		#endregion Fields

		#region Properties
		public Ticket Ticket
		{
			get => _ticket;
			set
			{
				_ticket = value;
				OnPropertyChanged(nameof(Ticket));
			}
		}
		public string SearchText
		{
			get => _searchText;
			set
			{
				_searchText = value;
				IsDropDownOpen = true;
				OnPropertyChanged(nameof(SearchText));

				if (_searchText.Length >= 3)
				{
					SearchUsers();
				}

				if (string.IsNullOrWhiteSpace(_searchText))
				{
					SearchUsers();
				}
			}
		}
		public IEnumerable<PartialUser> AssignedToList
		{
			get => _assignedToList;
			set
			{
				_assignedToList = value;
				OnPropertyChanged(nameof(AssignedToList));
			}
		}

		public bool AssignedToPopupIsOpen
		{
			get => _assignedToPopupIsOpen;
			set
			{
				_assignedToPopupIsOpen = value;
				IsDropDownOpen = value;
				SearchText = "";
				OnPropertyChanged(nameof(AssignedToPopupIsOpen));
			}
		}

		public bool IsDropDownOpen
		{
			get => _isDropDownOpen;
			set
			{
				_isDropDownOpen = value;
				OnPropertyChanged(nameof(IsDropDownOpen));
			}
		}

		public IEnumerable<Status> StatusList { get; }
		public IEnumerable<Priority> PriorityList { get; }
		#endregion Properties

		#region Commands
		public ICommand EditAssignedToCommand { get; }
		public ICommand ToggleAssignUserCommand { get; }
		public ICommand SaveTicketCommand { get; }
		public ICommand CancelCommand { get; }
		#endregion Commands

		private UserLogic userLogic;

		public CreateEditTicketViewModel(NavigationStore _navigationStore, Ticket ticket = null)
		{
			userLogic = new UserLogic();
			this._navigationStore = _navigationStore;

			if (ticket != null)
			{
				Ticket = ticket;
				SaveTicketCommand = new ViewModelCommand(_ => EditTicket());
			}
			else
			{
				Ticket.Occured_on = DateTime.Now;
				SaveTicketCommand = new ViewModelCommand(_ => SaveTicket());
			}

			SearchUsers();
			StatusList = Enum.GetValues(typeof(Status)).Cast<Status>();
			PriorityList = Enum.GetValues(typeof(Priority)).Cast<Priority>();

			EditAssignedToCommand = new ViewModelCommand(_ => ToggleAssignedToPopupIsOpen());
			ToggleAssignUserCommand = new ViewModelCommand(selectedUser => ToggleAssignUser((PartialUser)selectedUser));
			CancelCommand = new ViewModelCommand(_ => Cancel());
		}

		private void ToggleAssignedToPopupIsOpen()
		{
			AssignedToPopupIsOpen = !AssignedToPopupIsOpen;
		}

		private void ToggleAssignUser(PartialUser selectedUser)
		{
			if (selectedUser == null) return;

			if (Ticket.Assigned_to == null)
				Ticket.Assigned_to = new ObservableCollection<PartialUser>();

			var existing = Ticket.Assigned_to
				.FirstOrDefault(u => u._id == selectedUser._id);

			if (existing != null)
			{
				Ticket.Assigned_to.Remove(existing);
			}
			else
			{
				Ticket.Assigned_to.Add(selectedUser);
			}
		}

		private async Task SearchUsers()
		{
			AssignedToList = await GetUsers();
		}

		private async Task<IEnumerable<PartialUser>> GetUsers()
		{
			RequestParameters requestParameters = new RequestParameters();

			FilterDefinition<PartialUser> filter = string.IsNullOrWhiteSpace(SearchText)
				? Builders<PartialUser>.Filter.Empty
				: Builders<PartialUser>.Filter.Regex(
					x => x.Name,
					new BsonRegularExpression(SearchText, "i")
				);
			List<PartialUser> partialUser = await userLogic.ReadAll(requestParameters, filter);
			return partialUser;
		}

		private void SaveTicket()
		{
			Ticket.Reported_by = App.SessionService.CurrentUser;
			Ticket.Created_at = DateTime.Now;
			Ticket.Status = Status.Open;
			Ticket.Comments = new ObservableCollection<Comment>();

			TicketLogic ticketLogic = new TicketLogic();
			ticketLogic.Create(Ticket);
		}

		private async Task EditTicket()
		{
			TicketLogic ticketLogic = new TicketLogic();
			Ticket ticket = await ticketLogic.Update(Ticket._id, Ticket);

			Cancel();
		}

		private void Cancel()
		{
			_navigationStore.GoBack();
		}
	}
}
