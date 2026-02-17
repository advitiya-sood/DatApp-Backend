using System;
using System.ComponentModel.DataAnnotations;

namespace DatApp.Dtos
{
    public class PhotoForDetailDto
    {
        [Required]
        public int Id { get; set; }
        public string Url { get; set; }
        public string Caption { get; set; }
        public bool IsMain { get; set; }
        public DateTime DateAdded { get; set; }
    }
}

