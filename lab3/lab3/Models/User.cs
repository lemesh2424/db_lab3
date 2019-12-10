using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lab3.Models
{
    [Table("user", Schema = "public")]
    class User
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("login")]
        public string Login { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("bio")]
        public string Bio { get; set; }

        public User(long id, string login = null, string name = null, string bio = null)
        {
            Id = id;
            Login = login;
            Name = name;
            Bio = bio;
        }
    }
}
