using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FlightSimulation
{
    public partial class Form1 : Form
    {

        public static int[,] flyhours = new int[4, 4] { { 0, 1, 1, 6 }, { 1, 0, 2, 3 }, { 1, 2, 0, 4 }, { 6, 3, 4, 0 } };
        public int currenttime;
        public static Dictionary<int, Airport> airports = new Dictionary<int, Airport>();
        public int looplimit=16;
        public Form1()
        {
            InitializeComponent();

        
        }
        private void button1_Click(object sender, EventArgs e)
        {
            // Initialize objects and set values

            Airport a = new Airport("Bos", 760, 1500, 0);
            Airport b = new Airport("JFK", 960, 1500, 1);
            Airport c = new Airport("SFO", 820,1250, 2);
            Airport d = new Airport("SLC", 1900, 2400, 3);

            a.setUIObject(tb);
            b.setUIObject(tb2);
            c.setUIObject(tb3);
            d.setUIObject(tb4);
 
            a.setFlightDistribution(new double[1, 5] { { 0, 0.3, 0.2, 0.3, 45 } });
            b.setFlightDistribution(new double[1, 5] { { 0.2, 0, 0.3, 0.2, 37} });
            c.setFlightDistribution(new double[1, 5] { { 0.4, 0.1, 0, 0.3, 72 } });
            d.setFlightDistribution(new double[1, 5] { { 0.2, 0.2, 0.3, 0, 110 } });


            airports.Add(a.airportid, a);
            airports.Add(b.airportid, b);
            airports.Add(c.airportid, c);
            airports.Add(d.airportid, d);


            // start Simulating Flights 
            for ( currenttime = 0;  currenttime < looplimit ;  currenttime++)
            {
                foreach (Airport air in  airports.Values)
                {
                    air.CheckFlightArrived();
                    air.PassengersHome();
                    air.createDepartureFlight();
                    // print airport details
                    printAllAirports();
                }
                 currenttime++;
            }
        }

        private void printAllAirports()
        {
            foreach (Airport a in airports.Values  )
            {
                printAirportDetails(a);

            }
        }

        private void printAirportDetails(Airport a)
        {
            displayUI(a.uiobj," Airport Information here: ","" );
            displayUI(a.uiobj, " Airport name :", a.name.ToString());
            displayUI(a.uiobj, " Airport ID :", a.airportid.ToString());
            displayUI(a.uiobj, " Airport totalpassengers  current :", a.totalpassengers.ToString());
            displayUI(a.uiobj, " Airport MaxTotalPassengers :", a.maxpassengercapacity.ToString());
            displayUI(a.uiobj, " Flight Arrival Queue has the following flights :", "");

            if (a.FlightArrivalqueue.Count <= 0)
            {
                displayUI(a.uiobj, " No Flight is Scheduled For arrival right now.", "");
            }
            else
            {
                int i = 0;
                foreach (flight f in a.FlightArrivalqueue)
                {
                    i++;
                    displayUI(a.uiobj, " Flight Number: ", i.ToString());
                    displayUI(a.uiobj, " Fight Departure Airport :", f.departAirport.ToString());
                    displayUI(a.uiobj, " Flight Arrival Airport :", f.ArrivalAirport.ToString());
                    displayUI(a.uiobj, " Flight Dep time:", f.departtime.ToString());
                    displayUI(a.uiobj, " Flight Arival time :", f.arrivaltime.ToString());
                    displayUI(a.uiobj, " Flight Number of Passengers :", f.numberofpassengers.ToString());
                    displayUI(a.uiobj, " -------// Next Flight // ---------", "");
                }
            }

            displayUI(a.uiobj, " -------//New Loop here // ---------", "");

        }

        private void displayUI(TextBox tb, string str,string value)
        {
            tb.AppendText(str +value+ Environment.NewLine);         
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
