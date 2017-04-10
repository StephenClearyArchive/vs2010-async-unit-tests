Don't worry - it's not _really_ advanced. You only have one option, really: which type of async context is used to execute the tests.

There are four async contexts you can choose from:
* {{ None }} - Tests are run synchronously. This is the same as just using regular MSTest unit tests.
* {{ General }} - A general-purpose async context. This should be used unless you need another one.
* {{ WindowsForms }} - A WinForms async context. You can use this if you need to test an async component that depends on {{ Control.Invoke/BeginInvoke }} for synchronization.
* {{ WindowsPresentationFoundation }} - A WPF async context. You can use this if you need to test an async component that depends on {{ Dispatcher.Invoke/BeginInvoke }} for synchronization.

The default async context is {{ General }}. You can override the default for the entire test project by using an assembly attribute like this (e.g., in AssemblyInfo.cs):

{code:c#}
using Nito.AsyncEx.UnitTests;

[assembly:AsyncContext(AsyncContext.WindowsPresentationFoundation)](assembly_AsyncContext(AsyncContext.WindowsPresentationFoundation)) // Run all tests in a WPF context unless specified otherwise.
{code:c#}

{{ AsyncTextClass }} can also take an {{ AsyncContext }} as a named parameter:

{code:c#}
[AsyncTestClass(AsyncContext = AsyncContext.WindowsForms)](AsyncTestClass(AsyncContext-=-AsyncContext.WindowsForms))
public class MoreAdvancedAsyncUnitTests
{
  [TestMethod](TestMethod)
  public async void RequiresWindowsFormsContext()
  { ... }
}
{code:c#}

You can also apply the {{ [AsyncContext](AsyncContext) }} attribute to any {{ [TestMethod](TestMethod) }} that needs a specific one:

{code:c#}
// This test depends on a free-threaded context. It passes under Microsoft's [TestClass](TestClass), but it needs AsyncContext.None to pass under [AsyncTestClass](AsyncTestClass).
[TestMethod](TestMethod)
[AsyncContext(AsyncContext.None)](AsyncContext(AsyncContext.None))
public void RequiresFreeThreadedContext()
{
  Assert.IsNull(System.Threading.SynchronizationContext.Current);
}
{code:c#}

When the unit test is run, the {{ AsyncContext }} used is determined by these priorities:
# An {{ [AsyncContext](AsyncContext) }} applied to a {{ [TestMethod](TestMethod) }}
# An {{ AsyncContext }} argument passed to {{ [AsyncTestClass](AsyncTestClass) }}
# An {{ [AsyncContext](AsyncContext) }} applied to the assembly
# {{ AsyncContext.General }}

The {{ AsyncContext }} used to run a test is displayed in the Test Results Details screen:

![Here's how to open the test result details](Advanced Usage_ViewTestResultsDetails.png)

![See! The AsyncContext is right there! No more guessing!](Advanced Usage_TestResults.png)