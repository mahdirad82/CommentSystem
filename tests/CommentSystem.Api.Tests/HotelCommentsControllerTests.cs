using System.Net;
using System.Net.Http.Json;
using CommentSystem.Application.DTOs;
using FluentAssertions;
using Moq;
using CommentSystem.Application.Interfaces;
using CommentSystem.Domain.Entities;
using CommentSystem.Domain.Enums;

namespace CommentSystem.Api.Tests;

public class HotelCommentsControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly Mock<ICommentRepository> _commentRepositoryMock;

    public HotelCommentsControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _commentRepositoryMock = factory.CommentRepositoryMock;

        // Reset mocks before each test to ensure isolation
        _commentRepositoryMock.Invocations.Clear();
    }

    [Fact]
    public async Task GetApprovedCommentsForHotel_ReturnsOkWithComments_WhenCommentsExist()
    {
        // Arrange
        var hotelId = 1;
        var comments = new List<Comment>
        {
            new Comment { Id = 1, Content = "Hotel Comment 1", Booking = new Booking { HotelId = hotelId }, Status = CommentStatus.Approved },
            new Comment { Id = 2, Content = "Hotel Comment 2", Booking = new Booking { HotelId = hotelId }, Status = CommentStatus.Approved }
        };
        _commentRepositoryMock.Setup(r => r.GetByHotelIdAndStatusAsync(hotelId, CommentStatus.Approved))
            .ReturnsAsync(comments);

        // Act
        var response = await _client.GetAsync($"/api/hotels/{hotelId}/comments");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var publicComments = await response.Content.ReadFromJsonAsync<IEnumerable<PublicCommentDto>>();
        publicComments.Should().NotBeNull().And.HaveCount(2);
    }

    [Fact]
    public async Task GetApprovedCommentsForHotel_ReturnsNotFound_WhenNoCommentsExist()
    {
        // Arrange
        var hotelId = 1;
        _commentRepositoryMock.Setup(r => r.GetByHotelIdAndStatusAsync(hotelId, CommentStatus.Approved))
            .ReturnsAsync(new List<Comment>());

        // Act
        var response = await _client.GetAsync($"/api/hotel/comments/{hotelId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
