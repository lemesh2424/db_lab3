using System;
using System.Collections.Generic;
using System.Data;
using ConsoleTableExt;
using lab3.Database.DAO;
using lab3.Models;

namespace lab3.Views.CrudViews
{
    class MessageView : CrudView<Message>
    {
        private readonly Dao<User> _userDao;
        private readonly Dao<Chat> _chatDao;

        public MessageView(Dao<User> userDao, Dao<Chat> chatDao) 
            : base("Messages")
        {
            _userDao = userDao;
            _chatDao = chatDao;
        }

        public override Message Create()
        {
            Console.WriteLine("\n\rInput user id:");
            var user = GetUser();
            Console.WriteLine("Input chat id:");
            var chat = GetChat();
            Console.WriteLine("Input text:");
            var text = Console.ReadLine();
            return new Message(0, user, chat, text, DateTime.Now);
        }

        public override void ShowReadResult(Message data)
        {
            Console.WriteLine("Result:");
            ConsoleTableBuilder.From(DataToDataTable(new List<Message> { data }))
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .ExportAndWriteLine();
            Console.ReadKey();
        }

        public override Message Update(Message entity)
        {
            Console.WriteLine($"\n\rInput text. Old value: {entity.Text}");
            entity.Text = Console.ReadLine();
            return entity;
        }

        protected override DataTable DataToDataTable(List<Message> data)
        {
            var dataTable = new DataTable("Messages");
            dataTable.Columns.Add(new DataColumn("Id", typeof(long)));
            dataTable.Columns.Add(new DataColumn("User name", typeof(string)));
            dataTable.Columns.Add(new DataColumn("Chat Name", typeof(string)));
            dataTable.Columns.Add(new DataColumn("Text", typeof(string)));
            dataTable.Columns.Add(new DataColumn("Time", typeof(DateTime)));
            if (data.Count == 0)
            {
                var row = dataTable.NewRow();
                row.ItemArray = new object[] { -1, "Empty", "Empty", "Empty", DateTime.MinValue };
                dataTable.Rows.Add(row);
            }
            else
                foreach (var el in data)
                {
                    var row = dataTable.NewRow();
                    row.ItemArray = new object[]
                    {
                        el.Id,
                        el.User.Login,
                        el.Chat.Name,
                        el.Text,
                        el.Time
                    };
                    dataTable.Rows.Add(row);
                }

            return dataTable;
        }
        private User GetUser()
        {
            while (true)
            {
                var user = _userDao.Get(GetNum());
                if (user is null)
                    Console.WriteLine("No such user!");
                else
                    return user;
            }
        }
        private Chat GetChat()
        {
            while (true)
            {
                var chat = _chatDao.Get(GetNum());
                if (chat is null)
                    Console.WriteLine("No such chat!");
                else
                    return chat;
            }
        }

        private static long GetNum()
        {
            long number;
            while (!long.TryParse(Console.ReadLine(), out number)
                   || number <= 0)
                Console.WriteLine("Wrong input!");
            return number;
        }
    }
}
