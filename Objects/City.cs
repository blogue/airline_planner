using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace AirlinePlanner
{
  public class City
  {
    private string _name;
    private int _id;

    public City(string name, int id = 0)
    {
      _name = name;
      _id = id;
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
          bool idEquality = _id == newCity.GetId();
          bool nameEquality = _name == newCity.GetName();
          return (idEquality && nameEquality);
        }
    }

    public int GetId()
    {
      return _id;
    }
    public string GetName()
    {
      return _name;
    }

    public static List<City> GetAll()
    {
      List<City> allCities = new List<City>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM cities;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int cityId = rdr.GetInt32(0);
        string cityName = rdr.GetString(1);
        City newCity = new City(cityName, cityId);
        allCities.Add(newCity);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allCities;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO cities (name) OUTPUT INSERTED.id VALUES (@CityName);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@CityName";
      nameParameter.Value = _name;
      cmd.Parameters.Add(nameParameter);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        _id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
    }

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM cities WHERE id = @CityId;", conn);

      SqlParameter cityIdParameter = new SqlParameter();
      cityIdParameter.ParameterName = "@CityId";
      cityIdParameter.Value = _id;

      cmd.Parameters.Add(cityIdParameter);
      cmd.ExecuteNonQuery();

      if (conn != null) conn.Close();
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM cities;", conn);
      cmd.ExecuteNonQuery();
    }

    public static City Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM cities WHERE id = @CityId;", conn);
      SqlParameter cityIdParameter = new SqlParameter();
      cityIdParameter.ParameterName = "@CityId";
      cityIdParameter.Value = id.ToString();
      cmd.Parameters.Add(cityIdParameter);
      rdr = cmd.ExecuteReader();

      int foundCityId = 0;
      string foundCityName = null;

      while(rdr.Read())
      {
        foundCityId = rdr.GetInt32(0);
        foundCityName = rdr.GetString(1);
      }
      City foundCity = new City(foundCityName, foundCityId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundCity;
    }

    // public void AddFlight(Flight newFlight)
    // {
    //   SqlConnection conn = DB.Connection();
    //   conn.Open();
    //
    //   SqlCommand cmd = new SqlCommand("INSERT INTO departures_arrivals (flight_id) VALUES (@FlightId);", conn);
    //   SqlParameter flightIdParameter = new SqlParameter();
    //   flightIdParameter.ParameterName = "@FlightId";
    //   flightIdParameter.Value = newFlight.GetId();
    //   cmd.Parameters.Add(flightIdParameter);
    //
    //   cmd.ExecuteNonQuery();
    //
    //   if (conn != null) conn.Close();
    // }

    public List<Flight> GetDepartures()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT flight_id FROM departures_arrivals WHERE departure_city_id = @DepartureCityId;", conn);

      SqlParameter departureIdParameter = new SqlParameter();
      departureIdParameter.ParameterName = "@DepartureCityId";
      departureIdParameter.Value = _id;
      cmd.Parameters.Add(departureIdParameter);

      rdr = cmd.ExecuteReader();

      List<int> flightIds = new List<int> {};
      while(rdr.Read())
      {
        int flight = rdr.GetInt32(0);
        flightIds.Add(flight);
      }
      if(rdr != null) rdr.Close();

      List<Flight> flights = new List<Flight> {};
      foreach (int flightId in flightIds)
      {
        SqlDataReader queryReader = null;
        SqlCommand flightQuery = new SqlCommand("SELECT * FROM flights WHERE id = @FlightId;", conn);

        SqlParameter flightIdParameter = new SqlParameter();
        flightIdParameter.ParameterName = "@FlightId";
        flightIdParameter.Value = flightId;
        flightQuery.Parameters.Add(flightIdParameter);

        queryReader = flightQuery.ExecuteReader();
        while(queryReader.Read())
        {
          int thisFlightId = queryReader.GetInt32(0);
          string flightName = queryReader.GetString(1);
          string flightStatus = queryReader.GetString(2);
          DateTime? flightDepartureTime = queryReader.GetDateTime(3);
          DateTime? flightArrivalTime = queryReader.GetDateTime(4);
          Flight foundFlight = new Flight(flightName, flightStatus, flightDepartureTime, flightArrivalTime, thisFlightId);
          flights.Add(foundFlight);
        }

        if (queryReader != null) queryReader.Close();
      }

      if (conn != null) conn.Close();

      return flights;
     }

    public List<Flight> GetArrivals()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT flight_id FROM departures_arrivals WHERE arrival_city_id = @ArrivalCityId;", conn);

      SqlParameter arrivalIdParameter = new SqlParameter();
      arrivalIdParameter.ParameterName = "@ArrivalCityId";
      arrivalIdParameter.Value = _id;
      cmd.Parameters.Add(arrivalIdParameter);

      rdr = cmd.ExecuteReader();

      List<int> flightIds = new List<int> {};
      while(rdr.Read())
      {
        int flight = rdr.GetInt32(0);
        flightIds.Add(flight);
      }
      if(rdr != null) rdr.Close();

      List<Flight> flights = new List<Flight> {};
      foreach (int flightId in flightIds)
      {
        SqlDataReader queryReader = null;
        SqlCommand flightQuery = new SqlCommand("SELECT * FROM flights WHERE id = @FlightId;", conn);

        SqlParameter flightIdParameter = new SqlParameter();
        flightIdParameter.ParameterName = "@FlightId";
        flightIdParameter.Value = flightId;
        flightQuery.Parameters.Add(flightIdParameter);

        queryReader = flightQuery.ExecuteReader();
        while(queryReader.Read())
        {
          int thisFlightId = queryReader.GetInt32(0);
          string flightName = queryReader.GetString(1);
          string flightStatus = queryReader.GetString(2);
          DateTime? flightDepartureTime = queryReader.GetDateTime(3);
          DateTime? flightArrivalTime = queryReader.GetDateTime(4);
          Flight foundFlight = new Flight(flightName, flightStatus, flightDepartureTime, flightArrivalTime, thisFlightId);
          flights.Add(foundFlight);
        }

        if (queryReader != null) queryReader.Close();
      }

      if (conn != null) conn.Close();

      return flights;
     }



  }
}
