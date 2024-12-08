using System;
using System.Collections.Generic;

namespace courseA4.Models;

public partial class RecipeIngredient
{
    public int RecipeIngredientId { get; set; }

    public int RecipeId { get; set; }

    public int IngredientId { get; set; }

    public string Quantity { get; set; } = null!;

    public virtual Ingredient? Ingredient { get; set; } 

    public virtual Recipe? Recipe { get; set; } 
}
