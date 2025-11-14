using DAL;
using Model.Enums;
using Model.Models;
using Model.Models.Request;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Net.Mail;
using System.Threading.Tasks;
using Tools;

namespace Logic
{
	public class UserLogic
	{
		UserDao userDao = new UserDao();

		public UserLogic()
		{
		}

		public void Create(Employee entity)
		{
			userDao.Create(entity);
		}

		public async Task<PartialUser> Read(ObjectId id)
		{
			return await userDao.Read(id);
		}

		public async Task<List<PartialUser>> ReadAll(RequestParameters requestParameters, FilterDefinition<PartialUser>? filter = null)
		{
			return await userDao.ReadAll(requestParameters, filter);
		}

		public async Task Update(ObjectId id, PartialUser entity)
		{
			await userDao.Update(id, entity);
		}

		public async Task Delete(ObjectId id)
		{
			await userDao.Delete(id);
		}

		public void CreateUser(string name, string email, string phoneNumber, string password, Role role)
		{
			string salt = PasswordTools.GenerateSalt();
			string hashedPassword = PasswordTools.HashPassword(salt, password);

			Employee employee = new Employee
			{
				Name = name,
				Email = email,
				Phone_number = phoneNumber,
				Role = role,
				Password_hashed = hashedPassword,
				Password_salt = salt
			};

			userDao.Create(employee);
		}
	}
}
