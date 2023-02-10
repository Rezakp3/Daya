using Domain;
using System.Security.Cryptography;
using System.Text;
using UserService.Interface;
using UserService.Model;
using UserService.Model.Dto;

namespace UserService.Repository
{
    public class UserRepo : IUserRepo
    {
        private readonly IDbCon db;

        public UserRepo(IDbCon dbCon)
        {
            db = dbCon;
        }

        public bool ChangePassword(Guid id, string Password)
            => db.Execute($"UPDATE [User] SET PasswordHash = N'{HashPassword(Password)}' WHERE (Id = '{id}')") > 0;


        public bool Delete(Guid id)
            => db.Execute($"DELETE FROM [User] WHERE (Id = '{id}')") > 0;


        public List<User> GetAll()
        {
            var users = db.QueryList<User>("SELECT * FROM [User]");
            return users;
        }

        public User GetById(Guid id)
        {
            var User = db.Query<User>($"SELECT * FROM [User] Where Id = '{id}'");
            return User;
        }

        public (bool, Guid) Insert(AddUserDto user)
        {
            user.Password = HashPassword(user.Password);
            var id = Guid.NewGuid();
            var rowsEffected = db.Execute($"INSERT INTO [User] (Id, UserName, PasswordHash, IsActive, CreateDate, Name, Family) VALUES ('{id}', @UserName, @Password, 1, CURRENT_TIMESTAMP, @Name, @Family)", user) > 0;
            return (rowsEffected, id);
        }

        public User LoginCheck(string UserName, string Password)
        {
            var user = db.Query<User>($"select * from [User] where UserName = N'{UserName}' and PasswordHash = N'{HashPassword(Password)}'");
            return user;
        }

        public bool SetActivity(Guid id, bool state)
        {
            var val = state ? 1 : 0;
            return db.Execute($"UPDATE [User] SET IsActive = {val} WHERE (Id = '{id}')") > 0;
        }

        public bool Update(UpdateUserDto user)
            => db.Execute("UPDATE [User] SET UserName = @UserName, Name = @Name, Family = @Family WHERE (Id = @Id)", user) > 0;
        
        public bool GetUserActiveState(Guid id)
            => db.Query<bool>($"SELECT IsActive FROM [User] WHERE Id = '{id}'");

        // hash password with SHA256 Encryption 
        private string HashPassword(string pass)
        {
            StringBuilder sb = new StringBuilder();
            using (var sha256 = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(pass));
                var a = hash.Select(x => x.ToString("x2"));
                sb.AppendJoin("", a);
            }
            return sb.ToString();
        }
    }
}
