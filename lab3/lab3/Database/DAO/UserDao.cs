using System.Collections.Generic;
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
            return query.Take(10).Skip(page * 10).ToList();
        }

        public override void Update(User entity)
        {
            
        }

        public override void Delete(long id)
        {
            var connection = Dbconnection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM public.user WHERE id = :id";
            command.Parameters.Add(new NpgsqlParameter("id", id));
            command.ExecuteNonQuery();
            Dbconnection.Close();
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