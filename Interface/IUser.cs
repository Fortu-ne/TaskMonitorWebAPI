using TaskMonitorWebAPI.Entities;

namespace TaskMonitorWebAPI.Interface
{
    public interface IUser
    {
        bool Insert(User user);
        IEnumerable<User> GetAll();
        bool Delete(User user);
        bool Update(User user);
        bool DoesItExist(string email);
        User Find(string email);
        bool Save();

    }
}


