using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using courseA4.Data;
using courseA4.Models;
using PagedList.Core;


namespace courseA4.Controllers
{
    public class RecipeIngredientsController : Controller
    {
        private readonly CookingRecipeContext _context;

        public RecipeIngredientsController(CookingRecipeContext context)
        {
            _context = context;
        }

        // GET: RecipeIngredients
        public async Task<IActionResult> Index(int ? page)
        {
            int pageSize = 10; 
            int pageNumber = page ?? 1; 
            var recipeIngredientsQuery = _context.RecipeIngredients
                .Include(r => r.Ingredient)
                .Include(r => r.Recipe)
                //.OrderBy(r => r.RecipeIngredientId)
                ; 

            var pagedList = await recipeIngredientsQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalItemCount = await recipeIngredientsQuery.CountAsync();

            var model = new PagedList.Core.StaticPagedList<RecipeIngredient>(pagedList, pageNumber, pageSize, totalItemCount);

            return View(model);

            /*
            var db9093Context = _context.RecipeIngredients.Include(r => r.Ingredient).Include(r => r.Recipe);
            return View(await db9093Context.ToListAsync());
            */
        }

        // GET: RecipeIngredients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipeIngredient = await _context.RecipeIngredients
                .Include(r => r.Ingredient)
                .Include(r => r.Recipe)
                .FirstOrDefaultAsync(m => m.RecipeIngredientId == id);
            if (recipeIngredient == null)
            {
                return NotFound();
            }

            return View(recipeIngredient);
        }

        // GET: RecipeIngredients/Create
        public IActionResult Create()
        {
            ViewData["IngredientId"] = new SelectList(_context.Ingredients, "IngredientId", "IngredientId");
            ViewData["RecipeId"] = new SelectList(_context.Recipes, "RecipeId", "RecipeId");
            return View();
        }

        // POST: RecipeIngredients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RecipeIngredientId,RecipeId,IngredientId,Quantity")] RecipeIngredient recipeIngredient)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recipeIngredient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IngredientId"] = new SelectList(_context.Ingredients, "IngredientId", "IngredientId", recipeIngredient.IngredientId);
            ViewData["RecipeId"] = new SelectList(_context.Recipes, "RecipeId", "RecipeId", recipeIngredient.RecipeId);
            return View(recipeIngredient);
        }

        // GET: RecipeIngredients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipeIngredient = await _context.RecipeIngredients.FindAsync(id);
            if (recipeIngredient == null)
            {
                return NotFound();
            }
            ViewData["IngredientId"] = new SelectList(_context.Ingredients, "IngredientId", "IngredientId", recipeIngredient.IngredientId);
            ViewData["RecipeId"] = new SelectList(_context.Recipes, "RecipeId", "RecipeId", recipeIngredient.RecipeId);
            return View(recipeIngredient);
        }

        // POST: RecipeIngredients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RecipeIngredientId,RecipeId,IngredientId,Quantity")] RecipeIngredient recipeIngredient)
        {
            if (id != recipeIngredient.RecipeIngredientId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recipeIngredient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeIngredientExists(recipeIngredient.RecipeIngredientId))
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
            ViewData["IngredientId"] = new SelectList(_context.Ingredients, "IngredientId", "IngredientId", recipeIngredient.IngredientId);
            ViewData["RecipeId"] = new SelectList(_context.Recipes, "RecipeId", "RecipeId", recipeIngredient.RecipeId);
            return View(recipeIngredient);
        }

        // GET: RecipeIngredients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipeIngredient = await _context.RecipeIngredients
                .Include(r => r.Ingredient)
                .Include(r => r.Recipe)
                .FirstOrDefaultAsync(m => m.RecipeIngredientId == id);
            if (recipeIngredient == null)
            {
                return NotFound();
            }

            return View(recipeIngredient);
        }

        // POST: RecipeIngredients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recipeIngredient = await _context.RecipeIngredients.FindAsync(id);
            if (recipeIngredient != null)
            {
                _context.RecipeIngredients.Remove(recipeIngredient);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecipeIngredientExists(int id)
        {
            return _context.RecipeIngredients.Any(e => e.RecipeIngredientId == id);
        }
    }
}
