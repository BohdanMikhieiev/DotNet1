using System.Text.Json;
using System.Text.Json.Nodes;

namespace LINQToObjects.JSON;

public static class JsonNodeExample
{
    public static void MenuJsonNode()
    {
        Console.WriteLine("What do you want to do?\n1 - Add\t2 - Show");
        switch (Console.ReadLine())
        {
            case "1":
                Console.WriteLine("You can add next:\n1 - Hotel\t2 - Room\t3 - Booking");
                switch (Console.ReadLine())
                {
                    case "1":
                        AddHotel();
                        break;
                    case "2":
                        AddRoom();
                        break;
                    case "3":
                        AddBooking();
                        break;
                    default:
                        Console.WriteLine("Invalid option!");
                        return;
                }
                break;
            
            case "2":
                Console.WriteLine("You can show next:\n1 - Hotel\t2 - Room\t3 - Booking");
                switch (Console.ReadLine())
                {
                    case "1":
                        PrintHotels();
                        break;
                    case "2":
                        PrintRooms();
                        break;
                    case "3":
                        PrintBookings();
                        break;

                    default:
                        Console.WriteLine("Invalid option!");
                        return;
                }
                break;
            default:
                Console.WriteLine("Invalid option!");
                return;
        }
    }

    private static void AddHotel()
    {
        JsonNode root = LoadOrCreateRoot();
        JsonArray hotelsArray = root["Hotels"]!.AsArray();

        Console.WriteLine("To add hotel you need to write next:\nId: ");
        int id = int.Parse(Console.ReadLine() ?? "0");

        // Перевірка унікальності ID
        if (hotelsArray.Any(h => h?["Id"]?.GetValue<int>() == id))
        {
            Console.WriteLine("Hotel with this ID already exists.");
            return;
        }

        Console.WriteLine("Name: ");
        string name = Console.ReadLine() ?? "unknown";
        Console.WriteLine("Country: ");
        string country = Console.ReadLine() ?? "unknown";
        Console.WriteLine("City: ");
        string city = Console.ReadLine() ?? "unknown";
        Console.WriteLine("Rating: ");
        double rating = double.Parse(Console.ReadLine() ?? "0");

        JsonObject newHotel = new JsonObject
        {
            ["Id"] = id,
            ["Name"] = name,
            ["Country"] = country,
            ["City"] = city,
            ["Rating"] = rating,
            ["Rooms"] = new JsonArray()
        };

        hotelsArray.Add(newHotel);

        SaveJson(root);
        Console.WriteLine("Hotel is added.");
    }


    private static void AddRoom()
    {
        Console.WriteLine("For adding new room you need give next:\nHotel ID: ");
        int hotelId = int.Parse(Console.ReadLine());

        JsonNode root = LoadOrCreateRoot();

        JsonObject? hotel = root["Hotels"]!
            .AsArray()
            .OfType<JsonObject>()
            .FirstOrDefault(h => h["Id"]!.GetValue<int>() == hotelId);

        if (hotel == null)
        {
            Console.WriteLine("Hotel isn't found.");
            return;
        }

        JsonArray rooms = hotel["Rooms"]!.AsArray();

        Console.WriteLine("Room ID: ");
        int roomId = int.Parse(Console.ReadLine());

        // Перевірка унікальності room ID
        if (rooms.Any(r => r?["Id"]?.GetValue<int>() == roomId))
        {
            Console.WriteLine("Room with this ID already exists in this hotel.");
            return;
        }

        Console.WriteLine("Number: ");
        string number = Console.ReadLine();

        Console.WriteLine("Price: ");
        int price = int.Parse(Console.ReadLine());

        JsonObject newRoom = new JsonObject
        {
            ["Id"] = roomId,
            ["HotelId"] = hotelId,
            ["Number"] = number,
            ["Price"] = price,
            ["Bookings"] = new JsonArray()
        };

        rooms.Add(newRoom);

        SaveJson(root);
        Console.WriteLine("Room added.");
    }


    private static void AddBooking()
{
    JsonNode root = LoadOrCreateRoot();

    Console.WriteLine("For adding new booking you need give next:\nHotel ID: ");
    int hotelId = int.Parse(Console.ReadLine());

    JsonObject? hotel = root["Hotels"]!
        .AsArray()
        .OfType<JsonObject>()
        .FirstOrDefault(h => h["Id"]!.GetValue<int>() == hotelId);

    if (hotel == null)
    {
        Console.WriteLine("Hotel isn't found.");
        return;
    }

    Console.WriteLine("Room ID: ");
    int roomId = int.Parse(Console.ReadLine());

    JsonObject? room = hotel["Rooms"]!
        .AsArray()
        .OfType<JsonObject>()
        .FirstOrDefault(r => r["Id"]!.GetValue<int>() == roomId);

    if (room == null)
    {
        Console.WriteLine("Room isn't found.");
        return;
    }

    JsonArray bookings = room["Bookings"]!.AsArray();

    Console.WriteLine("Booking ID: ");
    int bookingId = int.Parse(Console.ReadLine());

    // Перевірка унікальності booking ID
    if (bookings.Any(b => b?["Id"]?.GetValue<int>() == bookingId))
    {
        Console.WriteLine("Booking with this ID already exists.");
        return;
    }

    Console.WriteLine("Client ID: ");
    int clientId = int.Parse(Console.ReadLine());

    Console.WriteLine("Client Name: ");
    string clientName = Console.ReadLine();

    // Перевірка відповідності ClientID ↔ ClientName
    bool clientConflict = root["Hotels"]!
        .AsArray()
        .SelectMany(h => h!["Rooms"]!.AsArray())
        .SelectMany(r => r!["Bookings"]!.AsArray())
        .Any(b => b?["Client"]?["Id"]?.GetValue<int>() == clientId &&
                  b?["Client"]?["Name"]?.GetValue<string>() != clientName);

    if (clientConflict)
    {
        Console.WriteLine("Conflict: This Client ID is already used with a different name.");
        return;
    }

    Console.WriteLine("Date (YYYY-MM-DD): ");
    DateTime date = DateTime.Parse(Console.ReadLine());

    Console.WriteLine("Client Rating: ");
    double clientRating = double.Parse(Console.ReadLine() ?? "0");

    JsonObject newBooking = new JsonObject
    {
        ["Id"] = bookingId,
        ["ClientId"] = clientId,
        ["RoomId"] = roomId,
        ["Date"] = date,
        ["ClientRating"] = clientRating,
        ["Client"] = new JsonObject
        {
            ["Id"] = clientId,
            ["Name"] = clientName
        }
    };

    bookings.Add(newBooking);

    SaveJson(root);
    Console.WriteLine("Booking is added.");
}


    private static void PrintHotels()
    {
        JsonNode root = LoadOrCreateRoot();
        JsonArray? hotels = root["Hotels"]?.AsArray();

        if (hotels is null || hotels.Count == 0)
        {
            Console.WriteLine("No hotels found.");
            return;
        }

        foreach (JsonNode? hotel in hotels)
        {
            MenuHolding.MakeHeader((string)hotel?["Name"]);
            Console.WriteLine($"\t\u2192 Hotel ID: {hotel?["Id"]}\n\t\u2192 Country: {hotel?["Country"]}\n\t\u2192 City: {hotel?["City"]}\n\t\u2192 Rating: {hotel?["Rating"]}");
        }
    }

    private static void PrintRooms()
    {
        JsonNode root = LoadOrCreateRoot();
        JsonArray? hotels = root["Hotels"]?.AsArray();

        if (hotels is null || hotels.Count == 0)
        {
            Console.WriteLine("No hotels available.");
            return;
        }

        Console.Write("Enter Hotel ID: ");
        if (!int.TryParse(Console.ReadLine(), out int hotelId))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        JsonNode? hotel = hotels.FirstOrDefault(h => h?["Id"]?.GetValue<int>() == hotelId);

        if (hotel is null)
        {
            Console.WriteLine("Hotel not found.");
            return;
        }

        JsonArray? rooms = hotel["Rooms"]?.AsArray();
        if (rooms is null || rooms.Count == 0)
        {
            Console.WriteLine("No rooms found for this hotel.");
            return;
        }

        foreach (JsonNode? room in rooms)
        {
            Console.WriteLine($"Room ID: {room?["Id"]}\n\t\u2192 Number: {room?["Number"]}\n\t\u2192 Price: {room?["Price"]}");
        }
    }

    private static void PrintBookings()
    {
        JsonNode root = LoadOrCreateRoot();
        JsonArray? hotels = root["Hotels"]?.AsArray();

        if (hotels is null || hotels.Count == 0)
        {
            Console.WriteLine("No hotels available.");
            return;
        }

        Console.Write("Enter Hotel ID: ");
        if (!int.TryParse(Console.ReadLine(), out int hotelId))
        {
            Console.WriteLine("Invalid ID.");
            return;
        }

        JsonNode? hotel = hotels.FirstOrDefault(h => h?["Id"]?.GetValue<int>() == hotelId);

        if (hotel is null)
        {
            Console.WriteLine("Hotel not found.");
            return;
        }

        JsonArray? rooms = hotel["Rooms"]?.AsArray();
        if (rooms is null || rooms.Count == 0)
        {
            Console.WriteLine("No rooms for this hotel.");
            return;
        }

        foreach (JsonNode? room in rooms)
        {
            JsonArray? bookings = room?["Bookings"]?.AsArray();
            if (bookings is null) continue;

            foreach (JsonNode? booking in bookings)
            {
                Console.WriteLine($"Booking ID: {booking?["Id"]}\n\t\u2192 Room: {room?["Number"]}\n\t\u2192 Date: {booking?["Date"]}, " +
                                  $"\n\t\u2192 Client: {booking?["Client"]?["Name"]}\n\t\u2192 Rating: {booking?["ClientRating"]}");
            }
        }
    }
    
    
    private static JsonNode LoadOrCreateRoot()
    {
        if (File.Exists(JsonSerialize.filePath))
        {
            return JsonNode.Parse(File.ReadAllText(JsonSerialize.filePath))!;
        }

        return new JsonObject
        {
            ["Hotels"] = new JsonArray()
        };
    }

    private static void SaveJson(JsonNode root)
    {
        File.WriteAllText(JsonSerialize.filePath, root.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));
    }

}