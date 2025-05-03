namespace LINQToObjects;

public class Booking
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public int RoomId { get; set; }
    public DateTime Date { get; set; }
    public double ClientRating { get; set; }
    public Room? Room { get; set; }
    public Client? Client { get; set; }
}