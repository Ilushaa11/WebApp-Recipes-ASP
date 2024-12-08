using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace courseA4.Models
{
    public class RecipeCreateViewModel
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Range(1, 1440, ErrorMessage = "Время приготволения не может быть больше 24 часов(1440 минут) или менее 1 минуты.")]
        public int CookingTime { get; set; }

        public string DifficultyLevel { get; set; }

        public List<RecipeStepViewModel> Steps { get; set; } = new List<RecipeStepViewModel>();

        public List<IngredientViewModel> Ingredients { get; set; } = new List<IngredientViewModel>();
    }

    public class RecipeStepViewModel
    {
        public string Description { get; set; }
    }

    public class IngredientViewModel
    {
        public string Name { get; set; }
        public string Quantity { get; set; }
    }
}
