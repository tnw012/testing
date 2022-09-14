using System;
using MyPortfolio.Database;

namespace Test
{
    class Program
    {
        const string FILE_NAME = "example.txt";


        static void Main(String[] args)
        {
            SQLiteHandler NewSQL = new SQLiteHandler();
            DbQueryData TestData;
            List<string> TestList;            

            TestData._tableName = "todos";
            TestData._columns   = new List<string>() { "Description", "Priority", "DateCreated", "GoalDate" };
            TestData._values    = new List<string>() { "please", "2", "today", "tomorrow" };
            TestData._insert    = true;
            TestData._all       = false;



            NewSQL.ExecuteQuery(TestData);

            NewSQL.ConnectionDetails();
        }
    }
}

// TEST FILES
//"D:\\Software Development\\Visual Studio\\C#\\test\\test\\bin\\Debug\\net6.0\\example.txt"