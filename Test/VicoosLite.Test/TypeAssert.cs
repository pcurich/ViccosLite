using System;
using NUnit.Framework;

namespace Base.Test
{
    public static class TypeAssert
    {
        public static void AreEqual(object expected, object instance)
        {
            if (expected == null)
                Assert.IsNull(instance);
            else
                Assert.IsNotNull(instance, "Instancia Nula");
            if (expected != null)
                Assert.AreEqual(expected.GetType(), instance.GetType(),
                    "Esperaba: " + expected.GetType() + ", era: " + instance.GetType() + " no era del tipo " +
                    instance.GetType());
        }

        public static void AreEqual(Type expected, object instance)
        {
            if (expected == null)
                Assert.IsNull(instance);
            else
                Assert.IsNotNull(instance, "Instance was null");

            if (instance != null)
                Assert.AreEqual(expected, instance.GetType(),
                    "Esperaba: " + expected + ", era: " + instance.GetType() + " no era del tipo " + instance.GetType());
        }

        public static void Equals<T>(object instance)
        {
            AreEqual(typeof(T), instance);
        }

        public static void Is<T>(object instance)
        {
            Assert.IsTrue(instance is T, "Instancia " + instance + " no era del tipo " + typeof(T));
        }
    }
}