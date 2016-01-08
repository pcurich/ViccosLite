using System;
using System.Data.Entity;
using Base.Test;
using NUnit.Framework;
using ViccosLite.Data.Entities;

namespace VicoosLite.Data.Test
{
    [TestFixture]
    public class SchemaTest
    {
        [Test]
        public void Can_generate_schema()
        {
            Database.SetInitializer<SoftContext>(null);
            var ctx = new SoftContext("Test");
            var result = ctx.CreateDatabaseScript();
            result.ShouldNotBeNull();
            Console.Write(result);
        }
    }
}