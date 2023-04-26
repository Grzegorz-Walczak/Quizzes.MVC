using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Quizzes.MVC.Areas.Identity.Data;
using Quizzes.Models;
using Quizzes.MVC.ViewModels;

namespace Quizzes.MVC.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuestionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Questions/Create?quizId=5
        public async Task<IActionResult> Create(int? quizId)
        {
            if (quizId == null || _context.Quiz == null)
            {
                return NotFound();
            }

            var quiz = await _context.Quiz.FindAsync(quizId);
            if (quiz == null)
            {
                return NotFound();
            }

            var question = new QuestionViewModel()
            {
                Answers = new List<AnswerViewModel>()
                {
                    new AnswerViewModel(),
                    new AnswerViewModel(),
                    new AnswerViewModel(),
                    new AnswerViewModel()
                },
                QuizId = (int)quizId
            };
            return View(question);
        }

        // POST: Questions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int correctAnswer, QuestionViewModel questionViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(questionViewModel);
            }

            if (_context.Quiz == null)
            {
                return NotFound();
            }

            var question = new Question()
            {
                Content = questionViewModel.Content,
                Answers = questionViewModel.Answers.Select(x => new Answer()
                {
                    Content = x.Content,
                    IsCorrect = x.IsCorrect
                }).ToList(),
                QuizId = questionViewModel.QuizId
            };

            question.Answers[correctAnswer].IsCorrect = true;

            var quiz = await _context.Quiz.FindAsync(question.QuizId);
            if (quiz == null)
            {
                return NotFound();
            }

            quiz.UpdatedAt = DateTime.Now;

            _context.Add(question);
            _context.Update(quiz);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Quizzes", new { id = question.QuizId });
        }

        // GET: Questions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Question == null)
            {
                return NotFound();
            }

            var question = await _context.Question.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // POST: Questions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int correctAnswer, Question question)
        {
            if (id != question.Id || _context.Quiz == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(question);
            }

            question.Answers[correctAnswer].IsCorrect = true;

            var quiz = await _context.Quiz.FindAsync(question.QuizId);
            if (quiz == null)
            {
                return NotFound();
            }

            quiz.UpdatedAt = DateTime.Now;

            _context.Update(question);
            _context.Update(quiz);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Quizzes", new { id = question.QuizId });
        }

        // GET: Questions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Question == null)
            {
                return NotFound();
            }

            var question = await _context.Question
                .Include(q => q.Quiz)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Question == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Question' is null.");
            }

            var question = await _context.Question.FindAsync(id);
            if (question != null)
            {
                _context.Question.Remove(question);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Quizzes", new { id = question?.QuizId });
        }

        private bool QuestionExists(int id)
        {
            return (_context.Question?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
