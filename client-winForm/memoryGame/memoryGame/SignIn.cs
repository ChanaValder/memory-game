using memoryGame.Models;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;


namespace memoryGame
{
    public partial class SignIn : Form
    {
        static HttpClient client = new HttpClient();
        public SignIn()
        {
            InitializeComponent();
        }

        private void btnSignIn_Click(object sender, EventArgs e)
        {
            dynamic NewUser;
            //Try converting age.
            try
            {
                //Validations 
                int userAge = int.Parse(age.Text);
                if (userAge < 18 || userAge > 120)
                {
                    MessageBox.Show("User age must be between 18 and 120.");
                    return;
                }
                if(name.Text.Length<2||name.Text.Length>10)
                {
                    MessageBox.Show("User name must contains 2 - 10 chars.");
                    return;
                }

                //Post Request for Login
                try
                {
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(@"http://localhost:52339/Login");
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        NewUser = new User { UserName = name.Text, Age = int.Parse(age.Text) };
                        string NewUserString = Newtonsoft.Json.JsonConvert.SerializeObject(NewUser, Formatting.None);
                        streamWriter.Write(NewUserString);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                    //Gettting response
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                    //Reading response
                    using (var streamReader = new System.IO.StreamReader(httpResponse.GetResponseStream(), ASCIIEncoding.ASCII))
                    {
                        string result = streamReader.ReadToEnd();
                        //If Login succeeded
                        if (result.Contains("true"))
                        {
                            GlobalProp.CurrentUser = NewUser;
                            ChoosePartner choosePartner = new ChoosePartner();
                            choosePartner.Show();

                        }
                        //Printing the matching error
                        else MessageBox.Show(result);
                    }
                }
                catch
                {
                    MessageBox.Show("User name is exists, choose another username.");
                }
            }
            catch
            {
                MessageBox.Show("age must be a number");
            }

        }

        private void btnSignIn_MouseHover(object sender, EventArgs e)
        {
            (sender as Button).BackColor = Color.Teal;
        }

        private void btnSignIn_MouseLeave(object sender, EventArgs e)
        {
            (sender as Button).BackColor = Color.LightGray;
        }
    }
}



