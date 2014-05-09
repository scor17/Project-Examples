using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Odbc;

class MyOdbcConnection
{
    public static void Main()
    {
        string myConnection = "DSN=myAccess";
        OdbcConnection myConn = new OdbcConnection(myConnection);
        OdbcCommand mycommand = new OdbcCommand();
        myConn.Open();
        mycommand.Connection = myConn;

        string mySelectQuery;

        while(true)
        {
            Console.WriteLine("Enter as select statement(enter 'exit' to quit): ");
            mySelectQuery = Console.ReadLine();

            if (mySelectQuery == "exit")
            {
                break;
            }

            DataTable tab = new DataTable();
            OdbcDataAdapter dbadapter = new OdbcDataAdapter(mySelectQuery, myConn);
            dbadapter.Fill(tab);


            foreach (DataRow row in tab.Rows)
            {
                for (int i = 0; i < row.ItemArray.Length; i++)
                {
                    Console.WriteLine(row[i]);
                }
                Console.WriteLine();
            }

        }

        myConn.Close();

    }
}