using System;
using Castle.MicroKernel.Registration;

namespace RabbitCache.Extensions
{
    public static class WindsorExtensions
    {
        public static ComponentRegistration<T> Override<T>(this ComponentRegistration<T> _componentRegistration) where T : class
        {
            return _componentRegistration.Named(Guid.NewGuid().ToString()).IsDefault();
        }
    }
}