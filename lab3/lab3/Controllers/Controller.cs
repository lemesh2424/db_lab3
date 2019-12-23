using System;
using lab3.Database;
using lab3.Database.DAO;
using lab3.Models;
using lab3.Views;
using lab3.Views.CrudViews;

namespace lab3.Controllers
{
    class Controller
    {
        private readonly Dao<User> _userDao;
        private readonly Dao<Chat> _chatDao;
        private readonly Dao<UserChat> _userChatDao;
        private readonly Dao<Message> _messageDao;

        public Controller(Context context)
        {
            _userDao = new UserDao(context);
            _chatDao = new ChatDao(context);
            _userChatDao = new UserChatDao(context);
            _messageDao = new MessageDao(context);
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
                v.ShowSearchResult(d.Search(query.value, query.isAdmin));
            }
        }
    }
}