using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gamepackage;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tests
{
    [TestClass]
    public class SerializationTests
    {
        [TestMethod]
        public void TestCanSerializeGenericPrototypeReference()
        {
            var r = new PrototypeReference<MotorPrototype>()
            {
                PrototypeUniqueIdentifier = "xx",
                Prototype = new MotorPrototype()
                {
                    UniqueIdentifier = "xx"
                }
            };

            var stringForObject = JsonConvert.SerializeObject(r, Formatting.Indented);
            var objectAfterDeserialization = JsonConvert.DeserializeObject<PrototypeReference<MotorPrototype>>(stringForObject);

            Assert.AreEqual("xx", objectAfterDeserialization.PrototypeUniqueIdentifier);
        }

    }
}
