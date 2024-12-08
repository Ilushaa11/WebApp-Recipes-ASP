using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using courseA4.Data;
using courseA4.Models;

namespace courseA4.Controllers
{
    public class IngredientTypesController : Controller
    {
        private readonly CookingRecipeContext _context;

        public IngredientTypesController(CookingRecipeContext context)
        {
            _context = context;
        }

        // GET: IngredientTypes
        public async Task<IActionResult> Index(int ? page)
        {
            int pageSize = 10; 
            int pageNumber = page ?? 1; 

            var ingredientTypesQuery = _context.IngredientTypes
                .OrderBy(i => i.TypeName);

            var pagedIngredientTypes = await ingredientTypesQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalItemCount = await ingredientTypesQuery.CountAsync();

            var model = new PagedList.Core.StaticPagedList<IngredientType>(
                pagedIngredientTypes, pageNumber, pageSize, totalItemCount);

            return View(model);

            //return View(await _context.IngredientTypes.ToListAsync());
        }

        // GET: IngredientTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingredientType = await _context.IngredientTypes
                .FirstOrDefaultAsync(m => m.TypeId == id);
            if (ingredientType == null)
            {
                return NotFound();
            }

            return View(ingredientType);
        }

        // GET: IngredientTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: IngredientTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TypeId,TypeName")] IngredientType ingredientType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ingredientType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ingredientType);
        }

        // GET: IngredientTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingredientType = await _context.IngredientTypes.FindAsync(id);
            if (ingredientType == null)
            {
                return NotFound();
            }
            return View(ingredientType);
        }

        // POST: IngredientTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TypeId,TypeName")] IngredientType ingredientType)
        {
            if (id != ingredientType.TypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ingredientType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IngredientTypeExists(ingredientType.TypeId))
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
            return View(ingredientType);
        }

        // GET: IngredientTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingredientType = await _context.IngredientTypes
                .FirstOrDefaultAsync(m => m.TypeId == id);
            if (ingredientType == null)
            {
                return NotFound();
            }

            return View(ingredientType);
        }

        // POST: IngredientTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ingredientType = await _context.IngredientTypes.FindAsync(id);
            if (ingredientType != null)
            {
                _context.IngredientTypes.Remove(ingredientType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IngredientTypeExists(int id)
        {
            return _context.IngredientTypes.Any(e => e.TypeId == id);
        }
    }
}
