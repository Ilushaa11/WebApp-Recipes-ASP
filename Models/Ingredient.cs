using System;
using System.Collections.Generic;

namespace courseA4.Models;

public partial class Ingredient
{
    public int IngredientId { get; set; }

    public string? Name { get; set; }

    public int TypeId { get; set; }

    public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();

    public virtual ICollection<RecipeStep> RecipeSteps { get; set; } = new List<RecipeStep>();

    public virtual IngredientType? Type { get; set; } 
}
