using CarWashApps.Models;
using CarWashApps.Models.Data;
using CarWashApps.ViewModels.Orders;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CarWashApps.Controllers
{
    // [Authorize(Roles = "admin, registeredUser")]
    public class OrdersController : Controller
    {
        private readonly AppCtx _context;
        private readonly UserManager<User> _userManager;

        public OrdersController(
            AppCtx context,
            UserManager<User> user)
        {
            _context = context;
            _userManager = user;
        }

        // GET: FormsOfStudy
        public async Task<IActionResult> Index()
        {
            // находим информацию о пользователе, который вошел в систему по его имени
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            // через контекст данных получаем доступ к таблице базы данных FormsOfStudy
            var appCtx = _context.Orders
                .Include(f => f.User)                // и связываем с таблицей пользователи через класс User
                .Include(f=>f.ListService)
                .Where(f => f.IdUser == user.Id)     // устанавливается условие с выбором записей форм обучения текущего пользователя по его Id
                .OrderBy(f => f.IdService);          // сортируем все записи по имени форм обучения

            // возвращаем в представление полученный список записей
            return View(await appCtx.ToListAsync());
        }

        // GET: FormsOfStudy/Create
        public async Task<IActionResult> CreateAsync()
        {
            ViewData["IdService"] = new SelectList(_context.ListServices.OrderBy(o => o.ServiceName), "Id", "ServiceName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateOrderViewModel model)
        {
            IdentityUser user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            if (_context.Orders
                .Where(f => f.OrderDateTime == model.OrderDateTime).FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Данное время уже занято");
            }

            /*if (ModelState.IsValid)
            {*/
                Order order = new()
                {
                    IdService = model.IdService,
                    OrderDateTime = (DateTime)model.OrderDateTime,
                    IdUser = user.Id
                };

                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
           /* }
            ViewData["IdService"] = new SelectList(_context.ListServices.OrderBy(o => o.ServiceName), "Id", "ServiceName", model.IdService);
            return View(model);*/
        }

        // GET: FormsOfStudy/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            EditOrderViewModel model = new()
            {
                Id = order.Id,
                IdService = order.IdService,
                OrderDateTime = (DateTime)order.OrderDateTime,
                IdUser = order.IdUser
            };

            ViewData["IdService"] = new SelectList(
                _context.ListServices.OrderBy(o => o.ServiceName),
                "Id", "ServiceName", order.IdService);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditOrderViewModel model)
        {
            Order order = await _context.Orders.FindAsync(id);

            if (_context.Orders
                .Where(f => f.OrderDateTime == model.OrderDateTime).FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Данное время уже занято");
            }

            if (id != order.Id)
            {
                return NotFound();
            }

            /*if (ModelState.IsValid)
            {*/
                try
                {
                    order.OrderDateTime = model.OrderDateTime;
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            return View(model);
        }

        // GET: FormsOfStudy/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: FormsOfStudy/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: FormsOfStudy/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(f => f.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}