namespace CommentSystem.Domain.Entities;

public class Booking
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int HotelId { get; set; }
    public DateTime BookingDate { get; set; }

    // Optional relation
    public Comment? Comment { get; set; }
}