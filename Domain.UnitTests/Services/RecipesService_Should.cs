using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Contracts.Enums;
using Domain.Models;
using Domain.Services;
using FluentAssertions;
using Moq;
using Persistence.Filters;
using Persistence.Models.ReadModels;
using Persistence.Models.WriteModels;
using Persistence.Repositories;
using TestHelpers.Attributes;
using Xunit;

namespace Domain.UnitTests.Services
{
    public class RecipesService_Should
    {
        [Theory, AutoMoqData]
        public async Task GetAllRecipes_When_RecipesFilterIsPassed(
            RecipesFilter recipesFilter,
            List<RecipeReadModel> recipes,
            [Frozen] Mock<IRecipesRepository> recipesRepositoryMock,
            RecipeService sut)
        {
            // Arrange
            recipesRepositoryMock
                .Setup(recipeRepository => recipeRepository.GetAll(recipesFilter))
                .ReturnsAsync(recipes);
            
            // Act
            var result = (await sut.GetAllAsync(recipesFilter)).ToList();

            // Assert
            recipesRepositoryMock.Verify(recipesRepository => recipesRepository.GetAll(recipesFilter), Times.Once);
            
            result.Should().BeEquivalentTo(recipes, options => options.ComparingByMembers<Recipe>());
        }
        
        [Theory, AutoMoqData]
        public async Task CreateAsync_When_RecipeModel_Is_Passed(
            Recipe recipe,
            [Frozen] Mock<IRecipesRepository> recipesRepositoryMock,
            [Frozen] Mock<IDescriptionsRepository> descriptionRepositoryMock,
            RecipeService sut)
        {
            // Act
            await sut.CreateAsync(recipe);
        
            // Assert
            recipesRepositoryMock
                .Verify(recipesRepository => recipesRepository.SaveAsync(It.Is<RecipeWriteModel>(model => 
                model.Id == recipe.Id &&
                model.Name == recipe.Name &&
                model.Difficulty == recipe.Difficulty &&
                model.TimeToComplete == recipe.TimeToComplete &&
                model.DateCreated == recipe.DateCreated)), Times.Once);
            
            descriptionRepositoryMock
                .Verify(descriptionRepository => descriptionRepository.SaveAsync(It.Is<DescriptionWriteModel>(model => 
                    model.RecipeId == recipe.Id &&
                    model.Description == recipe.Description)), Times.Once);
        }
        
        [Theory, AutoMoqData]
        public async Task EditAsync_When_UpdatedRecipeParametersArePassed(
            int id,
            string name,
            string description,
            [Frozen] Mock<IRecipesRepository> recipesRepositoryMock,
            [Frozen] Mock<IDescriptionsRepository> descriptionRepositoryMock,
            RecipeService sut)
        {
            // Act
            await sut.EditAsync(id, name, description);
            
            // Assert
            recipesRepositoryMock
                .Verify(recipeRepository => recipeRepository.EditNameAsync(id, name), Times.Once);
            
            descriptionRepositoryMock
                .Verify(descriptionRepository => descriptionRepository.EditDescriptionAsync(id,description), Times.Once);
        }
        
        [Theory, AutoMoqData]
        public async Task DeleteByIdAsync_When_RecipeId_Is_Passed(
            int id,
            [Frozen] Mock<IRecipesRepository> recipesRepositoryMock,
            [Frozen] Mock<IDescriptionsRepository> descriptionRepositoryMock,
            RecipeService sut)
        {
            // Act
            await sut.DeleteByIdAsync(id);
            
            // Arrange
            recipesRepositoryMock
                .Verify(recipeRepository => recipeRepository.DeleteByIdAsync(id), Times.Once);
            
            descriptionRepositoryMock
                .Verify(descriptionRepository => descriptionRepository.DeleteByIdAsync(id), Times.Once);
        }
    }
}