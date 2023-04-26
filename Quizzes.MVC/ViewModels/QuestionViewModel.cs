using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Quizzes.MVC.ViewModels;

public class QuestionViewModel
{
    [Required(ErrorMessage = "To pole jest wymagane.")]
    [DisplayName("Treść pytania")]
    public string Content { get; set; } = string.Empty;

    public IList<AnswerViewModel> Answers { get; set; } = new List<AnswerViewModel>();
    
    public int QuizId { get; set; }
}
