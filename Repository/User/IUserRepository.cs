using PostCrud.Dto.Auth;

namespace PostCrud.Repository.User;

public interface IUserRepository
{
    Task<Model.User> FindByUsername(string username);

    Task<Model.User> SaveUser(RegisterDto dto);
}