using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleBinder.ValueProvider;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace SimpleBinder.Test
{
    class Person
    {
        public int? age { get; set; }
        public string name { get; set; }
    }

    class Family
    {
        public string lastname { get; set; }
        public IEnumerable<Person> familymembers { get; set; }
    }

    class Food
    {
        public IEnumerable<string> item { get; set; }
    }

    [Template("mem_{iteration}_{contextName}")]
    class PersonWithTempate : Person
    { 
        
    }

    class FamilyWithTemplate
    {
        public string lastname { get; set; }
        public IEnumerable<PersonWithTempate> familymembers { get; set; }
    }

    [Template("fri_{iteration}_{contextName}")]
    class Friend
    {
        public string name { get; set; }
    }

    [Template("mem_{iteration}_{contextName}")]
    class PersonWithFriends : Person
    {
        public IEnumerable<Friend> friends { get; set; }
    }

    class FamilyWithFriends
    {
        public string lastname { get; set; }
        public IEnumerable<PersonWithFriends> familymembers { get; set; }
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

        [TestMethod]
        public void TestSimpleList()
        {
            string value = "item1=hotdog&item2=fries";

            var model = Binder.Bind<Food>(new QueryStringValueProvider(value));
            Assert.IsNotNull(model);

            Assert.AreEqual(2, model.item.Count());
            Assert.IsTrue(model.item.Contains("hotdog"));
            Assert.IsTrue(model.item.Contains("fries"));
        }

        [TestMethod]
        public void TestComplexList()
        {
            string value = "lastname=crosby&name1=michael&age1=34&name2=koye&age2=2";

            var model = Binder.Bind<Family>(new QueryStringValueProvider(value));
            Assert.IsNotNull(model);

            Assert.AreEqual(2, model.familymembers.Count());
            Assert.AreEqual("crosby", model.lastname);

            var michael = model.familymembers.Single(m => m.name == "michael");
            Assert.AreEqual(34, michael.age);

            var koye = model.familymembers.Single(m => m.name == "koye");
            Assert.AreEqual(2, koye.age);
        }

        [TestMethod]
        public void TestComplexListWithTemplate()
        {
            string value = "lastname=crosby&mem_1_name=michael&mem_1_age=34&mem_2_name=koye&mem_2_age=2";

            var model = Binder.Bind<FamilyWithTemplate>(new QueryStringValueProvider(value));
            Assert.IsNotNull(model);

            Assert.AreEqual(2, model.familymembers.Count());
            Assert.AreEqual("crosby", model.lastname);

            var michael = model.familymembers.Single(m => m.name == "michael");
            Assert.AreEqual(34, michael.age);

            var koye = model.familymembers.Single(m => m.name == "koye");
            Assert.AreEqual(2, koye.age);
        }

        [TestMethod]
        public void TestComplexListWithSubList()
        {
            string value = "lastname=crosby&mem_1_name=michael&mem_1_age=34&mem_2_name=koye&mem_2_age=2&mem_1_fri_1_name=sam&mem_1_fri_2_name=matt";

            var model = Binder.Bind<FamilyWithFriends>(new QueryStringValueProvider(value));
            Assert.IsNotNull(model);

            Assert.AreEqual(2, model.familymembers.Count());
            Assert.AreEqual("crosby", model.lastname);

            var michael = model.familymembers.Single(m => m.name == "michael");
            Assert.AreEqual(34, michael.age);
            Assert.AreEqual(2, michael.friends.Count());


            var koye = model.familymembers.Single(m => m.name == "koye");
            Assert.AreEqual(2, koye.age);
        }
    }
}
