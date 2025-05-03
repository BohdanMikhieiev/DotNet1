using System.Xml;

namespace LINQToObjects.LINQ_to_XML;

public static class MenuHolding
{
    private const string BaseDirectory = "xml";
    private static readonly XmlDocument Doc = new();

    public static void StartMenuHolding()
    {
        Console.WriteLine(
            "What do you want to try?\n1 - Read from .xml file\n2 - Write in .xml file (using XmlWriter)\n3 - Use LINQ To XML");
        switch (Console.ReadLine())
        {
            case "1":
                Console.WriteLine("Okey, choose how to read it: \n1 - XmlDocument\n2 - XmlSerializer");
                switch (Console.ReadLine())
                {
                    case "1":
                        DisplayFIle();
                        MenuCwHolding();
                        break;
                    case "2":
                        MenuSerializerCwHolding();
                        break;
                }

                break;
            case "2":
                MakeHeader("Write in .xml file (using XmlWriter)");
                MenuWrite();
                break;
            case "3":
                MakeHeader("LINQ To XML");
                MenuLinq();
                break;
        }
    }

    private static void DisplayFIle()
    {
        var file = "booking";

        Console.WriteLine("Reading from existing .xml file\n\nWhich file you want to use?\n1 - Example\n2 - Custom");
        switch (Console.ReadLine())
        {
            case "1":
                Doc.Load($"xml\\{file}.xml");
                break;
            case "2":
                Console.WriteLine("Enter file name:");
                Doc.Load($"{Path.Combine(BaseDirectory, Console.ReadLine()?.Trim() ?? string.Empty)}.xml");
                break;
            default:
                Console.WriteLine("Wrong input");
                break;
        }

        Console.Clear();
    }

    private static void MenuCwHolding()
    {
        var menuOptions = new Dictionary<string, Action>
        {
            { "1", () => ReadXmlDocument.ReadHotels(Doc) },
            { "2", () => ReadXmlDocument.ReadRooms(Doc) },
            { "3", () => ReadXmlDocument.ReadGuests(Doc) },
            { "4", () => ReadXmlDocument.ReadBookings(Doc) }
        };

        Console.WriteLine("Choose what to display:\n1 - Hotels\n2 - Rooms\n3 - Guests\n4 - Booking\n");

        menuOptions[Console.ReadLine() ?? "1"].Invoke();

        Console.ReadKey();
        Console.Clear();
    }

    private static void MenuSerializerCwHolding()
    {
        Console.Write("Enter file name (without .xml): ");
        var fileName = Console.ReadLine()?.Trim() ?? "";
        var path = Path.Combine(BaseDirectory, fileName + ".xml");

        var model = ReadXmlSerializer.LoadHotelsFromXml(path);

        var menuOptions = new Dictionary<string, Action>
        {
            { "1", () => ReadXmlSerializer.ReadHotels(model) },
            { "2", () => ReadXmlSerializer.ReadRooms(model) },
            { "3", () => ReadXmlSerializer.ReadClientsByHotelAndRoom(model) },
            { "4", () => ReadXmlSerializer.ReadBookingsByHotelAndRoom(model) }
        };

        Console.WriteLine(
            "Choose what to display:\n1 - Hotels\n2 - Rooms\n3 - Clients\n4 - Bookings");

        menuOptions[Console.ReadLine() ?? "1"].Invoke();

        Console.ReadKey();
        Console.Clear();
    }

    private static void MenuWrite()
    {
        string path;
        Console.WriteLine("Which file you want to use?\n1 - Example\n2 - Custom");
        switch (Console.ReadLine())
        {
            case "1":
                path = Path.Combine(BaseDirectory, "booking.xml");
                MenuWriteElement(path);
                break;
            case "2":
                Console.WriteLine("If you want to create new file or open existing write its name: ");
                path = Path.Combine(BaseDirectory, string.Concat(Console.ReadLine()?.Trim(), ".xml"));
                XmlWrite.EnsureXmlFileHasRoot(path);
                MenuWriteElement(path);
                break;
            default:
                Console.WriteLine("Wrong input");
                break;
        }

        Console.Clear();
    }

    private static void MenuWriteElement(string path)
    {
        var menuOptions = new Dictionary<string, Action>
        {
            { "1", () => XmlWrite.AddHotel(path) },
            { "2", () => XmlWrite.AddRoom(path) },
            { "3", () => XmlWrite.AddBooking(path) }
        };

        Console.WriteLine(
            "What do you want to write?\n1 - Hotel\n2 - Room\n3 - Booking");

        menuOptions[Console.ReadLine() ?? "1"].Invoke();
    }

    private static void MenuLinq()
    {
        var menuOptions = new Dictionary<string, Action>
        {
            { "1", LinqToXml.TopHotelsByGuestsPerMonth },
            { "2", LinqToXml.ClientsWithMostSimultaneousBookings },
            { "3", LinqToXml.FilterHotelsInFrance },
            { "4", LinqToXml.GroupByClients },
            { "5", LinqToXml.JoinHotelsAndRooms },
            { "6", LinqToXml.GetAverageClientRating },
            { "7", LinqToXml.GetAverageRoomPricePerHotel },
            { "8", LinqToXml.GetSortedBookingsWithHighRatings },
            { "9", LinqToXml.GetHotelsInUSAAndUK },
            { "10", LinqToXml.GetClientsDictionary },
            { "11", LinqToXml.GetMostExpensiveRoomPerHotel },
            { "12", LinqToXml.GetHotelsWithHighRepeatBookings },
            { "13", LinqToXml.GetClientsWithAtLeastThreeCountriesLastYear },
            { "14", LinqToXml.GetHotelsWithLostLoyalClients },
            { "15", LinqToXml.GetCityHoppingGuests }
        };

        Console.WriteLine(
            $"Which LINQ you want to try?\n{new string('-', 30)}\nTasks (from 1 to 2)\n" +
            $"{new string('-', 30)}\nCustom (from 3 to 15)\n{new string('-', 30)}\n");

        menuOptions[Console.ReadLine() ?? "1"].Invoke();

        Console.ReadKey();
        Console.Clear();
    }

    public static void MakeHeader(string str)
    {
        Console.WriteLine(string.Concat(new string('-', 55), "\n\t\t", str, "\n", new string('-', 55)));
    }
}