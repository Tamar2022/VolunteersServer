using System;
using System.Net;
using System.Net.Mail;
using DL;
using Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Data;

using GoogleMapsApi;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.Entities.Directions.Response;
using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi.Entities.Geocoding.Response;
using GoogleMapsApi.StaticMaps;
using GoogleMapsApi.StaticMaps.Entities;
using GoogleMapsApi.Entities.DistanceMatrix.Response;
using Microsoft.Extensions.Options;
using Volunteers;

namespace BL
{
    public class DriveBL : IDriveBL
    {
       
        IDriveDL driveDL;
        AppSettings appSettings;
        public DriveBL(IDriveDL driveDL)//, IOptions<AppSettings> _appSettings)
        {
            this.driveDL = driveDL;
            //appSettings = (AppSettings)_appSettings;

        }

        public async Task<List<Drive>> GetFutureDrivesBLAsync(int userId)
        {
            return await driveDL.GetFutureDrivesDLAsync(userId);
        }


        //get by driverId for history list
        public async Task<List<Drive>> GetDriveBLForHistoryAsync(int driverId)
        {
           
            return await driveDL.GetDriveDLForHistoryAsync(driverId);
        }
        //get by driverId for future list

        public async Task<List<Drive>> GetDriveBLForFutureAsync(int driverId)
        {
            return await driveDL.GetDriveDLForFutureAsync(driverId);
        }
        //post
        public async Task<Drive> PostDriveBLAsync(Drive d)
        {
            return await driveDL.PostDriveDLAsync(d);
        }
       
        //public void send()//string mapurl
        //{
        //    //string to = "212382261@mby.co.il"; //To address    
        //    //string from = "324102417@mby.co.il"; //From address    
        //    MailMessage message = new MailMessage();
        //    message.From = new MailAddress("324102417@mby.co.il");
        //    message.To.Add(new MailAddress("212382261@mby.co.il"));
        //    string mailbody = "we did itttttttttttttttttttttttttttttttttttttttttttt \n";
        //    string link = "<a href= https://localhost:44317/swagger/index.html > enter to match  </a>";
        //    message.Subject = "Sending Email Using Asp.Net & C#";
        //    message.Body = mailbody; //+ mapurl;
        //    message.BodyEncoding = Encoding.UTF8;
        //    message.IsBodyHtml = true;
        //    SmtpClient client = new SmtpClient("smtp.live.com", 587); //Gmail smtp    
        //    System.Net.NetworkCredential basicCredential1 = new
        //    System.Net.NetworkCredential("324102417@mby.co.il", "Student@264");
        //    client.EnableSsl = true;
        //    client.UseDefaultCredentials = false;
        //    client.Credentials = basicCredential1;
        //    try
        //    {
        //        client.Send(message);
        //    }

        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        

  public async Task DeleteDriveBLAsync(int id)
    {
       await driveDL.DeleteDriveDLAsync(id);
    }
     
    }}

  

    // public async Task calc()
    // { 
    //DirectionsRequest directionsRequest = new DirectionsRequest()
    //{
    //    Origin = "NYC, 5th and 39",
    //    Destination = "Philladephia, Chesnut and Wallnut",
    //};

    //DirectionsResponse directions = await GoogleMaps.Directions.QueryAsync(directionsRequest);
    //Console.WriteLine(directions);

    ////Instance class use (Geocode)  (Can be made from static/instance class)
    //GeocodingRequest geocodeRequest = new GeocodingRequest()
    //{
    //    Address = "new york city",
    //};
    //var geocodingEngine = GoogleMaps.Geocode;
    //GeocodingResponse geocode = await geocodingEngine.QueryAsync(geocodeRequest);
    //Console.WriteLine(geocode);

    //// Static maps API - get static map of with the path of the directions request
    //StaticMapsEngine staticMapGenerator = new StaticMapsEngine();

    ////Path from previos directions request
    //if (directions.Routes.Count() != 0)
    //{
    //    IEnumerable<Step> steps = directions.Routes.First().Legs.First().Steps;

    //    // All start locations
    //    IList<ILocationString> path = steps.Select(step => step.StartLocation).ToList<ILocationString>();
    //    // also the end location of the last step
    //    path.Add(steps.Last().EndLocation);

    //    string url = staticMapGenerator.GenerateStaticMapURL(new StaticMapRequest(new Location(40.38742, -74.55366), 9, new ImageSize(800, 400))
    //    {
    //        Pathes = new List<GoogleMapsApi.StaticMaps.Entities.Path>(){ new GoogleMapsApi.StaticMaps.Entities.Path()
    //{
    //        Style = new PathStyle()
    //        {
    //                Color = "red"
    //        },
    //        Locations = path
    //}}
    //    });
    //    sendEmail(url);
    //    Console.WriteLine("Map with path: " + url);
    //}


  
//public string googlemap()
//{//string origin, string destination
 // string url = @$"https://maps.googleapis.com/maps/api/directions/json?origin={origin}&destination={destination}&key={appSettings.GoogleApiKey}";

    //75+9th+Ave+New+York,+NY
    ////MetLife+Stadium+1+MetLife+Stadium+Dr+East+Rutherford,+NJ+07073
    //string url = @$"https://maps.googleapis.com/maps/api/directions/json?origin=75+9th+Ave+New+York,+NY&destination=MetLife+Stadium+1+MetLife+Stadium+Dr+East+Rutherford,+NJ+07073&key={appSettings.GoogleApiKey}";

    //    WebRequest request = WebRequest.Create(url);

    //    WebResponse response = request.GetResponse();

    //    Stream data = response.GetResponseStream();

    //    StreamReader reader = new StreamReader(data);

    //    // json-formatted string from maps api
    //    string responseFromServer = reader.ReadToEnd();

    //    response.Close();
    //    return responseFromServer;
    //Page_Load();
    //return " ";
//}


