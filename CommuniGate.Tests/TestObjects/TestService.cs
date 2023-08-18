namespace CommuniGate.Tests.TestObjects;

public interface ITestService
{
    string Test();
}

public class TestService : ITestService
{
    public string Test()
    {
        return "Test";
    }
}