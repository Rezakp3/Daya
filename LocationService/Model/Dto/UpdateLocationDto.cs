namespace LocationService.Model.Dto
{
    public class UpdateLocationDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Adres { get; set; }
        public string Location { get; set; }
        public int TypeId { get; set; }
    }
}
