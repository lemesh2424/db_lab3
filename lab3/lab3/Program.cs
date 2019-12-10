using System.IO;
using lab3.Controllers;
using lab3.Database;

namespace lab3
{
    internal class Program
    {
        static void Main()
        {
            using var sr = new StreamReader("../../../DataBaseSettings.txt");
            var line = sr.ReadToEnd();
            var connection = new DbConnection(line);
            var controller = new Controller(connection);
            controller.Begin();
        }
    }
}
