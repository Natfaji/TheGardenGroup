using Model.Enums;
using Model.Models;
using Model.Models.Request;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DAL
{
	public class AuthDao : BaseDao<Employee>
	{
		public AuthDao() : base("employees")
		{
		}

		public Employee getEmployeeByEmail(string email)
		{
			return collection
				.Aggregate()
				.Match(e => e.Email == email)
				.FirstOrDefault();
		}
	}
}
