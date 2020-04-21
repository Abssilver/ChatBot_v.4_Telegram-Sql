using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ChatBot_v._4_Telegram_Sql
{
    static class DataBaseClass
    {
        static SqlConnection connection;
        static DataBaseClass()
        {
            string connectionString = 
                @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=databaseTelegramBotLog;Integrated Security=True;Pooling=True";
            connection = new SqlConnection(connectionString);
            connection.Open();
        }

        public static void Add(Msg userMessage)
        {
            var sql = string.Format(
                @"INSERT INTO MessageDB(userId, userFirstName, userText)
                  VALUES ({0}, N'{1}', N'{2}')",
                userMessage.id,
                userMessage.name,
                userMessage.text
                );
            SqlCommand sqlCommand = new SqlCommand(sql, connection);
            sqlCommand.ExecuteNonQuery();
            //var result = sqlCommand.ExecuteReader();
        }
        public static void Free() 
        {
            connection.Close();
            connection.Dispose(); 
        }
    }
}
