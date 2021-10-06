using Domain.Models;
using Persistence.Models.ReadModels;

namespace Domain.Extensions
{
    public static class RecipeReadModelMappers
    {
        public static Recipe MapToRecipe(this RecipeReadModel model)
        {
            return new Recipe
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Difficulty = model.Difficulty,
                TimeToComplete = model.TimeToComplete,
                DateCreated = model.DateCreated
            };
        }
    }
}