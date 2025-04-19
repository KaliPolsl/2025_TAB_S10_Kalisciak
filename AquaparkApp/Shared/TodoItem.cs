/* Shared classes can be referenced by both the Client and Server */
// Shared/TodoItem.cs
public class TodoItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public bool IsDone { get; set; }
}