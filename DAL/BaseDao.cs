using Model.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Model.Models.Request;
using System.Threading.Tasks;

namespace DAL
{
	public class BaseDao<T> : DAO
	{
		public IMongoCollection<T> collection;

		public BaseDao(string collectionName)
		{
			collection = Db.GetCollection<T>(collectionName);
		}

		public virtual void Create(T entity)
		{
			collection.InsertOneAsync(entity);
		}

		public virtual async Task<T> Read(ObjectId id)
		{
			var filter = Builders<T>.Filter.Eq("_id", id);
			return await collection.Find(filter).FirstOrDefaultAsync();
		}

		public virtual async Task<List<T>> ReadAll(RequestParameters requestParameters, FilterDefinition<T>? filter = null)
		{
			filter ??= Builders<T>.Filter.Empty;

			return await collection
				.Find(filter)
				.Skip((requestParameters.PageNumber - 1) * requestParameters.ItemsPerPage)
				.Limit(requestParameters.ItemsPerPage)
				.ToListAsync();
		}

		public virtual async Task<T> Update(ObjectId id, T entity)
		{
			var filter = Builders<T>.Filter.Eq("_id", id);
			return await collection.FindOneAndReplaceAsync(filter, entity, new FindOneAndReplaceOptions<T>
			{
				ReturnDocument = ReturnDocument.After
			});
		}

		public virtual async Task Delete(ObjectId id)
		{
			var filter = Builders<T>.Filter.Eq("_id", id);
			await collection.DeleteOneAsync(filter);
		}
	}
}
