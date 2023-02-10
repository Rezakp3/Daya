using Domain;
using LocationService.Interface;
using LocationService.Model;

namespace LocationService.Repository
{
    public class LocationTypeRepo : ILocationTypeRepo
    {
        private readonly IDbCon db;

        public LocationTypeRepo(IDbCon db)
        {
            this.db = db;
        }

        public bool Delete(int id)
            => db.Execute($"DELETE FROM LocationType WHERE (Id = {id})") > 0;

        public List<LocationType> GetAll()
            => db.QueryList<LocationType>($"SELECT * FROM LocationType");

        public LocationType GetById(int id)
            => db.Query<LocationType>($"SELECT * FROM LocationType WHERE (Id = {id})");

        public bool Insert(string locT)
            => db.Execute($"INSERT INTO LocationType (Type) VALUES (N'{locT}')") > 0;


        public bool Update(LocationType locT)
            => db.Execute("UPDATE LocationType SET Type = @Type WHERE (Id = @Id)",locT) > 0;
    }
}
