using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using courseA4.Data;
using courseA4.Models;
using courseA4.Services;

namespace courseA4.Controllers
{
    public class IngredientsController : Controller
    {
        private readonly CookingRecipeContext _context;

        public IngredientsController(CookingRecipeContext context)
        {
            _context = context;
        }

        // GET: Ingredients
        public async Task<IActionResult> Index(int ? page)
        {
            int pageSize = 10; 
            int pageNumber = page ?? 1;

            var ingredientsQuery = _context.Ingredients
                .Include(i => i.Type) 
                //.OrderBy(i => i.Name)
                ;

            var pagedIngredients = await ingredientsQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            
            var totalItemCount = await ingredientsQuery.CountAsync();

            var model = new PagedList.Core.StaticPagedList<Ingredient>(pagedIngredients, pageNumber, pageSize, totalItemCount);

            return View(model);

            /*
            var db9093Context = _context.Ingredients.Include(i => i.Type);
            return View(await db9093Context.ToListAsync());
            */
        }

        // GET: Ingredients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingredient = await _context.Ingredients
                .Include(i => i.Type)
                .FirstOrDefaultAsync(m => m.IngredientId == id);
            if (ingredient == null)
            {
                return NotFound();
            }

            return View(ingredient);
        }

        // GET: Ingredients/Create
        [CustomAuthorize("User", "Admin")]
        public IActionResult Create()
        {
            ViewData["TypeId"] = new SelectList(_context.IngredientTypes, "TypeId", "TypeId");
            return View();
        }

        // POST: Ingredients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IngredientId,Name,TypeId")] Ingredient ingredient)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ingredient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TypeId"] = new SelectList(_context.IngredientTypes, "TypeId", "TypeId", ingredient.TypeId);
            return View(ingredient);
        }

        // GET: Ingredients/Edit/5
        [CustomAuthorize("User", "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingredient = await _context.Ingredients.FindAsync(id);
            if (ingredient == null)
            {
                return NotFound();
            }
            ViewData["TypeId"] = new SelectList(_context.IngredientTypes, "TypeId", "TypeId", ingredient.TypeId);
            return View(ingredient);
        }

        // POST: Ingredients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IngredientId,Name,TypeId")] Ingredient ingredient)
        {
            if (id != ingredient.IngredientId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ingredient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IngredientExists(ingredient.IngredientId))
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
            ViewData["TypeId"] = new SelectList(_context.IngredientTypes, "TypeId", "TypeId", ingredient.TypeId);
            return View(ingredient);
        }

        // GET: Ingredients/Delete/5
        [CustomAuthorize("Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingredient = await _context.Ingredients
                .Include(i => i.Type)
                .FirstOrDefaultAsync(m => m.IngredientId == id);
            if (ingredient == null)
            {
                return NotFound();
            }

            return View(ingredient);
        }

        // POST: Ingredients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ingredient = await _context.Ingredients.FindAsync(id);
            if (ingredient != null)
            {
                _context.Ingredients.Remove(ingredient);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IngredientExists(int id)
        {
            return _context.Ingredients.Any(e => e.IngredientId == id);
        }
    }
}
