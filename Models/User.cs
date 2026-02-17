using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public string LookingFor { get; set; }
        public DateTime ProfileCreated { get; set; }
        public DateTime LastActive { get; set; }
        public string AboutMe { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public ICollection<Photo> Photos  { get; set; }

        // Users that this user has liked (I am the liker)
        public ICollection<Like> Likees { get; set; }

        // Users that have liked this user (I am the likee)
        public ICollection<Like> Likers { get; set; }


        // for relationship between User and Message
        public ICollection<Message> MessagesSent { get; set; }
        public ICollection<Message> MessagesReceived { get; set; }

        //Password reset
        public string PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpiry { get; set; }
    }
}
