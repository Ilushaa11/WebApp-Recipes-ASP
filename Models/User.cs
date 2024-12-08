using System;
using System.Collections.Generic;

namespace courseA4.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? Login { get; set; } 

    public string? PasswordHash { get; set; } 

    public string? Email { get; set; }

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
}
