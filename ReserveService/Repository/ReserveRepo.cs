using Domain;
using ReserveService.Interface;
using ReserveService.Model;
using ReserveService.Model.Dto;

namespace ReserveService.Repository
{
    public class ReserveRepo : IReserveRepo
    {
        private readonly IDbCon db;

        public ReserveRepo(IDbCon db)
        {
            this.db = db;
        }

        public bool ExistForAdd(DateTime date, int locId)
            => db.Query<int>($"SELECT COUNT(*) FROM Reserves WHERE (ReserveDate = '{date.ToString("yyyy-MM-dd")}') AND (LocationId = {locId})") > 0;

        public bool ExistForUpdate(DateTime date, int locId, int id)
            => db.Query<int>($"SELECT COUNT(*) FROM Reserves WHERE (ReserveDate = '{date.ToString("yyyy-MM-dd")}') AND (LocationId = {locId}) AND (Id <> {id})") > 0;

        public bool Delete(int id)
            => db.Execute($"DELETE FROM Reserves WHERE (Id = {id})") > 0;

        public List<Reserve> GetAll()
            => db.QueryList<Reserve>("SELECT * FROM Reserves");

        public Reserve GetById(int id)
            => db.Query<Reserve>($"SELECT * FROM Reserves WHERE (Id = {id})");

        public bool Insert(AddReserveDto loc, Guid reserverId)
            => db.Execute($"INSERT INTO Reserves (CreateDate, ReserveDate, ReserverId, LocationId, Price) VALUES (CURRENT_TIMESTAMP,@ReserveDate,'{reserverId}',@LocationId,@Price)", loc) > 0;

        public bool Update(UpdateReserveDto loc)
            => db.Execute("UPDATE Reserves SET ReserveDate = @ReserveDate, LocationId = @LocationId, Price = @Price WHERE (Id = @Id)", loc) > 0;
    }
}
