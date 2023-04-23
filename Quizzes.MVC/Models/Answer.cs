using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Quizzes.Models;

public class Answer
{
    public int Id { get; set; }

    [Required(ErrorMessage = "To pole jest wymagane.")]
    [DisplayName("Treść odpowiedzi")]
    public string Content { get; set; } = string.Empty;

    [Required]
    public bool IsCorrect { get; set; }

    public int QuestionId { get; set; }
    public Question? Question { get; set; }
}
