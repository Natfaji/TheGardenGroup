using Model.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Model.Models
{
	public class PartialUser
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public ObjectId _id { get; set; }

		[BsonElement("name")]
		public string Name { get; set; }

		[BsonElement("email")]
		public string Email { get; set; }

		[BsonElement("phone_number")]
		public string Phone_number { get; set; }

		[BsonElement("role")]
		public Role Role { get; set; }

		public PartialUser ToPartialUser()
		{
			return new PartialUser
			{
				_id = this._id,
				Name = this.Name,
				Email = this.Email,
				Phone_number = this.Phone_number,
				Role = this.Role
			};
		}
	}
}
