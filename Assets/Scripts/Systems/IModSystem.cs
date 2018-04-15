using System.Collections;
using System.Data;

namespace Gamepackage
{
    public interface IModSystem
    {
        void PopulateModList();
        void LoadAssemblies();
        void LoadAssetBundles();
        void LoadSqlFiles(IDbConnection dbConnection);
    }
}
