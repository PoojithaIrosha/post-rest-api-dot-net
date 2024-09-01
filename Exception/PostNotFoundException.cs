namespace PostCrud.Exception;

public class PostNotFoundException: System.Exception
{
    public PostNotFoundException(int id) : base($"Post with ID {id} was not found.")
    {
    }
}