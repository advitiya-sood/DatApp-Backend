using System;

namespace DatApp.Models
{
    public class Message
    {
        public int Id { get; set; }
        
        // The User who sent the message
        public int SenderId { get; set; }
        public string SenderUsername { get; set; }
        public User Sender { get; set; }

        // The User receiving the message
        public int RecipientId { get; set; }
        public string RecipientUsername { get; set; }
        public User Recipient { get; set; }

        public string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime MessageSent { get; set; }

        // These allow a user to "delete" a message from their view 
        // without deleting it for the other person.
        public bool SenderDeleted { get; set; }
        public bool RecipientDeleted { get; set; }
    }
}