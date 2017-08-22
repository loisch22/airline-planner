using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace AirlinePlanner.Models
{
  public class City
  {
    public int _id;
    public string _cityName;

    public City(string cityName, int id = 0)
    {
      _cityName = cityName;
      _id = id;
    }

    public int GetId()
    {
      return _id;
    }
    public string GetCityName()
    {
      return _cityName;
    }

    public override bool Equals(System.Object otherCity)
    {
      if (!(otherCity is City))
      {
        return false;
      }
      else
      {
        City newCity = (City) otherCity;
        bool idEquality = (this.GetId() == newCity.GetId());
        bool nameEquality = (this.GetCityName() == newCity.GetCityName());
        return (idEquality && nameEquality);
      }
    }

    public override int GetHashCode()
    {
      return this.GetCityName().GetHashCode();
    }

    public static List<City> GetAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM cities;";

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<City> allCities = new List<City> {};

      while(rdr.Read())
      {
        int cityId = rdr.GetInt32(0);
        string cityName = rdr.GetString(1);
        City city = new City(cityName, cityId);
        allCities.Add(city);
      }
      rdr.Dispose();
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allCities;
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO cities (city_name) VALUES (@city_name);";

      MySqlParameter cityName = new MySqlParameter();
      cityName.ParameterName = "@city_name";
      cityName.Value = this._cityName;
      cmd.Parameters.Add(cityName);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"TRUNCATE TABLE cities;";

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = new MySqlCommand("DELETE FROM cities WHERE id = @cityId; DELETE FROM cities_flights WHERE city_id = @cityId;", conn);

      MySqlParameter cityIdParameter = new MySqlParameter();
      cityIdParameter.ParameterName = "@cityId";
      cityIdParameter.Value = this.GetId();

      cmd.Parameters.Add(cityIdParameter);
      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static City Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM cities WHERE id = @id;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@id";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int cityId = 0;
      string cityName = "";

      while(rdr.Read())
      {
        cityId = rdr.GetInt32(0);
        cityName = rdr.GetString(1);
      }
      City newCity = new City(cityName, cityId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return newCity;
    }

    public void AddFlight(Flight newFlight)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO cities_flights (city_id, flight_id) VALUES (@CityId, @FlightId);";

      MySqlParameter cityIdParameter = new MySqlParameter();
      cityIdParameter.ParameterName = "@CityId";
      cityIdParameter.Value = _id;
      cmd.Parameters.Add(cityIdParameter);

      MySqlParameter flightIdParameter = new MySqlParameter();
      flightIdParameter.ParameterName = "@FlightId";
      flightIdParameter.Value = newFlight.GetId();
      cmd.Parameters.Add(flightIdParameter);

      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public List<Flight> GetFlights()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT flight_id from cities_flights WHERE city_id = @CityId;";

      MySqlParameter cityIdParameter = new MySqlParameter();
      cityIdParameter.ParameterName = "@CityId";
      cityIdParameter.Value = _id;
      cmd.Parameters.Add(cityIdParameter);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<int> flightIds = new List<int> {};

      while(rdr.Read())
      {
        int flightId = rdr.GetInt32(0);
        flightIds.Add(flightId);
      }
      rdr.Dispose();

      List<Flight> flights = new List<Flight> {};
      foreach (int flightId in flightIds)
      {
        var flightQuery = conn.CreateCommand() as MySqlCommand;
        flightQuery.CommandText = @"SELECT * FROM flights WHERE id = @FlightId;";

        MySqlParameter flightIdParameter = new MySqlParameter();
        flightIdParameter.ParameterName = "@FlightId";
        flightIdParameter.Value = flightId;
        flightQuery.Parameters.Add(flightIdParameter);

        var flightQueryRdr = flightQuery.ExecuteReader() as MySqlDataReader;

        while (flightQueryRdr.Read())
        {
          int thisFlightId = flightQueryRdr.GetInt32(0);
          DateTime departureTime = flightQueryRdr.GetDateTime(1);
          string departureCity = flightQueryRdr.GetString(2);
          string arrivalCity = flightQueryRdr.GetString(3);
          string flightStatus = flightQueryRdr.GetString(4);
          Flight foundFlight = new Flight (departureTime, departureCity, arrivalCity, flightStatus, thisFlightId);
          flights.Add(foundFlight);
        }
        flightQueryRdr.Dispose();
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return flights;
    }

  }
}
