using Mono.Data.Sqlite;
using System.Collections;
using System.Data;
using System.IO;
using UnityEngine;

namespace Gamepackage
{
    public class ModSystem : IModSystem
    {
        public ModSystem()
        {

        }

        public void LoadAll(IDbConnection dbConnection)
        {
            var dbcmd = dbConnection.CreateCommand();
            dbcmd.CommandText = File.ReadAllText(Application.dataPath + "/GameData.sql");
            dbcmd.ExecuteNonQuery();
            dbcmd.Dispose();
        }
    }
}
