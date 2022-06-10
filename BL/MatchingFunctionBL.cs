using DL;
using DTO;
using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using AutoMapper;
using Microsoft.Extensions.Configuration;

namespace BL
{
    public class DComparer<T> : IComparer<T>
    {
        public string source { get; set; }
        public int Compare(T a, T b)
        {
            var fa = D(a);
            var fb = D(b);

            if (fa < fb) return -1;
            if (fa == fb) return 0;
            return 1;
        }
        public int D(T a)
        {
                Type x = a.GetType();               
                return MatchingFunctionBL.CalcDistance(x.GetProperty("SourceStreet").ToString(), source);          
        }
    }

    public class MatchingFunctionBL : IMatchingFunctionBL
    {
        IDriveDL driveDL;
        IDriverRequestDL driverRequestDL;
        IPassengerRequestDL PassengerRequestDL;
        IMapper mapper;
       static IConfiguration _configuration;
        public MatchingFunctionBL(IDriverRequestDL driverRequestDL, IPassengerRequestDL PassengerRequestDL,
        IDriveDL driveDL, IMapper mapper,IConfiguration configuration)
        {
            this.driverRequestDL = driverRequestDL;
            this.PassengerRequestDL = PassengerRequestDL;
            this.driveDL = driveDL;
            this.mapper = mapper;
            _configuration=configuration;
        }


        public async Task<bool> MatchingFunctionForDriverReq(DriverRequest dr)
        {
            bool isMatch = false;
            bool isMatch1 = false;
            
            List<DateTime> dtl = GetWeekdayInRange((DateTime)dr.StartDate, (DateTime)dr.EndDate, (DayOfWeek)dr.Day);
            foreach (DateTime item in dtl)
            {
                isMatch = await MatchingFunctionForOneDriverReq(dr, item);
                if (isMatch)
                    isMatch1 = true;
            }
           
            return isMatch1;

        }
        
        public async Task<bool> MatchingFunctionForOneDriverReq(DriverRequest dr, DateTime dt)//from driver request
        {
            bool isHandicappedCar =(bool) dr.Driver.IsHandicappedCar;//check if the driver has HandicappedCar

            bool  isMatch = false;
            int drSeats = (int)dr.NumOfSeats;//driver seats
            int usedSeats = await PassengerRequestDL.GetSumOfSeatsPassengerRequestByDriveDLAsync(dr, dt);//driver used seats
            int availableSeats = drSeats - usedSeats;//driver availableSeats

            List<PassengerRequest> prList = await PassengerRequestDL.GetPassengerRequestByDateDLAsync(dt, (DateTime)dr.LeavingHour, dr.DestinationStreet);//get all matches people on date time and destination
            List<PassengerRequest> matchPr = new List<PassengerRequest>();//empty list for match people
            if (prList == null)//there is no match people on date time and destination
                return false;

            int sumOfDist = 0;//sum of distance  
            string lastLocation = await driveDL.GetStreetByDriverRequestDLAsync(dr.DriverRequestId,dt);
            if (lastLocation == null)// he is  the first passenger
                lastLocation = dr.SourceStreet;
            float originalLengthWay = CalcDistance(dr.SourceStreet, dr.DestinationStreet);//from driver`s source to driver`s dest.
            
            foreach (var pr in prList)
            {
          
                if (availableSeats >= pr.NumOfSeats)//there are availableSeats
                {
                    float wayLength = sumOfDist + CalcDistance(lastLocation, pr.SourceStreet) + CalcDistance(pr.SourceStreet, pr.DestinationStreet);
                    //the way that driver passed until now+lastLocation of the driver to SourceStreet of the passenger+from SourceStreet of the passenger to DestinationStreet 
                    if (wayLength <= originalLengthWay * 3.3)// the way can be longer than the originalLengthWay only by 30%;!!!!!!!!!!need to take from secrets
                    {
                        if (pr.IsHandicapped)//if the passenger need HandicappedCar
                        {
                            if(isHandicappedCar)//if the driver has HandicappedCar
                            {
                                matchPr.Add(pr);
                            }
                        }
                        else//if the passenger dont need HandicappedCar
                            matchPr.Add(pr);

                    }
                }
            }
            DComparer<PassengerRequest> dComparer = new DComparer<PassengerRequest>();
            dComparer.source = lastLocation;
            matchPr.Sort(dComparer);// sort by distance             
            foreach (var mPr in matchPr)
            {
                if (availableSeats >= mPr.NumOfSeats)//there are availableSeats
                {
                    Drive d = new Drive() { IsApproved = false, Date = dt, PassengerRequestId = mPr.PassengerRequestId, DriverRequestId = dr.DriverRequestId };//create new drive
                    await driveDL.PostDriveDLAsync(d);//post drive
                    availableSeats -= mPr.NumOfSeats;//update  availableSeats
                    sumOfDist += MatchingFunctionBL.CalcDistance(lastLocation, mPr.SourceStreet);//update sumOfDist from the last location 
                    lastLocation = mPr.SourceStreet;//update lastLocation
                    string mailBody = " ?נמצאה התאמת נסיעה,האם הינך מאשר" + "\n  תאריך:" + mPr.Date + " : יציאה מ " + dr.SourceStreet + " :הגעה אל " + dr.DestinationStreet + '\n' + "    פרטי הנוסע\n"
                       +mPr.User.Person.FullName  +  ", " + "  יוצא מ" + mPr.SourceStreet + "מספר מקומות נדרש" + mPr.NumOfSeats;//???
                    //sendEmail("212382261@mby.co.il", mailBody);//mPr.User.Person.Email
                    isMatch= true;//match has found
                } 
            }
            return isMatch;
        }



        public async Task<bool> MatchingFunctionForPassengerRequest(PassengerRequest pr)//from PassengerRequest
        {
            bool isHandicappedCar = (bool)pr.IsHandicapped;//check if the Passenger need Handicappedcar
            bool isMatch1 = false;
            List<DriverRequest> drList = await driverRequestDL.GetDriverRequestByDayDLAsync(pr.Time, pr.DestinationStreet);
            List<DriverRequestDTO> matchDr = new List< DriverRequestDTO>();//empty list for match drivers

            if (drList == null)//there is no match drivers on  time and destination
                return false;
            foreach (var dr in drList)
            {
                int drSeats = (int)dr.NumOfSeats;//driver seats
                List<DateTime> dtl = GetWeekdayInRange((DateTime)dr.StartDate, (DateTime)dr.EndDate, (DayOfWeek)dr.Day);
                foreach (var date in dtl)
                {
                    if (date.Equals(pr.Date))
                    {
                        if (isHandicappedCar)//  the Passenger need Handicappedcar
                        {
                            isMatch1 = false;
                            if (dr.Driver.IsHandicappedCar == true)//driver has Handicappedcar
                            {
                                isMatch1 = true;
                            }
                        }

                        else//  the Passenger dont need Handicappedcar
                        {
                            isMatch1 = true;
                        }
                        if (isMatch1)
                        {
                            int usedSeats = await PassengerRequestDL.GetSumOfSeatsPassengerRequestByDriveDLAsync(dr, date);//driver used seats
                            int availableSeats = drSeats - usedSeats;//driver availableSeats

                            if (availableSeats >= pr.NumOfSeats)//there are availableSeats
                            {

                                string lastLocation = await driveDL.GetStreetByDriverRequestDLAsync(dr.DriverRequestId, date);
                                if (lastLocation == null)// he is  the first passenger
                                    lastLocation = dr.SourceStreet;
                                float wayLength = CalcDistance(lastLocation, pr.SourceStreet) + CalcDistance(pr.SourceStreet, pr.DestinationStreet);
                                //the way that driver passed until now+lastLocation of the driver to SourceStreet of the passenger+from SourceStreet of the passenger to DestinationStreet 
                                float originalLengthWay = CalcDistance(dr.SourceStreet, dr.DestinationStreet);//from driver`s source to driver`s dest.
                                if (wayLength <= originalLengthWay * 3.3)// the way can be longer than the originalLengthWay only by 30%;!!!!!!!!!!need to take from secrets
                                { var dto = mapper.Map<DriverRequest, DriverRequestDTO>(dr);
                                    dto.date = date;
                                    matchDr.Add(dto);

                                   
                                }
                            }
                        }
                    }
                }
            }
            if (matchDr.Count > 0)//there is match drivers 
            {
                DComparer<DriverRequestDTO> dComparer = new DComparer<DriverRequestDTO>();
                dComparer.source = pr.SourceStreet;
                matchDr.Sort(dComparer);// sort by distance

                DriverRequestDTO selectedDr = matchDr[0];//the closest driver
                Drive d = new Drive() { IsApproved = false, Date = selectedDr.date, PassengerRequestId = pr.PassengerRequestId, DriverRequestId = selectedDr.DriverRequestId };//create new drive
                d=await driveDL.PostDriveDLAsync(d);//post drive
                pr.IsValid = false;//match drive has been found.
                await PassengerRequestDL.PutPassengerRequestDLAsync(pr.PassengerRequestId, pr);                                    
                string mailBody = " ?נמצאה התאמת נסיעה,האם הינך מאשר" + "\n  תאריך:" + pr.Date + " : יציאה מ " + selectedDr.SourceStreet + " :הגעה אל " + selectedDr.DestinationStreet + '\n' + "    פרטי הנוסע\n"
                   + pr.User.Person.FullName + ", " + "  יוצא מ" + pr.SourceStreet + "מספר מקומות נדרש" + pr.NumOfSeats;
                //sendEmail("212382261@mby.co.il", mailBody);//mPr.User.Person.Email                                                                                                    

                return true;//match has been found
            }
            return false;
        }




        public async Task MatchingFunctionForCancelDriverRequest(DriverRequestDTO dr)//for driver`s cancel
        {
            List<Drive> drives = await driveDL.GetDriveByDriverRequestIdDLAsync(dr.DriverRequestId, dr.date);//get the driver`s drives
            if (drives .Count==0)//if there are no match drives for this driverRequest
            {
                return;
            }
            else
            {
                List<PassengerRequest> passengerReq = new List<PassengerRequest>();

                foreach (var drive in drives)
                {
                    passengerReq.Add(drive.PassengerRequest);
                    await driveDL.DeleteDriveDLAsync(drive.DriveId);//delete this drive

                }

                foreach (var pr in passengerReq)
                {
                    bool isMatched = await MatchingFunctionForPassengerRequest(pr);//found  another driver for PassengerRequest 
                    if (!isMatched)
                    {
                        pr.IsValid = true;//PassengerRequest is  valid because we didnt find a drive for it
                        PassengerRequest p= await  PassengerRequestDL.PutPassengerRequestDLAsync(pr.PassengerRequestId, pr);
                    }
                }
            }
        }



        public async Task MatchingFunctionForCancelPassengerRequest(PassengerRequest pr)//for driver`s cancel

        {
           Drive d= await driveDL.GetDriveByPassengerRequestIdDLAsync(pr.PassengerRequestId);
           await PassengerRequestDL.DeletePassengerRequestDLAsync(pr.PassengerRequestId);
           
           if (d!=null)//if there are  match drives for this passengerRequest
            {
                await driveDL.DeleteDriveDLAsync(d.DriveId);//delete this drive
                await MatchingFunctionForOneDriverReq(d.DriverRequest,(DateTime)d.Date);
                
            }

        }




        public List<DateTime> GetWeekdayInRange(DateTime from, DateTime to, DayOfWeek day)
        {
            const int daysInWeek = 7;
            var result = new List<DateTime>();
            var daysToAdd = ((int)day - (int)from.DayOfWeek + daysInWeek) % daysInWeek;

            do
            {
                if (from.AddDays(daysToAdd) < to)
                {
                    from = from.AddDays(daysToAdd);
                    result.Add(from);
                    daysToAdd = daysInWeek;
                }
                else
                    break;

            } while (from < to);

            return result;
        }



        public static int CalcDistance(string SourceStreet, string DestinationStreet)//פונקצית מרחק 
        {
            //"Oberoi Mall, Goregaon" "Infinity IT Park, Malad East"

            string origin = SourceStreet;
            string destination = DestinationStreet;
            string url = "https://maps.googleapis.com/maps/api/distancematrix/xml?origins=" + origin + "&destinations=" + destination + "&key="+ _configuration.GetValue<string>("googleApiKey");

            WebRequest request = WebRequest.Create(url);
            using (WebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    DataSet dsResult = new DataSet();
                    dsResult.ReadXml(reader);
                    string g = dsResult.ToString();
                    //dsResult.Tables["DistanceMatrixResponse"].Rows[0]["origin_address"].ToString();
                  //lblDestinationAddress.Text = dsResult.Tables["DistanceMatrixResponse"].Rows[0]["destination_address"].ToString();

                    var duration = dsResult.Tables["duration"].Rows[0]["text"].ToString();
                    var distance = Convert.ToInt32(dsResult.Tables["distance"].Rows[0]["value"]);
                    return distance;
                }
            }
        }

        public  void sendEmail(string to, string mailBody)//string mapurl,
        {
            //string to = "212382261@mby.co.il"; //To address    
            //string from = "324102417@mby.co.il"; //From address    
            MailMessage message = new MailMessage();
            message.From = new MailAddress("324102417@mby.co.il");
            message.To.Add(new MailAddress(to));
            string mailbody = mailBody + "\n";
            string link = "<a href= https://localhost:44317/swagger/index.html > enter to match  </a>";
            message.Subject = "Sending Email Using Asp.Net & C#";
            message.Body = mailbody;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("smtp.live.com", 587); //Gmail smtp    
            NetworkCredential basicCredential1 = new
            NetworkCredential("324102417@mby.co.il", "Student@264");
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = basicCredential1;
           
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            try
            {
                client.Send(message);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
} 

    