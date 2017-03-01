using System;
using Oracle.DataAccess.Client;
using Jesta.VStore.Automation.Framework.CommonLibrary;

namespace Jesta.VStore.Automation.Framework.Framework.CommonLibrary
{
    public class DatabaseUtility
    {
        OracleConnection OracleConn;
       

        public static string GetConnectionString()
        {
            string sConnectionString = "Data Source=base-posqa-01;User Id=vstore;Password=vstore";
            return sConnectionString;
        }

        public void ConnectToVStoreDB()
        {
            string sQuery = "select count(*) from customer";

            OracleConn = new OracleConnection();
            OracleConn.ConnectionString = GetConnectionString();
            OracleConn.Open();

            Console.WriteLine("State: {0}", OracleConn.State);
            Console.WriteLine("ConnectionString: {0}",
                              OracleConn.ConnectionString);

            OracleCommand Cmd = OracleConn.CreateCommand();
            Cmd.CommandText = sQuery;

            OracleDataReader Reader = Cmd.ExecuteReader();
      
            while (Reader.Read())
            {
                var myField = Reader["COUNT(*)"];
                LoggerUtility.WriteLog("<Info: The Value Of The Count - "+myField.ToString()+">");
            }

            OracleConn.Close();
            OracleConn.Dispose();
        }

        /// <summary>
        /// Method to Fetch the Value Of a Field From the DataBase
        /// </summary>
        /// <param name="sQuery">DB Query String</param>
        /// <param name="sFieldName">Name Of the Field in String </param>
        /// <returns></returns>
        public string RetrieveFieldValueFromDB(string sQuery, string sFieldName)
        {
            OracleConn = new OracleConnection();
            OracleConn.ConnectionString = GetConnectionString();
            string sFieldValue = null;

            try
            {
                OracleConn.Open();

                Console.WriteLine("State: {0}", OracleConn.State);
                Console.WriteLine("ConnectionString: {0}",
                                  OracleConn.ConnectionString);

                OracleCommand Cmd = OracleConn.CreateCommand();
                Cmd.CommandText = sQuery;
                OracleDataReader Reader = Cmd.ExecuteReader();

                while (Reader.Read())
                {
                    var myFieldValue = Reader[sFieldName];
                    LoggerUtility.WriteLog("<Info: The Value Of The Count - [" + myFieldValue.ToString() + "]>");
                    OracleConn.Close();
                    OracleConn.Dispose();
                    return sFieldValue = myFieldValue.ToString();
                }
            }
            catch (Exception ex)
            {
                //If there is an error Print it to the Page.
                return ex.Message;
            }
            return sFieldValue;
        }

      /*  public void RetrieveFieldValueFromDB(string sQuery, string sFieldName)
        {
            //string sValue;

            OracleConn = new OracleConnection();
            OracleConn.ConnectionString = GetConnectionString();
            OracleConn.Open();

            Console.WriteLine("State: {0}", OracleConn.State);
            Console.WriteLine("ConnectionString: {0}",
                              OracleConn.ConnectionString);

            OracleCommand Cmd = OracleConn.CreateCommand();
            Cmd.CommandText = sQuery;
            OracleDataReader Reader = Cmd.ExecuteReader();

            while (Reader.Read())
            {
                var myFieldValue = Reader["COUNT(*)"];
                LoggerUtility.WriteLog("<Info: The Value Of The Count - " + myFieldValue.ToString() + ">");
    //          return sValue = myFieldValue.ToString();
            }
            OracleConn.Close();
            OracleConn.Dispose();
        }


       /* public void ConnectVStoreDB()
        {
            OracleConn = new OracleConnection();
            OracleConn.
            con.ConnectionString = "User Id=<username>;Password=<password>;Data Source=<datasource>";
            con.Open();
            Console.WriteLine("Connected to Oracle" + con.ServerVersion);
        }

        void Close()
        {
            con.Close();
            con.Dispose();
        }

        static void Main()
        {
            //OraTest ot = new OraTest();
            //ot.Connect();
            //ot.Close();
        }*/
    }

}

