using Mono.Data.Sqlite;
using System.IO;
using UnityEngine;

namespace Gamepackage
{
    public class TestSQLLite : MonoBehaviour
    {
        public void Start()
        {
            SqliteConnection con = null;
            try
            {
                string cs = "Data Source=:memory:";
                con = new SqliteConnection(cs);
                con.Open();

                var sql = File.ReadAllText(System.IO.Path.Combine(Application.streamingAssetsPath, "Mods", "Core", "Definitions", "Core.sql"));
                var c1 = new SqliteCommand(sql, con);
                c1.ExecuteNonQuery();

                sql = "select * from Tilesets";
                var c2 = new SqliteCommand(sql, con);
                var reader = c2.ExecuteReader();
                while (reader.Read())
                {
                    Debug.Log("id: " + reader[0].ToString());
                }
            }
            finally
            {
                con.Close();
            }
        }
    }
}
