using DAL;
using Model.Models;
using MongoDB.Bson;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Logic
{
	public class CommentLogic
	{
		private readonly TicketDao ticketDao = new TicketDao();

		public void AddComment(ObjectId ticketId, Comment comment)
		{
			comment.Author = comment.Author.ToPartialUser();
			ticketDao.AddCommentToTicket(ticketId, comment);
		}

		public async Task<ObservableCollection<Comment>> GetCommentsForTicket(ObjectId ticketId)
		{
			return await ticketDao.GetCommentsByTicketId(ticketId);
		}
	}
}