using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsBlog.Models;
using NewsBlog.Models.Data;
using NewsBlog.Viewmodels.FormsOfCategory;



namespace NewsBlog.Controllers
{
    //[Authorize(Roles = "admin, registeredUser")]
    public class CategoriesController : Controller
    {
        private readonly AppCtx _context;
        private readonly UserManager<User> _userManager;

        public CategoriesController(AppCtx context, UserManager<User> user)
        {
            _context = context;
            _userManager = user;
        }

        // GET: Categories
        public async Task<IActionResult> Index(int? page)
        {

            var appCtx = _context.Categories
                .OrderBy(f => f.FormOfCategory);
              return _context.Categories != null ? 
                          View(await appCtx.ToListAsync()) :
                          Problem("Entity set 'AppCtx.Categories'  is null.");
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateFormOfCategory model)
        {
            if (_context.Categories
                .Where(f => f.FormOfCategory == model.FormOfCategory)
                .FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введеная категория уже существует");
            }
            if (ModelState.IsValid)
            {
                Category genre = new()
                {
                    FormOfCategory = model.FormOfCategory
                };

                _context.Add(genre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            EditFormsOfCategory model = new()
            {
                Id = category.Id,
                FormOfCategory = category.FormOfCategory
            };
            return View(model);
        }

        // POST: Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, EditFormsOfCategory model)
        {
            if (_context.Categories
               .Where(f => f.FormOfCategory == model.FormOfCategory)
               .FirstOrDefault() != null)
            {
                ModelState.AddModelError("", "Введеная категория уже существует");
            }

            Category category = await _context.Categories.FindAsync(id);

            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    category.FormOfCategory = model.FormOfCategory;
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'AppCtx.Categories'  is null.");
            }
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(short id)
        {
          return (_context.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
