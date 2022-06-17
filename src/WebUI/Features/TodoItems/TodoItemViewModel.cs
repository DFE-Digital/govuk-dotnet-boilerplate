using System.ComponentModel.DataAnnotations;
using CleanArchitecture.Application.TodoItems.Queries.GetTodoItemsWithPagination;

namespace CleanArchitecture.WebUI.Features.TodoItems;

public class TodoItemViewModel
{
    public bool IsCreating { get; set; }
    
    public int ListId { get; set; }

    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = default!;

    public bool Done { get; set; }
}
