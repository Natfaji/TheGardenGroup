using Model.Enums;
using Model.Models;
using Model.Models.Request;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DAL
{
	public class UserDao : BaseDao<PartialUser>
	{
		public UserDao() : base("employees")
		{
		}

		public override async Task<List<PartialUser>> ReadAll(RequestParameters requestParameters, FilterDefinition<PartialUser>? filter = null)
		{
			filter ??= Builders<PartialUser>.Filter.Empty;

			List<PartialUser> users = await collection
				.Find(filter)
				.Skip((requestParameters.PageNumber - 1) * requestParameters.ItemsPerPage)
				.Limit(requestParameters.ItemsPerPage)
				.Project(employee => new PartialUser
				{
					_id = employee._id,
					Name = employee.Name,
					Email = employee.Email,
					Phone_number = employee.Phone_number,
					Role = employee.Role
				})
				.ToListAsync();

			return users;
		}
	}
}
