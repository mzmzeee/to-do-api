public interface IMemDb
{
    List<User> users { get; }
    List<ToDoTask> tasks { get; }
    void Adduser(User user);
    void AddTask(ToDoTask task);
}

public class MemDb : IMemDb
{
    public List<User> users { get; } = [];
    public List<ToDoTask> tasks { get; } = [];

    public void Adduser(User user)
    {
        users.Add(user);
    }

    public void AddTask(ToDoTask task)
    {
        tasks.Add(task);
    }
}
