using System.Xml.Linq;

namespace LINQToObjects.LINQ_to_XML;


public static class LinqToXml
{
    static XDocument xml = XDocument.Load("xml/booking.xml");
    
    #region TaskLinq
    public static void TopHotelsByGuestsPerMonth()
    {
        Console.Write("Enter month like: YYYY-MM: ");
        var targetMonth = Console.ReadLine();

        var doc = XDocument.Load("xml/booking.xml");

        var hotelStats = doc.Descendants("Hotel")
            .Select(hotel => new
            {
                HotelId = (int)hotel.Attribute("Id")!,
                HotelName = (string)hotel.Attribute("Name")!,
                MonthlyBookings = hotel.Descendants("Booking")
                    .Where(b =>
                    {
                        var dateAttr = (string)b.Attribute("Date")!;
                        if (DateTime.TryParse(dateAttr, out var date)) return date.ToString("yyyy-MM") == targetMonth;
                        return false;
                    })
                    .ToList()
            })
            .Select(h => new
            {
                h.HotelId,
                h.HotelName,
                GuestsInMonth = h.MonthlyBookings.Count,
                AvgNights = h.MonthlyBookings
                    .Select(b => (int?)b.Attribute("Nights"))
                    .Average() ?? 0
            })
            .Where(h => h.GuestsInMonth > 0 && h.AvgNights != 0)
            .OrderByDescending(h => h.GuestsInMonth)
            .ToList();

        if (hotelStats.Count == 0)
        {
            Console.WriteLine("There's no data on this month.");
            return;
        }

        foreach (var h in hotelStats)
        {
            Console.WriteLine($"Hotel ID: {h.HotelId}, Name: {h.HotelName}");
            Console.WriteLine($"  Guests in {targetMonth}: {h.GuestsInMonth}");
            Console.WriteLine($"  Average nights: {h.AvgNights:F2}");
            Console.WriteLine("===================================");
        }
    }


public static void ClientsWithMostSimultaneousBookings()
{
    var doc = XDocument.Load("xml/booking.xml");

    var bookings = doc.Descendants("Booking")
        .Select(b => new
        {
            ClientId = (int?)b.Element("Client")?.Attribute("Id") ?? -1,
            ClientName = (string)b.Element("Client")?.Attribute("Name")! ?? "Unknown",
            StartDate = DateTime.Parse(((string)b.Attribute("Date"))!),
            Nights = (int?)b.Attribute("Nights") ?? 0,
            RoomId = (int?)b.Parent?.Parent?.Attribute("Id") // <-- ось правильно
        })
        .Where(b => b.ClientId > 0 && b.RoomId != null)
        .Select(b => new
        {
            b.ClientId,
            b.ClientName,
            b.RoomId,
            StartDate = b.StartDate,
            EndDate = b.StartDate.AddDays(b.Nights)
        })
        .ToList();

    var clientStats = bookings
        .GroupBy(b => new { b.ClientId, b.ClientName })
        .Select(g =>
        {
            var periods = g.ToList();
            int maxSimultaneous = 0;

            var allDates = periods
                .SelectMany(b => Enumerable.Range(0, (b.EndDate - b.StartDate).Days)
                                           .Select(offset => b.StartDate.AddDays(offset)))
                .Distinct();

            foreach (var date in allDates)
            {
                int simultaneous = periods.Count(b => b.StartDate <= date && b.EndDate > date);
                if (simultaneous > maxSimultaneous)
                    maxSimultaneous = simultaneous;
            }

            double avgSimultaneous = allDates
                .Select(date => periods.Count(b => b.StartDate <= date && b.EndDate > date))
                .DefaultIfEmpty(0)
                .Average();

            return new
            {
                g.Key.ClientId,
                g.Key.ClientName,
                MaxSimultaneous = maxSimultaneous,
                AvgSimultaneous = avgSimultaneous
            };
        })
        .OrderByDescending(c => c.MaxSimultaneous)
        .ToList();

        foreach (var c in clientStats)
        {
            Console.WriteLine($"Client ID: {c.ClientId}, Name: {c.ClientName}");
            Console.WriteLine($"  Max rooms simultaneously: {c.MaxSimultaneous}");
            Console.WriteLine($"  Average rooms per day: {c.AvgSimultaneous:F2}");
            Console.WriteLine("===================================");
        }
}
#endregion

#region CustomLinq

// 1. Фільтрація готелів у Франції з рейтингом вище 4.5
public static void FilterHotelsInFrance(){

    var frenchHotels = xml.Descendants("Hotel")
        .Where(h => (string)h.Attribute("Country")! == "France" && (double)(h.Attribute("Rating") ?? throw new InvalidOperationException()) > 4.5)
        .Select(h => (string)h.Attribute("Name")!)
        .ToList();

    Console.WriteLine("Hotels in France with rating > 4.5:");
    frenchHotels.ForEach(Console.WriteLine);
}

// 2. Групування бронювань за клієнтами
public static void GroupByClients()
{
    var bookingsByClient = xml.Descendants("Booking")
        .GroupBy(b => (string)b.Element("Client")?.Attribute("Name"))
        .Select(g => new {
            Client = g.Key,
            BookingsCount = g.Count(),
            HotelNames = g
                .Select(b => b.Ancestors("Hotel").FirstOrDefault())
                .Where(h => h != null)
                .Select(h => (string)h.Attribute("Name"))
                .Distinct()
        });

    Console.WriteLine("\nBookings by clients:");
    foreach (var item in bookingsByClient)
    {
        Console.WriteLine($"\t- {item.Client}: {item.BookingsCount} bookings in hotels:");
        int i = 1;
        foreach (var hotelName in item.HotelNames)
        {
            Console.WriteLine($"{i++}:{hotelName}");
        }
    }

}

// 3. З'єднання готелів та номерів
public static void JoinHotelsAndRooms()
{
    var hotelRooms = xml.Descendants("Hotel")
        .SelectMany(h => h.Descendants("Room"),
            (h, r) => new {
                HotelName = (string)h.Attribute("Name"),
                RoomNumber = (string)r.Attribute("Number"),
                Price = (decimal)r.Attribute("Price")
            })
        .ToList();

    Console.WriteLine("\nHotel rooms:");
    hotelRooms.ForEach(hr => Console.WriteLine($"{hr.HotelName}, Room {hr.RoomNumber}, Price: {hr.Price}"));
}

// 4. Агрегування – середній рейтинг клієнтів
public static void GetAverageClientRating()
{
    var averageRating = xml.Descendants("Booking")
        .Select(b => (double)b.Attribute("ClientRating"))
        .Average();

    Console.WriteLine($"\nAverage client rating: {averageRating:F2}");
}

// 5. Середня ціна номерів у кожному готелі
public static void GetAverageRoomPricePerHotel()
{
    var averageRoomPrice = xml.Descendants("Hotel")
        .Select(h => new {
            HotelName = (string)h.Attribute("Name"),
            AveragePrice = h.Descendants("Room")
                .Select(r => (decimal)r.Attribute("Price"))
                .DefaultIfEmpty(0)
                .Average()
        })
        .ToList();

    Console.WriteLine("\nAverage room price per hotel:");
    averageRoomPrice.ForEach(hp => Console.WriteLine($"{hp.HotelName}: {hp.AveragePrice:F2}"));
}

// 6. Сортування бронювань за датою та фільтрація низьких рейтингів
public static void GetSortedBookingsWithHighRatings()
{
    var sortedBookings = xml.Descendants("Booking")
        .Where(b => (double)b.Attribute("ClientRating") > 3.0)
        .OrderByDescending(b => DateTime.Parse((string)b.Attribute("Date")))
        .Select(b => new {
            ClientName = (string)b.Element("Client").Attribute("Name"),
            Date = DateTime.Parse((string)b.Attribute("Date")),
            Rating = (double)b.Attribute("ClientRating")
        })
        .ToList();

    Console.WriteLine("\nSorted bookings (rating > 3.0):");
    sortedBookings.ForEach(b => Console.WriteLine($"{b.ClientName}, Date: {b.Date.ToShortDateString()}, Rating: {b.Rating}"));
}

// 7. Об'єднання готелів у США та Великобританії
public static void GetHotelsInUSAAndUK()
{
    var usaUkHotels = xml.Descendants("Hotel")
        .Where(h => (string)h.Attribute("Country") == "USA" || (string)h.Attribute("Country") == "UK")
        .Select(h => (string)h.Attribute("Name"))
        .ToList();

    Console.WriteLine("\nHotels in USA and UK:");
    usaUkHotels.ForEach(Console.WriteLine);
}

// 8. Клієнти у словник
public static void GetClientsDictionary()
{
    var clientsDictionary = xml.Descendants("Client")
        .GroupBy(c => (int)c.Attribute("Id")) // уникнення дублікатів
        .Select(g => g.First())
        .ToDictionary(c => (int)c.Attribute("Id"), c => (string)c.Attribute("Name"));

    Console.WriteLine("\nClients dictionary:");
    foreach (var kvp in clientsDictionary)
    {
        Console.WriteLine($"{kvp.Key}: {kvp.Value}");
    }
}

// 9. Найдорожчий номер у кожному готелі
public static void GetMostExpensiveRoomPerHotel()
{
    var mostExpensiveRooms = xml.Descendants("Hotel")
        .Select(h => new {
            HotelName = (string)h.Attribute("Name"),
            MostExpensiveRoom = h.Descendants("Room")
                .OrderByDescending(r => (decimal)r.Attribute("Price"))
                .FirstOrDefault()
        })
        .Where(h => h.MostExpensiveRoom != null)
        .ToList();

    Console.WriteLine("\nMost expensive room in each hotel:");
    foreach (var item in mostExpensiveRooms)
    {
        Console.WriteLine($"{item.HotelName}: Room {(string)item.MostExpensiveRoom.Attribute("Number")}, Price: {(decimal)item.MostExpensiveRoom.Attribute("Price")}");
    }
}

// 10. Готелі з відсотком повторних бронювань > 50%
public static void GetHotelsWithHighRepeatBookings()
{
    var highRepeatHotels = xml.Descendants("Hotel")
        .Where(h =>
        {
            var hotelRooms = h.Descendants("Room").ToList();
            var hotelBookings = hotelRooms
                .SelectMany(r => r.Element("Bookings")?.Elements("Booking") ?? Enumerable.Empty<XElement>())
                .ToList();

            if (!hotelBookings.Any())
                return false;

            var groupedByClient = hotelBookings
                .GroupBy(b => (int)b.Element("Client").Attribute("Id"));

            var repeatCount = groupedByClient
                .Where(g => g.Count() > 1)
                .Sum(g => g.Count() - 1);

            var total = hotelBookings.Count;
            return (repeatCount / (double)total) > 0.5;
        })
        .Select(h => (string)h.Attribute("Name"))
        .ToList();

    Console.WriteLine("\nHotels with high repeat bookings:");
    highRepeatHotels.ForEach(Console.WriteLine);
}

// 11. Клієнти, які відпочивали в більш ніж у 3 країнах за останній рік
public static void GetClientsWithAtLeastThreeCountriesLastYear()
{
    var oneYearAgo = DateTime.Now.AddYears(-1);

    var bookings = xml.Descendants("Hotel")
        .SelectMany(h => h.Descendants("Room"),
            (h, r) => new {
                Country = (string)h.Attribute("Country"),
                Bookings = r.Element("Bookings")?.Elements("Booking")
                    .Select(b => new {
                        Date = DateTime.Parse((string)b.Attribute("Date")),
                        ClientId = (int)b.Element("Client").Attribute("Id"),
                        ClientName = (string)b.Element("Client").Attribute("Name")
                    }) ?? Enumerable.Empty<dynamic>()
            })
        .SelectMany(hr => hr.Bookings.Select(b => new {
            b.ClientId,
            b.ClientName,
            b.Date,
            Country = hr.Country
        }))
        .Where(b => b.Date >= oneYearAgo)
        .GroupBy(b => b.ClientId)
        .Where(g => g.Select(b => b.Country).Distinct().Count() >= 3)
        .Select(g => g.First().ClientName)
        .ToList();

    Console.WriteLine("\nClients who have vacationed in 3 or more countries in a year:");
    bookings.ForEach(Console.WriteLine);
}

// 12. Готелі, в яких > 30% постійних клієнтів перестали бронювати останні 6 місяців
public static void GetHotelsWithLostLoyalClients()
{
    var now = DateTime.Now;
    var sixMonthsAgo = now.AddMonths(-6);

    var affectedHotels = xml.Descendants("Hotel")
        .Where(h =>
        {
            var rooms = h.Descendants("Room").ToList();
            var bookings = rooms
                .SelectMany(r => r.Element("Bookings")?.Elements("Booking") ?? Enumerable.Empty<XElement>())
                .Select(b => new {
                    ClientId = (int)b.Element("Client").Attribute("Id"),
                    Date = DateTime.Parse((string)b.Attribute("Date"))
                })
                .ToList();

            var loyalClients = bookings
                .GroupBy(b => b.ClientId)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (!loyalClients.Any())
                return false;

            var recentClients = bookings
                .Where(b => b.Date >= sixMonthsAgo)
                .Select(b => b.ClientId)
                .Distinct()
                .ToList();

            var lostClientsCount = loyalClients.Except(recentClients).Count();

            return lostClientsCount > 0.3 * loyalClients.Count;
        })
        .Select(h => new {
            Name = (string)h.Attribute("Name"),
            Rating = (decimal?)h.Attribute("Rating") ?? 0
        })
        .ToList();

    Console.WriteLine("\nHotels with a loss of 30% of customers in the last 6 months:");
    affectedHotels.ForEach(h => Console.WriteLine($"{h.Name}, Rating: {h.Rating:F1}"));
}

// 13. Гості, які змінюють готелі в одному місті і порівняння їхньої оцінки
public static void GetCityHoppingGuests()
{
    var clientBookings = xml.Descendants("Booking")
        .Select(b =>
        {
            var client = b.Element("Client");
            var room = b.Parent;
            var hotel = room?.Parent;

            return new {
                ClientId = (int)client.Attribute("Id"),
                ClientName = (string)client.Attribute("Name"),
                Rating = (double?)b.Attribute("ClientRating") ?? 0.0,
                HotelId = (int)hotel?.Attribute("Id"),
                City = (string)hotel?.Attribute("City")
            };
        })
        .GroupBy(b => b.ClientId)
        .Where(g =>
        {
            var city = g.First().City;
            return g.All(b => b.City == city) &&
                   g.Select(b => b.HotelId).Distinct().Count() == g.Count() &&
                   g.Count() >= 2;
        })
        .Select(g => new {
            ClientName = g.First().ClientName,
            AvgRating = g.Average(b => b.Rating)
        })
        .ToList();

    Console.WriteLine("\nGuests who change hotels within the same city:");
    clientBookings.ForEach(g => Console.WriteLine($"{g.ClientName}, Rating: {g.AvgRating:F2}"));
}


    #endregion

}