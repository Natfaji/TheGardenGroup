using GardenGroup;
using Logic;
using Model.Enums;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UI.Stores;

namespace UI.ViewModel
{
	public class TicketDetailsViewModel : ViewModelBase
	{
		#region Fields
		private Ticket _ticket;
		private string _comment = "";
		#endregion Fields

		#region Properties
		public string Comment
		{
			get => _comment;
			set
			{
				_comment = value;
				OnPropertyChanged(nameof(Comment));
			}
		}

		public Ticket Ticket
		{
			get => _ticket;
			set
			{
				_ticket = value;
				OnPropertyChanged(nameof(Ticket));
			}
		}
		#endregion Properties

		#region Commands
		public ICommand AddCommentCommand { get; }
		public ICommand EditCommand { get; }
		public ICommand CancelCommand { get; }
		#endregion

		private readonly NavigationStore _navigationStore;

		private TicketLogic ticketLogic;

		public TicketDetailsViewModel(NavigationStore navigationStore, Ticket ticket)
		{
			ticketLogic = new TicketLogic();
			this._navigationStore = navigationStore;

			this._ticket = ticket;

			AddCommentCommand = new ViewModelCommand(_ => AddComment());
			EditCommand = new ViewModelCommand(_ => EditTicket());
			CancelCommand = new ViewModelCommand(_ => Cancel());
		}

		private async Task AddComment()
		{
			Comment newComment = new Comment
			{
				Author = App.SessionService.CurrentUser,
				Content = Comment,
				Created_at = DateTime.Now
			};

			Ticket.Comments.Add(newComment);

			await ticketLogic.Update(Ticket._id, Ticket);
		}

		private void EditTicket()
		{
			_navigationStore.CurrentViewModel = new CreateEditTicketViewModel(_navigationStore, _ticket);
		}

		private void Cancel()
		{
			_navigationStore.GoBack();
		}
	}
}
