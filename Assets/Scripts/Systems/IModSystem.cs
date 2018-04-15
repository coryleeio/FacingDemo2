using System.Data;

namespace Gamepackage
{
    public interface IModSystem
    {
        void LoadAll(IDbConnection dbConnection);
    }
}
