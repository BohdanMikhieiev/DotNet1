namespace LINQToObjects.LINQ_to_XML;

public class Program
{
    private static void Main()
    {
        do{
            var hotels = Seeder.Hotels;
            var clients = Seeder.Clients;
            var rooms = Seeder.Rooms;
            var bookings = Seeder.Bookings;

            Seeder.AssignHotelsToRooms();
            Seeder.AssignClientsToBookings();
            Seeder.AssignRoomsToBookings();

            // XmlImport.ExportHotelsToXml();

            MenuHolding.StartMenuHolding();
            Console.WriteLine("If you want to end program, click on Backspace");
        } while (Console.ReadKey().Key != ConsoleKey.Backspace);
    }
}