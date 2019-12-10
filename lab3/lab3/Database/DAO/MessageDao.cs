using System.Collections.Generic;
using lab3.Models;
using Npgsql;
using NpgsqlTypes;

namespace lab3.Database.DAO
{
    class MessageDao : Dao<Message>
    {
        public MessageDao(DbConnection db)
            : base(db) { }

        public override void Create(Message entity)
        {
            var connection = Dbconnection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
                "INSERT INTO public.message (chat_id, user_id, text, date) " +
                "VALUES (:chat_id, :user_id, :text, :date)";
            command.Parameters.Add(new NpgsqlParameter("chat_id", entity.Chat.Id));
            command.Parameters.Add(new NpgsqlParameter("user_id", entity.User.Id));
            command.Parameters.Add(new NpgsqlParameter("text", entity.Text));
            command.Parameters.Add(new NpgsqlParameter("date", NpgsqlDateTime.Now));
            command.ExecuteNonQuery();
            Dbconnection.Close();
        }

        public override Message Get(long id)
        {
            var connection = Dbconnection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
                "SELECT mes.id, us.id, us.login, ch.id, ch.tag, mes.text, mes.date " +
                "FROM public.message AS mes " +
                "INNER JOIN public.user AS us ON mes.user_id = us.id " +
                "INNER JOIN public.chat AS ch ON mes.chat_id = ch.id " +
                "WHERE mes.id = :id";
            command.Parameters.Add(new NpgsqlParameter("id", id));
            var reader = command.ExecuteReader();
            Message message = null;
            if (reader.Read())
                message = new Message(reader.GetInt64(0),
                    new User(reader.GetInt64(1), reader.GetString(2)),
                    new Chat(reader.GetInt64(3), reader.GetString(4)),
                    reader.GetString(5),
                    reader.GetTimeStamp(6).ToDateTime());
            Dbconnection.Close();
            return message;
        }

        public override List<Message> Get(int page)
        {
            var connection = Dbconnection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
                "SELECT mes.id, us.id, us.login, ch.id, ch.tag, mes.text, mes.date " +
                "FROM public.message AS mes " +
                "INNER JOIN public.user AS us ON mes.user_id = us.id " +
                "INNER JOIN public.chat AS ch ON mes.chat_id = ch.id " + 
                "LIMIT 10 OFFSET :offset";
            command.Parameters.Add(new NpgsqlParameter("offset", page * 10));
            var reader = command.ExecuteReader();
            var messages = new List<Message>();
            while (reader.Read())
                messages.Add(new Message(reader.GetInt64(0),
                    new User(reader.GetInt64(1), reader.GetString(2)),
                    new Chat(reader.GetInt64(3), reader.GetString(4)),
                    reader.GetString(5),
                    reader.GetTimeStamp(6).ToDateTime()));
            Dbconnection.Close();
            return messages;
        }

        public override void Update(Message entity)
        {
            var connection = Dbconnection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
                "UPDATE public.message SET text = :text WHERE id = :id";
            command.Parameters.Add(new NpgsqlParameter("id", entity.Id));
            command.Parameters.Add(new NpgsqlParameter("text", entity.Text));
            command.ExecuteNonQuery();
            Dbconnection.Close();
        }

        public override void Delete(long id)
        {
            var connection = Dbconnection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM public.message WHERE id = :id";
            command.Parameters.Add(new NpgsqlParameter("id", id));
            command.ExecuteNonQuery();
            Dbconnection.Close();
        }

        public override void Clear()
        {
            var connection = Dbconnection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "TRUNCATE TABLE public.message RESTART IDENTITY";
            command.ExecuteNonQuery();
            Dbconnection.Close();
        }
    }
}
