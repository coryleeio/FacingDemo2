using System.Collections;
using System.Data;

namespace Gamepackage
{
    public interface IResourceManager
    {
        TPrototype GetPrototypeByUniqueIdentifier<TPrototype>(string uniqueIdentifier) where TPrototype : IResource;
        void LoadAllPrototypes(IDbConnection dbConnection);
    }
}
