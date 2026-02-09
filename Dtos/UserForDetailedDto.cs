using System;
using System.Collections.Generic;

namespace DatApp.Dtos
{
    public class UserForDetailedDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string KnownAs { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string LookingFor { get; set; }
        public string AboutMe { get; set; }
        public string Intrests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PhotoUrl { get; set; }
        public ICollection<PhotoForDetailDto> Photos { get; set; }
    }
}

