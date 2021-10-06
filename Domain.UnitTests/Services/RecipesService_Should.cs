using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Enums;
using Domain.Models;
using Domain.Services;
using Moq;
using Persistence.Filters;
using Persistence.Models.ReadModels;
using Persistence.Models.WriteModels;
using Persistence.Repositories;
using Xunit;

namespace Domain.UnitTests.Services
{
    public class RecipesService_Should
    {
        private readonly Random _random = new Random();
        private readonly Mock<IRecipesRepository> _recipesRepositoryMock;
        private readonly Mock<IDescriptionsRepository> _descriptionRepositoryMock;
        private readonly RecipeService _sut;

        public RecipesService_Should()
        {
            _recipesRepositoryMock = new Mock<IRecipesRepository>();
            _descriptionRepositoryMock = new Mock<IDescriptionsRepository>();
            _sut = new RecipeService(_recipesRepositoryMock.Object, _descriptionRepositoryMock.Object);
        }
        
        [Fact]
        public async Task GetAllRecipes_When_RecipesFilterIsPassed()
        {
            // Arrange
            var recipesFilter = new RecipesFilter
            {
                OrderBy = Guid.NewGuid().ToString(),
                OrderHow = Guid.NewGuid().ToString()
            };
            
            var recipes = new List<RecipeReadModel>
            {
                GenerateRecipeReadModel(),
                GenerateRecipeReadModel(),
                GenerateRecipeReadModel()
            };

            _recipesRepositoryMock
                .Setup(recipeRepository => recipeRepository.GetAll(recipesFilter))
                .ReturnsAsync(recipes);
            
            // Act
            var result = (await _sut.GetAllAsync(recipesFilter)).ToList();

            // Assert
            _recipesRepositoryMock.Verify(recipesRepository => recipesRepository.GetAll(recipesFilter), Times.Once);

            Assert.Equal(recipes.Count, result.Count);
            
            for (var i = 0; i < result.Count; i++)
            {
                Assert.Equal(recipes[i].Id, result[i].Id);
                Assert.Equal(recipes[i].Name, result[i].Name);
                Assert.Equal(recipes[i].Description, result[i].Description);
                Assert.Equal(recipes[i].Difficulty, result[i].Difficulty);
                Assert.Equal(recipes[i].TimeToComplete, result[i].TimeToComplete);
                Assert.Equal(recipes[i].DateCreated, result[i].DateCreated);
            }
        }
        
        [Fact]
        public async Task CreateAsync_When_RecipeModel_Is_Passed()
        {
            // Arrange
            var recipe = GenerateRecipeModel();
            
            // Act
            await _sut.CreateAsync(recipe);
        
            // Assert
            _recipesRepositoryMock
                .Verify(recipesRepository => recipesRepository.SaveAsync(It.Is<RecipeWriteModel>(model => 
                model.Id == recipe.Id &&
                model.Name == recipe.Name &&
                model.Difficulty == recipe.Difficulty &&
                model.TimeToComplete == recipe.TimeToComplete &&
                model.DateCreated == recipe.DateCreated)), Times.Once);
            
            _descriptionRepositoryMock
                .Verify(descriptionRepository => descriptionRepository.SaveAsync(It.Is<DescriptionWriteModel>(model => 
                    model.RecipeId == recipe.Id &&
                    model.Description == recipe.Description)), Times.Once);
        }

        [Fact]
        public async Task EditAsync_When_UpdatedRecipeParametersArePassed()
        {
            // Arrange
            var id = _random.Next(0, int.MaxValue);
            var name = Guid.NewGuid().ToString();
            var description = Guid.NewGuid().ToString();
            
            // Act
            await _sut.EditAsync(id, name, description);
            
            // Assert
            _recipesRepositoryMock
                .Verify(recipeRepository => recipeRepository.EditNameAsync(id, name), Times.Once);
            
            _descriptionRepositoryMock
                .Verify(descriptionRepository => descriptionRepository.EditDescriptionAsync(id,description), Times.Once);
        }

        [Fact]
        public async Task DeleteByIdAsync_When_RecipeId_Is_Passed()
        {
            // Arrange
            var id = _random.Next(0, int.MaxValue);
            
            // Act
            await _sut.DeleteByIdAsync(id);
            
            // Arrange
            _recipesRepositoryMock
                .Verify(recipeRepository => recipeRepository.DeleteByIdAsync(id), Times.Once);
            
            _descriptionRepositoryMock
                .Verify(descriptionRepository => descriptionRepository.DeleteByIdAsync(id), Times.Once);
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
        
        private Recipe GenerateRecipeModel()
        {
            return new Recipe
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