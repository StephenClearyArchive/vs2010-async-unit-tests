using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Nito.AsyncEx.UnitTests
{
    /// <summary>
    /// An async-compatible replacement for Microsoft.VisualStudio.TestTools.UnitTesting.TestClassAttribute.
    /// </summary>
    [Serializable]
    public sealed class AsyncTestClassAttribute : TestClassExtensionAttribute
    {
        private static Uri uri = new Uri("http://mstesttypes.nitoprograms.com/async");

        /// <summary>
        /// Gets the extension id.
        /// </summary>
        public override Uri ExtensionId
        {
            get { return uri; }
        }

        /// <summary>
        /// Specifies the context in which to run the test methods in this class.
        /// </summary>
        public AsyncContext AsyncContext { get; set; }

        /// <summary>
        /// Retrieves the test execution extension that uses an async context.
        /// </summary>
        public override TestExtensionExecution GetExecution()
        {
            return new AsyncTaskExtensionExecution(this.AsyncContext);
        }
    }
}
