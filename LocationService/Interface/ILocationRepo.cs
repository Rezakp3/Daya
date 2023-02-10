using LocationService.Model;
using LocationService.Model.Dto;

namespace LocationService.Interface
{
    public interface ILocationRepo
    {
        public bool Insert(AddLocationDto loc , Guid creatorId);
        public bool Update(UpdateLocationDto loc);
        public bool Delete(int id);
        public List<Locations> GetAll();
        public Locations GetById(int id);
        public List<Locations> FilterBy(int TypeId, string Title);
    }
}
