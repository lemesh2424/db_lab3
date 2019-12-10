using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lab3.Models
{
    [Table("user_chat", Schema = "public")]
    class UserChat
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [ForeignKey("user_id")]
        public User User { get; set; }

        [ForeignKey("chat_id")]
        public Chat Chat { get; set; }

        [Column("isadmin")]
        public bool IsAdmin { get; set; }

        public UserChat() { }
        public UserChat(long id, User user, Chat chat, bool isAdmin)
        {
            Id = id;
            User = user;
            Chat = chat;
            IsAdmin = isAdmin;
        }
    }
}
