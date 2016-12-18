using System;
using System.Configuration;
using Microsoft.Practices.Unity;
using RoboBank.Email.Application.Adapters;
using RoboBank.Email.Application.Ports;
using StackExchange.Redis;

namespace RoboBank.Email.Service
{
    public static class UnityConfig
    {
        private static readonly Lazy<IUnityContainer> Container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        public static IUnityContainer GetConfiguredContainer()
        {
            return Container.Value;
        }

        private static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<ICustomerRepository, CustomerRepository>();
            container.RegisterType<IEmailService, EmailService>();

            var redisConnectionString = ConfigurationManager.AppSettings["RedisConnectionString"];
            var connectionMultiplexer = ConnectionMultiplexer.Connect(redisConnectionString);
            container.RegisterInstance(connectionMultiplexer);
        }
    }
}