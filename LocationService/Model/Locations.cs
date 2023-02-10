namespace LocationService.Model
{
    public class Locations
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Adres { get; set; }
            public string Location { get; set; }
            public DateTime CreateDate { get; set; }
            public Guid CreatorId { get; set; }
            public int TypeId { get; set; }
    }
}
