using Domain;
using LocationService.Interface;
using LocationService.Model;
using LocationService.Model.Dto;

namespace LocationService.Repository
{
    public class LocationRepo : ILocationRepo
    {
        private readonly IDbCon db;

        public LocationRepo(IDbCon db)
        {
            this.db = db;
        }

        public bool Delete(int id)
            => db.Execute($"DELETE FROM Locations WHERE (Id = {id})") > 0;

        public List<Locations> FilterBy(int TypeId, string Title)
        {
            var param = new Dictionary<string, dynamic> {
                {"LocationTypeId" , TypeId} ,
                {"Title" , Title}
            };

            var res = db.CallStoredProsedure<Locations>("filter_location_by_LocTypeId_and_Title", param);
            return res;
        }

        public List<Locations> GetAll()
            => db.QueryList<Locations>("SELECT * FROM Locations");

        public Locations GetById(int id)
            => db.Query<Locations>($"SELECT * FROM Locations WHERE (Id = {id})");

        public bool Insert(AddLocationDto loc , Guid creatorId)
            => db.Execute(@$"INSERT INTO Locations (Title, Adres, Location, TypeId, CreatorId, CreateDate)
                                VALUES ( @Title, @Adres, @Location, @TypeId, '{creatorId}', CURRENT_TIMESTAMP )", loc) > 0;

        public bool Update(UpdateLocationDto loc)
            => db.Execute("UPDATE Locations SET Title = @Title, Adres = @Adres, Location = @Location, TypeId = @TypeId WHERE Id = @Id", loc) > 0;
    }
}
