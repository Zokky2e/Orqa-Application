using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orqa_Application.Services
{
    public class ConnectionService
    {
        public MySqlConnection MySqlConnection { get; set; }
        public ConnectionService()
        {
            string sqlConnectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=workstationdb_test";
            MySqlConnection = new MySqlConnection(sqlConnectionString);
        }
    }
}
