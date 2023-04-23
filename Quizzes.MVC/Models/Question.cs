using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Quizzes.Models;

public class Question
{
    public int Id { get; set; }

    [Required(ErrorMessage = "To pole jest wymagane.")]
    [DisplayName("Treść")]
    public string Content { get; set; } = string.Empty;

    public int QuizId { get; set; }
    public Quiz? Quiz { get; set; }

    public IList<Answer>? Answers { get; set; }
}
