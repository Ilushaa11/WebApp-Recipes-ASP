using System;
using System.Collections.Generic;

namespace courseA4.Models;

public partial class RecipeDetail
{
    public int RecipeId { get; set; }

    public string? RecipeName { get; set; } 

    public string? RecipeDescription { get; set; } 

    public int CookingTime { get; set; }

    public string? DifficultyLevel { get; set; } 

    public int StepNumber { get; set; }

    public string? StepDescription { get; set; } 

    public string? IngredientName { get; set; }

    public string? IngredientQuantity { get; set; }
}
