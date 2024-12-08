using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using courseA4.Data;
using courseA4.Models;
using Microsoft.Data.SqlClient;

namespace courseA4.Controllers
{
    public class RecipesController : Controller
    {
        private readonly CookingRecipeContext _context;

        public RecipesController(CookingRecipeContext context)
        {
            _context = context;
        }

        // GET: Recipes
        [ResponseCache(Duration = 120, Location = ResponseCacheLocation.Client)]
        public async Task<IActionResult> Index(string sortOrder, string searchString, string ingredientFilter, int? page)
        {
            ViewData["NameSortParam"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DifficultySortParam"] = sortOrder == "difficulty" ? "difficulty_desc" : "difficulty";
            ViewData["CookingTimeSortParam"] = sortOrder == "cookingTime" ? "cookingTime_desc" : "cookingTime";
            ViewData["CurrentFilter"] = searchString;
            ViewData["IngredientFilter"] = ingredientFilter;

            var recipesQuery = _context.Recipes
                .Include(r => r.User)
                .Include(r => r.RecipeIngredients) 
                .ThenInclude(ri => ri.Ingredient)
                .AsQueryable();


            if (!string.IsNullOrEmpty(searchString))
            {
                recipesQuery = recipesQuery.Where(r => r.Name.Contains(searchString) || r.DifficultyLevel.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(ingredientFilter))
            {
                recipesQuery = recipesQuery.Where(r => r.RecipeIngredients
                    .Any(ri => ri.Ingredient.Name.Contains(ingredientFilter)));
            }

            recipesQuery = sortOrder switch
            {
                "name_desc" => recipesQuery.OrderByDescending(r => r.Name),
                "difficulty" => recipesQuery.OrderBy(r => r.DifficultyLevel),
                "difficulty_desc" => recipesQuery.OrderByDescending(r => r.DifficultyLevel),
                "cookingTime" => recipesQuery.OrderBy(r => r.CookingTime),
                "cookingTime_desc" => recipesQuery.OrderByDescending(r => r.CookingTime),
                _ => recipesQuery.OrderBy(r => r.Name),
            };

            int pageSize = 10;
            int pageNumber = page ?? 1;

            var pagedRecipes = await recipesQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalItemCount = await recipesQuery.CountAsync();

            var model = new PagedList.Core.StaticPagedList<Recipe>(
                pagedRecipes, pageNumber, pageSize, totalItemCount);

            return View(model);
            /*
            var db9093Context = _context.Recipes.Include(r => r.User);
            return View(await db9093Context.ToListAsync());
            */
        }

        // GET: Recipes/Details/5

        public async Task<IActionResult> Details(int? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }
            
            var recipeDetails = await _context.RecipeDetails
                .Where(rd => rd.RecipeId == id)
                .ToListAsync();

            /*
            if (recipeDetails == null || !recipeDetails.Any())
            {
                return NotFound();
            }
            */

            return View(recipeDetails);
        }

        // GET: Recipes/Create
        public IActionResult Create()
        {
            return View(new RecipeCreateViewModel());
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RecipeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                var recipe = new Recipe
                {
                    Name = model.Name,
                    Description = model.Description,
                    CookingTime = model.CookingTime,
                    DifficultyLevel = model.DifficultyLevel,
                    UserId =  1 //model.UserId  пока 1, потом добавить, что ID пользователя передается
                };

                if (model.Steps != null && model.Steps.Any())
                {
                    recipe.RecipeSteps = model.Steps
                        .Select((s, index) => new RecipeStep
                        {
                            StepNumber = index + 1,
                            Description = s.Description
                        })
                        .ToList();
                }

                if (model.Ingredients != null && model.Ingredients.Any())
                {
                    foreach (var ingredientModel in model.Ingredients)
                    {
                        var existingIngredient = await _context.Ingredients
                            .FirstOrDefaultAsync(i => i.Name == ingredientModel.Name);

                        if (existingIngredient != null)
                        {
                            recipe.RecipeIngredients.Add(new RecipeIngredient
                            {
                                Ingredient = existingIngredient, 
                                Quantity = ingredientModel.Quantity
                            });
                        }
                        else
                        {
                            recipe.RecipeIngredients.Add(new RecipeIngredient
                            {
                                Ingredient = new Ingredient { Name = ingredientModel.Name, TypeId = 6 }, 
                                Quantity = ingredientModel.Quantity
                            });
                        }
                    }
                }

                _context.Recipes.Add(recipe);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(model);
            /*
            if (ModelState.IsValid)
            {
                _context.Add(recipe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", recipe.UserId);
            return View(recipe);*/
        }

        // GET: Recipes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", recipe.UserId);
            return View(recipe);
        }

        // POST: Recipes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RecipeId,Name,Description,CookingTime,DifficultyLevel,UserId")] Recipe recipe)
        {
            if (id != recipe.RecipeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recipe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeExists(recipe.RecipeId))
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
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId", recipe.UserId);
            return View(recipe);
        }

        // GET: Recipes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.RecipeId == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe != null)
            {
                _context.Recipes.Remove(recipe);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecipeExists(int id)
        {
            return _context.Recipes.Any(e => e.RecipeId == id);
        }
    }
}
