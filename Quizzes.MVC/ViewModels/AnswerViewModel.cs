using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Quizzes.MVC.ViewModels;

public class AnswerViewModel
{
    [Required(ErrorMessage = "To pole jest wymagane.")]
    [DisplayName("Treść odpowiedzi")]
    public string Content { get; set; } = string.Empty;
    
    [Required]
    public bool IsCorrect { get; set; }
}
