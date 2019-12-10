using System.Collections.Generic;
using lab3.Models;
using Npgsql;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace lab3.Database.DAO
{
    class UserChatDao : Dao<UserChat>
    {

        public UserChatDao(Context context)
            : base(context) { }

        public override void Create(UserChat entity)
        {
            context.UserChat.Add(entity);
            context.SaveChanges();
        }

        public override UserChat Get(long id)
        {
            var query = from userChat in context.UserChat
                where userChat.Id == id
                select userChat;
            return query.First();
        }

        public override List<UserChat> Get(int page)
        {
            var query = from userChat in context.UserChat
                select userChat;
            return query
                .Include(uC => uC.User)
                .Include(uC => uC.Chat)
                .Skip(page * 10).Take(10)
                .OrderBy(x => x.Id).ToList();
        }

        public override void Update(UserChat entity)
        {
            var upd = context.UserChat.Single(x => x.Id == entity.Id);
            upd.IsAdmin = entity.IsAdmin;
            context.SaveChanges();
        }

        public override void Delete(long id)
        {
            context.UserChat.Remove(context.UserChat.Single(x => x.Id == id));
            context.SaveChanges();
        }

        public List<UserChat> Search(string value, bool isAdmin)
        {
            var query = from userChat in context.UserChat
                where (userChat.Chat.Tag.Equals(value)
                       || userChat.Chat.Name.Equals(value)
                       || userChat.User.Login.Equals(value)
                       || userChat.User.Name.Equals(value))
                      && userChat.IsAdmin == isAdmin
                select userChat;
            return query.ToList();
        }
    }
}
