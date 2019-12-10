using System;
using System.Collections.Generic;
using System.Data;
using ConsoleTableExt;
using lab3.Models;

namespace lab3.Views.CrudViews
{
    class UserView : CrudView<User>
    {
        public UserView() : base("Users") { }

        public override User Create()
        {
            Console.WriteLine("\n\rInput login");
            var login = Console.ReadLine();
            Console.WriteLine("Input name");
            var name = Console.ReadLine();
            return new User(0, login, name);
        }

        public override void ShowReadResult(User data)
        {
            Console.WriteLine("Result:");
            ConsoleTableBuilder.From(DataToDataTable(new List<User>{data}))
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .ExportAndWriteLine();
            Console.ReadKey();
        }

        public override User Update(User entity)
        {
            Console.WriteLine($"Input login. Old one: {entity.Login}");
            entity.Login = Console.ReadLine();
            Console.WriteLine($"Input name. Old one: {entity.Name}");
            entity.Name = Console.ReadLine();
            Console.WriteLine($"Input bio. Old one: {entity.Bio ?? "none"}");
            entity.Bio = Console.ReadLine();
            return entity;
        }

        public string Search()
        {
            Console.WriteLine("\r\nInput login");
            return Console.ReadLine();
        }

        protected override DataTable DataToDataTable(List<User> data)
        {
            var dataTable = new DataTable("Users");
            dataTable.Columns.Add(new DataColumn("Id", typeof(long)));
            dataTable.Columns.Add(new DataColumn("Login", typeof(string)));
            dataTable.Columns.Add(new DataColumn("Name", typeof(string)));
            dataTable.Columns.Add(new DataColumn("Bio", typeof(string)));
            if (data.Count == 0)
            {
                var row = dataTable.NewRow();
                row.ItemArray = new object[]{-1, "Empty", "Empty", "Empty"};
                dataTable.Rows.Add(row);
            }
            else
                foreach (var el in data)
                {
                    var row = dataTable.NewRow();
                    row.ItemArray = new object[]
                    {
                        el.Id,
                        el.Login,
                        el.Name ,
                        el.Bio
                    };
                    dataTable.Rows.Add(row);
                }

            return dataTable;
        }
    }
}
