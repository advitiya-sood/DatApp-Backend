using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DatApp.Dtos
{
    public class UserforRegisterdto
    {

        public string Username { get; set; }

        [Required]
        [StringLength(20, MinimumLength =5,ErrorMessage ="Letters should be between 5 and 20")]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
