using System;
using System.Collections.Generic;

namespace courseA4.Models;

public partial class Recipe
{
    public int RecipeId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int CookingTime { get; set; }

    public string DifficultyLevel { get; set; } = null!;

    public int UserId { get; set; }

    public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();

    public virtual ICollection<RecipeStep> RecipeSteps { get; set; } = new List<RecipeStep>();

    public virtual User? User { get; set; }
}
