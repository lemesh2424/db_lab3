using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lab3.Models
{
    [Table("user", Schema = "public")]
    class User
    {
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("login")]
        public string Login { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("bio")]
        public string Bio { get; set; }

        public User() { }
        public User(long id, string login = null, string name = null, string bio = null)
        {
            Id = id;
            Login = login;
            Name = name;
            Bio = bio;
        }

        public virtual ICollection<UserChat> UserChats { get; set; }

        public virtual ICollection<Message> Messages { get; set; }

    }
}
