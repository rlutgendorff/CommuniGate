using CommuniGate.Results;

namespace CommuniGate.Tests.Results;

public class ResultTests
{
    [Fact]
    public void IfSuccess_WhenSuccessful_CallsAction()
    {
        //Arrange
        var isCalled = false;
        var sut = new Result();

        //Act
        sut.IfSuccess(() => isCalled = true);

        //Assert
        Assert.True(sut.IsSuccess);
        Assert.True(isCalled);
    }

    [Fact]
    public void IfSuccess_WhenException_DoesntCallAction()
    {
        //Arrange
        var isCalled = false;
        var sut = new Result(new Exception("Test"));

        //Act
        sut.IfSuccess(() => isCalled = true);

        //Assert
        Assert.False(sut.IsSuccess);
        Assert.False(isCalled);
    }

    [Fact]
    public void IfFailure_WhenException_CallsAction()
    {
        //Arrange
        var isCalled = false;
        var sut = new Result(new ApplicationException("Test"));

        //Act
        sut.IfFailure((e) =>
        {
            isCalled = true;
            Assert.IsType<ApplicationException>(sut.Exception);
        });

        //Assert
        Assert.False(sut.IsSuccess);
        Assert.True(isCalled);
    }

    [Fact]
    public void IfFailure_WhenSuccessful_DoesntCallAction()
    {
        //Arrange
        var isCalled = false;
        var sut = new Result();

        //Act
        sut.IfFailure((e) =>
        {
            isCalled = true;
        });

        //Assert
        Assert.True(sut.IsSuccess);
        Assert.False(isCalled);
    }

    [Fact]
    public void Match_WhenSuccessful_CallsSuccessAction()
    {
        //Arrange
        var successIsCalled = false;
        var failureIsCalled = false;
        var sut = new Result();

        //Act
        sut.Match(() => successIsCalled = true, exception =>  failureIsCalled = true);

        //Assert
        Assert.True(sut.IsSuccess);
        Assert.True(successIsCalled);
        Assert.False(failureIsCalled);
    }

    [Fact]
    public void Match_WhenException_CallsFailureAction()
    {
        //Arrange
        var successIsCalled = false;
        var failureIsCalled = false;
        var sut = new Result(new ApplicationException("Test"));

        //Act
        sut.Match(
            () => successIsCalled = true,
            exception => failureIsCalled = true);

        //Assert
        Assert.False(sut.IsSuccess);
        Assert.False(successIsCalled);
        Assert.True(failureIsCalled);
    }

}