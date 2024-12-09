using courseA4.Data;
using courseA4.Models;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.Extensions.Options;

namespace courseA4.Services
{
    public class AccountService
    {
        private readonly ApplicationDbContext _context;

        public AccountService(ApplicationDbContext context)
        {
            _context = context;
        }

        public User Authenticate(string username, string password)
        {

            return _context.Users.SingleOrDefault(u => u.Login == username && u.PasswordHash == password);
        }
    }
}
