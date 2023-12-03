using System.Security.Cryptography;

namespace BlazorCRUD.Server.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _Context;
        public AuthService(DataContext context)
        {
                _Context = context;
        }
        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            if(await UserExists(user.Email)) 
            { 
                return new ServiceResponse<int>
                { 
                Success = false, 
                Message = "User already exists." 
                };
            }

            createPasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _Context.Users.Add(user);
            await _Context.SaveChangesAsync();

            return new ServiceResponse<int> { Data = user.Id };

        }

        public async Task<bool> UserExists(string email)
        {
            if(await _Context.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower())) 
            {
                return true;
            }

            return false;
        }

        private void createPasswordHash (string password, out byte[] passwordHash, out byte[] passwordSalt) 
        {
            using (var hmac = new HMACSHA512()) 
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
