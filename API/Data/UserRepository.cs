using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserReopsitory
    {
        private readonly DataContext dataContext;

        public UserRepository(DataContext context)
        {
            dataContext = context;
        }
        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await dataContext.Users.Include(u => u.Photos).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await dataContext.Users.Include(u => u.Photos).SingleOrDefaultAsync(c => c.UserName.ToLower() == username);
        }

        public async Task<IEnumerable<AppUser>> GetAllUsersAsync()
        {
            return await dataContext.Users.Include(u => u.Photos).ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await dataContext.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            dataContext.Entry(user).State = EntityState.Modified;
        }
    }
}
