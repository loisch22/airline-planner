using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using AirlinePlanner.Models;

namespace AirlinePlanner.Tests
{

  [TestClass]
  public class FlightTest : IDisposable
  {
    public FlightTest()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=airline_planner_test;";
    }
    public void Dispose()
    {
      Flight.DeleteAll();
    }

    [TestMethod]
    public void GetAll_ReturnsEmptyDatabase_0()
    {
      Flight.DeleteAll();
      int expected = 0;
      int actual = Flight.GetAll().Count;

      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Equals_TrueForSameFlightName_Flights()
    {
      Flight newFlight = new Flight(new DateTime (2017, 8, 21, 12, 00, 00), "Seattle", "LA", "On-time");
      Flight newFlight2 = new Flight(new DateTime (2017, 8, 21, 12, 00, 00), "Seattle", "LA", "On-time");

      Assert.AreEqual(newFlight, newFlight2);
    }

    [TestMethod]
    public void Save_SaveNewFightToFlights_1()
    {
      Flight newFlight = new Flight(new DateTime (2017, 8, 21, 12, 00, 00), "Seattle", "LA", "On-time");
      newFlight.Save();

      int expected = 1;
      int actual = Flight.GetAll().Count;

      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Save_AssignsIdToObject_id()
    {
      Flight newFlight = new Flight(new DateTime (2017, 8, 21, 12, 00, 00), "Seattle", "LA", "On-time");
      newFlight.Save();

      Flight savedFlight = Flight.GetAll()[0];

      int expected = savedFlight.GetId();
      int actual = newFlight.GetId();
      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Find_FindsFlightInDatabase_City()
    {
      Flight newFlight = new Flight(new DateTime (2017, 8, 21, 12, 00, 00), "Seattle", "LA", "On-time");
      newFlight.Save();

      Flight result = Flight.Find(newFlight.GetId());

      Assert.AreEqual(newFlight, result);
    }

    [TestMethod]
    public void DeleteAll_DeletesAllFlights_Void()
    {
      Flight newFlight = new Flight(new DateTime (2017, 8, 21, 12, 00, 00), "Seattle", "LA", "On-time");
      newFlight.Save();

      Flight.DeleteAll();

      int expected = 0;
      int actual = Flight.GetAll().Count;

      Assert.AreEqual(expected, actual);
    }


  }
}
