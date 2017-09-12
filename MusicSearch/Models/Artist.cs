using System.ComponentModel.DataAnnotations;

namespace MusicSearch.Models
{
    public class Artist
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Listeners { get; set; }        
        public string ImageLarge { get; set; }
    }
}