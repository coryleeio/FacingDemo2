using System;
using System.Linq;

namespace Gamepackage
{

    public static class TypeExtension
    {
        public static Type[] ConcreteFromInterface(this Type type)
        {
            var listOfTypes = (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                               from assemblyType in domainAssembly.GetTypes()
                               where type.IsAssignableFrom(assemblyType)
                               where type != assemblyType
                               select assemblyType).ToArray();
            return listOfTypes;
        }

        public static Type[] ConcreteFromAbstract(this Type type)
        {
            var listOfBs = (from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                            from assemblyType in domainAssembly.GetExportedTypes()
                            where type.IsAssignableFrom(assemblyType)
                            where (!assemblyType.IsAbstract)
                            select assemblyType).ToArray();
            return listOfBs;
        }
    }

}