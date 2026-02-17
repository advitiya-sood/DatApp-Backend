using System.ComponentModel.DataAnnotations;

namespace DatApp.Dtos
{
    public class UserForUpdateDto
    {
        public string KnownAs { get; set; }
        public string Gender { get; set; }

        [StringLength(50, MinimumLength =20,ErrorMessage ="Letters should be between 20 and 50")]
        public string LookingFor { get; set; }
        
        [StringLength(500, MinimumLength =100,ErrorMessage ="Letters should be between 100 and 500")]
        public string AboutMe { get; set; }
        public string Intrests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}

