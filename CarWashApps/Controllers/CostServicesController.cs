using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarWashApps.Models;
using CarWashApps.Models.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis;
using CarWashApps.ViewModels.CostServices;

namespace CarWashApps.Controllers
{
/*    [Authorize(Roles = "admin, registeredUser")]*/
    public class CostServicesController : Controller
    {
        private readonly AppCtx _context;
        private readonly UserManager<User> _userManager;

        public CostServicesController(AppCtx context, UserManager<User> user)
        {
            _context = context;
            _userManager = user;
        }

        // GET: Specialties
        public async Task<IActionResult> Index()
        {
            // IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            var appCtx = _context.CostServices
                .Include(s => s.ListService)                    // связываем специальности с формами обучения
                .OrderBy(f => f.DateCost);                          // сортировка по коду специальности
            return View(await appCtx.ToListAsync());            // полученный результат передаем в представление списком
        }

        // GET: Specialties/Create
        public async Task<IActionResult> CreateAsync()
        {
            // IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            // при отображении страницы заполняем элемент "выпадающий список" формами обучения
            // при этом указываем, что в качестве идентификатора используется поле "Id"
            // а отображать пользователю нужно поле "FormOfEdu" - название формы обучения
            ViewData["IdService"] = new SelectList(_context.ListServices, "Id", "ServiceName");
            return View();
        }

        // POST: Specialties/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCostServiceViewModel model)
        {
            // IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            if (_context.CostServices
                .Where(f => f.IdService == model.IdService &&
                    f.Cost == model.Cost &&
                    f.DateCost == model.DateCost)
                .FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введеная стоимость у услуги уже существует");
            }

            if (ModelState.IsValid)
            {
                // если введены корректные данные,
                // то создается экземпляр класса модели Specialty, т.е. формируется запись в таблицу Specialties
                CostService costService = new()
                {
                    Cost = model.Cost,
                    DateCost = model.DateCost,

                    // с помощью свойства модели получим идентификатор выбранной формы обучения пользователем
                    IdService = model.IdService
                };

                _context.Add(costService);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdService"] = new SelectList(
                _context.ListServices,
                "Id", "ServiceName", model.IdService);
            return View(model);
        }

        // GET: Specialties/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var costService = await _context.CostServices.FindAsync(id);
            if (costService == null)
            {
                return NotFound();
            }
            EditCostServiceViewModel model = new()
            {
                Id = costService.Id,
                Cost = costService.Cost,
                DateCost = costService.DateCost,
                IdService = costService.IdService
            };

            // IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            // в списке в качестве текущего элемента устанавливаем значение из базы данных,
            // указываем параметр specialty.IdFormOfStudy
            ViewData["IdService"] = new SelectList(
                _context.ListServices,
                "Id", "ServiceName", costService.IdService);
            return View(model);
        }

        // POST: Specialties/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, EditCostServiceViewModel model)
        {
            CostService costService = await _context.CostServices.FindAsync(id);
            
            if (_context.CostServices
                .Where(f => f.IdService == model.IdService &&
                    f.Cost == model.Cost &&
                    f.DateCost == model.DateCost)
                .FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Вы не изменили стоимость услуги");
            }
            if (id != costService.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    costService.Cost = model.Cost;
                    costService.DateCost = model.DateCost;
                    costService.IdService = model.IdService;
                    _context.Update(costService);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CostServiceExists(costService.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            // IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            ViewData["IdService"] = new SelectList(
                _context.ListServices,
                "Id", "ServiceName", costService.IdService);
            return View(model);
        }

        // GET: Specialties/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var costService = await _context.CostServices
                .Include(s => s.ListService)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (costService == null)
            {
                return NotFound();
            }

            return View(costService);
        }

        // POST: Specialties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            var costService = await _context.CostServices.FindAsync(id);
            _context.CostServices.Remove(costService);
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

            var costService = await _context.CostServices
                .Include(s => s.ListService)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (costService == null)
            {
                return NotFound();
            }

            return View(costService);
        }

        private bool CostServiceExists(short id)
        {
            return _context.CostServices.Any(e => e.Id == id);
        }
    }
}