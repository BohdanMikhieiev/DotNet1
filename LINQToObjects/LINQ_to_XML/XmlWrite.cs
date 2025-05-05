using System.Xml;
using System.Xml.Serialization;

namespace LINQToObjects.LINQ_to_XML;

public class XmlWrite
{
    public static void AddHotel(string filePath)
    {
        var model = XmlHelper.LoadHotelsModel(filePath);

        Console.Write("Enter hotel ID: ");
        var id = int.Parse(Console.ReadLine());

        if (model.Hotels.Any(h => h.Id == id))
        {
            Console.WriteLine("Hotel with this ID already exists.");
            return;
        }

        Console.Write("Enter hotel name: ");
        var name = Console.ReadLine();

        Console.Write("Enter country: ");
        var country = Console.ReadLine();

        Console.Write("Enter city: ");
        var city = Console.ReadLine();

        Console.Write("Enter rating: ");
        var rating = double.Parse(Console.ReadLine());

        model.Hotels.Add(new Hotel
        {
            Id = id,
            Name = name,
            Country = country,
            City = city,
            Rating = rating,
            Rooms = new List<Room>()
        });

        XmlHelper.SaveHotelsModel(filePath, model);
        Console.WriteLine("Hotel added successfully.");
        Console.ReadLine();
    }

    public static void AddRoom(string filePath)
    {
        var model = XmlHelper.LoadHotelsModel(filePath);

        Console.Write("Enter hotel ID to add room to: ");
        var hotelId = int.Parse(Console.ReadLine());

        var hotel = model.Hotels.FirstOrDefault(h => h.Id == hotelId);
        if (hotel == null)
        {
            Console.WriteLine("Hotel not found.");
            return;
        }

        Console.Write("Enter room ID: ");
        var id = int.Parse(Console.ReadLine());

        if (hotel.Rooms.Any(r => r.Id == id))
        {
            Console.WriteLine("Room with this ID already exists in this hotel.");
            return;
        }

        Console.Write("Enter room number: ");
        var number = int.Parse(Console.ReadLine());

        Console.Write("Enter price: ");
        var price = double.Parse(Console.ReadLine());

        hotel.Rooms.Add(new Room
        {
            Id = id,
            Number = number,
            Price = price,
            Bookings = new List<Booking>()
        });

        XmlHelper.SaveHotelsModel(filePath, model);
        Console.WriteLine("Room added successfully.");
        Console.ReadLine();
    }

    public static void AddBooking(string filePath)
{
    var model = XmlHelper.LoadHotelsModel(filePath);

    Console.Write("Enter hotel ID: ");
    var hotelId = int.Parse(Console.ReadLine());

    var hotel = model.Hotels.FirstOrDefault(h => h.Id == hotelId);
    if (hotel == null)
    {
        Console.WriteLine("Hotel not found.");
        return;
    }

    Console.Write("Enter room ID: ");
    var roomId = int.Parse(Console.ReadLine());

    var room = hotel.Rooms.FirstOrDefault(r => r.Id == roomId);
    if (room == null)
    {
        Console.WriteLine("Room not found.");
        return;
    }

    Console.Write("Enter booking ID: ");
    var bookingId = int.Parse(Console.ReadLine());

    if (room.Bookings.Any(b => b.Id == bookingId))
    {
        Console.WriteLine("Booking with this ID already exists in this room.");
        return;
    }

    Console.Write("Enter booking date (yyyy-MM-dd): ");
    var date = Console.ReadLine();

    Console.Write("Enter count of nights: ");
    var nights = Console.ReadLine();

    Console.Write("Enter client rating: ");
    var rating = double.Parse(Console.ReadLine());

    Console.Write("Enter client ID: ");
    var clientId = int.Parse(Console.ReadLine());

    Console.Write("Enter client name: ");
    var clientName = Console.ReadLine();

    bool isClientConflict = model.Hotels
        .SelectMany(h => h.Rooms)
        .SelectMany(r => r.Bookings)
        .Any(b => b.Client.Id == clientId && b.Client.Name != clientName);

    if (isClientConflict)
    {
        Console.WriteLine("Client ID is already used with a different name.");
        return;
    }

    room.Bookings.Add(new Booking
    {
        Id = bookingId,
        Date = date,
        Nights = nights,
        ClientRating = rating,
        Client = new Client
        {
            Id = clientId,
            Name = clientName
        }
    });

    XmlHelper.SaveHotelsModel(filePath, model);
    Console.WriteLine("Booking added successfully.");
    Console.ReadLine();
}


    public static void EnsureXmlFileHasRoot(string filePath)
    {
        if (!File.Exists(filePath))
        {
            using (var writer = XmlWriter.Create(filePath, new XmlWriterSettings { Indent = true }))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Hotels");
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            Console.WriteLine("New XML file created with root <Hotels>.");
        }
    }
}

public static class XmlHelper
{
    public static HotelsModel LoadHotelsModel(string filePath)
    {
        var serializer = new XmlSerializer(typeof(HotelsModel));
        using (var fs = new FileStream(filePath, FileMode.Open))
        {
            return (HotelsModel)serializer.Deserialize(fs);
        }
    }

    public static void SaveHotelsModel(string filePath, HotelsModel model)
    {
        var serializer = new XmlSerializer(typeof(HotelsModel));
        using (var fs = new FileStream(filePath, FileMode.Create))
        {
            serializer.Serialize(fs, model);
        }
    }
}