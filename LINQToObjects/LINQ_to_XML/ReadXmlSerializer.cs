using System.Diagnostics;
using System.Globalization;
using System.Xml.Serialization;

namespace LINQToObjects.LINQ_to_XML;

[XmlRoot("Hotels")]
public class HotelsModel
{
    [XmlElement("Hotel")] public List<Hotel>? Hotels { get; set; }
}

public class Hotel
{
    [XmlAttribute("Id")] public int Id { get; set; }

    [XmlAttribute("Name")] public string? Name { get; set; }

    [XmlAttribute("Country")] public string? Country { get; set; }

    [XmlAttribute("City")] public string? City { get; set; }

    [XmlAttribute("Rating")] public double Rating { get; set; }

    [XmlArray("Rooms")]
    [XmlArrayItem("Room")]
    public List<Room>? Rooms { get; set; }
}

public class Room
{
    [XmlAttribute("Id")] public int Id { get; set; }

    [XmlAttribute("Number")] public int Number { get; set; }

    [XmlAttribute("Price")] public double Price { get; set; }

    [XmlArray("Bookings")]
    [XmlArrayItem("Booking")]
    public List<Booking>? Bookings { get; set; }
}

public class Booking
{
    [XmlAttribute("Id")] public int Id { get; set; }

    [XmlAttribute("Date")] public string? Date { get; set; }

    [XmlAttribute("Nights")] public string? Nights { get; set; }

    [XmlAttribute("ClientRating")] public double ClientRating { get; set; }

    [XmlElement("Client")] public Client? Client { get; set; }
}

public class Client
{
    [XmlAttribute("Id")] public int Id { get; set; }

    [XmlAttribute("Name")] public string? Name { get; set; }
}

public static class ReadXmlSerializer
{
    public static HotelsModel? LoadHotelsFromXml(string filePath)
    {
        var serializer = new XmlSerializer(typeof(HotelsModel));
        using var fs = new FileStream(filePath, FileMode.Open);
        return (HotelsModel)serializer.Deserialize(fs)!;
    }

    public static void ReadHotels(HotelsModel? model)
    {
        Debug.Assert(model?.Hotels != null, "model.Hotels != null");
        foreach (var hotel in model.Hotels)
        {
            MenuHolding.MakeHeader(hotel.Name!);
            Console.WriteLine(
                $"Hotel ID: {hotel.Id}\nCity: {hotel.City}\nCountry: {hotel.Country}\nRating: {hotel.Rating}\n{new string('-', 50)}");
        }
    }

    public static void ReadRooms(HotelsModel? model)
    {
        Debug.Assert(model?.Hotels != null, "model.Hotels != null");

        foreach (var hotel in model.Hotels)
        {
            Console.WriteLine($"Hotel ID: {hotel.Id}".PadRight(25));

            if (hotel.Rooms != null)
                foreach (var room in hotel.Rooms)
                {
                    Console.WriteLine($"\tRoom ID: {room.Id.ToString(),-5}");
                    Console.WriteLine($"\tNumber:  {room.Number.ToString(),-5}");
                    Console.WriteLine(
                        $"\tPrice:   {room.Price.ToString(CultureInfo.CurrentCulture),-5}\n\t{new string('-', 10)}");
                }

            Console.WriteLine(new string('=', 15));
        }
    }


    public static void ReadClientsByHotelAndRoom(HotelsModel? model)
    {
        Console.Write("Enter hotel ID: ");
        var hotelId = int.Parse(Console.ReadLine() ?? "0");

        Console.Write("Enter room ID: ");
        var roomId = int.Parse(Console.ReadLine() ?? "0");

        var room = model?.Hotels?
            .FirstOrDefault(h => h.Id == hotelId)?
            .Rooms?.FirstOrDefault(r => r.Id == roomId);

        if (room?.Bookings == null)
        {
            Console.WriteLine("No clients found.");
            return;
        }

        foreach (var booking in room.Bookings)
        {
            Debug.Assert(booking.Client != null, "booking.Client != null");
            Console.WriteLine(
                $"Client ID: {booking.Client.Id}, Name: {booking.Client.Name}, Booked on: {booking.Date}");
        }
    }

    public static void ReadBookingsByHotelAndRoom(HotelsModel? model)
    {
        Console.Write("Enter hotel ID: ");
        var hotelId = int.Parse(Console.ReadLine() ?? "0");

        Console.Write("Enter room ID: ");
        var roomId = int.Parse(Console.ReadLine() ?? "0");

        var room = model?.Hotels?
            .FirstOrDefault(h => h.Id == hotelId)?
            .Rooms?.FirstOrDefault(r => r.Id == roomId);

        if (room?.Bookings == null)
        {
            Console.WriteLine("No bookings found.");
            return;
        }

        foreach (var booking in room.Bookings)
            Console.WriteLine(
                $"Booking ID: {booking.Id}, Date: {booking.Date}, Nights: {booking.Nights}, ClientRating: {booking.ClientRating}");
    }
}