namespace PostCrud.Exception;

public class UserNotFoundException: System.Exception
{
    public UserNotFoundException(int id) : base($"User with ID {id} was not found.")
    {
    }
    
    public UserNotFoundException(string username) : base($"User with Username {username} was not found.")
    {
    }
}