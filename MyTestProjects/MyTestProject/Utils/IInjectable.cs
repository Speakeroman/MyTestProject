using System;

namespace MyTestProject.Utils
{
  public enum LifetimeScope
  {
    Singleton = 1,
  }

  public class InjectableAttribute : Attribute
  {
    public LifetimeScope Lifetime { get; private set; }
    public InjectableAttribute(LifetimeScope lifetime)
    {
      Lifetime = lifetime;
    }
  }
}
