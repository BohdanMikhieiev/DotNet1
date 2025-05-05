using System.Text.Json;
using System.Text.Json.Serialization;

namespace LINQToObjects.JSON;

public class JsonSerialize
{
    internal static readonly string filePath = "json/booking.json";
    public static List<Hotel> Hotels { get; set; } = new();
    public static List<Room> Rooms { get; set; } = new();
    public static List<Booking> Bookings { get; set; } = new();
    public static List<Client> Clients { get; set; } = new();
    public static void SaveToJson()
    {
        var data = new HotelsData { Hotels = HotelsData.HotelsJsonModel() };
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        var json = JsonSerializer.Serialize(data, options);
        File.WriteAllText(filePath, json);
        Console.WriteLine("JSON saved.");
    }
    
    public static void LoadFromJson()
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine("File booking.json not found.");
            return;
        }

        string json = File.ReadAllText(filePath);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var data = JsonSerializer.Deserialize<HotelData>(json, options);
        if (data != null)
        {
            Hotels = data.Hotels ?? new List<Hotel>();
            Rooms = data.Rooms ?? new List<Room>();
            Bookings = data.Bookings ?? new List<Booking>();
            Clients = data.Clients ?? new List<Client>();
        }

        Console.WriteLine("All data loaded without problems.\n\nWhat do you want to check?\n1 - Hotels\t2 - Rooms\n3 - Clients\t4 - Booking\n5 - All\t6 - Nothing");
        switch (Console.ReadLine())
        {
            case "1":
                PrintHotels();
                break;
            case "2":
                PrintRooms();
                break;
            case "3":
                PrintClients();
                break;
            case "4":
                PrintBookings();
                break;
            case "5":
                PrintAll();
                break;
            default:
                Console.WriteLine("Oups... Wrong option!");
                break;
        }
    }

    private static void PrintHotels()
    {
        MenuHolding.MakeHeader("Hotels");
        foreach (var hotel in Hotels ?? new List<Hotel>())
        {
            Console.WriteLine($"- {hotel.Name} ({hotel.City}, {hotel.Country}), Rating: {hotel.Rating}");
        }
    }

    private static void PrintRooms()
    {
        MenuHolding.MakeHeader("Rooms");
        foreach (var room in Rooms ?? new List<Room>())
        {
            Console.WriteLine($"- Room {room.Number} in HotelId {room.HotelId}, Price: {room.Price}");
        }
    }

    private static void PrintClients()
    {
        MenuHolding.MakeHeader("Clients");
        foreach (var client in Clients ?? new List<Client>())
        {
            Console.WriteLine($"- {client.Name}    (ID: {client.Id})");
        }
    }

    private static void PrintBookings()
    {
        MenuHolding.MakeHeader("Bookings");
        foreach (var booking in Bookings ?? new List<Booking>())
        {
            Console.WriteLine($"- BookingId {booking.Id} Info:\n\t\u2192 Client {booking.ClientId}\n\t\u2192 Room {booking.RoomId} on {booking.Date:d}\n\t\u2192 Rating: {booking.ClientRating}");
        }
    }

    private static void PrintAll()
    {
        PrintHotels();
        PrintRooms();
        PrintClients();
        PrintBookings();
    }

}


public class HotelData
{
    public  List<Hotel> Hotels { get; set; }
    public  List<Room> Rooms { get; set; }
    public  List<Booking> Bookings { get; set; }
    public  List<Client> Clients { get; set; }
    
}