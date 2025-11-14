using Bogus;
using DAL;
using Model.Enums;
using Model.Models;
using System.Collections.ObjectModel;

namespace Tools
{
	public class Seeder
	{
		private static readonly UserDao userDao = new UserDao();
		private static readonly TicketDao ticketDao = new TicketDao();

		private static List<Employee> fakeEmployees;
		private static List<Ticket> fakeTickets;

		public static void Seed()
		{
			SeedEmployees(100);
			SeedTickets(200);
		}

		private static void SeedEmployees(int count)
		{
			var faker = new Faker();
			var employeeFaker = new Faker<Employee>()
				.RuleFor(e => e._id, f => MongoDB.Bson.ObjectId.GenerateNewId())
				.RuleFor(e => e.Name, f => f.Name.FullName())
				.RuleFor(e => e.Email, f => f.Internet.Email())
				.RuleFor(e => e.Phone_number, f => f.Phone.PhoneNumber())
				.RuleFor(e => e.Role, f => f.PickRandom<Role>())
				.RuleFor(e => e.Password_salt, f => PasswordTools.GenerateSalt())
				.RuleFor(e => e.Password_hashed, (f, e) => PasswordTools.HashPassword(e.Password_salt, f.Internet.Password()));

			fakeEmployees = employeeFaker.Generate(count);

			string Password_salt = PasswordTools.GenerateSalt();

			fakeEmployees.Add(new Employee
			{
				_id = MongoDB.Bson.ObjectId.GenerateNewId(),
				Name = "Admin User",
				Email = "admin@example.com",
				Phone_number = "123-456-7890",
				Role = Role.Admin,
				Password_salt = Password_salt,
				Password_hashed = PasswordTools.HashPassword(Password_salt, "AdminPassword123")
			});

			fakeEmployees.Add(new Employee
			{
				_id = MongoDB.Bson.ObjectId.GenerateNewId(),
				Name = "Service Desk User",
				Email = "servicedesk@example.com",
				Phone_number = "123-456-7890",
				Role = Role.ServiceDesk,
				Password_salt = Password_salt,
				Password_hashed = PasswordTools.HashPassword(Password_salt, "ServiceDeskPassword123")
			});

			fakeEmployees.Add(new Employee
			{
				_id = MongoDB.Bson.ObjectId.GenerateNewId(),
				Name = "Employee User",
				Email = "employee@example.com",
				Phone_number = "123-456-7890",
				Role = Role.Employee,
				Password_salt = Password_salt,
				Password_hashed = PasswordTools.HashPassword(Password_salt, "EmployeePassword123")
			});

			userDao.collection.InsertMany(fakeEmployees);
		}

		private static void SeedTickets(int count)
		{
			var faker = new Faker();
			var ticketFaker = new Faker<Ticket>()
				.RuleFor(t => t._id, f => MongoDB.Bson.ObjectId.GenerateNewId())
				.RuleFor(t => t.Title, f => f.Lorem.Sentence())
				.RuleFor(t => t.Description, f => f.Lorem.Paragraph())
				.RuleFor(t => t.Status, f => f.PickRandom<Status>())
				.RuleFor(t => t.Priority, f => f.PickRandom<Priority>())
				.RuleFor(t => t.Created_at, f => f.Date.Past())
				.RuleFor(t => t.Reported_by, f => f.PickRandom(fakeEmployees).ToPartialUser())
				.RuleFor(t => t.Assigned_to, f => new ObservableCollection<PartialUser>(
					f.PickRandom(fakeEmployees, f.Random.Int(1, 3))
					.Select(e => e.ToPartialUser())
					.ToList())
				)
				.RuleFor(t => t.Comments, f => new ObservableCollection<Comment>(
					GenerateComments(5, 30)
					.ToList())
				);

			fakeTickets = ticketFaker.Generate(count);
			ticketDao.collection.InsertMany(fakeTickets);
		}

		private static List<Comment> GenerateComments(int countMin, int countMax)
		{
			Faker faker = new Faker();
			var commentFaker = new Faker<Comment>()
				.RuleFor(c => c._id, f => MongoDB.Bson.ObjectId.GenerateNewId())
				.RuleFor(c => c.Author, f => f.PickRandom(fakeEmployees).ToPartialUser())
				.RuleFor(c => c.Content, f => f.Lorem.Sentences(3))
				.RuleFor(c => c.Created_at, f => f.Date.Recent(30));

			List<Comment> fakeComments = commentFaker.Generate(GenericTools.GenerateNumber(countMin, countMax));
			return fakeComments;
		}
	}
}
