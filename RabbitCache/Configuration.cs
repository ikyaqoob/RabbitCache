﻿using System;
using System.Configuration;
using System.Text.RegularExpressions;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using EasyNetQ;
using log4net;
using RabbitCache.Caches.Serialization;
using RabbitCache.Contracts;
using RabbitCache.Helpers;
using RabbitCache.Interfaces;

namespace RabbitCache
{
    public class Configuration : Singleton<Configuration>
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(Configuration));

        private IBus _rabbitMqCacheBus;
        private IWindsorContainer _windsorContainer;

        internal static string ServiceBusName
        {
            get { return ConfigurationManager.AppSettings["SERVICE_BUS_NAME"]; }
        }

        internal static IBus RabbitMqCacheBus
        {
            get
            {
                if (Configuration.Instance._rabbitMqCacheBus == null || !Configuration.Instance._rabbitMqCacheBus.IsConnected)
                {
                    if (Configuration.Instance._rabbitMqCacheBus != null)
                        Configuration.Instance._rabbitMqCacheBus.Dispose();

                    Configuration.Instance._rabbitMqCacheBus = Configuration.WindsorContainer.Resolve<IBus>(Configuration.ServiceBusName);
                }

                return Configuration.Instance._rabbitMqCacheBus;
            }
        }
        internal static IWindsorContainer WindsorContainer
        {
            get
            {
                if (Configuration.Instance._windsorContainer == null)
                    throw new NullReferenceException("_windsorContainer");

                return Configuration.Instance._windsorContainer;
            }
        }
  
        public static void Initialize(IWindsorContainer _windsorContainer = null)
        {
            if (_windsorContainer == null)
                _windsorContainer = new WindsorContainer();
            
            try
            {
                Configuration.Instance._windsorContainer = 
                    _windsorContainer
                        .Register(Component.For<IService>().ImplementedBy<Service>().LifeStyle.Transient.IsFallback())
                        .Register(Component.For<ISerializer>().ImplementedBy<CacheEntryJsonSerializer>().LifeStyle.Transient.IsFallback())
                        .Register(Component.For<IConsumerErrorStrategy>().ImplementedBy<CacheConsumerErrorStrategy>().LifeStyle.Transient.IsFallback())
                        .Register(Component.For<IBus>().Named(Configuration.ServiceBusName).UsingFactoryMethod(() =>
                            RabbitHutch.CreateBus(Dependency.OnAppSettingsValue("CONNECTION_STRING").Value, _x => _x
                                .Register(_y => (SerializeType)Configuration.TypeNameSerializer.Serialize)
                                .Register(_y => _windsorContainer.Resolve<ISerializer>())
                                .Register(_y => _windsorContainer.Resolve<IConsumerErrorStrategy>())))
                        .LifeStyle.Singleton.IsFallback());

                Configuration._logger.Info("RabbitCache Initialized");
            }
            catch (Exception _ex)
            {
                throw new SystemException(string.Format("Fatal IoC initialization error. Program stopped.{0}  ==> {1}", System.Environment.NewLine, _ex.Message), _ex);
            }
        }
        public static void Shutdown()
        {
            if (Configuration.Instance._rabbitMqCacheBus != null)
            {
                Configuration.Instance._rabbitMqCacheBus.Dispose();
                Configuration.Instance._rabbitMqCacheBus = null;
            }

            if (Configuration.Instance._windsorContainer != null)
            {
                Configuration.Instance._windsorContainer.Dispose();
                Configuration.Instance._windsorContainer = null;
            }

            Configuration._logger.Info("RabbitCache Shutdown");
        }

        // Nested class
        public static class TypeNameSerializer
        {
            public static string Serialize(Type _type)
            {
                var _fullName = _type.FullName;
                if (_fullName.Contains("["))
                    _fullName = Regex.Replace(_fullName, @"(?<generic>[^\[`]+)(`1\[\[)(?<internal>[^,]+)(, [^\]]+)(\]\])", "${generic}<${internal}>");

                return _fullName.Replace('.', '_');
            }
        }
    }
}