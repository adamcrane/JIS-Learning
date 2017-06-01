using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FlightScheduler.Tests
{
    [TestClass]
    public class MonkeyTests
    {
        // Customer can find available flights from A to B
        // Customer can't find flight from A to B
        // Customer can't book a flight that is full
        // Customer doesn't want a flight with a layover
        // Customer can't book a flight that is cancelled
        // must havea;lsjdf a;sd and anf;asf
        // Customer can get flight with adjacent seats
        // Customer doesn't want to book flight that costs too much
        // Customer can't go from a to unsupported airport

        [TestMethod]
        public void ShouldRetrieveRequestedFlights()
        {
            var flightRepo = new Mock<IFlightRepository>();
            var requestedOrigin = "DTW";
            var requestedDestination = "LHR";

            var availableFlights = new List<Flight> {new Flight {Origin = "DTW", Destination = "LHR"}};

            flightRepo.Setup(
                m => m.GetStuff(requestedOrigin, requestedDestination))
                .Returns(availableFlights);

            var flights = flightRepo.Object.GetStuff(requestedOrigin, requestedDestination);
            Assert.IsTrue(flights.All(f => f.Origin == requestedOrigin && f.Destination == requestedDestination));
        }

        [TestMethod]
        public void Scooby()
        {
            
        }
    }

    public interface IFlightRepository
    {
        List<Flight> GetStuff(string origin, string destination);
    }

    public class Flight
    {
        public string Origin { get; set; }
        public string Destination { get; set; }
    }
}
