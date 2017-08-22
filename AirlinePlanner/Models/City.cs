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
      // if (conn != null)
      // {
      //   conn.Dispose();
      // }
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

  }
}
