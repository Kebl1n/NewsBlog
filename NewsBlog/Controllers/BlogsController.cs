using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewsBlog.Models;
using NewsBlog.Models.Data;
using NewsBlog.Viewmodels.BlogsOfForm;

namespace NewsBlog.Controllers
{
    public class BlogsController : Controller
    {
        private readonly AppCtx _context;
        private readonly UserManager<User> _userManager;

        public BlogsController(AppCtx context, UserManager<User> user)
        {
            _context = context;
            _userManager = user;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            var appCtx = _context.Blogs.Include(i => i.Category);
            // возвращаем в представление полученный список записей
            return View(await appCtx.ToListAsync());
        }


        // GET: Blogs/Details/5
        public async Task<IActionResult> Details(short? id)
        {
            if (id == null || _context.Blogs == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // GET: Blogs/Create
        public IActionResult Create()
        {
            ViewData["IdCategory"] = new SelectList(_context.Categories, "Id", "FormOfCategory");
            return View();
        }

        // POST: Blogs/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateFormOfBlog model)
        {
            //if (_context.Blogs
            //    .Where(f => f.Title == model.Title)
            //    .FirstOrDefault() != null)
            //{
            //    ModelState.AddModelError("", "Введеная новость уже существует");
            //}


            ViewData["IdCategory"] = new SelectList(_context.Categories, "Id", "FormOfCategory", model);

            //if (ModelState.IsValid)
            //{
            Blog blog = new()
                {
                    Title = model.Title,
                    Text = model.Text,
                    IdCategory = model.IdCategory,
                    Id = model.Id,
                    Date = DateTime.Now
                };

                _context.Add(blog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            //}
        
        //return View(model);
        }

        // GET: Blogs/Edit/5
        public async Task<IActionResult> Edit(short? id)
        {
            if (id == null || _context.Blogs == null)
            {
                return NotFound();
            }

            var category = await _context.Blogs.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            ViewData["IdCategory"] = new SelectList(_context.Categories, "Id", "FormOfCategory");
            EditFormOfBlog model = new()
            {
                Id = category.Id,
                Title = category.Title,
                Text = category.Text,
                IdCategory = category.IdCategory,
            };
            return View(model);
        }

        // POST: Blogs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, EditFormOfBlog model)
        {
            //if (_context.Blogs
            //    .Where(f => f.Title == model.Title)
            //    .FirstOrDefault() != null)
            //{
            //    ModelState.AddModelError("", "Введеная новость уже существует");
            //}

            Blog blog = await _context.Blogs.FindAsync(id);

            if (id != blog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    blog.Title = model.Title;
                    blog.Text = model.Text;
                    blog.IdCategory = model.IdCategory;
                    _context.Update(blog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(blog.Id))
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

        private bool CategoryExists(short id)
        {
            return (_context.Blogs?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // GET: Blogs/Delete/5
        public async Task<IActionResult> Delete(short? id)
        {
            if (id == null || _context.Blogs == null)
            {
                return NotFound();
            }

            var category = await _context.Blogs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            if (_context.Blogs == null)
            {
                return Problem("Entity set 'AppCtx.Categories'  is null.");
            }
            var category = await _context.Blogs.FindAsync(id);
            if (category != null)
            {
                _context.Blogs.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        
    }
}
