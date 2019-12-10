using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lab3.Models
{
    [Table("message", Schema = "public")]
    class Message
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [ForeignKey("user_id")]
        public User User { get; set; }

        [ForeignKey("chat_id")]
        public Chat Chat { get; set; }

        [Column("text")]
        public string Text { get; set; }

        [Column("date")]
        public DateTime Time { get; set; }

        public Message() { }
        public Message(long id, User user, Chat chat, string text, DateTime time)
        {
            Id = id;
            User = user;
            Chat = chat;
            Text = text;
            Time = time;
        }
    }
}
