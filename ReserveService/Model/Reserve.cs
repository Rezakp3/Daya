namespace ReserveService.Model
{
    public class Reserve
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ReserveDate { get; set; }
        public Guid ReserverId { get; set; }
        public int LocationId { get; set; }
        public int Price { get; set; }
    }
}
