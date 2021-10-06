using System;
using Contracts.Enums;
using Domain.Extensions;
using Persistence.Models.ReadModels;
using Xunit;

namespace Domain.UnitTests.Extensions
{
    public class RecipeReadModelMappers_Should
    {
        private readonly Random _random = new Random();

        [Fact]
        public void ReturnRecipe_When_RecipeReadModel_Is_Passed()
        {
            // Arrange
            var recipeReadModel = GenerateRecipeReadModel();

            // Act
            var result = recipeReadModel.MapToRecipe();
            
            // Assert
            Assert.Equal(recipeReadModel.Id, result.Id);
            Assert.Equal(recipeReadModel.Name, result.Name);
            Assert.Equal(recipeReadModel.Description, result.Description);
            Assert.Equal(recipeReadModel.Difficulty, result.Difficulty);
            Assert.Equal(recipeReadModel.TimeToComplete, result.TimeToComplete);
            Assert.Equal(recipeReadModel.DateCreated, result.DateCreated);
        }
        
        private RecipeReadModel GenerateRecipeReadModel()
        {
            return new RecipeReadModel
            {
                Id = _random.Next(0, int.MaxValue),
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Difficulty = (Difficulty) _random.Next(0, 4),
                TimeToComplete = TimeSpan.FromMinutes(_random.Next(0, int.MaxValue)),
                DateCreated = DateTime.Now
            };
        }
    }
}