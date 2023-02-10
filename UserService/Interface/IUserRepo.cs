using UserService.Model;
using UserService.Model.Dto;

namespace UserService.Interface
{
    public interface IUserRepo
    {
        public (bool,Guid) Insert(AddUserDto user);
        public bool Update(UpdateUserDto user);
        public bool Delete(Guid id);
        public List<User> GetAll();
        public User GetById(Guid id);
        public User LoginCheck(string UserName, string Password);
        public bool SetActivity(Guid id, bool state);
        public bool  ChangePassword(Guid id, string Password);
        public bool GetUserActiveState(Guid id);
    }
}
