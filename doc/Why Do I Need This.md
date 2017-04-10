Consider the following unit tests:

````C#
[TestClass](TestClass)
public class SimpleAsyncUnitTests
{
  // A simple test that should pass; it throws an exception from an async continuation.
  [TestMethod](TestMethod)
  [ExpectedException(typeof(InvalidOperationException))](ExpectedException(typeof(InvalidOperationException)))
  public async void ShouldPass()
  {
    await Task.Yield(); // Change this to "TaskEx.Yield" on VS2010.
    throw new InvalidOperationException();
  }

  // A simple test that should fail; it fails an assertion from an async continuation.
  [TestMethod](TestMethod)
  public async void ShouldFail()
  {
    await Task.Yield(); // Change this to "TaskEx.Yield" on VS2010.
    Assert.Fail();
  }
}
````

If you run these tests, you'll get this result:

![Passing tests that should fail - Failing tests that should pass!](Why%20Do%20I%20Need%20This_BadMSTestBad.png)

_That'll_ mess up your Red/Green/Refactor cycle!!!

## Official Recommendation

The reason async unit tests don't work as expected is because they do not have a proper context.

The [Async CTP team](http://msdn.microsoft.com/en-us/vstudio/gg316360) has put together a context suitable for unit testing; if you have the Async CTP installed, it is under `My Documents\Microsoft Visual Studio Async CTP\Samples\(C# Testing) Unit Testing`, and it is called `GeneralThreadAffineContext`.

After copying the source files from that directory into your test project, you can use it like this:

````C#
[TestClass](TestClass)
public class SimpleAsyncUnitTests
{
  // A simple test that should pass; it throws an exception from an async continuation.
  [TestMethod](TestMethod)
  [ExpectedException(typeof(InvalidOperationException))](ExpectedException(typeof(InvalidOperationException)))
  public void ShouldPass()
  {
    GeneralThreadAffineContext.Run(async () =>
    {
      await Task.Yield(); // Change this to "TaskEx.Yield" on VS2010.
      throw new InvalidOperationException();
    }
  }

  // A simple test that should fail; it fails an assertion from an async continuation.
  [TestMethod](TestMethod)
  public void ShouldFail()
  {
    GeneralThreadAffineContext.Run(async () =>
    {
      await Task.Yield(); // Change this to "TaskEx.Yield" on VS2010.
      Assert.Fail();
    }
  }
}
````

Now, each test has its own context. The tests are no longer async methods; rather, they set up the context and then run an async lambda within that context. The async lambda is the _real_ test.

## There Must Be a Better Way

Copy files into each of your test projects?

Change **every** asynchronous test method to contain its own context?

There must be a better way. [And now there is!](Getting%20Started.md)