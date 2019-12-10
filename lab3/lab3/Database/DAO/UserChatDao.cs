using System.Collections.Generic;
using lab3.Models;
using Npgsql;

namespace lab3.Database.DAO
{
    class UserChatDao : Dao<UserChat>
    {

        public UserChatDao(DbConnection db)
            : base(db) { }

        public override void Create(UserChat entity)
        {
            var connection = Dbconnection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
                "INSERT INTO public.user_chat (user_id, chat_id, isAdmin) " +
                "VALUES (:user_id, :chat_id, :isAdmin)";
            command.Parameters.Add(new NpgsqlParameter("user_id", entity.User.Id));
            command.Parameters.Add(new NpgsqlParameter("chat_id", entity.Chat.Id));
            command.Parameters.Add(new NpgsqlParameter("isAdmin", entity.IsAdmin));
            command.ExecuteNonQuery();
            Dbconnection.Close();
        }

        public override UserChat Get(long id)
        {
            var connection = Dbconnection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
                "SELECT rel.id, us.id, us.login, ch.id, ch.tag, rel.isadmin " +
                "FROM public.user_chat AS rel " +
                "INNER JOIN public.user AS us ON rel.user_id = us.id " +
                "INNER JOIN public.chat AS ch ON rel.chat_id = ch.id " + 
                "WHERE rel.id = :id";
            command.Parameters.Add(new NpgsqlParameter("id", id));
            var reader = command.ExecuteReader();
            UserChat userChat = null;
            if (reader.Read())
                userChat = new UserChat(reader.GetInt64(0),
                    new User(reader.GetInt64(1), reader.GetString(2)),
                    new Chat(reader.GetInt64(3), reader.GetString(4)),
                    reader.GetBoolean(5));
            Dbconnection.Close();
            return userChat;
        }

        public override List<UserChat> Get(int page)
        {
            var connection = Dbconnection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
                "SELECT rel.id, us.id, us.login, ch.id, ch.tag, rel.isadmin " +
                "FROM public.user_chat AS rel " +
                "INNER JOIN public.user AS us ON rel.user_id = us.id " +
                "INNER JOIN public.chat AS ch ON rel.chat_id = ch.id " +
                "LIMIT 10 OFFSET :offset";
            command.Parameters.Add(new NpgsqlParameter("offset", page * 10));
            var reader = command.ExecuteReader();
            var userChats = new List<UserChat>();
            while (reader.Read())
                userChats.Add(new UserChat(reader.GetInt64(0),
                    new User(reader.GetInt64(1), reader.GetString(2)),
                    new Chat(reader.GetInt64(3), reader.GetString(4)),
                    reader.GetBoolean(5)));
            Dbconnection.Close();
            return userChats;
        }

        public override void Update(UserChat entity)
        {
            var connection = Dbconnection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
                "UPDATE public.user_chat SET isAdmin = :isAdmin WHERE id = :id";
            command.Parameters.Add(new NpgsqlParameter("id", entity.Id));
            command.Parameters.Add(new NpgsqlParameter("isAdmin", entity.IsAdmin));
            command.ExecuteNonQuery();
            Dbconnection.Close();
        }

        public override void Delete(long id)
        {
            var connection = Dbconnection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM public.user_chat WHERE id = :id";
            command.Parameters.Add(new NpgsqlParameter("id", id));
            command.ExecuteNonQuery();
            Dbconnection.Close();
        }

        public override void Clear()
        {
            var connection = Dbconnection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "TRUNCATE TABLE public.user_chat RESTART IDENTITY";
            command.ExecuteNonQuery();
            Dbconnection.Close();
        }

        public UserChat Search(string value, bool isAdmin)
        {
            var connection = Dbconnection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
                "SELECT rel.id, us.id, us.login, ch.id, ch.tag, rel.isadmin " +
                "FROM public.user_chat AS rel " +
                "INNER JOIN public.user AS us ON rel.user_id = us.id " +
                "INNER JOIN public.chat AS ch ON rel.chat_id = ch.id " +
                "WHERE (us.login = :value OR us.name = :value OR ch.tag = :value OR ch.name = :value) " +
                "AND rel.isadmin = :isadmin";
            command.Parameters.Add(new NpgsqlParameter("value", value));
            command.Parameters.Add(new NpgsqlParameter("isAdmin", isAdmin));
            var reader = command.ExecuteReader();
            UserChat userChat = null;
            if (reader.Read())
                userChat = new UserChat(reader.GetInt64(0),
                    new User(reader.GetInt64(1), reader.GetString(2)),
                    new Chat(reader.GetInt64(3), reader.GetString(4)),
                    reader.GetBoolean(5));
            Dbconnection.Close();
            return userChat;
        }
    }
}
