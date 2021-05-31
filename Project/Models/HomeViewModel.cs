using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Web;
using JR.Shared;


namespace ChinookMVC.Models
{
    public class HomeIndexViewModel
    {
        public Album album { get; set; }
        public Artist artist { get; set; }
        public Track track { get; set; }

        public int AlbumId { get; set; }
        public string Title { get; set; }

        public int ArtistId { get; set; }

        public IList<Album> albums { get; set; }
        public IList<Artist> artists { get; set; }
        public IList<Track> tracks { get; set; }
    }


}