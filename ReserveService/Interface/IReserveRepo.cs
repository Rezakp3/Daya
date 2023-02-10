using ReserveService.Model;
using ReserveService.Model.Dto;

namespace ReserveService.Interface
{
    public interface IReserveRepo
    {
        public bool Insert(AddReserveDto loc,Guid reserverId);
        public bool Update(UpdateReserveDto loc);
        public bool Delete(int id);
        public List<Reserve> GetAll();
        public Reserve GetById(int id);
        public bool ExistForAdd(DateTime date, int locId);
        public bool ExistForUpdate(DateTime date, int locId , int id);
    }
}
