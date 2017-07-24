using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms ;

namespace FlightSimulation
{
    public class Airport:Form1 
    {
        public string name;
        public int airportid;
        public int totalpassengers;
        public int maxpassengercapacity;

        public List<flight> FlightArrivalqueue = new List<flight>();
        //public SortedList<int, flight> FlightArrivalqueue = new SortedList<int, flight>();
        public double[,] flightdistribution = new double[1, 5];
        public TextBox uiobj;
        readonly object syncLock = new object();

        public Airport(string nameofairport, int starttotalpassengers, int maxpassengers, int id)
        {
            
            name = nameofairport;
            totalpassengers = starttotalpassengers;
            maxpassengercapacity = maxpassengers;
            airportid = id;

        }

        public void setUIObject(TextBox t)
        {
            uiobj = t;       
        }

        public void setFlightDistribution(double[,] val)
        {
            flightdistribution =val;
        }

        public void AddFlightsToQueue(Airport a, int flighthours, flight f)
        {
            lock(syncLock )
            {


                a.FlightArrivalqueue.Add( f);
            }

        }

        public void CheckFlightArrived()
        {
            lock(syncLock)
            {
                FlightArrivalqueue.OrderBy(o => o.arrivaltime);
                for(int i= 0;i < FlightArrivalqueue.Count; i++)
                {

                    flight obj = FlightArrivalqueue.ElementAt(i);
                    if (obj.arrivaltime <= currenttime)
                    {
                        if (obj.numberofpassengers + totalpassengers > maxpassengercapacity)
                        { ScheduleDepartureFlight(obj, true); }
                        else
                        { processArrivalFlight(obj); }

                        FlightArrivalqueue.RemoveAt(0);
                    }
                    i++;
                }
            }
                
        }

        public void PassengersHome()
        {
            int gohomepassengers = Convert.ToInt32(flightdistribution[0, 4]);
            if (totalpassengers - gohomepassengers >= 0)
                totalpassengers = totalpassengers- gohomepassengers;
            else
                totalpassengers = 0;
        }

        public void processArrivalFlight(flight flight)
        {
            totalpassengers = totalpassengers + flight.numberofpassengers;
         
        }

        public void createDepartureFlight()
        {
            for (int j = 0; j < 4; j++)
            {
                int destinationairportid = j;
                double distribution = flightdistribution[0, j];
                int arrivaltime = currenttime + flyhours[destinationairportid, airportid];
                int passengersinflight = Convert.ToInt32(distribution * totalpassengers);

                bool returned = false;
                if (passengersinflight > 0)
                {
                    flight f = new flight(currenttime, arrivaltime, airportid, destinationairportid, passengersinflight, returned);
                    ScheduleDepartureFlight(f, returned);
                }
            }
        }

        public void ScheduleDepartureFlight(flight flight, Boolean returnedflight)
        {
            if (returnedflight)
            {
                flight f = flight;

                flight.departAirport = airportid;
                flight.ArrivalAirport = f.departAirport;
                flight.departtime = currenttime;
                flight.arrivaltime = flight.departtime + flyhours[flight.departAirport, flight.ArrivalAirport];
                flight.returningflight = true;
            }

            // get another instance of airport object

            Airport arrivalairport = airports[flight.ArrivalAirport];
            //add to the arrival list of this airport.
            arrivalairport.AddFlightsToQueue(arrivalairport, flight.arrivaltime - flight.departtime, flight);
            totalpassengers -= flight.numberofpassengers;
        }
    }

}
