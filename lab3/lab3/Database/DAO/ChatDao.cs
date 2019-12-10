using System.Collections.Generic;
using lab3.Models;
using Npgsql;

namespace lab3.Database.DAO
{
    class ChatDao : Dao<Chat>
    {
        public ChatDao(DbConnection db) : base(db) { }

        public override void Create(Chat entity)
        {
            var connection = Dbconnection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
                "INSERT INTO public.chat (tag, name) VALUES (:tag, :name)";
            command.Parameters.Add(new NpgsqlParameter("tag", entity.Tag));
            command.Parameters.Add(new NpgsqlParameter("name", entity.Name));
            command.ExecuteNonQuery();
            Dbconnection.Close();
        }

        public override Chat Get(long id)
        {
            var connection = Dbconnection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
                "SELECT * FROM public.chat WHERE id = :id";
            command.Parameters.Add(new NpgsqlParameter("id", id));
            var reader = command.ExecuteReader();
            Chat chat = null;
            if (reader.Read())
                chat = new Chat(reader.GetInt64(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.IsDBNull(3)
                        ? null
                        : reader.GetString(3));
            Dbconnection.Close();
            return chat;
        }

        public override List<Chat> Get(int page)
        {
            var connection = Dbconnection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
                "SELECT * FROM public.chat LIMIT 10 OFFSET :offset";
            command.Parameters.Add(new NpgsqlParameter("offset", page * 10));
            var reader = command.ExecuteReader();
            var chats = new List<Chat>();
            while (reader.Read())
                chats.Add( new Chat(reader.GetInt64(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.IsDBNull(3)
                        ? null
                        : reader.GetString(3)));
            Dbconnection.Close();
            return chats;
        }

        public override void Update(Chat entity)
        {
            var connection = Dbconnection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
                "UPDATE public.chat SET tag = :tag, name = :name, bio = :bio WHERE id = :id";
            command.Parameters.Add(new NpgsqlParameter("id", entity.Id));
            command.Parameters.Add(new NpgsqlParameter("tag", entity.Tag));
            command.Parameters.Add(new NpgsqlParameter("name", entity.Name));
            command.Parameters.Add(new NpgsqlParameter("bio", entity.Bio));
            command.ExecuteNonQuery();
            Dbconnection.Close();
        }

        public override void Delete(long id)
        {
            var connection = Dbconnection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM public.chat WHERE id = :id";
            command.Parameters.Add(new NpgsqlParameter("id", id));
            command.ExecuteNonQuery();
            Dbconnection.Close();
        }

        public override void Clear()
        {
            var connection = Dbconnection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "TRUNCATE TABLE public.chat RESTART IDENTITY CASCADE";
            command.ExecuteNonQuery();
            Dbconnection.Close();
        }

        public Chat Search(string str)
        {
            var connection = Dbconnection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
                "SELECT * FROM public.chat WHERE tag = :tag";
            command.Parameters.Add(new NpgsqlParameter("tag", str));
            var reader = command.ExecuteReader();
            Chat chat = null;
            if (reader.Read())
                chat = new Chat(reader.GetInt64(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.IsDBNull(3)
                        ? null
                        : reader.GetString(3));
            Dbconnection.Close();
            return chat;
        }
    }
}
