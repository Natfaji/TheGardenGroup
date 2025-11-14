using DAL;
using Model.Enums;
using Model.Models;
using Model.Models.Request;
using MongoDB.Bson;
using System.Net.Mail;
using System.Threading.Tasks;
using Tools;

namespace Logic
{
	public class AuthLogic
	{
		AuthDao AuthDao = new AuthDao();

		public AuthLogic()
		{
		}

		public void Create(Employee entity)
		{
			AuthDao.Create(entity);
		}

		public async Task<PartialUser> Read(ObjectId id)
		{
			return await AuthDao.Read(id);
		}

		public async Task Update(ObjectId id, Employee entity)
		{
			await AuthDao.Update(id, entity);
		}

		public async Task Delete(ObjectId id)
		{
			await AuthDao.Delete(id);
		}

		public PartialUser VerifyLogin(string email, string password)
		{
			//Seeder.Seed();
			Employee emp = AuthDao.getEmployeeByEmail(email);
			if (emp == null) return null;

			string hashedLoginPassword = PasswordTools.HashPassword(emp.Password_salt, password);
			if (hashedLoginPassword != emp.Password_hashed) return null;

			return new PartialUser
			{
				_id = emp._id,
				Name = emp.Name,
				Email = emp.Email,
				Phone_number = emp.Phone_number,
				Role = emp.Role
			};
		}

		//methode om user te creeeren
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

			AuthDao.Create(employee);
		}
	}
}
