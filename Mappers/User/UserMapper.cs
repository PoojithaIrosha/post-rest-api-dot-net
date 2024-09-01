using PostCrud.Dto.Auth;
using PostCrud.Model;

namespace PostCrud.Mappers;

public class UserMapper
{

    public static User MapToModel(User? user, RegisterDto dto)
    {
        if (user == null)
        {
            user = new User();
        }

        user.Name = dto.Name;
        user.Username = dto.Username;
        user.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        user.Role = "USER";

        return user;
    }
    
}