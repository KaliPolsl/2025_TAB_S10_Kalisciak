// Server/Controllers/TodoController.cs
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TodoController : ControllerBase
{
    private static List<TodoItem> items = new()
    {
        new TodoItem { Id = 1, Title = "Test", IsDone = false }
    };

    [HttpGet]
    public IEnumerable<TodoItem> Get() => items;

    [HttpPost]
    public IActionResult Add(TodoItem item)
    {
        item.Id = items.Max(i => i.Id) + 1;
        items.Add(item);
        return Ok(item);
    }
}