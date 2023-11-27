using CarWashApps.Models;
using CarWashApps.Models.Data;
using CarWashApps.ViewModels.ListServices;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarWashApps.Controllers
{
    public class ListServicesController : Controller
    {
        private readonly AppCtx _context;

        public ListServicesController(AppCtx context)
        {
            _context = context;
        }

        // GET: Genres
        public async Task<IActionResult> Index()
        {
            // через контекст данных получаем доступ к таблице базы данных FormsOfStudy
            var appCtx = _context.ListServices
                .OrderBy(f => f.ServiceName);          // сортируем все записи по имени форм обучения

            // возвращаем в представление полученный список записей
            return View(await appCtx.ToListAsync());
        }

        // GET: Genres/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ListServices == null)
            {
                return NotFound();
            }

            var listService = await _context.ListServices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (listService == null)
            {
                return NotFound();
            }

            return View(listService);
        }

        // GET: Genres/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Genres/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateServiceViewModel model)
        {
            if (_context.ListServices
                .Where(f => f.ServiceName == model.ServiceName)
                .FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введеная услуга уже существует");
            }

            if (ModelState.IsValid)
            {
                ListService listService = new()
                {
                    ServiceName = model.ServiceName,
                    ServiceDescription = model.ServiceDescription
                };

                _context.Add(listService);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Genres/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ListServices == null)
            {
                return NotFound();
            }

            var listService = await _context.ListServices.FindAsync(id);
            if (listService == null)
            {
                return NotFound();
            }

            EditServiceViewModel model = new()
            {
                Id = listService.Id,
                ServiceName = listService.ServiceName,
                ServiceDescription = listService.ServiceDescription
            };
            return View(model);
        }

        // POST: Genres/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditServiceViewModel model)
        {
            ListService listService = await _context.ListServices.FindAsync(id);
            if (_context.ListServices
                .Where(f => f.ServiceName == model.ServiceName)
                .FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введеная услуга уже существует");
            }
            if (id != listService.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    listService.ServiceName = model.ServiceName;
                    listService.ServiceDescription = model.ServiceDescription;
                    _context.Update(listService);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ListServicesExists(listService.Id))
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
            return View(model);
        }

        // GET: Genres/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ListServices == null)
            {
                return NotFound();
            }

            var listService = await _context.ListServices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (listService == null)
            {
                return NotFound();
            }

            return View(listService);
        }

        // POST: Genres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ListServices == null)
            {
                return Problem("Не существует 'AppCtx.ListServices'");
            }
            var listService = await _context.ListServices.FindAsync(id);
            if (listService != null)
            {
                _context.ListServices.Remove(listService);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ListServicesExists(int id)
        {
            return (_context.ListServices?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}