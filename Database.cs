namespace MyPortfolio.Database
{
    using System.Data.SQLite;

    struct DbQueryData
    {
        public string _tableName;
        public List<string> _columns;
        public List<string> _values;
        public bool _insert;
        public bool _all;
    }

    class SQLiteHandler
    {
        // CONSTANTS
        const string DATABASE_FILE_NAME = @"D:\Software Development\Databases\Sqlite3\MyPortfolio.db";

        // ATTRIBUTES       
        ushort _dbVersion;
        string _dbQuery;
        string _dbConnectionStr;

        // CONSTRUCTORS
        public SQLiteHandler()
        {            
            _dbVersion = 3;
            _dbQuery = String.Empty;
            _dbConnectionStr = $"Data Source={DATABASE_FILE_NAME}; Version={_dbVersion};";
        }

        public SQLiteHandler(string dbFilename, ushort version)
        {
            _dbVersion = version;
            _dbQuery = String.Empty;
            _dbConnectionStr = $"Data Source={dbFilename}; Version={_dbVersion}";
        }

        // PUBLIC METHODS
        public void ExecuteQuery(DbQueryData InsertData)
        {   
            try
            {
                using(SQLiteConnection DbConnection = new SQLiteConnection(_dbConnectionStr))
                {
                    DbConnection.Open();
                    using (SQLiteCommand DatabaseCommand = new SQLiteCommand(DbConnection))
                    {
                        FormatQuery(InsertData);
                        DatabaseCommand.CommandText = _dbQuery;
                        for (int index = 0; index < InsertData._values.Count; index++)
                        {
                            if (InsertData._values[index].GetType() == typeof(string))
                            {
                                string commandParameters = $"'{InsertData._values[index]}'";
                                DatabaseCommand.Parameters.Add(new SQLiteParameter(commandParameters));
                            }
                            else
                            {
                                DatabaseCommand.Parameters.Add(InsertData._values[index]);
                            }
                        }

                        DatabaseCommand.ExecuteNonQuery();
                    }//END using SQLiteCommand
                }//END using SQLiteConnection
                
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }                
        }

        //public List<string> SelectQuery(DbQueryData OutputData)
        //{
        //    List<string> DatabaseData = new List<string>();

        //    _DatabaseConnection.Open();

        //    SQLiteCommand DatabaseCommand = _DatabaseConnection.CreateCommand();
        //    DatabaseCommand.CommandText = FormatQuery(OutputData);
        //    DatabaseCommand.CommandType = System.Data.CommandType.Text;
        //    SQLiteDataReader DatabaseReader = DatabaseCommand.ExecuteReader();

        //    while (DatabaseReader.Read())
        //    {
        //        DatabaseData.Add(Convert.ToString(DatabaseReader["FileName"]));
        //    }

        //    DatabaseCommand.ExecuteNonQuery();

        //    _DatabaseConnection.Close();

        //    return DatabaseData;
        //}

        public void ConnectionDetails()
        {
            using (SQLiteConnection DbConnection = new SQLiteConnection(_dbConnectionStr))
            {
                DbConnection.Open();
                string sqliteVersionQuery = "SELECT SQLITE_VERSION()";

                using (SQLiteCommand DatabaseCommand = new SQLiteCommand(DbConnection))
                {
                    DatabaseCommand.CommandText = sqliteVersionQuery;
                    string version = DatabaseCommand.ExecuteScalar().ToString();
                    Console.WriteLine($"SQLite Version: {version}");
                    Console.WriteLine($"Database File Path: {DbConnection.ConnectionString}");
                    Console.WriteLine($"Database Query: {_dbQuery}");
                }
            }
        }

        // PRIVATE METHODS
        void FormatQuery(DbQueryData FormatData)
        {
            string formattedQuery;

            if (FormatData._insert == true)
            {
                formattedQuery = String.Format($"INSERT INTO {FormatData._tableName} {FormatValuesString(FormatData._columns, FormatData._insert)} " +
                                               $"VALUES (?,?,?,?)");
            }
            else
            {
                if (FormatData._all)
                {
                    formattedQuery = $"SELECT * from {FormatData._tableName}";
                }
                else
                {
                    formattedQuery = String.Format($"SELECT {FormatValuesString(FormatData._columns, FormatData._insert)} from {FormatData._tableName}");
                }
            }

            _dbQuery = formattedQuery;
        }// TODO: Make this method private when class is not in testing

        string FormatValuesString(List<string> values, bool insert)
        {
            System.Text.StringBuilder FormattedValues = new System.Text.StringBuilder();
            ushort index = 0;

            while (index < values.Count)
            {
                if (insert == true)
                {
                    if (index == 0)
                    {
                        FormattedValues.Append($"({values.ElementAt(index)}, ");
                    }
                    else if (index == values.Count - 1)
                    {
                        FormattedValues.Append($"{values.ElementAt(index)})");
                    }
                    else
                    {               
                        FormattedValues.Append($"{values.ElementAt(index)}, "); 
                    }
                }
                else 
                {
                    if (index == 0)
                    {
                        FormattedValues.Append($"{values.ElementAt(index)},");
                    }
                    else if (index == values.Count - 1)
                    {
                        FormattedValues.Append($"{values.ElementAt(index)}");
                    }
                    else
                    {
                        FormattedValues.Append($"{values.ElementAt(index)}");
                    }
                }                

                index++;
            }
                return FormattedValues.ToString();
        }
    }//END class SQLiteHandler
}//END namespace MyPortfolio.Database