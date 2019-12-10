using System.Collections.Generic;
using System.ComponentModel.Design;
using lab3.Models;
using Npgsql;
using System.Linq;

namespace lab3.Database.DAO
{
    class ChatDao : Dao<Chat>
    {
        public ChatDao(Context context) : base(context) { }

        public override void Create(Chat entity)
        {
            context.Chat.Add(entity);
            context.SaveChanges();
        }

        public override Chat Get(long id)
        {
            var query = from chat in context.Chat
                where chat.Id == id
                select chat;
            return query.First();
        }

        public override List<Chat> Get(int page)
        {
            var query = from chat in context.Chat
                select chat;
            return query.Skip(page * 10).Take(10).ToList();
        }

        public override void Update(Chat entity)
        {
            var upd = context.Chat.Single(x => x.Id == entity.Id);
            upd.Tag = entity.Tag;
            upd.Name = entity.Name;
            upd.Bio = entity.Bio;
            context.SaveChanges();
        }

        public override void Delete(long id)
        {
            context.Chat.Remove(context.Chat.Single(x => x.Id == id));
            context.SaveChanges();
        }

        public Chat Search(string str)
        {
            var query = from chat in context.Chat
                where chat.Tag == str
                select chat;
            return query.First();
        }
    }
}
