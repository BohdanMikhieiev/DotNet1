namespace LINQToObjects;

public class Room
{
    public int Id { get; set; }
    public int HotelId { get; set; }
    public string Number { get; set; }
    public double Price { get; set; }
    public Hotel? Hotel { get; set; }
}