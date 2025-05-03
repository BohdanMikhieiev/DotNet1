using System.Xml;

namespace LINQToObjects.LINQ_to_XML;

public static class ReadXmlDocument
{
    public static void ReadHotels(XmlDocument doc)
    {
        var hotelNodes = doc.DocumentElement?.SelectNodes("Hotel");

        if (hotelNodes == null || hotelNodes.Count == 0)
            throw new ArgumentException("No hotels found in the XML file.");

        foreach (XmlNode hotelNode in hotelNodes)
        {
            var id = hotelNode.Attributes?["Id"]?.Value ?? "unknown";
            var name = hotelNode.Attributes?["Name"]?.Value ?? "unknown";
            var country = hotelNode.Attributes?["Country"]?.Value ?? "unknown";
            var city = hotelNode.Attributes?["City"]?.Value ?? "unknown";
            var rating = hotelNode.Attributes?["Rating"]?.Value ?? "unknown";

            MenuHolding.MakeHeader(name);
            Console.WriteLine($"Hotel ID: {id}, Country: {country}, City: {city}, Rating: {rating}");

            var roomNodes = hotelNode.SelectNodes("Rooms/Room");
            if (roomNodes != null)
                foreach (XmlNode roomNode in roomNodes)
                {
                    var roomId = roomNode.Attributes?["Id"]?.Value ?? "unknown";
                    var roomNumber = roomNode.Attributes?["Number"]?.Value ?? "unknown";
                    var price = roomNode.Attributes?["Price"]?.Value ?? "unknown";

                    Console.WriteLine($"  Room ID: {roomId}, Number: {roomNumber}, Price: {price}");

                    var bookingNodes = roomNode.SelectNodes("Bookings/Booking");
                    if (bookingNodes != null)
                        foreach (XmlNode bookingNode in bookingNodes)
                        {
                            var bookingId = bookingNode.Attributes?["Id"]?.Value ?? "unknown";
                            var date = bookingNode.Attributes?["Date"]?.Value ?? "unknown";
                            var nights = bookingNode.Attributes?["Nights"]?.Value ?? "unknown";
                            var clientRating = bookingNode.Attributes?["ClientRating"]?.Value ?? "unknown";
                            var clientNode = bookingNode.SelectSingleNode("Client");

                            var clientId = clientNode?.Attributes?["Id"]?.Value ?? "unknown";
                            var clientName = clientNode?.Attributes?["Name"]?.Value ?? "unknown";

                            Console.WriteLine($"    Booking ID: {bookingId}, Date: {date}, Nights: {nights}");
                            Console.WriteLine(
                                $"      Client ID: {clientId}, Name: {clientName}, Rating: {clientRating}");
                        }
                }
        }
    }

    public static void ReadRooms(XmlDocument doc)
    {
        var hotelNodes = doc.DocumentElement?.SelectNodes("Hotel");

        if (hotelNodes == null || hotelNodes.Count == 0)
            throw new ArgumentException("No hotels found in the XML file.");

        var foundRooms = false;

        foreach (XmlNode hotelNode in hotelNodes)
        {
            var hotelId = hotelNode.Attributes?["Id"]?.Value ?? "unknown";

            var roomNodes = hotelNode.SelectNodes("Rooms/Room");
            if (roomNodes != null && roomNodes.Count > 0)
            {
                foundRooms = true;
                Console.WriteLine($"Hotel ID: {hotelId}");

                foreach (XmlNode roomNode in roomNodes)
                {
                    var roomId = roomNode.Attributes?["Id"]?.Value ?? "unknown";
                    var number = roomNode.Attributes?["Number"]?.Value ?? "unknown";
                    var price = roomNode.Attributes?["Price"]?.Value ?? "unknown";

                    Console.WriteLine($"\tRoom ID: {roomId}");
                    Console.WriteLine($"\tNumber: {number}");
                    Console.WriteLine($"\tPrice: {price}");
                    Console.WriteLine("\t---------");
                }

                Console.WriteLine(new string('=', 20));
            }
        }

        if (!foundRooms)
            throw new ArgumentException("No rooms found in the XML file.");
    }

    public static void ReadGuests(XmlDocument doc)
    {
        Console.WriteLine("Enter hotel ID:");
        var hotelId = Console.ReadLine();

        Console.WriteLine("Enter room ID:");
        var roomId = Console.ReadLine();
        var hotelNode = doc.DocumentElement?
            .SelectSingleNode($"Hotel[@Id='{hotelId}']");

        if (hotelNode == null)
            throw new ArgumentException($"Hotel with ID {hotelId} not found.");

        var roomNodes = hotelNode.SelectNodes("Rooms/Room");
        var foundGuests = false;

        foreach (XmlNode roomNode in roomNodes)
        {
            var currentRoomId = roomNode.Attributes?["Id"]?.Value ?? "unknown";

            if (roomId != null && currentRoomId != roomId)
                continue; // Пропускаємо нецікаву кімнату

            var bookingNodes = roomNode.SelectNodes("Bookings/Booking");

            foreach (XmlNode bookingNode in bookingNodes)
            {
                var clientNode = bookingNode.SelectSingleNode("Client");
                if (clientNode != null)
                {
                    foundGuests = true;
                    var guestId = clientNode.Attributes?["Id"]?.Value ?? "unknown";
                    var name = clientNode.Attributes?["Name"]?.Value ?? "unknown";

                    Console.WriteLine($"Room ID: {currentRoomId}");
                    Console.WriteLine($"\tGuest ID: {guestId}");
                    Console.WriteLine(
                        $"\tName: {name}\n\tBooked on: {bookingNode.Attributes?["Date"]?.Value}\n{new string('-', 30)}");
                }
            }
        }

        if (!foundGuests)
            Console.WriteLine("No guests found for the specified hotel and room.");
    }


    public static void ReadBookings(XmlDocument doc)
    {
        Console.WriteLine("Enter hotel ID:");
        var hotelId = Console.ReadLine();

        Console.WriteLine("Enter room ID:");
        var roomId = Console.ReadLine();

        var hotelNode = doc.DocumentElement?
            .SelectSingleNode($"Hotel[@Id='{hotelId}']");

        if (hotelNode == null)
            throw new ArgumentException($"Hotel with ID {hotelId} not found.");

        var roomNodes = hotelNode.SelectNodes("Rooms/Room");
        var foundBookings = false;

        foreach (XmlNode roomNode in roomNodes)
        {
            var currentRoomId = roomNode.Attributes?["Id"]?.Value ?? "unknown";

            if (roomId != null && currentRoomId != roomId)
                continue;

            var bookingNodes = roomNode.SelectNodes("Bookings/Booking");

            foreach (XmlNode bookingNode in bookingNodes)
            {
                foundBookings = true;
                var bookingId = bookingNode.Attributes?["Id"]?.Value ?? "unknown";
                var date = bookingNode.Attributes?["Date"]?.Value ?? "unknown";
                var nights = bookingNode.Attributes?["Nights"]?.Value ?? "unknown";

                var clientRating = bookingNode.Attributes?["ClientRating"]?.Value ?? "unknown";

                var clientNode = bookingNode.SelectSingleNode("Client");
                var clientId = clientNode?.Attributes?["Id"]?.Value ?? "unknown";
                var clientName = clientNode?.Attributes?["Name"]?.Value ?? "unknown";

                Console.WriteLine(
                    $"Room ID: {currentRoomId}\n\tBooking ID: {bookingId}\n\tDate: {date}\n\tNights: {nights}\n\tClient Rating: {clientRating}\n\tClient ID: {clientId}\n\tClient Name: {clientName}\n{new string('-', 30)}");
            }
        }

        if (!foundBookings)
            Console.WriteLine("No bookings found for the specified hotel and room.");
    }
}