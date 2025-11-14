using Model.Enums;
using Model.Models;
using Model.Models.Request;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;

namespace DAL
{
	public class TicketDao : BaseDao<Ticket>
	{
		public TicketDao() : base("tickets")
		{
		}

		public List<Ticket> GetTicketsEmployees(PartialUser loggedInUser, RequestParameters requestParameters)
		{
			IAggregateFluent<Ticket> tickets = collection
				.Aggregate()
				.Match(e => e.Status == Status.Open);

			if (loggedInUser.Role == Role.Employee)
			{
				tickets = tickets.Match(t => t.Reported_by._id == loggedInUser._id);
			}

			return tickets.SortByDescending(t => t.Created_at)
				.Skip((requestParameters.PageNumber - 1) * requestParameters.ItemsPerPage)
				.Limit(requestParameters.ItemsPerPage)
				.ToList();
		}

		public List<Ticket> GetTicketsByStatus(Status status, RequestParameters requestParameters)
		{
			List<Ticket> tickets = collection
				.Aggregate()
				.Match(Builders<Ticket>.Filter.Eq(ticket => ticket.Status, status))
				.SortByDescending(t => t.Created_at)
				.Skip((requestParameters.PageNumber - 1) * requestParameters.ItemsPerPage)
				.Limit(requestParameters.ItemsPerPage)
				.ToList();

			return tickets;
		}

		public async Task<List<Ticket>> Search(FilterDefinition<Ticket> filter, RequestParameters requestParameters)
		{
			return await collection.Find(filter)
				.SortByDescending(t => t.Created_at)
				.Skip((requestParameters.PageNumber - 1) * requestParameters.ItemsPerPage)
				.Limit(requestParameters.ItemsPerPage)
				.ToListAsync();
		}

		public void ChangeTicketStatus(ObjectId ticketId, Status status)
		{
			var tickets = collection
				.UpdateOne(
				Builders<Ticket>.Filter.Eq(ticket => ticket._id, ticketId),
				Builders<Ticket>.Update.Set(ticket => ticket.Status, status)
				);
		}

		public List<Ticket> getAllTicketsPerYear(int year, RequestParameters requestParameters)
		{
			var start = new DateTime(year, 1, 1);
			var end = start.AddYears(1);
			var tickets = collection
				.Find(ticket => ticket.Created_at >= start && ticket.Created_at < end)
				.SortByDescending(t => t.Created_at)
				.Skip((requestParameters.PageNumber - 1) * requestParameters.ItemsPerPage)
				.Limit(requestParameters.ItemsPerPage)
				.ToList();
			return tickets;
		}

		public List<int> GetTicketsCountByStatusPerWeek(FilterDefinition<Ticket> filter, Status status, int? year = null)
		{
			try
			{
				var start = new DateTime(year ?? DateTime.Now.Year, 1, 1);
				var end = start.AddYears(1);

				FilterDefinition<Ticket> matchFilter = Builders<Ticket>.Filter.And(
					filter,
					Builders<Ticket>.Filter.Eq(t => t.Status, status),
					Builders<Ticket>.Filter.Gte(t => t.Created_at, start),
					Builders<Ticket>.Filter.Lt(t => t.Created_at, end)
				);

				var tickets = collection.Aggregate()
					.Match(matchFilter)
					.AppendStage<BsonDocument>(new BsonDocument("$project", new BsonDocument
					{
						{ "Week", new BsonDocument("$isoWeek", "$created_at") }
					}))
					.AppendStage<TicketsCount>(new BsonDocument("$group", new BsonDocument
					{
						{ "_id", "$Week" },
						{ "count", new BsonDocument("$sum", 1) }
					}))
					.Sort(Builders<TicketsCount>.Sort.Ascending(tc => tc._id))
					.ToList();

				List<int> result = new List<int>();
				int currentWeek = ISOWeek.GetWeekOfYear(DateTime.Now);
				for (int i = 1; i <= currentWeek; i++)
				{
					TicketsCount? weekData = tickets.FirstOrDefault(w => w._id == i);
					result.Add(weekData?.count ?? 0);
				}

				return addCumulative(result.ToList());
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return new List<int> { 0 };
		}

		private List<int> addCumulative(List<int> list)
		{
			List<int> cumulativeList = new List<int>();
			for (int i = 0; i < list.Count; i++)
			{
				cumulativeList.Add(list.Take(i + 1).Sum());
			}
			return cumulativeList;
		}

		public List<TicketsCount> GetTicketsPastDeadlineCount()
		{
			var tickets = collection
				.Aggregate()
				.AppendStage<BsonDocument>(new BsonDocument
				{
					{ "$match", new BsonDocument
						{
							{ "Status", "open" }
						}
					}
				})
				.AppendStage<BsonDocument>(new BsonDocument
				{
					{ "$addFields", new BsonDocument
						{
							{ "createdAtDate", new BsonDocument
								{
									{ "$dateFromString", new BsonDocument { { "dateString", "$Created_at" } } }
								}
							}
						}
					}
				})
				.AppendStage<BsonDocument>(new BsonDocument
				{
					{ "$match", new BsonDocument
						{
							{ "createdAtDate", new BsonDocument { { "$lt", DateTime.UtcNow } } }
						}
					}
				})
				.AppendStage<BsonDocument>(new BsonDocument
				{
					{ "$group", new BsonDocument
						{
							{ "_id", "Tickets Past Deadline" },
							{ "count", new BsonDocument { { "$sum", 1 } } }
						}
					}
				})
				.ToList()
				.Select(g => new TicketsCount
				{
					_id = g["_id"].AsInt32,
					count = g["count"].AsInt32
				})
				.ToList();

			return tickets;
		}

		public void UpdateUserNameInTickets(ObjectId userId, string newName)
		{
			var filter = Builders<Ticket>.Filter.Or(
				Builders<Ticket>.Filter.Eq("Reported_by._id", userId),
				Builders<Ticket>.Filter.Eq("Assigned_to._id", userId),
				Builders<Ticket>.Filter.Eq("Resolved_by._id", userId)
			);

			var update = Builders<Ticket>.Update
				.Set("Reported_by.Name", newName)
				.Set("Assigned_to.Name", newName)
				.Set("Resolved_by.Name", newName);

			collection.UpdateMany(filter, update);
		}

		public void AddCommentToTicket(ObjectId ticketId, Comment comment)
		{
			var filter = Builders<Ticket>.Filter.Eq("_id", ticketId);
			var update = Builders<Ticket>.Update.Push("Comments", comment);
			collection.UpdateOne(filter, update);
		}

		public async Task<ObservableCollection<Comment>> GetCommentsByTicketId(ObjectId ticketId)
		{
			Ticket ticket = await Read(ticketId);
			return ticket.Comments ?? new ObservableCollection<Comment>();
		}

		public async Task<int> GetCommentCountByTicket(ObjectId ticketId)
		{
			var ticket = await Read(ticketId);
			return ticket?.Comments?.Count ?? 0;
		}

		public void UpdateAssigneTo(ObjectId ticketId, PartialUser employee)
		{
			var filter = Builders<Ticket>.Filter.Or(
				Builders<Ticket>.Filter.Eq("_id", ticketId)
			);

			var update = Builders<Ticket>.Update
				.Set("Assigned_to", employee);

			collection.UpdateOne(filter, update);
		}
	}
}
