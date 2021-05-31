using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JR.Shared
{
    public class Album
    {
        public int AlbumID { get; set; }
        public string Title { get; set; }
        public int ArtistID { get; set; }
        public Artist Artist { get; set; }
    }
}


