using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Configuration;

namespace Lab7Project
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        [WebInvoke(Method="GET", ResponseFormat = WebMessageFormat.Json, UriTemplate="QueryInfo/{name}")]
        public Person QueryInfo(string name)
        {
            //Create connection object to underlying database
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DbString"].ConnectionString);

            //Open connection
            conn.Open();

            //Set initial values
            string age = "";
            string temp_name = "";
            string company = "";
            string position = "";

            //Create sql command object
            SqlCommand cmd = new SqlCommand("Select * From testTable where name='" + name + "'", conn);

            //Create reader object to read in results
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read()){
                age = age + ";" + reader["age"];
                temp_name = temp_name + ";" + reader["name"].ToString();
                company = company + ";" + reader["company"].ToString();
                position = position + ";" + reader["position"].ToString();
            }
            reader.Close();

            conn.Close();

            //Return person
            return new Person { Name = temp_name, Age = age, Company = company, Position = position };
        }

        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "InsertInfo/{newName}/{newAge}/{newCompany}/{newPosition}")]
        public Person InsertInfo(string newName, string newAge, string newCompany, string newPosition)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DbString"].ConnectionString);

            conn.Open();

            SqlCommand cmd = new SqlCommand(
                "Insert Into testTable(name, age, company, position) values ('" + newName + "','" 
                + newAge + "','" + newCompany + "','" + newPosition + "')"
                , conn);

            //Insert values
            cmd.ExecuteNonQuery();

            //Get rid of sql command instance
            cmd.Dispose();

            conn.Close();

            //Return person object
            return new Person { Name = newName, Age = newAge, Company = newCompany, Position = newPosition };
        }

        [WebInvoke(Method="GET", ResponseFormat=WebMessageFormat.Json, UriTemplate="DeleteInfo/{name}")]
        public string DeleteInfo(string name)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DbString"].ConnectionString);

            conn.Open();

            SqlCommand cmd = new SqlCommand(
                "Delete From testTable Where name='" + name + "'"
                , conn);

            cmd.ExecuteNonQuery();

            cmd.Dispose();

            conn.Close();

            return "Delete Successful";
        }

        //Default functions
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
