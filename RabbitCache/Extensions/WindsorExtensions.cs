using System;
using Castle.MicroKernel.Registration;

namespace RabbitCache.Extensions
{
    public static class WindsorExtensions
    {
        public static ComponentRegistration<T> Override<T>(this ComponentRegistration<T> _componentRegistration) where T : class
        {
            return _componentRegistration.Override(Guid.NewGuid().ToString());
        }
        public static ComponentRegistration<T> Override<T>(this ComponentRegistration<T> _componentRegistration, string _name) where T : class
        {
            return _componentRegistration.Named(_name).IsDefault();
        }
    }
}