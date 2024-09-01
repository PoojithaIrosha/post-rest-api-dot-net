using Microsoft.EntityFrameworkCore;
using PostCrud.Config;
using PostCrud.Dto.Auth;
using PostCrud.Exception;
using PostCrud.Mappers;

namespace PostCrud.Repository.User;

public class UserRepository: IUserRepository
{
    private readonly AppDbContext _context;
    
    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Model.User> FindByUsername(string username)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username.Equals(username));
        if (user == null)
        {
            throw new UserNotFoundException(username);
        }
        return user;
    }

    public async Task<Model.User> SaveUser(RegisterDto dto)
    {
        var user = UserMapper.MapToModel(null, dto);
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }
}