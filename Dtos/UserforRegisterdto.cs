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
        [StringLength(10, MinimumLength =6,ErrorMessage ="Letters should be between 6 and 10")]
        public string Password { get; set; }
    }
}
