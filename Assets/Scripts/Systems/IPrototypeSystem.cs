using System.Collections;
using System.Data;

namespace Gamepackage
{
    public interface IPrototypeSystem
    {
        TPrototype GetPrototypeByUniqueIdentifier<TPrototype>(string uniqueIdentifier) where TPrototype : IPrototype;
        void LoadAllPrototypes(IDbConnection dbConnection);
    }
}
