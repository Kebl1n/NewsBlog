using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewsBlog.Models;
using NewsBlog.Models.Data;
using NewsBlog.Viewmodels.BlogsOfForm;
using PagedList.Mvc;
using PagedList;

namespace NewsBlog.Controllers
{
    public class BlogsController : Controller
    {
        private readonly AppCtx _context;


        public BlogsController(AppCtx context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index(int? pageNumber, string searchString, BlogOfStudySort sortOrder = BlogOfStudySort.TitleOfEduAsc)
        {
            ViewData["CurrentFilter"] = searchString;
            var orders = from s in _context.Blogs.Include(i => i.Category)
                         select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                orders = orders.Where(s => s.Category.FormOfCategory.Contains(searchString));
            }

            ViewData["TitleSort"] = sortOrder == BlogOfStudySort.TitleOfEduAsc ? BlogOfStudySort.TitleOfEduDesc : BlogOfStudySort.TitleOfEduAsc;
            ViewData["TextSort"] = sortOrder == BlogOfStudySort.TextOfEduAsc ? BlogOfStudySort.TextOfEduDesc : BlogOfStudySort.TextOfEduAsc;
            ViewData["DateOfSort"] = sortOrder == BlogOfStudySort.DateOfBlogAsc ? BlogOfStudySort.DateOfBlogDesc : BlogOfStudySort.DateOfBlogAsc;
            ViewData["CategorySort"] = sortOrder == BlogOfStudySort.CategoryOfAsc ? BlogOfStudySort.CategoryOfDesc : BlogOfStudySort.CategoryOfAsc;

            // Сортировка
            orders = sortOrder switch
            {
                BlogOfStudySort.TitleOfEduAsc => orders.OrderBy(s => s.Title),
                BlogOfStudySort.TitleOfEduDesc => orders.OrderByDescending(s => s.Title),
                BlogOfStudySort.TextOfEduAsc => orders.OrderBy(s => s.Text), // Предполагаем, что у вас есть свойство Text в модели
                BlogOfStudySort.TextOfEduDesc => orders.OrderByDescending(s => s.Text),
                BlogOfStudySort.DateOfBlogAsc => orders.OrderBy(s => s.Date),
                BlogOfStudySort.DateOfBlogDesc => orders.OrderByDescending(s => s.Date),
                BlogOfStudySort.CategoryOfAsc => orders.OrderBy(s => s.IdCategory),
                BlogOfStudySort.CategoryOfDesc => orders.OrderByDescending(s => s.IdCategory),

            };

            int pageSize = 3;
            return View(await PaginatedList<Blog>.CreateAsync(orders.AsNoTracking(), pageNumber ?? 1, pageSize));
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

            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }

            // Создаем SelectList для dropdown, содержащего категории, и передаем его в ViewBag
            ViewData["IdCategory"] = new SelectList(_context.Categories, "Id", "FormOfCategory", blog.IdCategory);

            // Создаем экземпляр модели представления, заполненный данными из бд
            EditFormOfBlog model = new EditFormOfBlog
            {
                Id = blog.Id,
                Title = blog.Title,
                Text = blog.Text,
                IdCategory = blog.IdCategory,
                // Здесь нужно добавить другие свойства, если они есть в EditFormOfBlog
            };

            // Возвращаем модель представления вместе с данными во view для редактирования
            return View(model);
        }

        // POST: Blogs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, EditFormOfBlog model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
                try
                {
                    var blog = await _context.Blogs.FindAsync(id);
                    if (blog == null)
                    {
                        return NotFound();
                    }
                    blog.Title = model.Title;
                    blog.Text = model.Text;
                    blog.IdCategory = model.IdCategory;

                    _context.Blogs.Update(blog);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index)); // Предполагается, что Index — это метод представления списка блогов.
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            //}

            // Если ModelState невалиден, возвращаем пользователя обратно на форму редактирования
            ViewData["IdCategory"] = new SelectList(_context.Categories, "Id", "FormOfCategory", model.IdCategory);
            return View(model);
        }

        private bool BlogExists(short id)
        {
            return _context.Blogs.Any(e => e.Id == id);
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
