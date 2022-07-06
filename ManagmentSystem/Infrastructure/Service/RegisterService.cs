namespace Infrastructure.Service;

public interface ISingleton
{
    // implement any class from this interface to register the class as "Singleton"
}

public interface IScopped
{
    // implement any class from this interface to register the class as "Scopped"
}

public static class RegisterService
{
    public static void Register<ServiceType>(this IServiceCollection services)
    {
        IEnumerable<Type> myServices = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes());

        myServices.Where(service => typeof(ServiceType).IsAssignableFrom(service) && service != typeof(ServiceType))
            .ToList().ForEach((service) =>
            {
                Type? interfaceType = myServices.FirstOrDefault(x => x.Name == $"I{service.Name.Replace("Service", "")}");

                if (interfaceType == null && typeof(ServiceType) == typeof(IScopped))
                {
                    services.AddScoped(service);
                }
                else if (interfaceType == null && typeof(ServiceType) == typeof(ISingleton))
                {
                    services.AddSingleton(service);
                }
                else if (interfaceType != null && typeof(ServiceType) == typeof(IScopped))
                {
                    services.AddScoped(interfaceType, service);
                }
                else if (interfaceType != null && typeof(ServiceType) == typeof(ISingleton))
                {
                    services.AddSingleton(interfaceType, service);
                }
            });
    }
}
