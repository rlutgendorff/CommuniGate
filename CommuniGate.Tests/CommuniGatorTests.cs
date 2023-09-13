using CommuniGate.Abstraction.Commands;
using CommuniGate.Commands;
using CommuniGate.Extensions;
using CommuniGate.TestHelpers.TestObjects;
using CommuniGate.TestHelpers.TestObjects.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace CommuniGate.Tests
{
    public class CommuniGatorTests
    {
        private readonly IServiceProvider _serviceProvider;

        public CommuniGatorTests()
        {
            var services = new ServiceCollection();
            services.AddTransient<ITestService, TestService>();

            services.AddCommuniGate(new[] { GetType().Assembly, typeof(TestService).Assembly, typeof(ICommandHandler<,>).Assembly, typeof(CommuniGator).Assembly });

            _serviceProvider = services.BuildServiceProvider();
            _serviceProvider.UseCommuniGate();
        }

        [Fact]
        public async Task TestQueryHandler()
        {
            //Arrange
            var sut = _serviceProvider.GetService<ICommuniGator>();

            //Act
            var result = await sut!.Execute(new TestQuery{Name = "Rolf"}, CancellationToken.None);

            //Assert
            Assert.False(result.IsSuccess);
            Assert.IsType<ApplicationException>(result.Exception);
        }

        [Fact]
        public async Task WithResultCommandHandler()
        {
            //Arrange
            var sut = _serviceProvider.GetService<ICommuniGator>();

            //Act
            var result = await sut!.Execute(new WithResultCommand { Test = 1}, CancellationToken.None);

            //Assert
        }

        [Fact]
        public async Task WithoutResultCommandHandler()
        {
            //Arrange
            var sut = _serviceProvider.GetService<ICommuniGator>();

            //Act
            var result = await sut!.Execute(new WithoutResultCommand(), CancellationToken.None);

            //Assert
        }

        [Fact]
        public async Task Execute_ThrowsException_ResultIsFailed()
        {
            //Arrange
            var sut = _serviceProvider.GetService<ICommuniGator>();

            //Act
            var result = await sut!.Execute(new ExceptionCommand(), CancellationToken.None);

            //Assert
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task Execute_WithFuncTask_FuncExecuted()
        {
            //Arrange
            var sut = _serviceProvider.GetService<ICommuniGator>();

            //Act
            await sut!.Execute(container =>
            {
                var handler = container.GetInstance(typeof(ICommandHandler<WithoutResultCommand>));
                return Task.CompletedTask;
            });

            //Assert
        }

        [Fact]
        public async Task Execute_()
        {
            //Arrange
            var sut = _serviceProvider.GetService<ICommuniGator>();

            //Act
            var result = await sut!.Execute(container =>
            {
                var handler = (WithoutResultCommandHandler)container.GetInstance(typeof(ICommandHandler<WithoutResultCommand>));
                return handler.HandleAsync(new WithoutResultCommand());
            });

            //Assert
        }

        [Fact]
        public async Task Execute_2()
        {
            //Arrange
            var sut = _serviceProvider.GetService<ICommuniGator>();

            //Act
            var result = await sut!.Execute<int>(container =>
            {
                var handler = (WithResultCommandHandler)container.GetInstance(typeof(ICommandHandler<WithResultCommand, int>));
                return handler.HandleAsync(new WithResultCommand());
            });

            //Assert
        }
    }
}