using System.IO;
using lab3.Controllers;
using lab3.Database;

namespace lab3
{
    internal class Program
    {
        static void Main()
        {
            var context = new Models.Context();
            var controller = new Controller(context);
            controller.Begin();
        }
    }
}
