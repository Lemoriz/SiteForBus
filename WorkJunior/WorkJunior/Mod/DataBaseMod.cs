using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WorkJunior.Mod
{
    public class DBookingTickets : DbContext
    {
        public DbSet<TypeOfBus> TypeOfBus { get; set; }
        public DbSet<CopyOfBus> CopyOfBus { get; set; }
        public DbSet<Itinerary> Itinerary { get; set; }
        public DbSet<Trip> Trip { get; set; }    
        public DbSet<BusDriver> BusDriver { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<TicketData> TicketData { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder opdb)
        {
            opdb.UseSqlite("Data Source = BookingTickets.db");
        }
    }

    public interface IId
    {
        int Id { get; set; }
    }

    public class TypeOfBus : IId
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public int Seats { get; set; }
    }

    public class CopyOfBus : IId
    {
        public int Id { get; set; }
        public string LicensePlate { get; set; }
        public int TypeOfBusId { get; set; }
        [JsonIgnore]
        public TypeOfBus TypeOfBus { get; set; }
    }

    public class Itinerary : IId
    {
        public int Id { get; set; }
        public string BeginningWay { get; set; }
        public string EndWay { get; set; }
    }

    public class Trip : IId
    {
        public int Id { get; set; }
        public DateTime DateTrip { get; set; }
        public int ItineraryId { get; set; }
        [JsonIgnore]
        public Itinerary Itinerary { get; set; }
        public int CopyOfBusId { get; set; }
        [JsonIgnore]
        public CopyOfBus CopyOfBus { get; set; }
    }

    public class BusDriver : IId
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public int CopyOfBusId { get; set; }
        [JsonIgnore]
        public CopyOfBus CopyOfBus { get; set; }
    }

    public class User : IId
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string TelephoneNumber { get; set; }
        public string Access { get; set; }
        [Required]
        public string Mail { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class TicketData : IId
    {
        public int Id { get; set; }
        public int TripId { get; set; }
        [JsonIgnore]
        public Trip Trip { get; set; }
        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
    }

    public class ForBookingQr
    {
        [Required]
        public int TripId { get; set; }
        [Required]
        public int UserId { get; set; }
    }

    public class ForFindTripQr
    {
        [Required]
        public string Start { get; set; }
        [Required]
        public string End { get; set; }
        public DateTime Date { get; set; }
    }

    public class ForFindAllInfTrip
    {
        [Required]
        public string Begining { get; set; }
        [Required]
        public string End { get; set; }
        public DateTime Date { get; set; }
        public string BusModel { get; set; }
        public int BusSeats { get; set; }
        public string LicensePlate { get; set; }
        public int Id { get; set; }
        public int FreeBusSeats { get; set; }
        public string ViewBusSeats { get; set; }
    }

    public class Driver
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string SecondName { get; set; }
        public DateTime Date { get; set; }
        public string LicensePlate { get; set; }
        public string Begining { get; set; }
        public string End { get; set; }
    }

    public class BookingTicketUser
    {
        [Required]
        public int TicketId { get; set; }
        public DateTime Date { get; set; }
        public string Begining { get; set; }
        public string End { get; set; }
    }

    public class UserAuthorization
    {
        [Required]
        public string Mail { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
