using System;

namespace RabbitCache.Helpers
{
    public abstract class Singleton<T> where T : class, new()
    {
        protected Singleton() { }

        // ReSharper disable ClassNeverInstantiated.Local
        private class SingletonCreator
            // ReSharper restore ClassNeverInstantiated.Local
        {
            static SingletonCreator() { }

            // ReSharper disable StaticFieldInGenericType
            internal static readonly T _instance = (T)Activator.CreateInstance(typeof(T));
            // ReSharper restore StaticFieldInGenericType
        }

        protected internal static T Instance
        {
            get { return SingletonCreator._instance; }
        }
    }
}