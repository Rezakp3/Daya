namespace UserService.Model.Dto
{
    public class UpdateUserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
    }
}
