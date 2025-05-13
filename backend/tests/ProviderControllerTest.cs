using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieCompare.Backend.Controllers;
using MovieCompare.Backend.Models;
using MovieCompare.Backend.Services;
using Xunit;

namespace MovieCompare.Backend.Tests
{
    public class ProvidersControllerTests
    {
        private readonly Mock<IMovieService> _movieService;
        private readonly ProvidersController _controller;

        public ProvidersControllerTests()
        {
            _movieService = new Mock<IMovieService>();
            _controller = new ProvidersController(_movieService.Object);
        }

        [Fact]
        public async Task GetMoviesByProvider_ReturnsHealthyPagedResult_WhenServiceSucceeds()
        {
            const string providerId = "cinemaworld";
            var summaries = Enumerable.Range(1, 5)
                .Select(i => new MovieSummaryDto
                {
                    Id = $"id{i}",
                    Title = $"Title{i}",
                    Year = 2000 + i,
                    Type = "movie",
                    Poster = $"poster{i}.jpg",
                    ProviderId = providerId
                })
                .ToList();

            _movieService
                .Setup(s => s.GetMovieSummaries(providerId, false))
                .ReturnsAsync(summaries);

            var actionResult = await _controller.GetMoviesByProvider(providerId);

            var ok = Assert.IsType<OkObjectResult>(actionResult.Result);
            dynamic value = ok.Value;

            Assert.Equal(5, (int)value.total);

            var dto = value.ProviderDto;
            Assert.Equal(providerId, (string)dto.ProviderId);
            Assert.True((bool)dto.IsHealthy);

            var returnedSummaries = ((IEnumerable<dynamic>)dto.MovieSummaries).ToList();
            Assert.Equal(5, returnedSummaries.Count);

            Assert.Equal("Title1", (string)returnedSummaries[0].Title);
            Assert.Equal("id1", (string)returnedSummaries[0].Id);
        }
    }
}
