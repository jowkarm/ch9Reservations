//Behrooz Kazemi, and Mohammad Jokar-Konavi
//May 25, 2021
//Extra 9-1 Calculate reservation totals 


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reservations
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //The IsValidData method is added.
        public bool IsValidData()
        {
            bool s = true;
            List<bool> success = new List<bool>(6);
            DateTime min = DateTime.Today;
            DateTime max = min.AddYears(5);

            //Validate the arrival date text box
            success.Add(IsPresent(txtArrivalDate, txtArrivalDate.Tag.ToString()));
            success.Add(IsDateTime(txtArrivalDate, txtArrivalDate.Tag.ToString()));
            success.Add(IsWithinRange(txtArrivalDate, txtArrivalDate.Tag.ToString(), min, max));

            //Validate the departure date text box
            success.Add(IsPresent(txtDepartureDate, txtDepartureDate.Tag.ToString()));
            success.Add(IsDateTime(txtDepartureDate, txtDepartureDate.Tag.ToString()));
            success.Add(IsWithinRange(txtDepartureDate, txtDepartureDate.Tag.ToString(), min, max));

            if (success.Contains(false))
            {
                s = false;
            }
            return s;
        }

        public bool IsPresent(TextBox textBox, string name)
        {
            if (textBox.Text == "")
            {
                MessageBox.Show(name + " is a required field.", "Entry Error");
                textBox.Focus();
                return false;
            }
            return true;
        }


        //The IsDateTime method is added.
        public bool IsDateTime(TextBox textBox, string name)
        {
            
            if (!DateTime.TryParse(textBox.Text, out DateTime localDate))
            {
                MessageBox.Show(name + " must have a valid date format.",
                    "Format Error");
                textBox.Focus();
                return false;
            }
            return true;
        }

        //The IsWithinRange method is added.
        public bool IsWithinRange(TextBox textBox, string name,
            DateTime min, DateTime max)
        {
            if (DateTime.TryParse(textBox.Text, out DateTime localDate))
            {
                if (localDate < min || localDate > max)
                {
                    MessageBox.Show(name + " must be between " + min.ToShortDateString() + 
                        " and " + max.ToShortDateString() + ".\n",
                   "Range Error");
                    textBox.Focus();
                    return false;
                }
                   
            }
            return true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                //The IsValidData mehod is called.
                if (IsValidData())
                {
                    //Get the arrival and departure date
                    DateTime arrival = DateTime.Parse(txtArrivalDate.Text);
                    DateTime departure = DateTime.Parse(txtDepartureDate.Text);

                    //Check the departure date 
                    if (arrival > departure)
                    {
                        MessageBox.Show("The departure date must be " +
                            "after the arrival date.", "Entry Error");
                    }


                    //Calculate the number of days
                    TimeSpan ts = departure.Subtract(arrival);
                    int nights = ts.Days;

                    //Calculate the total price
                    int total = 0;

                    while (arrival < departure)
                    {
                        //Find the day of week for pricing
                        DayOfWeek dayofWeek = arrival.DayOfWeek;
                        int price;
                        if (dayofWeek == DayOfWeek.Friday ||
                            dayofWeek == DayOfWeek.Saturday)
                        {
                            price = 150;
                        }

                        else
                        {
                            price = 120;
                        }


                        total += price;
                        arrival = arrival.AddDays(1);
                        continue;
                    }




                    decimal average = total / nights;

                    //Display the results
                    txtNights.Text = nights.ToString();
                    txtAvgPrice.Text = average.ToString("c");
                    txtTotalPrice.Text = total.ToString("c");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n\n" +
                    ex.GetType().ToString() + "\n" +
                    ex.StackTrace, "Exception");
            }
        }



        //The Load event is added.
        private void Form1_Load(object sender, EventArgs e)
        {
            //Show the current date for arrival date.
            DateTime arrival = DateTime.Today;

            //Show three days later as depurture date.
            DateTime departure = arrival.AddDays(3);

            //Format the dates
            txtArrivalDate.Text = arrival.ToShortDateString();
            txtDepartureDate.Text = departure.ToShortDateString();

        }
    }
}
