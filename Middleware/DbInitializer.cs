
using courseA4.Models;
using courseA4.Data;
using System;
using System.Linq;

namespace courseA4.Middleware
{
    public static class DbInitializer
    {
        public static void Initialize(CookingRecipeContext db)
        {
            db.Database.EnsureCreated();

            if (db.Recipes.Any())
            {
                return;
            }

            int recipesNumber = 20;
            int ingredientsNumber = 50;
            int stepsNumber = 100;

            Random randObj = new(1);

            string[] ingredientTypeNames = { "Фрукты", "Овощи", "Мясо", "Грибы", "Ягоды", "Другое" };
            int ingredientTypeNameCount = ingredientTypeNames.Length;

            for (int i = 1; i <= ingredientTypeNameCount; i++)
            {
                string ingredientTypeName = ingredientTypeNames[i];
                db.IngredientTypes.Add(new IngredientType { TypeId = i, TypeName = ingredientTypeName });
            }
            db.SaveChanges();

            string[] recipeNames = { "Паста_", "Салат_", "Суп_", "Пицца_", "Пирог_", "Гратен_", "Курица_" };
            int recipeNameCount = recipeNames.Length;

            for (int recipeId = 1; recipeId <= recipesNumber; recipeId++)
            {
                string recipeName = recipeNames[randObj.Next(recipeNameCount)] + recipeId;
                string description = "Описание для " + recipeName;
                db.Recipes.Add(new Recipe { Name = recipeName, Description = description });
            }
            db.SaveChanges();

            string[] ingredientNames = { "Помидор", "Сыр", "Молоко", "Яйцо", "Мука", "Мясо", "Капуста", "Картофель", "Лук", "Чеснок" };
            int ingredientNameCount = ingredientNames.Length;

            for (int ingredientId = 1; ingredientId <= ingredientsNumber; ingredientId++)
            {
                string ingredientName = ingredientNames[randObj.Next(ingredientNameCount)];
                db.Ingredients.Add(new Ingredient { Name = ingredientName, TypeId = randObj.Next(1, 6) });
            }
            db.SaveChanges();

            for (int stepId = 1; stepId <= stepsNumber; stepId++)
            {
                int recipeId = randObj.Next(1, recipesNumber + 1);
                string stepDescription = $"Шаг {stepId} для рецепта {recipeId}";
                int stepOrder = randObj.Next(1, 10);
                db.RecipeSteps.Add(new RecipeStep { RecipeId = recipeId, Description = stepDescription, StepId = stepOrder });
            }
            db.SaveChanges();

            for (int recipeIngredientId = 1; recipeIngredientId <= ingredientsNumber; recipeIngredientId++)
            {
                int recipeId = randObj.Next(1, recipesNumber + 1);
                int ingredientId = randObj.Next(1, ingredientsNumber + 1);
                float quantity = (float)(randObj.NextDouble() * 500); 
                db.RecipeIngredients.Add(new RecipeIngredient { RecipeId = recipeId, IngredientId = ingredientId, Quantity = quantity.ToString() });
            }
            db.SaveChanges();
        }
    }
}
