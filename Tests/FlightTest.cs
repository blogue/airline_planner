using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace AirlinePlanner
{
  public class FlightTest : IDisposable
  {
    public FlightTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=airline_planner_test;Integrated Security=SSPI;";
    }

    public void Dispose()
    {
      Flight.DeleteAll();
    }

    [Fact]
    public void Test_FlightsEmptyAtFirst()
    {
      //Arrange, Act
      int result = Flight.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueForSameName()
    {
      //Arrange, Act
      Flight firstFlight = new Flight("United 404", "Delayed");
      Flight secondFlight = new Flight("United 404", "Delayed");

      //Assert
      Assert.Equal(firstFlight, secondFlight);
    }

    [Fact]
    public void Test_Save_SavesFlightToDatabase()
    {
      //Arrange
      Flight testFlight = new Flight("United 404", "Delayed");
      testFlight.Save();

      //Act
      List<Flight> result = Flight.GetAll();
      List<Flight> testList = new List<Flight>{testFlight};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Save_AssignsIdToFlightObject()
    {
      //Arrange
      Flight testFlight = new Flight("United 404", "Delayed");
      testFlight.Save();

      //Act
      Flight savedFlight = Flight.GetAll()[0];

      int result = savedFlight.GetId();
      int testId = testFlight.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_Find_FindsFlightInDatabase()
    {
      //Arrange
      Flight testFlight = new Flight("United 404", "Delayed");
      testFlight.Save();

      //Act
      Flight foundFlight = Flight.Find(testFlight.GetId());

      //Assert
      Assert.Equal(testFlight, foundFlight);
    }

    [Fact]
    public void Test_Delete_DeletesFlightFromDatabase()
    {
      //Arrange
      string name1 = "United 404";
      Flight testFlight1 = new Flight(name1, "Delayed");
      testFlight1.Save();

      string name2 = "Virgin 501";
      Flight testFlight2 = new Flight(name2, "Delayed");
      testFlight2.Save();

      //Act
      testFlight1.Delete();
      List<Flight> resultCategories = Flight.GetAll();
      List<Flight> testFlightList = new List<Flight> {testFlight2};

      //Assert
      Assert.Equal(testFlightList, resultCategories);

    }

    //Will add city and arrival time to database
    // [Fact]
    // public void Test_AddArrival_AddsCityAndArrivalTimeToDatabase()
    // {
    //   //Arrange
    //   Flight testFlight = new Flight("United 404", "Delayed");
    //   testFlight.Save();
    //
    //   DateTime? newArrivalTime = new DateTime(2016, 7, 18, 8, 0, 0);
    //   City newArrivalCity = new City("Portland");
    //
    //
    // }
  }
}
