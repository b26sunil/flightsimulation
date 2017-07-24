using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlightSimulation
{
    public class flight
    {
        public int departtime;
        public int arrivaltime;
        public int departAirport;
        public int ArrivalAirport;
        public int numberofpassengers;
        public Boolean returningflight;

        public flight(int dep_time, int arv_time, int dep_airportid, int arv_airportid, int passengers, bool ret)
        {
            departtime = dep_time;
            arrivaltime = arv_time;
            departAirport = dep_airportid;
            ArrivalAirport = arv_airportid;
            numberofpassengers = passengers;
            returningflight = ret;
        }

    }
}
