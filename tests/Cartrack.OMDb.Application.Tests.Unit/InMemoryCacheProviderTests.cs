using AutoFixture;
using Cartrack.OMDb.Application.Caching;
using Cartrack.OMDb.Web.Models.Results.Models;
using FluentAssertions;
using System;
using Xunit;

namespace Cartrack.OMDb.Application.Tests.Unit
{
    public class InMemoryCacheProviderTests
    {
        private readonly InMemoryCacheProvider<TitleResult> _sut;
        private readonly IFixture _fixture = new Fixture();

        public InMemoryCacheProviderTests()
        {
            _sut = new InMemoryCacheProvider<TitleResult>();
        }

        [Fact]
        public void GetAllItems_ShouldBeEmpty_WhenInitialized()
        {
            _sut.GetAllItems().Should().BeEmpty();
        }

        [Fact]
        public void GetAllItems_ShouldHaveACountOfX_WhenXEntriesAreAddedToCache()
        {
            // Arrange
            const string imdbID = "tt0848228";
            const int count = 1;
            var title = _fixture.Create<TitleResult>();


            _sut.AddOrUpdate(imdbID, title);

            _sut.GetAllItems().Should().HaveCount(count, "Only one title was added to the cache.");
        }

        [Fact]
        public void AddOrUpdate_ShouldContainUpdateEntry_WhenEntryIsAddedMoreThanOnceToCached()
        {
            // Arrange
            const string imdbID = "tt0848228";
            var title = _fixture.Create<TitleResult>();

            // Act
            _sut.AddOrUpdate(imdbID, title);

            // Assert
            _sut.GetAllItems().Should().Contain(title);
        }

        [Fact]
        public void AddOrUpdate_ShouldUpdateAnEntry_WhenEntryIsAlreadyCached()
        {
            // Arrange
            const string imdbID = "tt0848228";
            var initialEntry = _fixture.Create<TitleResult>();
            var updatedEntry = _fixture.Create<TitleResult>();
            _sut.AddOrUpdate(imdbID, initialEntry);

            // Act
            _sut.AddOrUpdate(imdbID, updatedEntry);

            // Assert
            _sut.GetAllItems().Should().NotContain(initialEntry);
            _sut.GetAllItems().Should().Contain(updatedEntry);
        }

        [Fact]
        public void AddOrUpdate_ShouldThrowException_WhenCacheKeyIsInvalid()
        {
            // Arrange
            const string imdbID = "";
            var title = _fixture.Create<TitleResult>();

            // Act
            Action act = () => _sut.AddOrUpdate(imdbID, title);

            // Assert
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("Cannot add/update an entry to the cache with null, empty or whitespace key.")
                .WithInnerException<ArgumentException>();
        }

        [Fact]
        public void TryGetValue_ShouldReturnEntry_WhenEntryWithKeyExists()
        {
            // Arrange
            const string imdbID = "tt0848228";
            var title = _fixture.Create<TitleResult>();
            _sut.AddOrUpdate(imdbID, title);

            // Act

            _sut.TryGetValue(imdbID, out var cachedEntry);

            // Assert
            cachedEntry.Should().NotBeNull();
        }

        [Fact]
        public void TryGetValue_ShouldReturnNull_WhenEntryWithKeyNotExists()
        {
            // Arrange
            const string imdbID = "tt0848228";
            const string nonExistingIMDbID = "tt1234567";
            var title = _fixture.Create<TitleResult>();
            _sut.AddOrUpdate(imdbID, title);

            // Act

            _sut.TryGetValue(nonExistingIMDbID, out var cachedEntry);

            // Assert
            cachedEntry.Should().BeNull();
        }

        [Fact]
        public void Delete_ShouldRemoveEntry_WhenEntryWithKeyExists()
        {
            // Arrange
            const string imdbID = "tt0848228";
            var title = _fixture.Create<TitleResult>();
            _sut.AddOrUpdate(imdbID, title);

            // Act
            _sut.Delete(imdbID);

            // Assert
            _sut.GetAllItems().Should().NotContain(title);
        }
    }
}
