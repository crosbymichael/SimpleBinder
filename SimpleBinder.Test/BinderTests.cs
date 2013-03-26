using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleBinder.ValueProvider;

namespace SimpleBinder.Test
{
    class Person
    {
        public int? age { get; set; }
        public string name { get; set; }
    }

    [TestClass]
    public class BinderTests
    {
        [TestMethod]
        public void TestSimpleClass()
        {
            string value = "name=michael&age=25";

            var model = Binder.Bind<Person>(new QueryStringValueProvider(value));

            Assert.IsNotNull(model);
            Assert.AreEqual("michael", model.name);
            Assert.AreEqual(25, model.age);
        }
    }
}
