using System;
using System.Collections.Generic;
using System.Linq;
using lab3.Database.DAO;
using lab3.Models;

namespace lab3.Database
{
    class Randomiser
    {
        private readonly Dao<User> _userDao;
        private readonly Dao<Chat> _chatDao;
        private readonly Dao<UserChat> _userChatDao;
        private readonly Dao<Message> _messageDao;
        private readonly Random _random = new Random();

        public Randomiser(Dao<User> userDao, Dao<Chat> chatDao,
            Dao<UserChat> userChatDao, Dao<Message> messageDao)
        {
            _userDao = userDao;
            _chatDao = chatDao;
            _userChatDao = userChatDao;
            _messageDao = messageDao;
        }

        public void Randomise(int number)
        {
            _messageDao.Clear();
            _userChatDao.Clear();
            _userDao.Clear();
            _chatDao.Clear();
            FeelDB(_userDao, GenerateUsers(number));
            FeelDB(_chatDao, GenerateChats(number));
            FeelDB(_userChatDao, GenerateUserChats(number));
            FeelDB(_messageDao, GenerateMessages(number));
        }

        private void FeelDB<T>(Dao<T> dao, IEnumerable<T> entities)
        {
            foreach (var entity in entities)
                dao.Create(entity);
        }

        private IEnumerable<User> GenerateUsers(int number)
        {
            var users = new Dictionary<string, User>();
            while (users.Count != number)
            {
                var user = new User(0, 
                    RandomAlphaNumericString(16), 
                    RandomAlphaNumericString(32));
                users.TryAdd(user.Login, user);
            }
            return users.Values;
        }

        private IEnumerable<Chat> GenerateChats(int number)
        {
            var chats = new Dictionary<string, Chat>();
            while (chats.Count != number)
            {
                var chat = new Chat(0,
                    RandomAlphaNumericString(16),
                    RandomAlphaNumericString(32));
                chats.TryAdd(chat.Tag, chat);
            }
            return chats.Values;
        }

        private IEnumerable<UserChat> GenerateUserChats(int number)
        {
            var userChats = new Dictionary<(long, long), UserChat>();
            while (userChats.Count != number * 3)
            {
                var userChat = new UserChat(0,
                    new User(_random.Next(1, number)),
                    new Chat(_random.Next(1, number)),
                    _random.Next() > (Int32.MaxValue / 2));
                userChats.TryAdd((userChat.User.Id, userChat.Chat.Id), userChat);
            }
            return userChats.Values;
        }

        private IEnumerable<Message> GenerateMessages(int number)
        {
            var messages = new List<Message>();
            while (messages.Count != number * 5)
                messages.Add(new Message(0,
                    new User(_random.Next(1, number)),
                    new Chat(_random.Next(1, number)),
                    RandomAlphaNumericString(64),
                    DateTime.MinValue));
            return messages;
        }

        private string RandomAlphaNumericString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
        }
    }
}
