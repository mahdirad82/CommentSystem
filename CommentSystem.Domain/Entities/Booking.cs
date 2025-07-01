namespace CommentSystem.Domain.Entities;

public class Booking
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public int HotelId { get; init; }
    public DateTime BookingDate { get; init; }

    // Optional relation
    public Comment? Comment { get; init; }
}