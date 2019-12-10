using System.Collections.Generic;
using lab3.Models;
using Npgsql;
using NpgsqlTypes;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace lab3.Database.DAO
{
    class MessageDao : Dao<Message>
    {
        public MessageDao(Context context)
            : base(context) { }

        public override void Create(Message entity)
        {
            context.Message.Add(entity);
            context.SaveChanges();
        }

        public override Message Get(long id)
        {
            var query = from message in context.Message
                where message.Id == id
                select message;
            return query.First();
        }

        public override List<Message> Get(int page)
        {
            var query = from message in context.Message
                select message;
            return query
                .Include(m => m.User)
                .Include(m => m.Chat)
                .Skip(page * 10).Take(10)
                .OrderBy(x => x.Id).ToList();
        }

        public override void Update(Message entity)
        {
            var upd = context.Message.Single(x => x.Id == entity.Id);
            upd.Text = entity.Text;
            upd.Time = entity.Time;
            context.SaveChanges();
        }

        public override void Delete(long id)
        {
            context.Message.Remove(context.Message.Single(x => x.Id == id));
            context.SaveChanges();
        }
    }
}
