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
      // City.DeleteAll();
    }

    [TestMethod]
    public void GetAll_ReturnsEmptyDatabase_0()
    {
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

  }
}
