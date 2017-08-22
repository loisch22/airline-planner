using Microsoft.VisualStudio.TestTools.UnitTesting;
using AirlinePlanner.Models;
using System.Collections.Generic;
using System;

namespace AirlinePlanner.Tests
{
  [TestClass]
  public class CityTests : IDisposable
  {
    public CityTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=	8889;database=airline_planner_test;";
    }

    public void Dispose()
    {
      City.DeleteAll();
    }

    [TestMethod]
    public void GetAll_ReturnsEmptyDatabase_0()
    {
      City.DeleteAll();
      int expected = 0;
      int actual = City.GetAll().Count;

      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Equals_TrueForSameCityName_Cities()
    {
      City newCity = new City("Seattle");
      City newCity2 = new City("Seattle");

      Assert.AreEqual(newCity, newCity2);
    }

    [TestMethod]
    public void Save_SaveNewCityToCities_1()
    {
      City newCity = new City("Seattle");
      newCity.Save();

      int expected = 1;
      int actual = City.GetAll().Count;

      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Save_AssignsIdToObject_id()
    {
      City newCity = new City("Seattle");
      newCity.Save();

      City savedCity = City.GetAll()[0];

      int expected = savedCity.GetId();
      int actual = newCity.GetId();
      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Find_FindsCityInDatabase_City()
    {
      City newCity = new City("Seattle");
      newCity.Save();

      City result = City.Find(newCity.GetId());

      Assert.AreEqual(newCity, result);
    }

    [TestMethod]
    public void DeleteAll_DeletesAllCities_Void()
    {
      City newCity = new City("Seattle");
      newCity.Save();
      City.DeleteAll();

      int expected = 0;
      int actual = City.GetAll().Count;

      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Delete_DeletesSelectedCity_Void()
    {
      City newCity = new City("Seattle");
      newCity.Save();
      City newCity2 = new City("Houston");
      newCity2.Save();
      newCity.Delete();

      int expected = 1;
      int actual = City.GetAll().Count;

      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Test_AddFlight_AddsFlightToCity()
    {
      City testCity = new City("Seattle");
      testCity.Save();

      Flight testFlight = new Flight(new DateTime (2017, 8, 21, 12, 00, 00), "Seattle", "LA", "On-time");
      testFlight.Save();

      Flight testFlight2 = new Flight(new DateTime (2017, 8, 21, 12, 00, 00), "Singapore", "LA", "On-time");
      testFlight2.Save();

      testCity.AddFlight(testFlight);
      testCity.AddFlight(testFlight2);

      List<Flight> result = testCity.GetFlights();
      List<Flight> testList = new List<Flight>{testFlight, testFlight2};

      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void GetFlights_ReturnsAllFlightsForCity_Flights()
    {
      City testCity = new City("Seattle");
      testCity.Save();

      Flight testFlight = new Flight(new DateTime (2017, 8, 21, 12, 00, 00), "Seattle", "LA", "On-time");
      testFlight.Save();

      Flight testFlight2 = new Flight(new DateTime (2017, 8, 21, 12, 00, 00), "Singapore", "LA", "On-time");
      testFlight2.Save();

      testCity.AddFlight(testFlight);

      List<Flight> expected = new List<Flight> {testFlight};
      List<Flight> actual = testCity.GetFlights();

      CollectionAssert.AreEqual(expected, actual);
    }

  }
}
