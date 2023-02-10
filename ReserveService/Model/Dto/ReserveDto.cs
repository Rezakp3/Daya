namespace ReserveService.Model.Dto
{
    public class AddReserveDto
    {
        
        public DateTime ReserveDate { get; set; }
        public int LocationId { get; set; }
        public int Price { get; set; }
    }

    public class UpdateReserveDto : AddReserveDto
    {
        public int Id { get; set; }
    }
}
