using System.Xml.Linq;

namespace LINQToObjects.LINQ_to_XML;

public static class XmlImport
{
    public static void ExportHotelsToXml(string filePath = "xml/booking.xml")
    {
        var rnd = new Random();

        var xml = new XElement("Hotels",
            Seeder.Hotels.Select(hotel => new XElement("Hotel",
                new XAttribute("Id", hotel.Id),
                new XAttribute("Name", hotel.Name),
                new XAttribute("Country", hotel.Country),
                new XAttribute("City", hotel.City),
                new XAttribute("Rating", hotel.Rating),
                new XElement("Rooms",
                    Seeder.Rooms
                        .Where(r => r.HotelId == hotel.Id)
                        .Select(room => new XElement("Room",
                            new XAttribute("Id", room.Id),
                            new XAttribute("Number", room.Number),
                            new XAttribute("Price", room.Price),
                            new XElement("Bookings",
                                Seeder.Bookings
                                    .Where(b => b.RoomId == room.Id)
                                    .Select(booking => new XElement("Booking",
                                        new XAttribute("Id", booking.Id),
                                        new XAttribute("Date", booking.Date.ToString("yyyy-MM-dd")),
                                        new XAttribute("ClientRating", booking.ClientRating),
                                        new XAttribute("Nights", rnd.Next(1, 22)),
                                        new XElement("Client",
                                            new XAttribute("Id", booking.Client?.Id ?? 0),
                                            new XAttribute("Name", booking.Client?.Name ?? "Unknown")
                                        )
                                    ))
                            )
                        ))
                )
            ))
        );

        xml.Save(filePath);
    }
}