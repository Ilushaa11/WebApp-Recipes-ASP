using System;
using System.Collections.Generic;

namespace courseA4.Models;

public partial class IngredientType
{
    public int TypeId { get; set; }

    public string? TypeName { get; set; } 

    public virtual ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
}
