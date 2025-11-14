using MongoDB.Driver;
using System.Configuration;

namespace DAL
{
	public class DAO
	{
		protected MongoClient client;

		private IMongoDatabase db;
		protected IMongoDatabase Db { get { return db; } }

		public DAO()
		{
			string dbName = "IMS";
			string connectionString = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
			client = new MongoClient(connectionString);
			db = client.GetDatabase(dbName);
		}
	}
}