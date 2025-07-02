using System.Net;
using System.Net.Http.Json;
using CommentSystem.Application.DTOs;
using FluentAssertions;
using Moq;
using CommentSystem.Application.Interfaces;
using CommentSystem.Domain.Entities;
using CommentSystem.Domain.Enums;

namespace CommentSystem.Api.Tests;

public class UserCommentsControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly Mock<ICommentRepository> _commentRepositoryMock;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;

    public UserCommentsControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _commentRepositoryMock = factory.CommentRepositoryMock;
        _currentUserServiceMock = factory.CurrentUserServiceMock;

        // Reset mocks before each test to ensure isolation
        _commentRepositoryMock.Invocations.Clear();
        _currentUserServiceMock.Invocations.Clear();

        // Default setup for current user
        _currentUserServiceMock.Setup(s => s.UserId).Returns(1);
        _currentUserServiceMock.Setup(s => s.Role).Returns(UserRole.User);
    }

    [Fact]
    public async Task CreateComment_ReturnsCreated_WhenCommentIsValid()
    {
        // Arrange
        var createCommentDto = new CreateCommentDto(1, "Test Content", 5);
        _commentRepositoryMock
            .Setup(r => r.GetBookingAvailableForCommentAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(new Booking { Id = 1, UserId = 1, HotelId = 1 });

        // Act
        var response = await _client.PostAsJsonAsync("/api/user/comments", createCommentDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        _commentRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Comment>()), Times.Once);
        _commentRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task GetUserComments_ReturnsOkWithComments_WhenCommentsExist()
    {
        // Arrange
        var userId = 1;
        var comments = new List<Comment>
        {
            new Comment { Id = 1, Content = "User Comment 1", Booking = new Booking { UserId = userId } },
            new Comment { Id = 2, Content = "User Comment 2", Booking = new Booking { UserId = userId } }
        };
        _commentRepositoryMock.Setup(r => r.GetByUserIdAsync(userId))
            .ReturnsAsync(comments);

        // Act
        var response = await _client.GetAsync($"/api/user/comments");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var userComments = await response.Content.ReadFromJsonAsync<IEnumerable<UserCommentDto>>();
        userComments.Should().NotBeNull().And.HaveCount(2);
    }

    [Fact]
    public async Task GetUserComments_ReturnsNotFound_WhenNoCommentsExist()
    {
        // Arrange
        var userId = 1;
        _commentRepositoryMock.Setup(r => r.GetByUserIdAsync(userId))
            .ReturnsAsync(new List<Comment>());

        // Act
        var response = await _client.GetAsync($"/api/user/comments");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}