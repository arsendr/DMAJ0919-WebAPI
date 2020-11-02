using Dapper;
using System;
using WebAPI.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace WebAPI.Models
{
    public class CoronaOperations
    {

        private readonly IDbConnection _db;

        public CoronaOperations()
        {
            _db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        }

        public List<Datum> GetTheRecords(string sqlQuery)
        {
            SqlConnectionStringBuilder connString = new SqlConnectionStringBuilder();
            connString.UserID = "sa";
            connString.Password = "Technology3";
            connString.DataSource = "l2.kaje.ucnit20.eu";
            connString.IntegratedSecurity = true; // if true then windows authentication
            connString.InitialCatalog = "Corona";
            List<Datum> theReply = new List<Datum>();
            using (SqlConnection connDB = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    connDB.Open();
                    var sqlCmd = new SqlCommand(sqlQuery, connDB);
                    var reader = sqlCmd.ExecuteReader();
                    while (reader.Read())
                    {
                        int index = 0;
                        Datum newData = new Datum();
                        newData.countrycode = reader.GetString(index++);
                        newData.date = reader.GetDateTime(index++).ToString();
                        newData.cases = reader.GetInt32(index++).ToString();
                        newData.deaths = reader.GetInt32(index++).ToString();
                        newData.recovered = reader.GetInt32(index++).ToString();
                        theReply.Add(newData);
                    }
                    reader.Close();
                    connDB.Close();
                }
                catch (SqlException ex)
                {
                    return (theReply);
                }

            }
            return (theReply);
        }

        public bool PutData(Datum data)
        {
            SqlConnectionStringBuilder connString = new SqlConnectionStringBuilder();
            connString.UserID = "sa";
            connString.Password = "Technology3";
            connString.DataSource = "l2.kaje.ucnit20.eu";
            connString.IntegratedSecurity = true; // if true then windows authentication
            connString.InitialCatalog = "Corona";
            //List<Datum> theReply = new List<Datum>();
            using (SqlConnection _db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                int rowsAffected = this._db.Execute(@"INSERT theStats([countrycode],[date],[deaths],[recovered]) values (@countrycode, @date, @deaths, @recovered)",
                new { countrycode = data.countrycode, date = data.date, deaths = data.deaths, recovered = data.recovered});

                if (rowsAffected > 0)
                {
                    return true;
                }

                return false;
            }
        }

        public bool InsertData(Datum data)
        {
            int rowsAffected = _db.Execute(@"INSERT theStats([countrycode],[date],[cases],[deaths],[recovered]) values (@countrycode, @date, @cases, @deaths, @recovered)",
                new { countrycode = data.countrycode, date = data.date, cases=data.cases, deaths = data.deaths, recovered = data.recovered });

            if (rowsAffected > 0)
            {
                return true;
            }

            return false;
        }

    }
}