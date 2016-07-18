using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace AirlinePlanner
{
  public class Flight
  {
    private string _name;
    private int _id;
    private string _status;
    private DateTime? _departureTime;
    private DateTime? _arrivalTime;

    public Flight(string name, string status, DateTime? departureTime, DateTime? arrivalTime, int id = 0)
    {
      _name = name;
      _status = status;
      _departureTime = departureTime;
      _arrivalTime = arrivalTime;
      _id = id;
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
          bool idEquality = _id == newFlight.GetId();
          bool nameEquality = _name == newFlight.GetName();
          bool statusEquality = _status == newFlight.GetStatus();
          bool departureTimeEquality = _departureTime == newFlight.GetDepartureTime();
          bool arrivalTimeEquality = _arrivalTime == newFlight.GetArrivalTime();
          return (idEquality && nameEquality && statusEquality && departureTimeEquality && arrivalTimeEquality);
        }
    }

    public int GetId()
    {
      return _id;
    }
    public string GetStatus()
    {
      return _status;
    }
    public string GetName()
    {
      return _name;
    }
    public DateTime? GetDepartureTime()
    {
      return _departureTime;
    }
    public DateTime? GetArrivalTime()
    {
      return _arrivalTime;
    }
    public void SetStatus(string newStatus)
    {
      _status = newStatus;
    }

    public static List<Flight> GetAll()
    {
      List<Flight> allFlights = new List<Flight>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM flights;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int flightId = rdr.GetInt32(0);
        string flightName = rdr.GetString(1);
        string flightStatus = rdr.GetString(2);
        DateTime? flightDepartureTime = rdr.GetDateTime(3);
        DateTime? flightArrivalTime = rdr.GetDateTime(4);
        Flight newFlight = new Flight(flightName, flightStatus, flightDepartureTime, flightArrivalTime, flightId);
        allFlights.Add(newFlight);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allFlights;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO flights (name, status, departure_time, arrival_time) OUTPUT INSERTED.id VALUES (@FlightName, @FlightStatus, @FlightDepartureTime, @FlightArrivalTime);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@FlightName";
      nameParameter.Value = _name;
      cmd.Parameters.Add(nameParameter);


      SqlParameter statusParameter = new SqlParameter();
      statusParameter.ParameterName = "@FlightStatus";
      statusParameter.Value = _status;
      cmd.Parameters.Add(statusParameter);

      SqlParameter departureTimeParameter = new SqlParameter();
      departureTimeParameter.ParameterName = "@FlightDepartureTime";
      departureTimeParameter.Value = _departureTime;
      cmd.Parameters.Add(departureTimeParameter);

      SqlParameter arrivalTimeParameter = new SqlParameter();
      arrivalTimeParameter.ParameterName = "@FlightArrivalTime";
      arrivalTimeParameter.Value = _arrivalTime;
      cmd.Parameters.Add(arrivalTimeParameter);

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

      SqlCommand cmd = new SqlCommand("DELETE FROM flights WHERE id = @FlightId;", conn);

      SqlParameter flightIdParameter = new SqlParameter();
      flightIdParameter.ParameterName = "@FlightId";
      flightIdParameter.Value = _id;

      cmd.Parameters.Add(flightIdParameter);
      cmd.ExecuteNonQuery();

      if (conn != null) conn.Close();
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM flights;", conn);
      cmd.ExecuteNonQuery();
    }

    public static Flight Find(int id)
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM flights WHERE id = @FlightId;", conn);
      SqlParameter flightIdParameter = new SqlParameter();
      flightIdParameter.ParameterName = "@FlightId";
      flightIdParameter.Value = id.ToString();
      cmd.Parameters.Add(flightIdParameter);
      rdr = cmd.ExecuteReader();

      int foundFlightId = 0;
      string foundFlightName = null;
      string foundFlightStatus = null;
      DateTime? foundDepartureTime = null;
      DateTime? foundArrivalTime = null;

      while(rdr.Read())
      {
        foundFlightId = rdr.GetInt32(0);
        foundFlightName = rdr.GetString(1);
        foundFlightStatus = rdr.GetString(2);
        foundDepartureTime = rdr.GetDateTime(3);
        foundArrivalTime = rdr.GetDateTime(4);
      }
      Flight foundFlight = new Flight(foundFlightName, foundFlightStatus, foundDepartureTime, foundArrivalTime, foundFlightId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundFlight;
    }

    public void AddCity(City departureCity, City arrivalCity, int flightId)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO departures_arrivals (departure_city_id, arrival_city_id, flight_id) VALUES (@DepartureCityId, @ArrivalCityId, @FlightId);", conn);
      SqlParameter departureIdParameter = new SqlParameter();
      departureIdParameter.ParameterName = "@DepartureCityId";
      departureIdParameter.Value = departureCity.GetId();
      cmd.Parameters.Add(departureIdParameter);

      SqlParameter arrivalCityIdParameter = new SqlParameter();
      arrivalCityIdParameter.ParameterName = "@ArrivalCityId";
      arrivalCityIdParameter.Value = arrivalCity.GetId();
      cmd.Parameters.Add(arrivalCityIdParameter);

      SqlParameter flightIdParameter = new SqlParameter();
      flightIdParameter.ParameterName = "@FlightId";
      flightIdParameter.Value = flightId;
      cmd.Parameters.Add(flightIdParameter);

      cmd.ExecuteNonQuery();

      if (conn != null) conn.Close();
    }

    public List<City> GetCities()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT departure_city_id, arrival_city_id FROM departures_arrivals WHERE flight_id = @FlightId;", conn);
      SqlParameter flightIdParameter = new SqlParameter();
      flightIdParameter.ParameterName = "@FlightId";
      flightIdParameter.Value = _id;
      cmd.Parameters.Add(flightIdParameter);

      rdr = cmd.ExecuteReader();

      List<int> cityIds = new List<int> {};
      while(rdr.Read())
      {
        int departureCityId = rdr.GetInt32(0);
        int arrivalCityId = rdr.GetInt32(1);
        cityIds.Add(departureCityId);
        cityIds.Add(arrivalCityId);
      }
      if(rdr != null) rdr.Close();

      List<City> cities = new List<City> {};
      foreach (int cityId in cityIds)
      {
        SqlDataReader queryReader = null;
        SqlCommand cityQuery = new SqlCommand("SELECT * FROM cities WHERE id = @CityId ORDER BY name;", conn);

        SqlParameter cityIdParameter = new SqlParameter();
        cityIdParameter.ParameterName = "@CityId";
        cityIdParameter.Value = cityId;
        cityQuery.Parameters.Add(cityIdParameter);

        queryReader = cityQuery.ExecuteReader();
        while(queryReader.Read())
        {
          int thisCityId = queryReader.GetInt32(0);
          string cityName = queryReader.GetString(1);
          City foundCity = new City(cityName, thisCityId);
          cities.Add(foundCity);
        }

        if (queryReader != null) queryReader.Close();
      }

      if (conn != null) conn.Close();

      return cities;
     }

  }
}
