using Nancy;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace AirlinePlanner
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => View["index.cshtml"];

      Get["/flights"] = _ => {
        List<Flight> allFlights = Flight.GetAll();
        return View["flights.cshtml", allFlights];
      };
      Get["/flight/new"] = _ => {
        List<City> allCities = City.GetAll();
        return View["add_flight.cshtml", allCities];
      };
      Post["/flights"] = _ => {
        City departureCity = new City(Request.Form["departure-city"]);
        City arrivalCity = new City(Request.Form["arrival-city"]);
        Flight newFlight = new Flight(Request.Form["name"], Request.Form["status"], Request.Form["departure-time"], Request.Form["arrival-time"]);
        newFlight.Save();
        newFlight.AddCity(departureCity, arrivalCity, newFlight.GetId());
        List<Flight> allFlights = Flight.GetAll();
        return View["flights.cshtml", allFlights];
      };
      Get["/cities"] = _ => {
        List<City> allCities = City.GetAll();
        return View["cities.cshtml", allCities];
      };
      Get["/city/new"] = _ => View["add_city.cshtml"];

      Get["/city/{id}"] = parameters => {
        City selectedCity = City.Find(parameters.id);
        return View["city.cshtml", selectedCity];
      };

      Post["/cities"] = _ => {
        City newCity = new City(Request.Form["city-name"]);
        newCity.Save();
        List<City> allCities = City.GetAll();
        return View["cities.cshtml", allCities];
      };
    }
  }

}
