using TaskMonitorWebAPI.Data;
using TaskMonitorWebAPI.Entities;
using TaskMonitorWebAPI.Interface;

namespace TaskMonitorWebAPI.Repository
{
    public class UserRep : IUser
    {
        private DataContext _context;
        public UserRep(DataContext context)
        {
            _context = context;
        }

        public bool Delete(User user)
        {
            var currentUser = Find(user.EmailAddress);
            _context.User.Remove(currentUser);
            return Save();
        }

        public bool DoesItExist(string email)
        {
            return _context.User.Any(r => r.EmailAddress == email);
        }


        public User Find(string email)
        {
            return _context.User.Where(r => r.EmailAddress.ToLower() == email.ToLower()).FirstOrDefault();
        }

        public IEnumerable<User> GetAll()
        {
            return _context.User.ToList();
        }

        public bool Insert(User user)
        {
            _context.User.Add(user);
            return Save();
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0 ? true : false;
        }
        public bool Update(User user)
        {
            _context.User.Update(user);
            return Save();
        }

    }

  
}