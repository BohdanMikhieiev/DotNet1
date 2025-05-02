using System;
using System.Collections.Generic;
using System.Linq;
using LINQToObjects;

class Program
{
    static void Main()
    {
        var hotels = Seeder.Hotels;
        var clients = Seeder.Clients;
        var rooms = Seeder.Rooms;
        var bookings = Seeder.Bookings;
        Seeder.AssignHotelsToRooms();
        Seeder.AssignClientsToBookings();
        Seeder.AssignRoomsToBookings();
        
         // Фільтрація готелів у Франції з рейтингом вище 4.5
        var frenchHotels = hotels
            .Where(h => h.Country == "France" && h.Rating > 4.5)
            .ToList();
        Console.WriteLine("Hotels in France with rating > 4.5:");
        frenchHotels.ForEach(h => Console.WriteLine(h.Name));

        // Групування бронювань за клієнтам
        var bookingsByClient = from b in bookings
                               group b by b.Client.Name into g
                               select new { Client = g.Key, BookingsCount = g.Count(), HotelName = g.Select(h => h.Room.Hotel.Name) };
        Console.WriteLine("\nBookings by clients:");
        foreach (var item in bookingsByClient)
        {
            Console.WriteLine($"{item.Client}: {item.BookingsCount} bookings");
            foreach (var hotelName in item.HotelName)
            {
                Console.WriteLine($"{hotelName}");
            }
        }

        // З'єднання готелів та номерів
        var hotelRooms = rooms
            .Join(hotels,
                r => r.HotelId,
                h => h.Id,
                (r, h) => new { h.Name, r.Number, r.Price })
            .ToList();
        Console.WriteLine("\nHotel rooms:");
        hotelRooms.ForEach(hr => Console.WriteLine($"{hr.Name}, Room {hr.Number}, Price: {hr.Price}"));

        // Агрегування – середній рейтинг клієнтів
        var averageRating = (from b in bookings
                             select b.ClientRating).Average();
        Console.WriteLine($"\nAverage client rating: {averageRating:F2}");

        // Групування та агрегація – середня ціна номерів у кожному готелі
        var averageRoomPrice = rooms
            .GroupBy(r => r.Hotel.Name)
            .Select(g => new { Hotel = g.Key, AveragePrice = g.Average(r => r.Price) })
            .ToList();
        Console.WriteLine("\nAverage room price per hotel:");
        averageRoomPrice.ForEach(hp => Console.WriteLine($"{hp.Hotel}: {hp.AveragePrice:F2}"));

        // Сортування бронювань за датою та фільтрація низьких рейтингів
        var sortedBookings = (from b in bookings
                              where b.ClientRating > 3.0
                              orderby b.Date descending
                              select b).ToList();
        Console.WriteLine("\nSorted bookings (rating > 3.0):");
        sortedBookings.ForEach(b => Console.WriteLine($"{b.Client.Name}, Date: {b.Date.ToShortDateString()}, Rating: {b.ClientRating}"));

        // Об'єднання готелів у США та Великобританії
        var usaUkHotels = hotels
            .Where(h => h.Country == "USA")
            .Union(hotels.Where(h => h.Country == "UK"))
            .ToList();
        Console.WriteLine("\nHotels in USA and UK:");
        usaUkHotels.ForEach(h => Console.WriteLine(h.Name));

        // Перетворення в інші структури – список клієнтів у словник
        var clientsDictionary = clients.ToDictionary(c => c.Id, c => c.Name);
        Console.WriteLine("\nClients dictionary:");
        foreach (var kvp in clientsDictionary)
            Console.WriteLine($"{kvp.Key}: {kvp.Value}");

        // Вибірка найдорожчого номера в кожному готелі
        var mostExpensiveRooms = from r in rooms
                                 group r by r.Hotel.Name into g
                                 select new { Hotel = g.Key, MostExpensiveRoom = g.OrderByDescending(r => r.Price).First() };
        Console.WriteLine("\nMost expensive room in each hotel:");
        foreach (var item in mostExpensiveRooms)
            Console.WriteLine($"{item.Hotel}: Room {item.MostExpensiveRoom.Number}, Price: {item.MostExpensiveRoom.Price}");

        // Визначення клієнтів, які не бронювали номери
        var clientsWithoutBookings = clients
            .Where(c => !bookings.Any(b => b.ClientId == c.Id))
            .ToList();
        Console.WriteLine("\nClients without bookings:");
        clientsWithoutBookings.ForEach(c => Console.WriteLine(c.Name));
        
        // Знайти готелі, у яких відсоток повторних бронювань вище 70%
        var highRepeatHotels = hotels.Where(h =>
        {
            var hotelBookings = bookings.Where(b => rooms.Any(r => r.Id == b.RoomId && r.HotelId == h.Id));
            if (!hotelBookings.Any())   
                return false;
            var bookingGroups = hotelBookings.GroupBy(b => b.ClientId);
            var repeatBookingsCount = bookingGroups
                .Where(g => g.Count() > 1) 
                .Sum(g => g.Count() - 1);
            var totalBookingsCount = hotelBookings.Count();
            return (repeatBookingsCount / (double)totalBookingsCount) > 0.7;
        }); 
        Console.WriteLine("\nHotels with high repeat bookings:");
        foreach (var hotel in highRepeatHotels)
        {
            Console.WriteLine(hotel.Name);
        }

        // Визначити клієнтів, які відпочивали у 3 або більше різних країнах за рік
        var clientsOneYearAgo = clients.Where(c =>
            bookings.Where(b => b.ClientId == c.Id && b.Date >= DateTime.Now.AddYears(-1))
                .Select(b => hotels.First(h => h.Id == rooms.First(r => r.Id == b.RoomId).HotelId).Country)
                .Distinct().Count() >= 3);
        
        Console.WriteLine("\nClients who have vacationed in 3 or more countries in a year:");
        foreach (var client in clientsOneYearAgo)
        {
            Console.WriteLine(client.Name);
        }

        //Визначити готелі, у яких більше 30% постійних клієнтів перестали бронювати номери за останні 6 місяців, і знайти можливу причину (рейтинг, ціна, відгуки)
        var affectedHotels = hotels.Where(h =>
        {
            var hotelBookings = bookings.Where(b => rooms.Any(r => r.Id==b.RoomId && r.HotelId == h.Id));
            var loyalClients = hotelBookings
                .GroupBy(b => b.ClientId)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key);
            var recentClients = hotelBookings.Where(b => b.Date >= DateTime.Now.AddMonths(-6)).Select(b => b.ClientId).Distinct();
            var lostClients = loyalClients.Except(recentClients).Count();
            return lostClients > 0.3 * loyalClients.Count();
        });
        
        Console.WriteLine("\nHotels with a loss of 30% of customers in the last 6 months:");
        foreach (var hotel in affectedHotels)
        {
            Console.WriteLine($"{hotel.Name}, Rating: {hotel.Rating}");
        }

        // Знайти гостей, які бронюють номери в одному і тому ж місті, але щоразу в різних готелях, і порівняти середню оцінку їхнього задоволення проживанням
        var cityHoppers = bookings.GroupBy(b => b.ClientId)
            .Where(cb =>
            {
                var city = cb.First().Room.Hotel.City;
                var hotelIds = cb.Select(b => b.Room.Hotel.Id).Distinct();
                return cb.All(b => b.Room.Hotel.City == city) 
                       && hotelIds.Count() == cb.Count()
                       && cb.Count() >= 2;
            })
            .Select(cb => new
            {
                client = clients.First(c => c.Id == cb.Key),
                averageRating = cb.Average(b => b.ClientRating)
            })
            .ToList();
        
        Console.WriteLine("\nGuests who change hotels within the same city:");
        foreach (var cb in cityHoppers)
        {
            Console.WriteLine($"{cb.client.Name}, Rating: {cb.averageRating}");
        }
        
    }
}


