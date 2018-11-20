using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Mail;
using System.Text;
using System.Net.NetworkInformation;

namespace Holiday
{
    public partial class Form1 : Form
    {
        //Constants and tutorial
        //In order to customize any of the text in the program window itsef, you should do that in the designer
        //You can also change the images too that way

        //Put the earliest date that the Gift can be sent in the format MM/dd/yyyy
        string date = "12/25/2018";
        //Put the name of the holiday
        string holiday = "Christmas";
        //Name of recipient to be dispayed
        string recipient = "Test";
        //Email of Recipient
        string recipientEmail = "";
        //Subject of Email
        string emailSubject = "HolidayTest";
        //Email text
        string emailText = "Test" + System.Environment.NewLine+"Hello there" + System.Environment.NewLine + "General Kenobi" + "A bunch of test text that is just testing if I can test the line overflow with testing it but also testing how the email client in c# works, oh jeez I sure do love testing huh";
        //Email Attatchment, give it a local file path
        string attatchment = "attatchment.png";
        //Sender Email
        string senderEmail = "";
        //Sender Password
        string senderPassword = "";


        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Check if today is before the holiday date
            if (DateTime.Today < DateTime.ParseExact(date, "MM/dd/yyyy", CultureInfo.InvariantCulture))
            {
                //If so, display a message that the recipient is trying to open early
                MessageBox.Show(recipient + ", I know you are trying to open this early you goof" + System.Environment.NewLine + "You gotta wait till " + date);
            }
            //IF its past the holiday but the user can't connect to the internet, tell them they can't recieve the gift
            else if (!checkInternetStatus())
            {
                MessageBox.Show("Hey hey, looks like you're not connected to the internet. "+System.Environment.NewLine+"This present gifter needs email to work, try again once you are online.  ");
            }
            //Else its on or past the date and they can open it
            else
            {
                try
                {
                    //First disable and hide the button and enable everything else
                    presentButton.Enabled = false;
                    presentButton.Visible = false;
                    pictureBox1.Enabled = true;
                    pictureBox1.Visible = true;
                    pictureBox2.Enabled = true;
                    pictureBox2.Visible = true;
                    pictureBox3.Enabled = true;
                    pictureBox3.Visible = true;
                    textBox1.Enabled = true;
                    textBox1.Visible = true;
                    textBox2.Enabled = true;
                    textBox2.Visible = true;
                    textBox3.Enabled = true;
                    textBox3.Visible = true;

                    using (MailMessage emailMessage = new MailMessage())
                    {
                        emailMessage.From = new MailAddress(senderEmail, senderPassword);
                        emailMessage.To.Add(new MailAddress(recipientEmail));
                        emailMessage.Subject = emailSubject;
                        emailMessage.Body = emailText;
                        emailMessage.Attachments.Add(new Attachment(attatchment));
                        emailMessage.Priority = MailPriority.Normal;
                        using (SmtpClient MailClient = new SmtpClient("smtp.gmail.com", 587))
                        {
                            MailClient.EnableSsl = true;
                            MailClient.UseDefaultCredentials = false;
                            MailClient.Credentials = new System.Net.NetworkCredential(senderEmail, senderPassword);
                            MailClient.Send(emailMessage);
                        }
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("An error occured.  Complain to the guy who programmed or sent you this and send him this"+System.Environment.NewLine+ System.Environment.NewLine + System.Environment.NewLine + ex.Message.ToString());
                }
                

            }
        }

        private bool checkInternetStatus()
        {
            try
            {
                Ping myPing = new Ping();
                PingReply myReply = myPing.Send("Google.com");
                return (myReply.Status == IPStatus.Success);
            }
            catch(Exception ex)
            {
                //Don't really care, it means we can't reach google anyways 
            }
            return false;
            

        }
    }
}
