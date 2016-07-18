using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace AirlinePlanner
{
  public class FlightTest : IDisposable
  {
    private DateTime? departureTime = new DateTime(2016, 7, 18, 6, 0, 0);
    private DateTime? arrivalTime = new DateTime(2016, 7, 18, 10, 0, 0);

    public FlightTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=airline_planner_test;Integrated Security=SSPI;";
    }

    public void Dispose()
    {
      Flight.DeleteAll();
      City.DeleteAll();
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
      Flight firstFlight = new Flight("United 404", "Delayed", departureTime, arrivalTime);
      Flight secondFlight = new Flight("United 404", "Delayed", departureTime, arrivalTime);

      //Assert
      Assert.Equal(firstFlight, secondFlight);
    }

    [Fact]
    public void Test_Save_SavesFlightToDatabase()
    {
      //Arrange
      Flight testFlight = new Flight("United 404", "Delayed", departureTime, arrivalTime);
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
      Flight testFlight = new Flight("United 404", "Delayed", departureTime, arrivalTime);
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
      Flight testFlight = new Flight("United 404", "Delayed", departureTime, arrivalTime);
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
      Flight testFlight1 = new Flight(name1, "Delayed", departureTime, arrivalTime);
      testFlight1.Save();

      string name2 = "Virgin 501";
      Flight testFlight2 = new Flight(name2, "Delayed", departureTime, arrivalTime);
      testFlight2.Save();

      //Act
      testFlight1.Delete();
      List<Flight> resultCategories = Flight.GetAll();
      List<Flight> testFlightList = new List<Flight> {testFlight2};

      //Assert
      Assert.Equal(testFlightList, resultCategories);

    }

    [Fact]
    public void Test_AddCities_AddsCitiesToFlight()
    {
      //Arrange
      Flight testFlight = new Flight("United 404", "Delayed", departureTime, arrivalTime);
      testFlight.Save();

     City departureCity = new City("Portland");
     departureCity.Save();

     City arrivalCity = new City("Atlanta");
     arrivalCity.Save();

     //Act
     testFlight.AddCity(departureCity, arrivalCity, testFlight.GetId());

     List<City> result = testFlight.GetCities();
     List<City> cityList = new List<City>{departureCity, arrivalCity};
     //Assert
     Assert.Equal(cityList, result);
    }




  }
}
