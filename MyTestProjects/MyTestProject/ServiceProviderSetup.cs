﻿using System;
using System.Reflection;
using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using MyTestProject.Services;
using MyTestProject.Utils;

namespace MyTestProject
{
  public static class ServiceProviderSetup
  {
    /// <summary>
    /// runtime register DI
    /// </summary>
    public static IServiceProvider CreateServiceProvider(this IServiceCollection services)
    {
      var container = new WindsorContainer();

      container.RegisterTypes(Assembly.GetExecutingAssembly());
      services.AddSingleton<ILocationService, LocationService>();

      return WindsorRegistrationHelper.CreateServiceProvider(container, services);
    }

    private static void RegisterTypes(this IWindsorContainer container, Assembly assembly)
    {
      container.Register(Types.FromAssembly(assembly).Pick()
                    .WithService.DefaultInterfaces().If(t => Attribute.IsDefined(t, typeof(InjectableAttribute)))
                    .Configure(s => new Func<ComponentRegistration<object>>[] { s.LifestyleTransient, s.LifestyleSingleton }[(int)((Attribute.GetCustomAttribute(s.Implementation, typeof(InjectableAttribute)) as InjectableAttribute).Lifetime)].Invoke()));
    }

    /// <summary>
    /// integration tests
    /// </summary>
    public static IServiceProvider CreateStubServiceProvider(this IServiceCollection services)
    {
      var container = new WindsorContainer();
      container.RegisterTypes(Assembly.GetExecutingAssembly());
      services.AddSingleton<ILocationService, LocationService>();

      return WindsorRegistrationHelper.CreateServiceProvider(container, services);
    }
  }
}
