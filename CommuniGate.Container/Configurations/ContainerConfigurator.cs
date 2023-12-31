﻿using System.Reflection;
using CommuniGate.Container.Abstraction.Configurations;

namespace CommuniGate.Container.Configurations;

public class ContainerConfigurator
{
    public static void Configure<TContainer>(SimpleInjector.Container container, Assembly[] assemblies)
    {
        var desiredInterface = typeof(IContainerConfiguration<>);
        var desiredGenericType = typeof(TContainer);

        foreach (var assembly in assemblies)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (!IsOfInterface(type, desiredInterface, desiredGenericType)) continue;

                var instance = Activator.CreateInstance(type); // Create an instance of the type
                var methodInfo = type.GetMethod("Register"); // Replace with your method name
                if (methodInfo == null) continue;

                var @params = new object[]
                {
                    container,
                    assemblies
                };

                methodInfo.Invoke(instance, @params);
            }
        }
    }

    private static bool IsOfInterface(Type type, Type desiredInterface, Type desiredGenericType)
    {
        if (type.IsClass && 
            !type.IsAbstract && 
            type.GetInterfaces().Any(i => 
                i.IsGenericType && 
                i.GetGenericTypeDefinition() == desiredInterface &&
                i.GetGenericArguments()[0] == desiredGenericType)
            )
        {
            return true;
        }

        return type.GetInterfaces().Any(interfaceType => IsOfInterface(interfaceType, desiredInterface, desiredGenericType));
    }
}