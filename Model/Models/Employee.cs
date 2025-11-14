using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Model.Models
{
	public class Employee : PartialUser
	{
		[BsonElement("password_hashed")]
		public string Password_hashed { get; set; }

		[BsonElement("password_salt")]
		public string Password_salt { get; set; }

		[BsonElement("password_reset_hashed")]
		public string Password_reset_hashed { get; set; }

		[BsonElement("password_reset_salt")]
		public string Password_reset_salt { get; set; }
	}
}
