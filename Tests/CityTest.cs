using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace AirlinePlanner
{
  public class CityTest : IDisposable
  {
    public CityTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=airline_planner_test;Integrated Security=SSPI;";
    }

    public void Dispose()
    {
      City.DeleteAll();
    }

    [Fact]
    public void Test_CitiesEmptyAtFirst()
    {
      //Arrange, Act
      int result = City.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueForSameName()
    {
      //Arrange, Act
      City firstCity = new City("Portland");
      City secondCity = new City("Portland");

      //Assert
      Assert.Equal(firstCity, secondCity);
    }

    [Fact]
    public void Test_Save_SavesCityToDatabase()
    {
      //Arrange
      City testCity = new City("Portland");
      testCity.Save();

      //Act
      List<City> result = City.GetAll();
      List<City> testList = new List<City>{testCity};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Save_AssignsIdToCityObject()
    {
      //Arrange
      City testCity = new City("Portland");
      testCity.Save();

      //Act
      City savedCity = City.GetAll()[0];

      int result = savedCity.GetId();
      int testId = testCity.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_Find_FindsCityInDatabase()
    {
      //Arrange
      City testCity = new City("Portland");
      testCity.Save();

      //Act
      City foundCity = City.Find(testCity.GetId());

      //Assert
      Assert.Equal(testCity, foundCity);
    }

    [Fact]
    public void Test_Delete_DeletesCityFromDatabase()
    {
      //Arrange
      string name1 = "Portland";
      City testCity1 = new City(name1);
      testCity1.Save();

      string name2 = "Atlanta";
      City testCity2 = new City(name2);
      testCity2.Save();

      //Act
      testCity1.Delete();
      List<City> resultCategories = City.GetAll();
      List<City> testCityList = new List<City> {testCity2};

      //Assert
      Assert.Equal(testCityList, resultCategories);

    }
  }
}
