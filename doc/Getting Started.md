Let's take those tests that do not work as expected:

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

To give these tests an async-compatible context, all we have to do is [install the NuGet package](https://nuget.org/packages/AsyncUnitTests-MSTest), and change `[TestClass]` to `[AsyncTestClass]`.

````C#
using Nito.AsyncEx.UnitTests; // THIS LINE WAS ADDED

[AsyncTestClass](AsyncTestClass) // THIS LINE WAS CHANGED
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

That's it! Two lines of code and we're good to go!

![Much Better!](Getting%20Started_OKMSTestIForgiveYou.png)

`[AsyncTestClass]` works for both async and non-async unit tests, so use it everywhere! There is [an item template](Optional%20Component.md) to make it even easier.