using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lab3.Models
{
    [Table("chat", Schema = "public")]
    class Chat
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("tag")]
        public string Tag { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("bio")]
        public string Bio { get; set; }

        public Chat() { }
        public Chat(long id, string tag = null, string name = null, string bio = null)
        {
            Id = id;
            Tag = tag;
            Name = name;
            Bio = bio;
        }

        
        public virtual ICollection<UserChat> UserChats { get; set; }

        
        public virtual ICollection<Message> Messages { get; set; }
    }
}
