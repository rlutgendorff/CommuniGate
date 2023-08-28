namespace CommuniGate.Tests.TestObjects;

public interface ITestService
{
    string Test(string name);
}

public class TestService : ITestService
{
    public string Test(string name)
    {
        return $"Hello {name}";
    }
}