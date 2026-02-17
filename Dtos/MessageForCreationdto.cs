using System;
using System.ComponentModel.DataAnnotations;

namespace DatApp.Dtos
{
    public class MessageForCreationDto
    {
        public int RecipientId { get; set; }
        public DateTime MessageSent { get; set; }

        [Required]
        public string Content { get; set; }

        public MessageForCreationDto()
        {
            MessageSent = DateTime.Now;
        }
    }
}