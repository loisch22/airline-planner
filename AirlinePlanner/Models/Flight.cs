using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace AirlinePlanner.Models
{
  public class Flight
  {
    private int _id;
    private DateTime _departureTime;
    private string _departureCity;
    private string _arrivalCity;
    private string _status;

    public Flight(DateTime departureTime, string departureCity, string arrivalCity, string status, int id = 0)
    {
      _departureTime = departureTime;
      _departureCity = departureCity;
      _arrivalCity = arrivalCity;
      _status = status;
      _id = id;
    }

    public int GetId()
    {
      return _id;
    }
    public DateTime GetDepartureTime()
    {
      return _departureTime;
    }
    public string GetDepartureCity()
    {
      return _departureCity;
    }
    public string GetArrivalCity()
    {
      return _arrivalCity;
    }
    public string GetStatus()
    {
      return _status;
    }

    public override bool Equals(System.Object otherFlight)
    {
      if (!(otherFlight is Flight))
      {
        return false;
      }
      else
      {
        Flight newFlight = (Flight) otherFlight;
        bool idEquality = (this.GetId() == newFlight.GetId());
        bool departureTimeEquality = (this.GetDepartureTime() == newFlight.GetDepartureTime());
        bool departureCityEquality = (this.GetDepartureCity() == newFlight.GetDepartureCity());
        bool arrivalCityEquality = (this.GetArrivalCity() == newFlight.GetArrivalCity());
        bool statusEquality = (this.GetStatus() == newFlight.GetStatus());
        return (idEquality && departureTimeEquality && departureCityEquality && arrivalCityEquality);
      }
    }

    public override int GetHashCode()
    {
      return this.GetId().GetHashCode(); //what to put here since there are so many objects
    }

    public static List<Flight> GetAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM flights;";

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<Flight> allFlights = new List<Flight> {};

      while(rdr.Read())
      {
        int flightId = rdr.GetInt32(0);
        DateTime departureTime = rdr.GetDateTime(1);
        string departureCity = rdr.GetString(2);
        string arrivalCity = rdr.GetString(3);
        string flightStatus = rdr.GetString(4);
        Flight flight = new Flight(departureTime, departureCity, arrivalCity, flightStatus, flightId);
        allFlights.Add(flight);
      }
      rdr.Dispose();
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allFlights;
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO flights (departure_time, departure_city, arrival_city, status) VALUES (@departure_time, @departure_city, @arrival_city, @status);";

      MySqlParameter departureTime = new MySqlParameter();
      departureTime.ParameterName = "@departure_time";
      departureTime.Value = this._departureTime;
      cmd.Parameters.Add(departureTime);

      MySqlParameter departureCity = new MySqlParameter();
      departureCity.ParameterName = "@departure_City";
      departureCity.Value = this._departureCity;
      cmd.Parameters.Add(departureCity);

      MySqlParameter arrivalCity = new MySqlParameter();
      arrivalCity.ParameterName = "@arrival_city";
      arrivalCity.Value = this._arrivalCity;
      cmd.Parameters.Add(arrivalCity);

      MySqlParameter statusParameter = new MySqlParameter();
      statusParameter.ParameterName = "@status";
      statusParameter.Value = this._status;
      cmd.Parameters.Add(statusParameter);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"TRUNCATE TABLE flights;";

      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public static Flight Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM flights WHERE id = @id;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@id";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int flightId = 0;
      DateTime departureTime = DateTime.MinValue;
      string departureCity = "";
      string arrivalCity = "";
      string flightStatus = "";

      while(rdr.Read())
      {
        flightId = rdr.GetInt32(0);
        departureTime = rdr.GetDateTime(1);
        departureCity = rdr.GetString(2);
        arrivalCity = rdr.GetString(3);
        flightStatus = rdr.GetString(4);
      }
      Flight foundFlight = new Flight(departureTime, departureCity, arrivalCity, flightStatus, flightId);
      conn.Close();
      return foundFlight;
    }


  }
}
