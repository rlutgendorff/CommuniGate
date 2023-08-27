using CommuniGate.Extensions;
using CommuniGate.Tests.TestObjects;
using CommuniGate.Tests.TestObjects.Handlers;
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

            services.AddCommuniGate(new[] { this.GetType().Assembly });

            _serviceProvider = services.BuildServiceProvider();
            _serviceProvider.UseCommuniGate();
        }

        [Fact]
        public async Task TestQueryHandler()
        {
            //Arrange
            var sut = _serviceProvider.GetService<ICommuniGator>();

            //Act
            var result = await sut.Execute(new TestQuery{Name = "Rolf"}, CancellationToken.None);

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
            var result = await sut.Execute(new WithResultCommand { Test = 1}, CancellationToken.None);

            //Assert
        }

        [Fact]
        public async Task WithoutResultCommandHandler()
        {
            //Arrange
            var sut = _serviceProvider.GetService<ICommuniGator>();

            //Act
            var result = await sut.Execute(new WithoutResultCommand(), CancellationToken.None);

            //Assert
        }

        [Fact]
        public async Task Execute_TestEventHandler()
        {
            //Arrange
            var calledPipelines = new List<string>();
            TestEventPipelineMiddleware.OnHandling += (sender, _) =>
            {
                var name = sender?.GetType().Name;
                if (name != null) calledPipelines.Add(name);
            };

            var calledHandlers = new List<string>();
            TestEventHandler.OnHandling += (sender, _) =>
            {
                var name = sender?.GetType().Name;
                if (name != null) calledHandlers.Add(name);
            };

            TestEventHandler2.OnHandling += (sender, _) =>
            {
                var name = sender?.GetType().Name;
                if (name != null) calledHandlers.Add(name);
            };

            var sut = _serviceProvider.GetService<ICommuniGator>();

            //Act
            await sut.Publish(new TestEvent(), CancellationToken.None);

            //Assert
            Assert.Contains(nameof(TestEventPipelineMiddleware), calledPipelines);
            Assert.Collection(calledHandlers, 
                item => Assert.Contains(nameof(TestEventHandler), item), 
                item => Assert.Contains(nameof(TestEventHandler2), item));
        }

        [Fact]
        public async Task Execute_ThrowsException_ResultIsFailed()
        {
            //Arrange

            var sut = _serviceProvider.GetService<ICommuniGator>();

            //Act
            var result = await sut.Execute(new ExceptionCommand(), CancellationToken.None);

            //Assert
            Assert.False(result.IsSuccess);
        }
    }
}