using System.Text.Json;

namespace LINQToObjects.JSON;

public class JsonDoc
{
    
    public static void ReadHotelsJsonWithJsonDocument(string filePath)
        {
            string jsonString = File.ReadAllText(filePath);

            using JsonDocument doc = JsonDocument.Parse(jsonString);
            JsonElement root = doc.RootElement;

            Console.WriteLine("Choose what to show:\n1 - Hotels\n2 - Rooms by Hotel\n3 - Bookings by Room");

            switch (Console.ReadLine())
            {
                case "1":
                    ShowHotels(root);
                    break;
                case "2":
                    ShowRoomsByHotel(root);
                    break;
                case "3":
                    ShowBookingsByRoom(root);
                    break;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }

    private static void ShowHotels(JsonElement root)
    {
        MenuHolding.MakeHeader("Hotels");
        foreach (var hotel in root.GetProperty("Hotels").EnumerateArray())
        {
            Console.WriteLine($"\t ===== {hotel.GetProperty("Name")} ===== \t");
            Console.WriteLine($"ID: {hotel.GetProperty("Id")}");
            Console.WriteLine($"Location: {hotel.GetProperty("City")}, {hotel.GetProperty("Country")}");
            Console.WriteLine($"Rating: {hotel.GetProperty("Rating")}\n{new string('-', 40)}");
        }
    }

    private static void ShowRoomsByHotel(JsonElement root)
    {
        Console.WriteLine("Enter Hotel ID:");
        string inputId = Console.ReadLine();

        foreach (var hotel in root.GetProperty("Hotels").EnumerateArray())
        {
            if (hotel.GetProperty("Id").ToString() == inputId)
            {
                Console.WriteLine($"\nRooms in {hotel.GetProperty("Name")}:\n");
                foreach (var room in hotel.GetProperty("Rooms").EnumerateArray())
                {
                    Console.WriteLine($"Room ID: {room.GetProperty("Id")}\n\t\u2192 Number: {room.GetProperty("Number")}\n\t\u2192 Price: {room.GetProperty("Price")}");
                }
                return;
            }
        }
        Console.WriteLine("Hotel not found.");
    }

    private static void ShowBookingsByRoom(JsonElement root)
    {
        Console.WriteLine("Enter Hotel ID:");
        string hotelId = Console.ReadLine();

        Console.WriteLine("Enter Room ID:");
        string roomId = Console.ReadLine();

        foreach (var hotel in root.GetProperty("Hotels").EnumerateArray())
        {
            if (hotel.GetProperty("Id").ToString() == hotelId)
            {
                foreach (var room in hotel.GetProperty("Rooms").EnumerateArray())
                {
                    if (room.GetProperty("Id").ToString() == roomId)
                    {
                        Console.WriteLine($"\nBookings for Room {room.GetProperty("Number")}:\n");
                        foreach (var booking in room.GetProperty("Bookings").EnumerateArray())
                        {
                            string date = booking.GetProperty("Date").GetString();
                            double rating = booking.GetProperty("ClientRating").GetDouble();
                            string clientName = booking.GetProperty("Client").GetProperty("Name").GetString();
                            Console.WriteLine($"\n\t\u2192 Client: {clientName}\n\t\u2192  Date: {date}\n\t\u2192  Rating: {rating}");
                        }
                        return;
                    }
                }
            }
        }
        Console.WriteLine("Hotel or Room not found.");
    }
}