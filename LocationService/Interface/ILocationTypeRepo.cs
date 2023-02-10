using LocationService.Model;

namespace LocationService.Interface
{
    public interface ILocationTypeRepo
    {
        public bool Insert(string locT);
        public bool Update(LocationType locT);
        public bool Delete(int id);
        public List<LocationType> GetAll();
        public LocationType GetById(int id);
    }
}
