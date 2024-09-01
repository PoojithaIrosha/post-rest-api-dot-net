using PostCrud.Model;

namespace PostCrud.Services;

public interface ITokenService
{
    public string CreateToken(User user);
}