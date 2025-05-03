namespace LINQToObjects;

public class Seeder
{
    public static List<Hotel> Hotels = new()
    {
        new Hotel { Id = 1, Name = "Grand Hotel", Country = "France", City = "Paris", Rating = 4.5 },
        new Hotel { Id = 2, Name = "Ocean View", Country = "USA", City = "Miami", Rating = 4.2 },
        new Hotel { Id = 3, Name = "Mountain Lodge", Country = "Switzerland", City = "Zermatt", Rating = 4.8 },
        new Hotel { Id = 4, Name = "Le Royal Monceau", Country = "France", City = "Paris", Rating = 4.9 },
        new Hotel { Id = 5, Name = "The Ritz", Country = "UK", City = "London", Rating = 4.7 },
        new Hotel { Id = 6, Name = "Skyline Hotel", Country = "USA", City = "New York", Rating = 4.3 },
        new Hotel { Id = 7, Name = "Palm Resort", Country = "UAE", City = "Dubai", Rating = 4.6 },
        new Hotel { Id = 8, Name = "City Lights Hotel", Country = "USA", City = "New York", Rating = 4.1 },
        new Hotel { Id = 9, Name = "Beach Paradise", Country = "Spain", City = "Barcelona", Rating = 4.4 },
        new Hotel { Id = 10, Name = "Historic Inn", Country = "Italy", City = "Rome", Rating = 4.0 },
        new Hotel { Id = 11, Name = "Test", Country = "2", City = "2", Rating = 4.0 }
    };

    public static List<Client> Clients = new()
    {
        new Client { Id = 1, Name = "John Doe" },
        new Client { Id = 2, Name = "Jane Smith" },
        new Client { Id = 3, Name = "Emily Johnson" },
        new Client { Id = 4, Name = "Michael Brown" },
        new Client { Id = 5, Name = "David Wilson" },
        new Client { Id = 6, Name = "Sophia Davis" },
        new Client { Id = 7, Name = "Chris Evans" },
        new Client { Id = 8, Name = "Natalie Portman" },
        new Client { Id = 9, Name = "Robert Downey" },
        new Client { Id = 10, Name = "Scarlett Johansson" },
        new Client { Id = 11, Name = "John Hotel" }
    };

    public static List<Room> Rooms = new()
    {
        new Room { Id = 1, HotelId = 1, Number = "101", Price = 100 },
        new Room { Id = 2, HotelId = 2, Number = "202", Price = 150 },
        new Room { Id = 3, HotelId = 3, Number = "303", Price = 200 },
        new Room { Id = 4, HotelId = 4, Number = "411", Price = 430 },
        new Room { Id = 5, HotelId = 5, Number = "505", Price = 350 },
        new Room { Id = 6, HotelId = 6, Number = "606", Price = 220 },
        new Room { Id = 7, HotelId = 7, Number = "707", Price = 500 },
        new Room { Id = 8, HotelId = 8, Number = "808", Price = 180 },
        new Room { Id = 9, HotelId = 1, Number = "102", Price = 110 },
        new Room { Id = 10, HotelId = 2, Number = "203", Price = 140 },
        new Room { Id = 11, HotelId = 9, Number = "909", Price = 300 },
        new Room { Id = 12, HotelId = 10, Number = "1010", Price = 270 }
    };

    public static List<Booking> Bookings = new()
    {
        new Booking { Id = 1, ClientId = 1, RoomId = 1, Date = DateTime.Now.AddMonths(-1), ClientRating = 4.0 },
        new Booking { Id = 2, ClientId = 1, RoomId = 2, Date = DateTime.Now.AddMonths(-3), ClientRating = 2.5 },
        new Booking { Id = 3, ClientId = 1, RoomId = 3, Date = DateTime.Now.AddMonths(-4), ClientRating = 1.8 },
        new Booking { Id = 4, ClientId = 2, RoomId = 3, Date = DateTime.Now.AddMonths(-8), ClientRating = 4.5 },
        new Booking { Id = 5, ClientId = 2, RoomId = 4, Date = DateTime.Now.AddMonths(-4), ClientRating = 3.2 },
        new Booking { Id = 6, ClientId = 3, RoomId = 5, Date = DateTime.Now.AddMonths(-2), ClientRating = 4.8 },
        new Booking { Id = 7, ClientId = 4, RoomId = 7, Date = DateTime.Now.AddMonths(-6), ClientRating = 2.0 },
        new Booking { Id = 8, ClientId = 5, RoomId = 9, Date = DateTime.Now.AddMonths(-7), ClientRating = 3.9 },
        new Booking { Id = 9, ClientId = 6, RoomId = 10, Date = DateTime.Now.AddMonths(-9), ClientRating = 1.0 },
        new Booking { Id = 10, ClientId = 7, RoomId = 11, Date = DateTime.Now.AddMonths(-12), ClientRating = 4.7 },
        new Booking { Id = 11, ClientId = 8, RoomId = 12, Date = DateTime.Now.AddMonths(-3), ClientRating = 4.5 },
        new Booking { Id = 12, ClientId = 9, RoomId = 6, Date = DateTime.Now.AddMonths(-10), ClientRating = 2.2 },
        new Booking { Id = 13, ClientId = 10, RoomId = 8, Date = DateTime.Now.AddMonths(-12), ClientRating = 3.8 },
        new Booking { Id = 14, ClientId = 1, RoomId = 1, Date = DateTime.Now.AddMonths(-6), ClientRating = 4.1 },
        new Booking { Id = 15, ClientId = 2, RoomId = 2, Date = DateTime.Now.AddMonths(-8), ClientRating = 3.0 },
        new Booking { Id = 16, ClientId = 9, RoomId = 8, Date = DateTime.Now.AddMonths(-1), ClientRating = 2.5 },
        new Booking { Id = 15, ClientId = 7, RoomId = 11, Date = DateTime.Now.AddMonths(-9), ClientRating = 3.1 },
        new Booking { Id = 16, ClientId = 7, RoomId = 11, Date = DateTime.Now.AddMonths(-8), ClientRating = 3.5 },
        new Booking { Id = 17, ClientId = 7, RoomId = 11, Date = DateTime.Now.AddMonths(-7), ClientRating = 4.2 },
        new Booking { Id = 18, ClientId = 8, RoomId = 11, Date = DateTime.Now.AddMonths(-5), ClientRating = 4.7 },
        new Booking { Id = 19, ClientId = 1, RoomId = 1, Date = new DateTime(2025, 4, 10), ClientRating = 4.2 },
        new Booking { Id = 20, ClientId = 2, RoomId = 9, Date = new DateTime(2025, 4, 15), ClientRating = 4.0 },
        new Booking { Id = 21, ClientId = 3, RoomId = 9, Date = new DateTime(2025, 4, 20), ClientRating = 3.8 },
        new Booking { Id = 22, ClientId = 4, RoomId = 1, Date = new DateTime(2025, 4, 25), ClientRating = 4.5 },
        new Booking { Id = 23, ClientId = 5, RoomId = 1, Date = new DateTime(2025, 4, 25), ClientRating = 3.9 },
        new Booking { Id = 24, ClientId = 6, RoomId = 1, Date = new DateTime(2025, 3, 5), ClientRating = 4.1 },
        new Booking { Id = 25, ClientId = 6, RoomId = 9, Date = new DateTime(2025, 3, 5), ClientRating = 4.0 },
        new Booking { Id = 26, ClientId = 6, RoomId = 3, Date = new DateTime(2025, 3, 5), ClientRating = 3.7 },
        new Booking { Id = 27, ClientId = 7, RoomId = 2, Date = new DateTime(2025, 2, 10), ClientRating = 3.5 },
        new Booking { Id = 28, ClientId = 7, RoomId = 3, Date = new DateTime(2025, 2, 10), ClientRating = 3.6 },
        new Booking { Id = 29, ClientId = 8, RoomId = 8, Date = new DateTime(2025, 1, 20), ClientRating = 4.8 },
        new Booking { Id = 30, ClientId = 8, RoomId = 9, Date = new DateTime(2025, 1, 20), ClientRating = 4.9 },
        new Booking { Id = 31, ClientId = 10, RoomId = 4, Date = new DateTime(2025, 4, 5), ClientRating = 4.0 },
        new Booking { Id = 32, ClientId = 3, RoomId = 5, Date = new DateTime(2025, 4, 7), ClientRating = 3.7 },
        new Booking { Id = 33, ClientId = 4, RoomId = 6, Date = new DateTime(2025, 4, 10), ClientRating = 4.2 },
        new Booking { Id = 34, ClientId = 5, RoomId = 7, Date = new DateTime(2025, 4, 15), ClientRating = 3.9 },
        new Booking { Id = 35, ClientId = 6, RoomId = 8, Date = new DateTime(2025, 4, 18), ClientRating = 4.3 },
        new Booking { Id = 36, ClientId = 10, RoomId = 1, Date = new DateTime(2025, 3, 1), ClientRating = 4.0 },
        new Booking { Id = 37, ClientId = 10, RoomId = 2, Date = new DateTime(2025, 3, 1), ClientRating = 4.1 },
        new Booking { Id = 38, ClientId = 10, RoomId = 3, Date = new DateTime(2025, 3, 1), ClientRating = 3.9 },
        new Booking { Id = 39, ClientId = 10, RoomId = 4, Date = new DateTime(2025, 3, 1), ClientRating = 4.2 },
        new Booking { Id = 40, ClientId = 10, RoomId = 5, Date = new DateTime(2025, 3, 1), ClientRating = 4.3 },
        new Booking { Id = 41, ClientId = 9, RoomId = 6, Date = new DateTime(2025, 2, 5), ClientRating = 3.8 },
        new Booking { Id = 42, ClientId = 9, RoomId = 7, Date = new DateTime(2025, 2, 5), ClientRating = 3.9 },
        new Booking { Id = 43, ClientId = 9, RoomId = 8, Date = new DateTime(2025, 2, 5), ClientRating = 4.0 },
        new Booking { Id = 44, ClientId = 8, RoomId = 10, Date = new DateTime(2025, 1, 20), ClientRating = 4.5 },
        new Booking { Id = 45, ClientId = 8, RoomId = 11, Date = new DateTime(2025, 1, 20), ClientRating = 4.6 },
        new Booking { Id = 46, ClientId = 7, RoomId = 1, Date = new DateTime(2025, 2, 10), ClientRating = 3.5 },
        new Booking { Id = 47, ClientId = 7, RoomId = 2, Date = new DateTime(2025, 2, 15), ClientRating = 3.4 },
        new Booking { Id = 48, ClientId = 7, RoomId = 3, Date = new DateTime(2025, 2, 17), ClientRating = 3.6 }
    };

    public static void AssignHotelsToRooms()
    {
        foreach (var room in Rooms) room.Hotel = Hotels.FirstOrDefault(h => h.Id == room.HotelId);
    }

    public static void AssignClientsToBookings()
    {
        foreach (var booking in Bookings) booking.Client = Clients.FirstOrDefault(c => c.Id == booking.ClientId);
    }

    public static void AssignRoomsToBookings()
    {
        foreach (var booking in Bookings) booking.Room = Rooms.FirstOrDefault(r => r.Id == booking.RoomId);
    }
}