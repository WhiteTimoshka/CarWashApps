using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarWashApps.Models;
using CarWashApps.Models.Data;
using CarWashApps.ViewModels.Reviews;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CarWashApps.Reviews
{
    /*    [Authorize(Roles = "admin, registeredUser")]*/
    public class ReviewsController : Controller
    {
        private readonly AppCtx _context;
        private readonly UserManager<User> _userManager;
        public enum RatingList
        {
            [Display(Name = "Выбери оценку")]
            SelectRating,
            Плохо = 1,
            Удовлетворительно = 2,
            Нормально = 3,
            Хорошо = 4,
            Отлично = 5
        }
        public ReviewsController(AppCtx context, UserManager<User> user)
        {
            _context = context;
            _userManager = user;
        }

        // GET: Specialties
        public async Task<IActionResult> Index()
        {
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            var appCtx = _context.Reviews
                .Include(s => s.User)                    // связываем специальности с формами обучения
                .OrderBy(f => f.Rating);                          // сортировка по коду специальности
            return View(await appCtx.ToListAsync());            // полученный результат передаем в представление списком
        }

        // GET: Specialties/Create
        public async Task<IActionResult> CreateAsync()
        {
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            /*var rtg = from RatingList e in Enum.GetValues(typeof(RatingList))
                      select new
                      {
                          Id = (int)e,
                          Name = e.ToString(),
                      };
            SelectList selectLists = new SelectList(rtg, "Id", "Name");
            ViewBag.Rating = selectLists;*/
            // при отображении страницы заполняем элемент "выпадающий список" формами обучения
            // при этом указываем, что в качестве идентификатора используется поле "Id"
            // а отображать пользователю нужно поле "FormOfEdu" - название формы обучения
            /*ViewData["IdUser"] = new SelectList(_context.Users.OrderBy(o => o.NameUser), "Id", "ServiceName");*/
            return View();
        }

        // POST: Specialties/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReviewViewModel model)
        {
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            /*if (ModelState.IsValid)
            {*/
                // если введены корректные данные,
                // то создается экземпляр класса модели Specialty, т.е. формируется запись в таблицу Specialties
                
                Review review = new()
                {
                    Rating = model.Rating,
                    ReviewText = model.ReviewText,
                    ReviewDateTime = DateTime.Now,
                    IdUser = user.Id,
                };

                _context.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            /*}*/

            return View(model);
        }

        // GET: Specialties/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            EditReviewViewModel model = new()
            {
                Id = review.Id,
                Rating = review.Rating,
                ReviewText = review.ReviewText,
                ReviewDateTime = review.ReviewDateTime,
                IdUser = review.IdUser,
            };

            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            /*var rtg = from RatingList e in Enum.GetValues(typeof(RatingList))
                      select new
                      {
                          Id = (int)e,
                          Name = e.ToString(),
                      };
            SelectList selectLists = new SelectList(rtg, "Id", "Name");
            ViewBag.Rating = selectLists;*/
            // в списке в качестве текущего элемента устанавливаем значение из базы данных,
            // указываем параметр specialty.IdFormOfStudy
            return View(model);
        }

        // POST: Specialties/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, EditReviewViewModel model)
        {
            Review review = await _context.Reviews.FindAsync(id);

            if (_context.Reviews
                .Where(f => f.IdUser == model.IdUser)
                .FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Этот пользователь уже оставлял отзыв");
            }
            if (id != review.Id)
            {
                return NotFound();
            }
            /*if (ModelState.IsValid)
            {*/
                try
                {
                    review.Rating = ViewBag.rtg;
                    review.ReviewText = model.ReviewText;
                    review.ReviewDateTime = model.ReviewDateTime;
                    review.IdUser = model.IdUser;
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            /*}*/
           IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            return View(model);
        }

        // GET: Specialties/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // POST: Specialties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            var review = await _context.Reviews.FindAsync(id);
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Specialties/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        private bool ReviewExists(short id)
        {
            return _context.Reviews.Any(e => e.Id == id);
        }
    }
}
