using AutoMapper;
using CommentSystem.Application.DTOs;
using CommentSystem.Application.Interfaces;
using CommentSystem.Application.Mappings;
using CommentSystem.Application.Services;
using CommentSystem.Domain.Entities;
using FluentAssertions;
using Moq;

namespace CommentSystem.Application.Tests;

public class CommentServiceTests
{
    private readonly Mock<ICommentRepository> _commentRepositoryMock;
    private static IMapper _mapper;
    private readonly CommentService _commentService;

    static CommentServiceTests()
    {
        var mapperConfig = new MapperConfiguration(cfg =>
            cfg.AddProfile(new AutoMapperProfile()));
        _mapper = mapperConfig.CreateMapper();
    }

    public CommentServiceTests()
    {
        _commentRepositoryMock = new Mock<ICommentRepository>();
        _commentService = new CommentService(_commentRepositoryMock.Object, _mapper);
    }

    [Fact]
    public async Task CreateCommentAsync_ShouldReturnSuccessResult_WhenCommentIsCreatedSuccessfully()
    {
        // Arrange
        var createCommentDto = new CreateCommentDto(1, "Test Comment", 5);
        var userId = 1;
        _commentRepositoryMock
            .Setup(r => r.GetBookingAvailableForCommentAsync(createCommentDto.BookingId, userId))
            .ReturnsAsync(new Booking());

        // Act
        var result = await _commentService.CreateCommentAsync(createCommentDto, userId);

        // Assert
        result.Should().NotBeNull();
        result.IsFailure.Should().BeFalse();
        _commentRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Comment>()), Times.Once);
        _commentRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateCommentAsync_ShouldReturnFailureResult_WhenBookingIsNotAvailableForComment()
    {
        // Arrange
        var createCommentDto = new CreateCommentDto(1, "Test Comment", 5);
        var userId = 1;
        _commentRepositoryMock
            .Setup(r => r.GetBookingAvailableForCommentAsync(createCommentDto.BookingId, userId))
            .ReturnsAsync((Booking)null!);

        // Act
        var result = await _commentService.CreateCommentAsync(createCommentDto, userId);

        // Assert
        result.Should().NotBeNull();
        result.IsFailure.Should().BeTrue();
        result.Error.Should()
            .Be(
                "This booking either does not exist, does not belong to you, or already has a comment associated with it.");
        _commentRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Comment>()), Times.Never);
        _commentRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task GetApprovedCommentsForHotelAsync_ShouldReturnPublicCommentDtos_WhenCommentsExist()
    {
        // Arrange
        var hotelId = 1;
        var comments = new List<Comment>
        {
            new Comment
            {
                Id = 1, Content = "Comment 1", Booking = new Booking { HotelId = hotelId },
                Status = Domain.Enums.CommentStatus.Approved
            },
            new Comment
            {
                Id = 2, Content = "Comment 2", Booking = new Booking { HotelId = hotelId },
                Status = Domain.Enums.CommentStatus.Approved
            }
        };
        _commentRepositoryMock
            .Setup(r => r.GetByHotelIdAndStatusAsync(hotelId, Domain.Enums.CommentStatus.Approved))
            .ReturnsAsync(comments);

        // Act
        var result = await _commentService.GetApprovedCommentsForHotelAsync(hotelId);

        // Assert
        result.IsFailure.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetApprovedCommentsForHotelAsync_ShouldReturnFailureResult_WhenNoCommentsExist()
    {
        // Arrange
        var hotelId = 1;
        _commentRepositoryMock
            .Setup(r => r.GetByHotelIdAndStatusAsync(hotelId, Domain.Enums.CommentStatus.Approved))
            .ReturnsAsync(new List<Comment>());

        // Act
        var result = await _commentService.GetApprovedCommentsForHotelAsync(hotelId);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("No approved comments found for this hotel.");
    }

    [Fact]
    public async Task GetCommentsForUserAsync_ShouldReturnUserCommentDtos_WhenCommentsExist()
    {
        // Arrange
        var userId = 1;
        var comments = new List<Comment>
        {
            new Comment { Id = 1, Content = "Comment 1", Booking = new Booking { UserId = userId } },
            new Comment { Id = 2, Content = "Comment 2", Booking = new Booking { UserId = userId } }
        };
        _commentRepositoryMock.Setup(r => r.GetByUserIdAsync(userId))
            .ReturnsAsync(comments);

        // Act
        var result = await _commentService.GetCommentsForUserAsync(userId);

        // Assert
        result.IsFailure.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetCommentsForUserAsync_ShouldReturnFailureResult_WhenNoCommentsExist()
    {
        // Arrange
        var userId = 1;
        _commentRepositoryMock.Setup(r => r.GetByUserIdAsync(userId))
            .ReturnsAsync(new List<Comment>());

        // Act
        var result = await _commentService.GetCommentsForUserAsync(userId);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("No comments found for this user.");
    }

    [Fact]
    public async Task GetAllSystemCommentsAsync_ShouldReturnAllComments_WhenStatusIsNull()
    {
        // Arrange
        var comments = new List<Comment>
        {
            new Comment { Id = 1, Content = "Comment 1" },
            new Comment { Id = 2, Content = "Comment 2" }
        };
        _commentRepositoryMock.Setup(r => r.GetAllAsync(null))
            .ReturnsAsync(comments);

        // Act
        var result = await _commentService.GetAllSystemCommentsAsync(null);

        // Assert
        result.IsFailure.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetAllSystemCommentsAsync_ShouldReturnFailureResult_WhenNoCommentsExist()
    {
        // Arrange
        _commentRepositoryMock.Setup(r => r.GetAllAsync(null))
            .ReturnsAsync(new List<Comment>());

        // Act
        var result = await _commentService.GetAllSystemCommentsAsync(null);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("No comments found.");
    }

    [Fact]
    public async Task UpdateCommentStatusAsync_ShouldReturnSuccess_WhenStatusIsUpdated()
    {
        // Arrange
        var commentId = 1;
        var updateDto = new UpdateCommentStatusDto(Domain.Enums.CommentStatus.Approved);
        var comment = new Comment
            { Id = commentId, Content = "Test", Status = Domain.Enums.CommentStatus.Pending };
        _commentRepositoryMock.Setup(r => r.GetByIdAsync(commentId)).ReturnsAsync(comment);

        // Act
        var result = await _commentService.UpdateCommentStatusAsync(commentId, updateDto);

        // Assert
        result.IsFailure.Should().BeFalse();
        comment.Status.Should().Be(Domain.Enums.CommentStatus.Approved);
        _commentRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateCommentStatusAsync_ShouldReturnFailure_WhenCommentNotFound()
    {
        // Arrange
        var commentId = 1;
        var updateDto = new UpdateCommentStatusDto(Domain.Enums.CommentStatus.Approved);
        _commentRepositoryMock.Setup(r => r.GetByIdAsync(commentId)).ReturnsAsync((Comment)null!);

        // Act
        var result = await _commentService.UpdateCommentStatusAsync(commentId, updateDto);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Comment not found.");
    }

    [Fact]
    public async Task UpdateCommentStatusAsync_ShouldReturnFailure_WhenCommentNotPending()
    {
        // Arrange
        var commentId = 1;
        var updateDto = new UpdateCommentStatusDto(Domain.Enums.CommentStatus.Approved);
        var comment = new Comment
            { Id = commentId, Content = "Test", Status = Domain.Enums.CommentStatus.Rejected };
        _commentRepositoryMock.Setup(r => r.GetByIdAsync(commentId)).ReturnsAsync(comment);

        // Act
        var result = await _commentService.UpdateCommentStatusAsync(commentId, updateDto);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Comment is not pending.");
    }

    [Fact]
    public async Task UpdateCommentStatusAsync_ShouldReturnFailure_WhenNewStatusIsInvalid()
    {
        // Arrange
        var commentId = 1;
        var updateDto = new UpdateCommentStatusDto(Domain.Enums.CommentStatus.Pending);
        var comment = new Comment
            { Id = commentId, Content = "Test", Status = Domain.Enums.CommentStatus.Pending };
        _commentRepositoryMock.Setup(r => r.GetByIdAsync(commentId)).ReturnsAsync(comment);

        // Act
        var result = await _commentService.UpdateCommentStatusAsync(commentId, updateDto);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("New Status is not approved or not rejected.");
    }
}