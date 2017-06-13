using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Security.Cryptography.X509Certificates;
using Castle.Components.DictionaryAdapter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FlightScheduler.Tests
{
    [TestClass]
    public class MonkeyTests
    {
        private const string DetroitMetro = "DTW";
        private const string LondonHeathrow = "LHR";
        private List<Flight> _availableFlights = null;
        private FlightIdentifier _flightIdentifier = new FlightIdentifier { FlightNumber = "DL 1234", Departure = DateTime.Now };

        // Customer can find available flights from A to B
        // Customer can't find flight from A to B
        // Customer can't book a flight that is full
        // Customer doesn't want a flight with a layover
        // Customer can't book a flight that is cancelled
        // Customer can get flight with adjacent seats
        // Customer doesn't want to book flight that costs too much
        // Customer can't go from a to unsupported airport

        [TestInitialize]
        public void SetupFlight()
        {
            _availableFlights = new List<Flight>();
            var flight = new Flight
            {
                Origin = DetroitMetro,
                Destination = LondonHeathrow,
                AvailableSeats = 2,
                FlightIdentifier = _flightIdentifier
            };
            _availableFlights.Add(flight);
        }

        [TestMethod]
        public void ShouldRetrieveRequestedFlights()
        {
            var flightRepo = new Mock<IFlightRepository>();
            var requestedOrigin = DetroitMetro;
            var requestedDestination = LondonHeathrow;

            var availableFlights = new List<Flight> { new Flight { Origin = DetroitMetro, Destination = LondonHeathrow } };

            flightRepo.Setup(
                m => m.GetAvailableFlights(requestedOrigin, requestedDestination))
                .Returns(availableFlights);

            var flights = flightRepo.Object.GetAvailableFlights(requestedOrigin, requestedDestination);
            Assert.IsTrue(flights.All(f => f.Origin == requestedOrigin && f.Destination == requestedDestination));
        }

        [TestMethod]
        public void CustomerCannotBookFullFlight()
        {
            var bookingService = new BookingService();
            var bookingRequest = new BookingRequest { };
            var bookingResponse = bookingService.BookFlightForCustomer(bookingRequest);
            Assert.IsNull(bookingResponse.Confirmation);
        }

        [TestMethod, Ignore]
        public void CustomerCanBookFlightWithAvailableSeats()
        {
            var bookingService = new BookingService();
            var bookingRequest = new BookingRequest { RequestedFlight = _flightIdentifier };
            var bookingResponse = bookingService.BookFlightForCustomer(bookingRequest);
            Assert.IsNotNull(bookingResponse.Confirmation);
        }

        [TestMethod, Ignore]
        public void CanBookAFlight()
        {
            var bookingService = new BookingService();
            var bookingRequest = new BookingRequest { RequestedFlight = _flightIdentifier };
            var bookingConfirmation = bookingService.BookFlightForCustomer(bookingRequest);
            var confirmationNumber = string.Empty;
            Assert.AreEqual(bookingConfirmation.Confirmation.ConfirmationNumber, confirmationNumber);
        }
    }

    public class BookingService
    {
        public BookingResponse BookFlightForCustomer(BookingRequest bookingRequest)
        {
            //TODO: Start back up here
            //Create a method for the service to return the
            // number of seats on the requested flight

            //if (bookingRequest.RequestedFlight > 0)
            //{
            //    return new BookingResponse { Confirmation = new BookingConfirmation { ConfirmationNumber = "" } };
            //}
            return new BookingResponse { Message = "Flight is Full" };
        }
    }

    public class BookingResponse
    {
        public string Message { get; set; }
        public BookingConfirmation Confirmation { get; set; }
    }

    public class BookingRequest
    {
        public FlightIdentifier RequestedFlight { get; set; }
        public List<Passenger> Passengers { get; set; }
    }

    public class FlightIdentifier
    {
        public string FlightNumber { get; set; }
        public DateTime Departure { get; set; }
    }

    public class Passenger
    {

    }

    public class BookingConfirmation
    {
        public string ConfirmationNumber { get; set; }
    }

    public interface IFlightRepository
    {
        List<Flight> GetAvailableFlights(string origin, string destination);
    }

    public class Flight
    {
        public string Origin { get; set; }
        public string Destination { get; set; }
        public int AvailableSeats { get; set; }
        public FlightIdentifier FlightIdentifier { get; set; }
        public string Time { get; set; }
    }
}
