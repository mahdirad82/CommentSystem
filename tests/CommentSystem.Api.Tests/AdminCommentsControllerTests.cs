using System.Net;
using System.Net.Http.Json;
using CommentSystem.Application.DTOs;
using FluentAssertions;
using Moq;
using CommentSystem.Application.Interfaces;
using CommentSystem.Domain.Entities;
using CommentSystem.Domain.Enums;

namespace CommentSystem.Api.Tests;

public class AdminCommentsControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly Mock<ICommentRepository> _commentRepositoryMock;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;

    public AdminCommentsControllerTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _commentRepositoryMock = factory.CommentRepositoryMock;
        _currentUserServiceMock = factory.CurrentUserServiceMock;

        // Clear invocations and reset mocks before each test to ensure isolation
        _commentRepositoryMock.Invocations.Clear();
        _currentUserServiceMock.Invocations.Clear();

        // Default setup for admin user
        _currentUserServiceMock.Setup(s => s.Role).Returns(UserRole.Admin);
    }

    [Fact]
    public async Task GetAllSystemComments_ReturnsOkWithComments_WhenCommentsExist()
    {
        // Arrange
        var comments = new List<Comment>
        {
            new Comment { Id = 1, Content = "Admin Comment 1", Status = CommentStatus.Pending },
            new Comment { Id = 2, Content = "Admin Comment 2", Status = CommentStatus.Approved }
        };
        _commentRepositoryMock.Setup(r => r.GetAllAsync(null))
            .ReturnsAsync(comments);

        // Act
        var response = await _client.GetAsync("/api/admin/comments");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var adminComments = await response.Content.ReadFromJsonAsync<IEnumerable<AdminCommentDto>>();
        adminComments.Should().NotBeNull().And.HaveCount(2);
    }

    [Fact]
    public async Task GetAllSystemComments_ReturnsNotFound_WhenNoCommentsExist()
    {
        // Arrange
        _commentRepositoryMock.Setup(r => r.GetAllAsync(null))
            .ReturnsAsync(new List<Comment>());

        // Act
        var response = await _client.GetAsync("/api/admin/comments");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateCommentStatus_ReturnsNoContent_WhenStatusIsUpdatedSuccessfully()
    {
        // Arrange
        var commentId = 1;
        var updateDto = new UpdateCommentStatusDto(CommentStatus.Approved);
        var comment = new Comment { Id = commentId, Content = "Test", Status = CommentStatus.Pending };
        _commentRepositoryMock.Setup(r => r.GetByIdAsync(commentId)).ReturnsAsync(comment);

        // Act
        var response = await _client.PutAsJsonAsync($"/api/admin/comments/{commentId}/status", updateDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        _commentRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateCommentStatus_ReturnsNotFound_WhenCommentDoesNotExist()
    {
        // Arrange
        var commentId = 1;
        var updateDto = new UpdateCommentStatusDto(CommentStatus.Approved);
        _commentRepositoryMock.Setup(r => r.GetByIdAsync(commentId)).ReturnsAsync((Comment)null!);

        // Act
        var response = await _client.PutAsJsonAsync($"/api/admin/comments/{commentId}/status", updateDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        _commentRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task UpdateCommentStatus_ReturnsBadRequest_WhenCommentIsNotPending()
    {
        // Arrange
        var commentId = 1;
        var updateDto = new UpdateCommentStatusDto(CommentStatus.Approved);
        var comment = new Comment { Id = commentId, Content = "Test", Status = CommentStatus.Approved };
        _commentRepositoryMock.Setup(r => r.GetByIdAsync(commentId)).ReturnsAsync(comment);

        // Act
        var response = await _client.PutAsJsonAsync($"/api/admin/comments/{commentId}/status", updateDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        _commentRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task UpdateCommentStatus_ReturnsBadRequest_WhenNewStatusIsInvalid()
    {
        // Arrange
        var commentId = 1;
        var updateDto = new UpdateCommentStatusDto(CommentStatus.Pending);
        var comment = new Comment { Id = commentId, Content = "Test", Status = CommentStatus.Pending };
        _commentRepositoryMock.Setup(r => r.GetByIdAsync(commentId)).ReturnsAsync(comment);

        // Act
        var response = await _client.PutAsJsonAsync($"/api/admin/comments/{commentId}/status", updateDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        _commentRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task AdminCommentsController_ReturnsForbidden_WhenUserIsNotAdmin()
    {
        // Arrange
        _currentUserServiceMock.Setup(s => s.Role).Returns(UserRole.User);

        // Act
        var response = await _client.GetAsync("/api/admin/comments");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
