public interface IMemDb
{
    List<User> users { get; }
    List<ToDoTask> tasks { get; }
    Task<bool> Adduser(User user);
    Task<bool> AddTask(ToDoTask task);
}

public class MemDb : IMemDb
{
    public List<User> users { get; } = [];
    public List<ToDoTask> tasks { get; } = [];

    public async Task<bool> Adduser(User user)
    {
        users.Add(user);
        return true;
    }

    public async Task<bool> AddTask(ToDoTask task)
    {
        tasks.Add(task);
        return true;
    }

    public async Task<bool> UpdateTask(Guid Id, ToDoTask task)
    {
        var tasky = tasks.FirstOrDefault(x => x.Id == Id);
        if (tasky is null)
        {
            return false;
        }
        tasky.Title = task.Title;
        tasky.Description = task.Description;
        return true;
    }
}
