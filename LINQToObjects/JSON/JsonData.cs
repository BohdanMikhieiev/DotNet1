using System.Text.Json.Serialization;

namespace LINQToObjects.JSON;

public class HotelsData
{
    public List<HotelNested>? Hotels { get; set; }

    public static List<HotelNested> HotelsJsonModel()
    {
        var hotels = Seeder.Hotels;
        var clients = Seeder.Clients;
        var rooms = Seeder.Rooms;
        var bookings = Seeder.Bookings;

        Seeder.AssignHotelsToRooms();
        Seeder.AssignClientsToBookings();
        Seeder.AssignRoomsToBookings();
        
        var hotelsNested = hotels.Select(hotel => new HotelNested
        {
            Id = hotel.Id,
            Name = hotel.Name,
            Country = hotel.Country,
            City = hotel.City,
            Rating = hotel.Rating,
            Rooms = rooms
                .Where(r => r.HotelId == hotel.Id)
                .Select(room => new RoomNested
                {
                    Id = room.Id,
                    HotelId = room.HotelId,
                    Number = room.Number,
                    Price = room.Price,
                    Bookings = bookings
                        .Where(b => b.RoomId == room.Id)
                        .Select(b => new BookingNested
                        {
                            Id = b.Id,
                            RoomId = b.RoomId,
                            ClientId = b.ClientId,
                            Date = b.Date,
                            ClientRating = b.ClientRating,
                            Client = clients.FirstOrDefault(c => c.Id == b.ClientId)
                        })
                        .ToList()
                })
                .ToList()
        }).ToList();
        return hotelsNested;
    }
}

public class HotelNested : Hotel
{
    [JsonPropertyOrder(1000)]
    public List<RoomNested>? Rooms { get; set; }
}

public class BookingNested : Booking
{
    [JsonPropertyOrder(1000)]
    public new Client? Client { get; set; }
    [JsonIgnore]
    public new Room? Room { get; set; }
}

public class RoomNested : Room
{
    [JsonPropertyOrder(1000)]
    public new List<BookingNested>? Bookings { get; set; }
    [JsonIgnore]
    public new Hotel? Hotel { get; set; }
}