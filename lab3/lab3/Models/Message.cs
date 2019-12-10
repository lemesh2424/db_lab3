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
        public long Id { get; set; }

        [Column("user_id")]
        public long UserId { get; set; }

        [Column("chat_id")]
        public long ChatId { get; set; }

        [Column("text")]
        public string Text { get; set; }

        [Column("date")]
        public DateTime Time { get; set; }

        public Message(long id, long userId, long chatId, string text, DateTime time)
        {
            Id = id;
            UserId = userId;
            ChatId = chatId;
            Text = text;
            Time = time;
        }
    }
}
