using Model.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.ObjectModel;

namespace Model.Models
{
	public class Ticket
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public ObjectId _id { get; set; }

		[BsonElement("title")]
		public string Title { get; set; }

		[BsonElement("description")]
		public string Description { get; set; }

		[BsonElement("status")]
		public Status? Status { get; set; }

		[BsonElement("priority")]
		public Priority Priority { get; set; }

		[BsonElement("commens")]
		public ObservableCollection<Comment> Comments { get; set; } = new ObservableCollection<Comment>();

		[BsonElement("occured_on")]
		public DateTime Occured_on { get; set; }

		[BsonElement("created_at")]
		public DateTime Created_at { get; set; }

		[BsonElement("reported_by")]
		public PartialUser Reported_by { get; set; }

		[BsonElement("assigned_to")]
		public ObservableCollection<PartialUser> Assigned_to { get; set; } = new ObservableCollection<PartialUser>();

		[BsonElement("resolved_by")]
		public PartialUser Resolved_by { get; set; }

		[BsonIgnore]
		public string AssigneesNames
		{
			get
			{
				if (Assigned_to == null || Assigned_to.Count == 0)
					return "None";
				return string.Join(", ", Assigned_to.Select(u => u.Name));
			}
		}
	}
}
