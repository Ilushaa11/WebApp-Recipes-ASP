using System;
using System.Collections.Generic;

namespace courseA4.Models;

public partial class RecipeStep
{
    public int StepId { get; set; }

    public int RecipeId { get; set; }

    public int StepNumber { get; set; }

    public string? Description { get; set; }

    public int? IngredientId { get; set; }

    public string? Quantity { get; set; }

    public virtual Ingredient? Ingredient { get; set; }

    public virtual Recipe? Recipe { get; set; } 
}
