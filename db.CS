using System.Collections.Generic;
using System;
using System.Linq;
using System.Text.Json;
using System.Net;
using jogos;
using System.IO;
using MySql.Data.MySqlClient;



namespace db
{

    static class DB
    {
        static public MySqlCommand query;

    static public string connectstring = "Server=localhost;Port=3306;Database=pokkoan;Uid=root;Pwd=admin;";
        
    }
    
}