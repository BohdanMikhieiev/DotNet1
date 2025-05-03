namespace LINQToObjects.LINQ_to_XML;

public class Program
{
    private static void Main()
    {
        var hotels = Seeder.Hotels;
        var clients = Seeder.Clients;
        var rooms = Seeder.Rooms;
        var bookings = Seeder.Bookings;

        Seeder.AssignHotelsToRooms();
        Seeder.AssignClientsToBookings();
        Seeder.AssignRoomsToBookings();

        // XmlImport.ExportHotelsToXml();

        MenuHolding.StartMenuHolding();
    }
}