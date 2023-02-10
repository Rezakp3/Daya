namespace UserService.Model
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
    }
}
