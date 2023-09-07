using CommuniGate.EventSourcing.Abstraction;
using CommuniGate.Extensions;
using CommuniGate.TestHelpers.TestObjects;
using CommuniGate.EventSourcing.Extensions;
using CommuniGate.EventSourcing.Tests.TestObjects;
using CommuniGate.Middlewares;
using CommuniGate.EventSourcing.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace CommuniGate.EventSourcing.Tests.Extensions;

public class CommuniGatorExtensionsTests
{
    private readonly IServiceProvider _serviceProvider;

    public CommuniGatorExtensionsTests()
    {
        var services = new ServiceCollection();
        services.AddTransient<ITestService, TestService>();

        services.AddCommuniGate(new[]
        {
            GetType().Assembly,
            typeof(TestService).Assembly,
            typeof(IEventPipelineMiddleware<>).Assembly,
            typeof(IEventPipelineMiddleware<,>).Assembly
        });

        _serviceProvider = services.BuildServiceProvider();
        _serviceProvider.UseCommuniGate();
    }

    [Fact]
    public async Task Test()
    {
        //Arrange
        var sut = _serviceProvider.GetService<ICommuniGator>();

        var entity = new TestEntity();
        var @event = new TestEntityEvent();

        //Act
        await sut!.Execute(entity, @event);

        //Assert
        //Assert.False(result.IsSuccess);
        //Assert.IsType<ApplicationException>(result.Exception);
    }
}