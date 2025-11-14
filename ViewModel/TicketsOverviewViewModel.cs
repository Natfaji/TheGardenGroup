using Logic;
using Model.Models;
using Model.Models.Request;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using UI.Stores;

namespace UI.ViewModel
{
	public class TicketsOverviewViewModel : ViewModelBase
	{
		#region Fields
		private string _searchQuery = "";
		private ObservableCollection<Ticket> _tickets;
		#endregion

		#region Properties
		public string SearchQuery
		{
			get => _searchQuery;
			set
			{
				_searchQuery = value;
				OnPropertyChanged(nameof(SearchQuery));
				SearchCommand.Execute(null);
			}
		}

		public ObservableCollection<Ticket> Tickets
		{
			get => _tickets;
			set
			{
				_tickets = value;
				OnPropertyChanged(nameof(Tickets));
			}
		}
		#endregion

		#region Pagination Commands
		public ICommand NextPageCommand { get; }
		public ICommand PreviousPageCommand { get; }
		public ICommand ChangeItemsPerPageCommand { get; }
		#endregion

		#region Commands
		public ICommand SearchCommand { get; }
		public ICommand CreateTicketCommand { get; }
		public ICommand CloseTicketCommand { get; }
		public ICommand OpenTicketDetailsCommand { get; }
		#endregion

		public RequestParameters RequestParameters { get; } = new RequestParameters();
		private readonly NavigationStore _navigationStore;

		private TicketLogic ticketLogic;

		public TicketsOverviewViewModel(NavigationStore navigationStore)
		{
			ticketLogic = new TicketLogic();
			this._navigationStore = navigationStore;
			NextPageCommand = new ViewModelCommand(_ => NextPage());
			PreviousPageCommand = new ViewModelCommand(_ => PreviousPage());
			ChangeItemsPerPageCommand = new ViewModelCommand(param => ChangeItemsPerPage((int)param));
			SearchCommand = new ViewModelCommand(_ => loadItems());
			CreateTicketCommand = new ViewModelCommand(_ => CreateTicket());
			CloseTicketCommand = new ViewModelCommand(param => CloseTicket((Ticket)param));
			OpenTicketDetailsCommand = new ViewModelCommand(param => OpenTicketDetails((Ticket)param));

			loadItems();
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
			var ticketList = await ticketLogic.Search(SearchQuery, RequestParameters); //, partialUser
			Tickets = new ObservableCollection<Ticket>(ticketList);
		}

		private void CreateTicket()
		{
			//NavigationRequested?.Invoke(new CreateTicket(partialUser));
			_navigationStore.CurrentViewModel = new CreateEditTicketViewModel(_navigationStore);
		}

		private void CloseTicket(Ticket ticket)
		{
			//ticketLogic.ChangeTicketStatus(_ticket._id, Status.Closed); // <----- TODO: add command
		}

		private void OpenTicketDetails(Ticket ticket)
		{
			_navigationStore.CurrentViewModel = new TicketDetailsViewModel(_navigationStore, ticket);
		}
	}
}
