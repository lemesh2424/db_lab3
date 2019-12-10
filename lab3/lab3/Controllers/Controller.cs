using lab3.Database;
using lab3.Database.DAO;
using lab3.Models;
using lab3.Views;
using lab3.Views.CrudViews;

namespace lab3.Controllers
{
    public class Controller
    {
        private readonly Dao<User> _userDao;
        private readonly Dao<Chat> _chatDao;
        private readonly Dao<UserChat> _userChatDao;
        private readonly Dao<Message> _messageDao;
        private readonly FullTextSearch _fullTextSearch;

        public Controller(DbConnection dbConnection)
        {
            _userDao = new UserDao(dbConnection);
            _chatDao = new ChatDao(dbConnection);
            _userChatDao = new UserChatDao(dbConnection);
            _messageDao = new MessageDao(dbConnection);
            _fullTextSearch = new FullTextSearch(dbConnection);
        }

        public void Begin()
        {
            while (true)
            {
                var menuCom = MenuView.ShowMain();
                if (menuCom == MenuCommands.Exit)
                    break;
                if (menuCom == MenuCommands.Crud)
                    BeginCrud();
                if (menuCom == MenuCommands.Random)
                    ExecuteRandomise();
                if (menuCom == MenuCommands.FullTextSearch)
                    ExecuteFullTestSearch();
            }
        }

        private void BeginCrud()
        {
            while (true)
            {
                var entity = MenuView.ShowEntities();
                if (entity == Entities.None)
                    break;
                if (entity == Entities.User)
                    ExecuteCrud(new UserView(), _userDao);
                if (entity == Entities.Chat)
                    ExecuteCrud(new ChatView(), _chatDao);
                if (entity == Entities.Message)
                    ExecuteCrud(new MessageView(_userDao, _chatDao), _messageDao);
                if (entity == Entities.UserChat)
                    ExecuteCrud(new UserChatView(_userDao, _chatDao),  _userChatDao);
            }
        }

        private void ExecuteCrud<T>(CrudView<T> view, Dao<T> dao)
        {
            var page = 0;
            while (true)
            {
                var com = view.Begin(dao.Get(page), page);
                if (com == CrudOperations.None)
                    break;
                if (com == CrudOperations.PageLeft && page > 0)
                    page--;
                if (com == CrudOperations.PageRight)
                    page++;
                try
                {
                    if (com == CrudOperations.Create)
                        dao.Create(view.Create());
                    if (com == CrudOperations.Read)
                        view.ShowReadResult(dao.Get(view.Read()));
                    if (com == CrudOperations.Update)
                        dao.Update(view.Update(dao.Get(view.Read())));
                    if (com == CrudOperations.Delete)
                        dao.Delete(view.Read());
                    if (com == CrudOperations.Search)
                        ExecuteSearch(view, dao);
                    if (com == CrudOperations.Create || com == CrudOperations.Delete
                                                     || com == CrudOperations.Update)
                        view.OperationStatusOutput(true);
                }
                catch 
                {
                    view.OperationStatusOutput(false);
                }
            }
        }

        private void ExecuteSearch<T>(CrudView<T> view, Dao<T> dao)
        {
            var type = typeof(T);
            if (type == typeof(Message))
                return;
            if (type == typeof(User))
            {
                var v = view as UserView;
                var d = dao as UserDao;
                v.ShowReadResult(d.Search(v.Search()));
            }

            if (type == typeof(Chat))
            {
                var v = view as ChatView;
                var d = dao as ChatDao;
                v.ShowReadResult(d.Search(v.Search()));
            }

            if (type == typeof(UserChat))
            {
                var v = view as UserChatView;
                var d = dao as UserChatDao;
                var query = v.Search();
                v.ShowReadResult(d.Search(query.value, query.isAdmin));
            }
        }

        private void ExecuteRandomise()
        {
            var number = RandomiseView.ShowRandomise();
            if (number == -1)
                return;
            var randomiser = new Randomiser(_userDao, _chatDao,
                _userChatDao, _messageDao);
            randomiser.Randomise(number);
        }

        private void ExecuteFullTestSearch()
        {
            var command = FullTextSearchView.Begin();
            FullTextSearchView.ShowResults(command.Item1 == FullTextSearchCommands.FullPhrase
                ? _fullTextSearch.GetFullPhrase("text", "message", command.Item2)
                : _fullTextSearch.GetAllWithIncludedWord("text", "message", command.Item2));
        }
    }
}