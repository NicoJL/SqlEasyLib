using System;
using System.Data;
using System.Data.SqlClient;

namespace SqlEasyLib
{
	public static class SqlEasy
	{
    /// <summary>
    /// Return a SqlConnectionStringBuilder to alow to create a SqlConnection.
    /// </summary>
    /// <param name="dataSource">Server Name</param>
    /// <param name="user">UserId</param>
    /// <param name="pass">Password</param>
    /// <param name="db">Database Name</param>
    /// <returns>SqlConnectionStringBuilder</returns>
		public static SqlConnectionStringBuilder ConnectBase(string dataSource, string user, string pass,string db) {
      SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
      builder.DataSource = dataSource;
      builder.UserID = user;
      builder.Password = pass;
      builder.InitialCatalog = db;
      
      return builder;
    }

    /// <summary>
    /// Return a DataTable filled from a sql select query.
    /// </summary>
    /// <param name="connection">connection name</param>
    /// <param name="query">SQL Query</param>
    /// <returns>DataTable</returns>
    public static DataTable GetData(this SqlConnectionStringBuilder connection, string query) {
      DataTable dt = new DataTable();
      try {
        using (SqlConnection con = new SqlConnection(connection.ConnectionString))
        {
          con.Open();
          SqlCommand command = con.CreateCommand();
          command.Connection = con;
          command.CommandText = query;
          SqlDataAdapter adapter = new SqlDataAdapter(command);
          adapter.SelectCommand.CommandTimeout = 1200;
          adapter.Fill(dt);
        }
      } catch(Exception e) {
        Console.WriteLine(e.Message);
			}
      
      return dt;
		}

    public static DataTable GetData(this SqlConnection connection, string query, ref SqlCommand command) {
      DataTable dt = new DataTable();
      try
      {
        //command = connection.CreateCommand();
        command.CommandText = query;
        SqlDataAdapter adapter = new SqlDataAdapter(command);
        adapter.SelectCommand.CommandTimeout = 1200;
        adapter.Fill(dt);
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
      }
      return dt;
		}

    public static int InsertAndGetLastId(this SqlConnection connection, string query,SqlCommand command)
    {
      Int32 id = 0;
      try
      {
        query = $"{query}; select cast(scope_identity() AS int)";
        command.CommandText = query;
        id = (Int32)command.ExecuteScalar();
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
      }

      return (int)id;
    }

    /// <summary>
    /// This function execute insert,update and delete operations
    /// </summary>
    /// <param name="connection">connection name</param>
    /// <param name="query">SQL Query</param>
    /// <returns>int value with number of affected rows</returns>
    public static int ModifyData(this SqlConnectionStringBuilder connection, string query) {
      int affectedRows = 0;
      try {
        using (SqlConnection con = new SqlConnection(connection.ConnectionString))
        {
          con.Open();
          SqlCommand command = con.CreateCommand();
          command.Connection = con;
          command.CommandText = query;
          affectedRows = command.ExecuteNonQuery();
        }
      }
      catch (Exception e)
      {
        Console.WriteLine(e.Message);
      }

      return affectedRows;
		}
	}
}
