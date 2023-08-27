using System.Reflection.Metadata.Ecma335;
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
    public void IfSuccess_WhenGenericSuccessful_CallsAction()
    {
        //Arrange
        var isCalled = false;
        var sut = new Result<string>("Test");

        //Act
        sut.IfSuccess(() => isCalled = true);

        //Assert
        Assert.True(sut.IsSuccess);
        Assert.True(isCalled);
    }

    [Fact]
    public void IfSuccess_WhenGenericSuccessful_CallsFunction()
    {
        //Arrange
        var isCalled = false;
        var sut = new Result<string>("Test");

        //Act
        var result = sut.IfSuccess(s =>
        {
            isCalled = true;
            return s += " test";
        });

        //Assert
        Assert.True(result.IsSuccess);
        Assert.True(isCalled);
        Assert.Equal("Test test", result.Value);
    }

    [Fact]
    public void IfSuccess_WhenGenericSuccessful_CallsGenericAction()
    {
        //Arrange
        var isCalled = false;
        var sut = new Result<string>("Test");

        //Act
        sut.IfSuccess(s =>
        {
            isCalled = true;
        });

        //Assert
        Assert.True(sut.IsSuccess);
        Assert.True(isCalled);
        Assert.Equal("Test", sut.Value);
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
    public void IfSuccess_WhenGenericException_DoesntCallAction()
    {
        //Arrange
        var isCalled = false;
        var sut = new Result<string>(new Exception("Test"));

        //Act
        sut.IfSuccess(() => isCalled = true);

        //Assert
        Assert.False(sut.IsSuccess);
        Assert.False(isCalled);
    }

    [Fact]
    public void IfSuccess_WhenGenericException_DoesntCallGenericAction()
    {
        //Arrange
        var isCalled = false;
        var sut = new Result<string>(new Exception("Test"));

        //Act
        sut.IfSuccess(s => isCalled = true);

        //Assert
        Assert.False(sut.IsSuccess);
        Assert.False(isCalled);
    }

    [Fact]
    public void IfSuccess_WhenGenericException_DoesntCallFunction()
    {
        //Arrange
        var isCalled = false;
        var sut = new Result<string>(new Exception("Test"));

        //Act
        var result = sut.IfSuccess(s =>
        {
            isCalled = true;
            return s += " test";
        });

        //Assert
        Assert.False(sut.IsSuccess);
        Assert.False(isCalled);
        Assert.Null(result.Value);
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
    public void IfFailure_WhenGenericException_CallsAction()
    {
        //Arrange
        var isCalled = false;
        var sut = new Result<string>(new ApplicationException("Test"));

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
    public void IfFailure_WhenGenericException_CallsFunction()
    {
        //Arrange
        var sut = new Result<string>(new ApplicationException("Test"));

        //Act
        var result = sut.IfFailure(e => e.Message);

        //Assert
        Assert.False(sut.IsSuccess);
        Assert.True(result.IsSuccess);
        Assert.Equal("Test", result.Value);
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
    public void Match_WhenGenericSuccessful_CallsSuccessAction()
    {
        //Arrange
        var successIsCalled = false;
        var failureIsCalled = false;
        var sut = new Result<string>("Hello");

        //Act
        var result = sut.Match(s =>
        {
            successIsCalled = true;
            return s + " world";
        }, 
        exception =>
        {
            failureIsCalled = true;
            return exception.Message;
        });

        //Assert
        Assert.True(sut.IsSuccess);
        Assert.True(result.IsSuccess);
        Assert.True(successIsCalled);
        Assert.False(failureIsCalled);
        Assert.Equal("Hello", sut.Value);
        Assert.Equal("Hello world", result.Value);
    }

    [Fact]
    public void Match_WhenException_CallsFailureAction()
    {
        //Arrange
        var successIsCalled = false;
        var failureIsCalled = false;
        var sut = new Result(new ApplicationException("Test"));

        //Act
        sut.Match(() => successIsCalled = true, exception => failureIsCalled = true);

        //Assert
        Assert.False(sut.IsSuccess);
        Assert.False(successIsCalled);
        Assert.True(failureIsCalled);
    }


    [Fact]
    public void Match_WhenException_CallsGenericFailureAction()
    {
        //Arrange
        var successIsCalled = false;
        var failureIsCalled = false;
        var sut = new Result<string>(new ApplicationException("Test"));

        //Act
        var result = sut.Match(s =>
            {
                successIsCalled = true;
                return s + " world";
            },
            exception =>
            {
                failureIsCalled = true;
                return exception.Message;
            });

        //Assert
        Assert.False(sut.IsSuccess);
        Assert.True(result.IsSuccess);
        Assert.False(successIsCalled);
        Assert.True(failureIsCalled);
        Assert.Equal("Test", result.Value);
    }

    [Fact]
    public void Map_WhenGenericSuccess_MapToInt()
    {
        //Arrange
        var successIsCalled = false;
        var failureIsCalled = false;
        var sut = new Result<string>("1");

        //Act
        var result = sut.Map(s=>
        {
            successIsCalled = true;
            return int.Parse(s);
        });

        //Assert
        Assert.True(sut.IsSuccess);
        Assert.True(result.IsSuccess);
        Assert.True(successIsCalled);
        Assert.False(failureIsCalled);
        Assert.Equal(1, result.Value);
    }

    [Fact]
    public void Map_WhenException_DoesntCallFunction()
    {
        //Arrange
        var successIsCalled = false;
        var sut = new Result<string>(new ApplicationException("Test"));

        //Act
        var result = sut.Map(s =>
        {
            successIsCalled = true;
            return int.Parse(s);
        });

        //Assert
        Assert.False(sut.IsSuccess);
        Assert.False(result.IsSuccess);
        Assert.False(successIsCalled);
    }

    [Fact]
    public void Map_WhenGenericSuccessWithFailureHandling_MapToInt()
    {
        //Arrange
        var successIsCalled = false;
        var failureIsCalled = false;
        var sut = new Result<string>("1");

        //Act
        var result = sut.Map(s =>
        {
            successIsCalled = true;
            return int.Parse(s);
        }, exception =>
        {
            failureIsCalled = true;
            return 0;
        } );

        //Assert
        Assert.True(sut.IsSuccess);
        Assert.True(result.IsSuccess);
        Assert.True(successIsCalled);
        Assert.False(failureIsCalled);
        Assert.Equal(1, result.Value);
    }

    [Fact]
    public void Map_WhenExceptionWithFailureHandling_CallsFailureFunction()
    {
        //Arrange
        var successIsCalled = false;
        var failureIsCalled = false;
        var sut = new Result<string>(new ApplicationException("Test"));

        //Act
        var result = sut.Map(s =>
        {
            successIsCalled = true;
            return int.Parse(s);
        }, exception =>
        {
            failureIsCalled = true;
            return 0;
        });

        //Assert
        Assert.False(sut.IsSuccess);
        Assert.True(result.IsSuccess);
        Assert.True(failureIsCalled);
        Assert.False(successIsCalled);
        Assert.Equal(0, result.Value);
    }

}