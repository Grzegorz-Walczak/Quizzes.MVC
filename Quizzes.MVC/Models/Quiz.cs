using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Quizzes.Models;

public class Quiz
{
    public int Id { get; set; }

    [DisplayName("Nazwa")]
    [Required(ErrorMessage = "To pole jest wymagane.")]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [DisplayName("Opis")]
    [MaxLength(500)]
    public string? Description { get; set; }

    [DisplayFormat(DataFormatString = "{0:d}")]
    [DisplayName("Utworzono")]
    public DateTime CreatedAt { get; set; }

    [DisplayFormat(DataFormatString = "{0:d}")]
    [DisplayName("Ostatnia aktualizacja")]
    public DateTime UpdatedAt { get; set; }

    public bool isPublic { get; set; }

    public IList<Question> Questions { get; set; } = new List<Question>();
}
