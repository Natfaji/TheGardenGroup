using DAL;
using Model.Enums;
using Model.Models;
using Model.Models.Request;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Logic
{
	public class TicketLogic
	{
		TicketDao ticketDao = new TicketDao();
		private readonly CommentLogic commentLogic = new CommentLogic();

		public TicketLogic()
		{
		}

		public void Create(Ticket entity)
		{
			ticketDao.Create(entity);
		}

		public async Task<Ticket> Read(ObjectId id)
		{
			return await ticketDao.Read(id);
		}

		public async Task<List<Ticket>> ReadAll(RequestParameters requestParameters)
		{
			return await ticketDao.ReadAll(requestParameters);
		}

		public async Task<Ticket> Update(ObjectId id, Ticket entity)
		{
			return await ticketDao.Update(id, entity);
		}

		public async Task Delete(ObjectId id)
		{
			await ticketDao.Delete(id);
		}




		public List<Ticket> GetTicketsEmployees(PartialUser loggedInUser, RequestParameters requestParameters)
		{
			return ticketDao.GetTicketsEmployees(loggedInUser, requestParameters);
		}

		public async Task<List<Ticket>> Search(string searchQuery, RequestParameters requestParameters, PartialUser? user = null)
		{
			var builder = Builders<Ticket>.Filter;
			var filters = new List<FilterDefinition<Ticket>>();

			var tokens = Regex.Matches(searchQuery, @"[^\s""]+|""([^""]*)""")
					  .Cast<Match>()
					  .Select(m => m.Value.Trim('"'))
					  .ToList();

			bool useAnd = false;
			bool useOr = false;

			foreach (string token in tokens)
			{
				// Check for logical keywords
				if (token.Equals("AND", System.StringComparison.OrdinalIgnoreCase) || token == "&&")
				{
					useAnd = true;
					continue;
				}
				if (token.Equals("OR", System.StringComparison.OrdinalIgnoreCase) || token == "||")
				{
					useOr = true;
					continue;
				}

				// Handle negation
				bool isNegative = token.StartsWith("-") || token.StartsWith("!");
				var keyword = isNegative ? token[1..] : token;

				// Create OR filter across Title + Description
				var titleFilter = builder.Regex("Title", new BsonRegularExpression(keyword, "i"));
				var descFilter = builder.Regex("Description", new BsonRegularExpression(keyword, "i"));
				var wordFilter = builder.Or(titleFilter, descFilter);

				if (isNegative)
					wordFilter = builder.Not(wordFilter);

				filters.Add(wordFilter);
			}

			FilterDefinition<Ticket> finalFilter;
			if (filters.Count == 0)
				finalFilter = builder.Empty;
			else if (useAnd && !useOr)
				finalFilter = builder.And(filters);
			else if (useOr && !useAnd)
				finalFilter = builder.Or(filters);
			else
				// Default for if both appear or none, use AND for stricter matching
				finalFilter = builder.And(filters);

			if (user != null)
			{
				var userFilter = builder.Eq("Assigned_to._id", user._id);
				finalFilter = builder.And(finalFilter, userFilter);
			}

			return await ticketDao.Search(finalFilter, requestParameters);
		}

		public void ChangeTicketStatus(ObjectId ticketId, Status status)
		{
			ticketDao.ChangeTicketStatus(ticketId, status);
		}

		//methode om de ticket en bijbehorende comment op te halen
		public async Task<(Ticket, ObservableCollection<Comment>)> GetTicketWithComments(ObjectId ticketId)
		{
			var ticket = await ticketDao.Read(ticketId);
			var comments = await commentLogic.GetCommentsForTicket(ticketId);
			return (ticket, comments);
		}

		public void AssignTicketToEmployee(ObjectId ticketId, PartialUser employee)
		{

			ticketDao.UpdateAssigneTo(ticketId, employee);
		}
	}
}
