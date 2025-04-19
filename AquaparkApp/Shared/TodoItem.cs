using System.ComponentModel.DataAnnotations;

public class TodoItem
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Tytuł jest wymagany.")]
    [StringLength(100, ErrorMessage = "Tytuł nie może przekraczać 100 znaków.")]
    public string Title { get; set; }

    public bool IsDone { get; set; }
}
