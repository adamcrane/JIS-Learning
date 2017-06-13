using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FlightScheduler.Tests
{
    [TestClass]
    public class WhenRetrievingListOfFlights
    {
        Mock<IAirlineFlightService> _mockRepo;
        ScoobyFlights _flightService;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<IAirlineFlightService>();
            _flightService = new ScoobyFlights(_mockRepo.Object);
        }

        [TestMethod]
        public void FlightServiceDoesNotReturnsAEmptyList()
        {
            var criteria = new ScoobySnacks
            {
                DateRange = new DateRange(new DateTime(), new DateTime())
            };
            _mockRepo.Setup(m => m.GetFlightsFromAirline(criteria)).Returns(new List<Flight> { });
            var flights = _flightService.GetAvailableFlights(criteria);
            Assert.IsNotNull(flights);
        }


        [TestMethod]
        public void FlightServiceGetsDataFromAirline()
        {
            var criteria = new ScoobySnacks
            {
                DateRange = new DateRange(new DateTime(), new DateTime())
            };
            Flight flightA = new Flight() { Origin = "DTW", Destination = "OHR", Time = "8:00am" };
            Flight flightB = new Flight() { Origin = "DTW", Destination = "OHR", Time = "6:00pm" };
            Flight flightC = new Flight() { Origin = "DTW", Destination = "OHR", Time = "10:00pm" };
            _mockRepo.Setup(m => m.GetFlightsFromAirline(criteria)).Returns(new List<Flight> { flightA, flightB, flightC });
            var flights = _flightService.GetAvailableFlights(criteria);
            _mockRepo.Verify(m => m.GetFlightsFromAirline(criteria), Times.Once);

            Assert.IsTrue(flights.Any(f => f.Origin == "DTW" && f.Destination == "OHR"));
        }
    }

    public interface IAirlineFlightService
    {
        List<Flight> GetFlightsFromAirline(ScoobySnacks criteria);
    }

    public class ScoobyFlights
    {
        private IAirlineFlightService _airlineFlightService;

        public ScoobyFlights(IAirlineFlightService airlineFlightService)
        {
            this._airlineFlightService = airlineFlightService;
        }

        public List<Flight> GetAvailableFlights(ScoobySnacks criteria)
        {
            return _airlineFlightService.GetFlightsFromAirline(criteria);
        }
    }

    public class ScoobySnacks
    {
        public DateRange DateRange { get; set; }
    }

    public class DateRange
    {
        public DateTime StartDateTime { get; private set; }
        public DateTime EndDateTime { get; private set; }

        public DateRange(DateTime StartDateTime, DateTime EndDateTime)
        {
            this.StartDateTime = StartDateTime;
            this.EndDateTime = EndDateTime;
        }
    }
}
