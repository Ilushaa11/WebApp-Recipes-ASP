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
    public class RecipeStepsController : Controller
    {
        private readonly CookingRecipeContext _context;

        public RecipeStepsController(CookingRecipeContext context)
        {
            _context = context;
        }

        // GET: RecipeSteps
        public async Task<IActionResult> Index(int ? page)
        {

            int pageSize = 10; 
            int pageNumber = page ?? 1; 

            var recipeStepsQuery = _context.RecipeSteps
                .Include(rs => rs.Ingredient) 
                .Include(rs => rs.Recipe)    
                //.OrderBy(rs => rs.StepNumber)
                ; 

            var pagedSteps = await recipeStepsQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalItemCount = await recipeStepsQuery.CountAsync();

            var model = new PagedList.Core.StaticPagedList<RecipeStep>(
                pagedSteps, pageNumber, pageSize, totalItemCount);

            return View(model);

            /*
            var db9093Context = _context.RecipeSteps.Include(r => r.Ingredient).Include(r => r.Recipe);
            return View(await db9093Context.ToListAsync());
            */
        }

        // GET: RecipeSteps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipeStep = await _context.RecipeSteps
                .Include(r => r.Ingredient)
                .Include(r => r.Recipe)
                .FirstOrDefaultAsync(m => m.StepId == id);
            if (recipeStep == null)
            {
                return NotFound();
            }

            return View(recipeStep);
        }

        // GET: RecipeSteps/Create
        public IActionResult Create()
        {
            ViewData["IngredientId"] = new SelectList(_context.Ingredients, "IngredientId", "IngredientId");
            ViewData["RecipeId"] = new SelectList(_context.Recipes, "RecipeId", "RecipeId");
            return View();
        }

        // POST: RecipeSteps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StepId,RecipeId,StepNumber,Description,IngredientId,Quantity")] RecipeStep recipeStep)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recipeStep);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IngredientId"] = new SelectList(_context.Ingredients, "IngredientId", "IngredientId", recipeStep.IngredientId);
            ViewData["RecipeId"] = new SelectList(_context.Recipes, "RecipeId", "RecipeId", recipeStep.RecipeId);
            return View(recipeStep);
        }

        // GET: RecipeSteps/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipeStep = await _context.RecipeSteps.FindAsync(id);
            if (recipeStep == null)
            {
                return NotFound();
            }
            ViewData["IngredientId"] = new SelectList(_context.Ingredients, "IngredientId", "IngredientId", recipeStep.IngredientId);
            ViewData["RecipeId"] = new SelectList(_context.Recipes, "RecipeId", "RecipeId", recipeStep.RecipeId);
            return View(recipeStep);
        }

        // POST: RecipeSteps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StepId,RecipeId,StepNumber,Description,IngredientId,Quantity")] RecipeStep recipeStep)
        {
            if (id != recipeStep.StepId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recipeStep);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeStepExists(recipeStep.StepId))
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
            ViewData["IngredientId"] = new SelectList(_context.Ingredients, "IngredientId", "IngredientId", recipeStep.IngredientId);
            ViewData["RecipeId"] = new SelectList(_context.Recipes, "RecipeId", "RecipeId", recipeStep.RecipeId);
            return View(recipeStep);
        }

        // GET: RecipeSteps/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipeStep = await _context.RecipeSteps
                .Include(r => r.Ingredient)
                .Include(r => r.Recipe)
                .FirstOrDefaultAsync(m => m.StepId == id);
            if (recipeStep == null)
            {
                return NotFound();
            }

            return View(recipeStep);
        }

        // POST: RecipeSteps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recipeStep = await _context.RecipeSteps.FindAsync(id);
            if (recipeStep != null)
            {
                _context.RecipeSteps.Remove(recipeStep);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecipeStepExists(int id)
        {
            return _context.RecipeSteps.Any(e => e.StepId == id);
        }
    }
}
