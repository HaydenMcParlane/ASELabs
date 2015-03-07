using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Lab7Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Login" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Login.svc or Login.svc.cs at the Solution Explorer and start debugging.
    public class Login : ILogin
    {       
        [WebInvoke(Method="POST", ResponseFormat=WebMessageFormat.Json, UriTemplate = "/{username}/{password}")]
        public bool Authenticate(string username, string password)
        {                        
            bool resp = false;                               

            //Connect to DB
            SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\v11.0;AttachDbFilename=C:\\Users\\Hayden\\Documents\\Lab7DB.mdf;Integrated Security=True;Connect Timeout=30");

            //Open connection
            conn.Open();

            //Crude authentication
            SqlCommand cmd = new SqlCommand("Select * From Users Where Username = '" + username + "' AND "
                + "Password = '" + password + "'", conn);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {                    
                resp = true;
            }

            cmd.Dispose();
            conn.Close();

            return resp;
        }

        [WebInvoke(Method="POST", ResponseFormat=WebMessageFormat.Json, UriTemplate="/Register/{username}/{password}")]
        public bool Register(string username, string password)
        {
            try
            {
                SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\v11.0;AttachDbFilename=C:\\Users\\Hayden\\Documents\\Lab7DB.mdf;Integrated Security=True;Connect Timeout=30");
                conn.Open();
                SqlCommand cmd = new SqlCommand("Insert Into Users(Username, Password) Values ('" + username + "', '" + password + "')", conn);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                conn.Close();

                return true;
            }
            catch (Exception b)
            {
                Console.WriteLine(b);
                return false;
            }
        }
    }
}
