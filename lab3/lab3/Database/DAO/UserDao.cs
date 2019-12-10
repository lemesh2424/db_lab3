using System.Collections.Generic;
using System.Data;
using lab3.Models;
using Npgsql;
using System.Linq;
using System.Linq.Expressions;

namespace lab3.Database.DAO
{
    internal class UserDao : Dao<User>
    {
        public UserDao(Context db) : base(db) { }

        public override void Create(User entity)
        {
            context.User.Add(entity);
            context.SaveChanges();
        }

        public override User Get(long id)
        {
            var query = from user in context.User
                where user.Id == id
                select user;
            return query.First();
        }

        public override List<User> Get(int page)
        {
            var query = from user in context.User
                select user;
            return query.Skip(page * 10).Take(10).ToList();
        }

        public override void Update(User entity)
        {
            var upd = context.User.Single(x => x.Id == entity.Id);
            upd.Login = entity.Login;
            upd.Name = entity.Name;
            upd.Bio = entity.Bio;
            context.SaveChanges();
        }

        public override void Delete(long id)
        {
            context.User.Remove(context.User.Single(x => x.Id == id));
            context.SaveChanges();
        }

        public User Search(string str)
        {
            var query = from user in context.User
                where user.Login == str
                select user;
            return query.First();
        }
    }
}