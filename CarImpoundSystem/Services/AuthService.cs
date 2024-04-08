using CarImpoundSystem.Data;

namespace CarImpoundSystem.Services
{
    public class AuthService
    {
        private readonly AppDBContext _dbContext;

        public AuthService(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public bool Authentificate(string username, string password)
        {
            var user = _dbContext.users.FirstOrDefault(u => u.username == username && u.password == password);
            return user != null; // User exists if not null
        }
        public bool HasRole(string username, string role)
        {
            // Query the database to check if the user has the specified role
            var user = _dbContext.users.FirstOrDefault(u => u.username == username);
            if (user != null)
            {
                // Check if the user has the specified role
                return user.role == role;
            }
            return false; // User not found or doesn't have the specified role
        }
    }
}
