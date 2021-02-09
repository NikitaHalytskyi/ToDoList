using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationContext db;
        public HomeController(ApplicationContext context)
        {
            db = context;
        }

        public async Task<IActionResult> Index(SortState sortOrder = SortState.IsDoneAsc)
        {
            IQueryable<ItemToDo> item = db.ToDo;

            ViewData["IsDoneSort"] = sortOrder == SortState.IsDoneAsc ? SortState.IsDoneDesc : SortState.IsDoneAsc;

            item = sortOrder switch
            {
                SortState.IsDoneDesc => item.OrderByDescending(s => s.IsDone),
                _ => item.OrderBy(s => s.IsDone),
            };
            return View(await item.AsNoTracking().ToListAsync());
        }

        //public async Task<IActionResult> Index()
        //{
        //    return View(await db.ToDo.ToListAsync());
        //}

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ItemToDo item)
        {
            db.ToDo.Add(item);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id != null)
            {
                ItemToDo item = await db.ToDo.FirstOrDefaultAsync(p => p.Id == id);
                if (item != null)
                    return View(item);
            }
            return NotFound();
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                ItemToDo item = await db.ToDo.FirstOrDefaultAsync(p => p.Id == id);
                if (item != null)
                    return View(item);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ItemToDo item)
        {
            db.ToDo.Update(item);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                ItemToDo item = await db.ToDo.FirstOrDefaultAsync(p => p.Id == id);
                if (item != null)
                    return View(item);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                ItemToDo item = new ItemToDo { Id = id.Value };
                db.Entry(item).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return NotFound();
        }
        
    }
}
