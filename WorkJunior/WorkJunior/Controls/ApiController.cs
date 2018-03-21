using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkJunior.Mod;

namespace WorkJunior.Controls
{
    public abstract class Api<Entity> : Controller where Entity : class
    {
        private DBookingTickets bookingTic;

        public Api(DBookingTickets bookingTic)
        {
            this.bookingTic = bookingTic;
        }

        [HttpGet]
        public virtual IEnumerable<Entity> Get()
        {
            bookingTic.Set<Entity>().Load();
            return bookingTic.Set<Entity>();
        }

        [HttpGet("{id}")]
        public virtual IQueryable<Entity> Get(int Id) => bookingTic.Set<Entity>().Where(q => ((IId)q).Id == Id);

        [HttpPost]
        public virtual Entity Post([FromBody]Entity type)
        {
            if (type == null) return default;

            ((IId)type).Id = 0;

            bookingTic.Add(type);
            bookingTic.SaveChanges();

            return type;
        }

        [HttpDelete("{id}")]
        public virtual void Delete(int Id)
        {
            var t = bookingTic.Set<Entity>().FirstOrDefault(q => ((IId)q).Id == Id);
            if (t == null) return;

            bookingTic.Remove(t);
            bookingTic.SaveChanges();
        }

        [HttpPut]
        public virtual void Update([FromBody]Entity type)
        {
            if (type == null) return;

            var qw = bookingTic.Set<Entity>().FirstOrDefault(q => ((IId)q).Id == ((IId)type).Id);
            bookingTic.Entry<Entity>(qw).CurrentValues.SetValues(type);

            bookingTic.SaveChanges();
        }
    }

    [Route("api/[controller]")]
    public class ItineraryController : Api<Itinerary>
    {
        public ItineraryController(DBookingTickets repo) : base(repo)
        {
        }
    }

    [Route("api/[controller]")]
    public class TypeOfBusController : Api<TypeOfBus>
    {
        public TypeOfBusController(DBookingTickets repo) : base(repo)
        {
        }
    }

    [Route("api/[controller]")]
    public class CopyOfBusController : Api<CopyOfBus>
    {
        public CopyOfBusController(DBookingTickets repo) : base(repo)
        {
        }
    }


    [Route("api/[controller]")]
    public class BusDriverController : Api<BusDriver>
    {
        public BusDriverController(DBookingTickets repo) : base(repo)
        {
        }
    }

    [Route("api/[controller]")]
    public class TripController : Api<Trip>
    {
        public TripController(DBookingTickets repo) : base(repo)
        {
        }
    }

    [Route("api/[controller]")]
    public class UserController : Api<User>
    {
        public UserController(DBookingTickets repo) : base(repo)
        {
        }
    }

    [Route("api/[controller]")]
    public class TicketDataController : Api<TicketData>
    {
        public TicketDataController(DBookingTickets repo) : base(repo)
        {
        }
    }


    [Route("api/[controller]")]
    public class AllBusDriver : Controller
    {
        private DBookingTickets repo;

        public AllBusDriver(DBookingTickets repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public IActionResult Get()
        {
            if (!ModelState.IsValid) return BadRequest();

            return Ok(repo.BusDriver);
        }
    }

    [Route("api/[controller]")]
    public class AllCopyOfBus : Controller
    {
        private DBookingTickets repo;

        public AllCopyOfBus(DBookingTickets repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public IActionResult Get()
        {
            if (!ModelState.IsValid) return BadRequest();

            return Ok(repo.CopyOfBus);
        }
    }

    [Route("api/[controller]")]
    public class FindAllInfTrip : Controller
    {
        private DBookingTickets repo;

        public FindAllInfTrip(DBookingTickets repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public IQueryable<ForFindAllInfTrip> Get(ForFindAllInfTrip query)
        {
            if (!ModelState.IsValid) return null;

            var table = repo.Itinerary.Join(
                repo.Trip, i => i.Id, t => t.ItineraryId, (i, t) => new ForFindAllInfTrip
                {
                    Id = t.Id,
                    Date = t.DateTrip,
                    Begining = i.BeginningWay,
                    End = i.EndWay,
                    BusModel = t.CopyOfBus.TypeOfBus.Model,
                    BusSeats = t.CopyOfBus.TypeOfBus.Seats,
                    LicensePlate = t.CopyOfBus.LicensePlate,
                    FreeBusSeats = t.CopyOfBus.TypeOfBus.Seats - repo.TicketData.Where(w => w.TripId == t.Id).Count(),
                    ViewBusSeats = t.CopyOfBus.TypeOfBus.Seats - repo.TicketData.Where(w => w.TripId == t.Id).Count() + "/" + t.CopyOfBus.TypeOfBus.Seats
                });

            var attempt = table.Where(t => string.Equals(t.Begining, query.Begining, StringComparison.CurrentCultureIgnoreCase) &&
            String.Equals(t.End, query.End, StringComparison.CurrentCultureIgnoreCase));


            if (attempt == null) return null;

            if (query.Date == default)
            {
                return attempt;
            }
            else
            {
                if (attempt.Where(d => d.Date == query.Date) != null)
                {
                    return attempt;
                }
                return null;
            }
        }
    }

    [Route("api/[controller]")]
    public class Booking : Controller
    {
        private DBookingTickets repo;
        public Booking(DBookingTickets repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public IQueryable<TicketData> Get()
        {
            return repo.TicketData;
        }

        [HttpGet("{id}")]
        public TicketData Get(int id)
        {
            return repo.TicketData.FirstOrDefault(w => w.Id == id);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var ticket = repo.TicketData.FirstOrDefault(w => w.Id == id);
            if (ticket == null) return;
            repo.Remove(ticket);
            repo.SaveChanges();
        }

        [HttpPost]
        public TicketData Post([FromBody]ForBookingQr query)
        {
            if (!ModelState.IsValid) return null;


            if (!(repo.User.Any(w => w.Id == query.UserId) && repo.Trip.Any(w => w.Id == query.TripId)))
                return null;

            repo.Trip.Include(w => w.CopyOfBus);
            repo.CopyOfBus.Load();
            repo.TypeOfBus.Load();
            var trip = repo.Trip.FirstOrDefault(w => w.Id == query.TripId);

            if (repo.TicketData.Where(w => w.TripId == query.TripId).Count() >= trip.CopyOfBus.TypeOfBus.Seats)
                return null;

            TicketData ticket = new TicketData
            {
                Trip = trip,
                UserId = query.UserId

            };

            repo.TicketData.Add(ticket);

            repo.SaveChanges();

            return ticket;
        }
    }

    [Route("api/[controller]")]
    public class FindSchedule : Controller
    {
        private DBookingTickets repo;

        public FindSchedule(DBookingTickets repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public IActionResult Get(Driver query)
        {
            if (!ModelState.IsValid) return BadRequest();

            var c = repo.BusDriver.Where(d =>
            String.Equals(d.FirstName, query.FirstName, StringComparison.CurrentCultureIgnoreCase)
            && String.Equals(d.SecondName, query.SecondName, StringComparison.CurrentCultureIgnoreCase));

            var res = c.Join(repo.Trip, q => q.CopyOfBus.Id, w => w.CopyOfBusId, (e, r) => new
                   Driver
            {
                FirstName = e.FirstName,
                SecondName = e.SecondName,
                Begining = r.Itinerary.BeginningWay,
                End = r.Itinerary.EndWay,
                Date = r.DateTrip,
                LicensePlate = e.CopyOfBus.LicensePlate
            });

            if (!res.Any()) return BadRequest();

            return Ok(res);
        }
    }

    [Route("api/[controller]")]
    public class BookingTripForUser : Controller
    {
        private DBookingTickets repo;

        public BookingTripForUser(DBookingTickets repo)
        {
            this.repo = repo;
        }

        [HttpGet("{id}")]
        public IQueryable<BookingTicketUser> Get(int id)
        {
            if (!ModelState.IsValid) return null;

            var tikest = repo.TicketData.Where(w => w.UserId == id);

            var trips = repo.Itinerary.Join(repo.Trip, i => i.Id, t => t.ItineraryId, (i, t) => new
            {
                TripId = t.Id,
                i.BeginningWay,
                i.EndWay,
                t.DateTrip
            });

            var result = tikest.Join(trips, tik => tik.TripId, trip => trip.TripId, (tik, trip) => new BookingTicketUser
            {
                Begining = trip.BeginningWay,
                End = trip.EndWay,
                Date = trip.DateTrip,
                TicketId = tik.Id
            });


            if (result == null) return null;

            return result;
        }
    }

    [Route("api/[controller]")]
    public class EditItinerary : Controller
    {
        private DBookingTickets repo;
        public EditItinerary(DBookingTickets repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public IQueryable<Itinerary> Get()
        {
            if (!ModelState.IsValid) return null;

            return repo.Itinerary;
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromBody]Itinerary query, int id)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = repo.Itinerary.FirstOrDefault(i => i.Id == id);

            result.BeginningWay = query.BeginningWay;
            result.EndWay = query.EndWay;

            repo.SaveChanges();

            return Ok(result);
        }
    }

    [Route("api/[controller]")]
    public class ChangeDriverBus : Controller
    {
        private DBookingTickets repo;
        public ChangeDriverBus(DBookingTickets repo)
        {
            this.repo = repo;
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromBody]BusDriver query, int id)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = repo.BusDriver.FirstOrDefault(f => f.Id == id);

            result.CopyOfBusId = query.CopyOfBusId;

            repo.SaveChanges();

            return Ok(result);
        }
    }

    [Route("api/[controller]")]
    public class AddNewUser : Controller
    {
        private DBookingTickets repo;

        public AddNewUser(DBookingTickets repo)
        {
            this.repo = repo;
        }

        [HttpPost]
        public User Post([FromBody]User query)
        {
            if (!ModelState.IsValid) return null;

            query.Access = "user";

            repo.User.Add(query);

            repo.SaveChanges();

            return query;
        }
    }
}
