using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nito.AsyncEx.UnitTests;
using System.Runtime.Serialization;

// This type is defined in the global namespace, which allows us to "intercept" the [TestClass] definition at compile time.
//   This is a horrible hack and only works if no one else does the same thing. :(
// Strictly speaking, we should name this [AsyncTestClass] and register it as a new test type. However:
//   We'd have to install this dll into a specific VS location.
//   We'd have to modify the registry to let VS know about the test type.
//   The documentation on how to do both steps above is wrong.
//   The end user would have to change every [TestClass] to [AsyncTestClass], both in existing tests and with each new unit test.
// So, even though it's a hack, we use global::TestClass instead of Nito.AsyncEx.UnitTests.AsyncTestClass.

/// <summary>
/// An async-compatible replacement for Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute.
/// </summary>
[Serializable]
public sealed class TestClassAttribute : TestClassExtensionAttribute, ISerializable
{
    private static Uri uri = new Uri("http://mstesttypes.nitoprograms.com/async");

    public override Uri ExtensionId
    {
        get { return uri; }
    }

    /// <summary>
    /// Specifies the context in which to run the test methods in this class.
    /// </summary>
    public AsyncContext AsyncContext { get; set; }

    public override TestExtensionExecution GetExecution()
    {
        return new AsyncTaskExtensionExecution(this.AsyncContext);
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.SetType(typeof(Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute));
    }
}