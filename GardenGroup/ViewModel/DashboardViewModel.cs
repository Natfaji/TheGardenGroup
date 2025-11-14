using DAL;
using GardenGroup;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Model.Enums;
using Model.Models;
using MongoDB.Driver;
using SkiaSharp;
using System.Collections.Generic;
using System.Windows.Input;

namespace UI.ViewModel
{
	public class DashboardViewModel : ViewModelBase
	{
		public PartialUser LoggedInUser { get; } = App.SessionService.CurrentUser;
		public List<int> openTicketsCounts { get; set; } = new List<int>();
		public List<int> inProgressTicketsCounts { get; set; } = new List<int>();
		public List<int> resolvedTicketsCounts { get; set; } = new List<int>();
		public List<int> closedTicketsCounts { get; set; } = new List<int>();
		public List<int> doneTicketsCounts { get; set; } = new List<int>();
		public List<int> Filler { get; set; } = new List<int>();

		private TicketDao ticketDao;

		public ISeries[] Series { get; set; }

		public DashboardViewModel()
		{
			ticketDao = new TicketDao();

			var filters = new List<FilterDefinition<Ticket>>();

			// Always include a basic filter
			filters.Add(Builders<Ticket>.Filter.Empty);

			// Filter by user if not admin
			if (LoggedInUser.Role == Role.Employee)
			{
				// Normal user → only show _tickets they created
				filters.Add(Builders<Ticket>.Filter.Eq(t => t.Reported_by, LoggedInUser));
			}

			// Combine everything into a single filter
			var finalFilter = Builders<Ticket>.Filter.And(filters);

			openTicketsCounts = ticketDao.GetTicketsCountByStatusPerWeek(finalFilter, Status.Open);
			inProgressTicketsCounts = ticketDao.GetTicketsCountByStatusPerWeek(finalFilter, Status.InProgress);
			resolvedTicketsCounts = ticketDao.GetTicketsCountByStatusPerWeek(finalFilter, Status.Resolved);
			closedTicketsCounts = ticketDao.GetTicketsCountByStatusPerWeek(finalFilter, Status.Closed);

			doneTicketsCounts = CreateDoneList();

			CreateSeries();
		}

		private void CreateSeries()
		{
			CalculateFiller();

			Series = new ISeries[]
			{
				new StackedStepAreaSeries<int> {
					Values = openTicketsCounts,
					Name = "Open",
					Fill = new SolidColorPaint(SKColors.Red),
				},
				new StackedStepAreaSeries<int> {
					Values = inProgressTicketsCounts,
					Name = "InProgress",
					Fill = new SolidColorPaint(SKColors.Yellow)
				},
				new StackedStepAreaSeries<int> {
					Values = doneTicketsCounts,
					Name = "Resolved/Closed",
					Fill = new SolidColorPaint(SKColors.Green)
				},
				new StackedStepAreaSeries<int> {
					Values = Filler,
					Fill = new SolidColorPaint(SKColors.Gray),
				}
			};
		}

		private List<int> CreateDoneList()
		{
			List<int> doneTicketsCounts = new List<int>();

			for (int i = 0; i < resolvedTicketsCounts.Count; i++)
			{
				doneTicketsCounts.Add(resolvedTicketsCounts[i] + closedTicketsCounts[i]);
			}

			return doneTicketsCounts;
		}

		private void CalculateFiller()
		{
			int length = openTicketsCounts.Count;

			// Find the max sum at any index
			int maxSum = 0;
			for (int i = 0; i < length; i++)
			{
				int sum = openTicketsCounts[i] + inProgressTicketsCounts[i] + resolvedTicketsCounts[i] + closedTicketsCounts[i];
				if (sum > maxSum) maxSum = sum;
			}

			// Calculate filler for each index
			for (int i = 0; i < length; i++)
			{
				int currentSum = openTicketsCounts[i] + inProgressTicketsCounts[i] + resolvedTicketsCounts[i] + closedTicketsCounts[i];
				Filler.Add(maxSum - currentSum);
			}
		}
	}
}
