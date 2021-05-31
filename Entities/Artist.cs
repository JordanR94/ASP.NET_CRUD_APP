using System.Collections.Generic;

namespace JR.Shared
{
    public class Artist
    {
        public int ArtistID { get; set; }
        public string Name { get; set; }
        public ICollection<Album> Album { get; set; }
    }
}