namespace JR.Shared
{
    public class Track
    {
        public int TrackID { get; set; }
        public string Name { get; set; }
        public int AlbumID { get; set; }

        public int MediaTypeId { get; set; }
        public int Milliseconds { get; set; }

        public double UnitPrice { get; set; }
        public Album Album { get; set; }
    }
}
