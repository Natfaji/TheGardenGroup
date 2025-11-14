using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Model.Models
{
	public class Comment
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public ObjectId _id { get; set; }

		[BsonElement("author")]
		public PartialUser Author { get; set; }

		[BsonElement("content")]
		public string Content { get; set; }

		[BsonElement("created_at")]
		public DateTime Created_at { get; set; } = DateTime.UtcNow;
	}
}
