using System;

namespace DatApp.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public DateTime DateAdded { get; set; }
        public string Url { get; set; }
        public string Caption { get; set; }
        public bool IsMain { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
    }
}