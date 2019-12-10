using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lab3.Models
{
    [Table("user_chat", Schema = "public")]
    class UserChat
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("user_id")]
        public long UserId { get; set; }

        [Column("chat_id")]
        public long ChatId { get; set; }

        [Column("isadmin")]
        public bool IsAdmin { get; set; }

        public UserChat(long id, long userId, long chatId, bool isAdmin)
        {
            Id = id;
            UserId = userId;
            ChatId = chatId;
            IsAdmin = isAdmin;
        }
    }
}
