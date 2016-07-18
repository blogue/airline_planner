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

    public Flight(string name, string status, int id = 0)
    {
      _name = name;
      _status = status;
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
          return (idEquality && nameEquality && statusEquality);
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
        Flight newFlight = new Flight(flightName, flightStatus, flightId);
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

      SqlCommand cmd = new SqlCommand("INSERT INTO flights (name, status) OUTPUT INSERTED.id VALUES (@FlightName, @FlightStatus);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@FlightName";
      nameParameter.Value = _name;
      cmd.Parameters.Add(nameParameter);


      SqlParameter statusParameter = new SqlParameter();
      statusParameter.ParameterName = "@FlightStatus";
      statusParameter.Value = _status;
      cmd.Parameters.Add(statusParameter);
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

      while(rdr.Read())
      {
        foundFlightId = rdr.GetInt32(0);
        foundFlightName = rdr.GetString(1);
        foundFlightStatus = rdr.GetString(2);
      }
      Flight foundFlight = new Flight(foundFlightName, foundFlightStatus, foundFlightId);

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

    // public static AddArrival(City name, DateTime? arrivalTime)
    // {
    //
    // }



  }
}
