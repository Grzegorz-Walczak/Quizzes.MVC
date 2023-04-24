using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quizzes.MVC.Areas.Identity.Data;
using Quizzes.Models;
using Quizzes.MVC.Models;
using Quizzes.MVC.ViewModels;

namespace Quizzes.MVC.Controllers
{
    public class QuizzesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuizzesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Quizzes
        public async Task<IActionResult> Index(string? searchPhrase)
        {
            if (_context.Quiz == null)
            {
                return NotFound();
            }

            var quizzes = await _context.Quiz.ToListAsync();

            if (searchPhrase is not null)
            {
                quizzes = quizzes
                    .Where(q => q.Name.Contains(searchPhrase) || (q.Description != null && q.Description.Contains(searchPhrase)))
                    .ToList();
            }

            return View(quizzes);
        }

        // GET: Quizzes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Quiz == null)
            {
                return NotFound();
            }

            var quiz = await _context.Quiz
                .Include(x => x.Questions)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (quiz == null)
            {
                return NotFound();
            }

            return View(quiz);
        }

        // GET: Quizzes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Quizzes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Quiz quiz)
        {
            if (!ModelState.IsValid)
            {
                return View(quiz);
            }

            quiz.CreatedAt = DateTime.Now;
            quiz.UpdatedAt = DateTime.Now;

            _context.Add(quiz);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = quiz.Id });
        }

        // GET: Quizzes/Solve/5
        public async Task<IActionResult> Solve(int? id)
        {
            if (id == null || _context.Quiz == null)
            {
                return NotFound();
            }

            var quiz = await _context.Quiz
                .Include(q => q.Questions)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (quiz == null)
            {
                return NotFound();
            }

            return View(quiz);
        }

        // POST: Solve/5
        [HttpPost]
        public IActionResult Solve(int id, int[] checkedAnswers)
        {
            return RedirectToAction("Result", new { id, checkedAnswers });
        }

        // GET: Result/5
        public async Task<IActionResult> Result(int id, int[] checkedAnswers)
        {
            try
            {
                var quiz = await _context.Quiz
                    .Include(q => q.Questions)
                    .FirstOrDefaultAsync(q => q.Id == id);

                var answers = quiz.Questions.Aggregate(new List<Answer>(), (a, b) =>
                {
                    a.AddRange(b.Answers);
                    return a;
                });

                int correctAnswers = checkedAnswers.Count(id => answers.Find(a => a.Id == id).IsCorrect);
                int numberOfQuestions = quiz.Questions.Count;

                ViewBag.checkedAnswers = checkedAnswers;
                ViewBag.correctAnswers = correctAnswers;
                ViewBag.numberOfQuestions = numberOfQuestions;

                return View(quiz);
            }
            catch
            {
                NotFound();
            }

            return View();
        }

        // GET: Quizzes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Quiz == null)
            {
                return NotFound();
            }

            var quiz = await _context.Quiz.FindAsync(id);
            if (quiz == null)
            {
                return NotFound();
            }

            return View(quiz);
        }

        // POST: Quizzes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Quiz quiz)
        {
            if (id != quiz.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(quiz);
            }

            try
            {
                _context.Update(quiz);
                quiz.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuizExists(quiz.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction("Details", new { id });
        }

        // GET: Quizzes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Quiz == null)
            {
                return NotFound();
            }

            var quiz = await _context.Quiz
                .FirstOrDefaultAsync(m => m.Id == id);

            if (quiz == null)
            {
                return NotFound();
            }

            return View(quiz);
        }

        // POST: Quizzes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Quiz == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Quiz' is null.");
            }

            var quiz = await _context.Quiz.FindAsync(id);
            if (quiz != null)
            {
                _context.Quiz.Remove(quiz);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool QuizExists(int id)
        {
            return (_context.Quiz?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
