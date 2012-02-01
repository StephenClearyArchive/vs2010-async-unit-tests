using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

// Notes:
//  1) RESTART Visual Studio if this is the first time you've installed this package.
//  2) If you're running VS2010, you'll need to reference AsyncCtpLibrary to write async unit tests.
//  3) Feel free to delete this file.

// It's useful to pull in this namespace, especially if you use AsyncContext (see the advanced examples, below).
using Nito.AsyncEx.UnitTests;

// Recommended usage: replace [TestClass] references with [AsyncTestClass].
// The following line does this for all tests in this file.
using TestClassAttribute = Nito.AsyncEx.UnitTests.AsyncTestClassAttribute;

namespace SimpleAsyncExamples
{
    // These are examples of the types of tests that don't behave correctly under the built-in [TestClass].
    // Try uncommenting the "using TestClassAttribute = ..." line above and these tests will not behave correctly.
    [TestClass]
    public class SimpleAsyncUnitTests
    {
        // A simple test that should pass; it throws an exception from an async continuation.
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async void ShouldPass()
        {
            await TaskEx.Yield(); // Change this to "Task.Yield" on VS 11.
            throw new InvalidOperationException();
        }

#if FAILTEST
        // A simple test that should fail; it fails an assertion from an async continuation.
        [TestMethod]
        public async void ShouldFail()
        {
            await TaskEx.Yield(); // Change this to "Task.Yield" on VS 11.
            Assert.Fail();
        }
#endif
    }

    // Now for some more advanced async examples.
    // The only option you really have is to specify which *type* of async context is used to execute the tests.
    // There are four async contexts you can choose from:
    //   None - Tests are run synchronously. This is the same as just using regular MSTest unit tests.
    //   General - A general-purpose async context. This should be used unless you need another one.
    //   WindowsForms - A WinForms async context. You can use this if you need to test an async component that depends on Control.Invoke/BeginInvoke for synchronization.
    //   WindowsPresentationFoundation - A WPF async context. You can use this if you need to test an async component that depends on Dispatcher.Invoke/BeginInvoke for synchronization.

    // The default async context is General. You can override the default for the entire test project by using an assembly attribute like this (e.g., in AssemblyInfo.cs):
    //   [assembly:AsyncContext(AsyncContext.WindowsPresentationFoundation)] // Run all tests in a WPF context unless specified otherwise.
    // The AsyncContext attribute is in the Nito.AsyncEx.UnitTests namespace. AsyncContext is both an attribute and an enumeration.

    // [AsyncTextClass] can take an AsyncContext as a named parameter.
    [AsyncTestClass(AsyncContext = AsyncContext.WindowsForms)]
    public class MoreAdvancedAsyncUnitTests
    {
        // You can also apply the AsyncContext attribute to any [TestMethod] that needs a specific one.

        // This test depends on a free-threaded context. It passes under Microsoft's [TestClass], but it needs AsyncContext.None to pass under [AsyncTestClass].
        [TestMethod]
        [AsyncContext(AsyncContext.None)]
        public void RequiresFreeThreadedContext()
        {
            Assert.IsNull(System.Threading.SynchronizationContext.Current);
        }

        // AsyncTestClass will determine the correct AsyncContext using these priorities:
        //  1) An [AsyncContext] applied to a [TestMethod]
        //  2) An AsyncContext argument passed to [AsyncTestClass]
        //  3) An [AsyncContext] applied to the assembly
        //  4) AsyncContext.General
        // The AsyncContext used to run a test is displayed in the Test Results Details screen.
    }
}

