using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebApplication3.Models;
namespace WebApplication3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ModelContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ModelContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var questions = _context.Questions.ToList();
            return View(questions);
        }

        public IActionResult Adding()
        {
            var questions = _context.Questions.ToList();
            return View(questions);
        }
        [Authorize]
        public IActionResult Change()
        {
            var questions = _context.Questions.ToList();
            return View(questions);
        }
        [Authorize]
        public IActionResult Change2(int id)
        {
            var question = _context.Questions.FirstOrDefault(q => q.Id == id);
            if (question == null)
            {
                return NotFound();
            }
            return View(question);
        }
        [Authorize]
        [HttpPost]
        public IActionResult Adding(Question question)
        {
            _context.Questions.Add(question);

            // Обработка ошибок при сохранении данных
            try
            {
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DbUpdateException)
            {
                return View("Error");
            }
        }

        //[Authorize]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var question = _context.Questions.Find(id);
            if (question != null)
            {
                try
                {
                    _context.Questions.Remove(question);
                    _context.SaveChanges();
                    return Json(new { success = true, message = "Вопрос успешно удален!" });
                }
                catch (DbUpdateException)
                {
                    return Json(new { success = false, message = "Ошибка при удалении вопроса." });
                }
            }

            return Json(new { success = false, message = "Ошибка при удалении вопроса." });
        }


        public IActionResult Privacy()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult Change2(int id, string matter, string answer)
        {
            var question = _context.Questions.FirstOrDefault(q => q.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            // Проверяем, изменились ли данные перед обновлением
            if (question.Matter != matter || question.Answer != answer)
            {
                question.Matter = matter;
                question.Answer = answer;

                // Добавляем проверку на ошибки при сохранении данных
                try
                {
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException)
                {
                    return View("Error");
                }
            }

            // Возвращаем, если данные не изменились
            return RedirectToAction("Index");
        }
    }
}
