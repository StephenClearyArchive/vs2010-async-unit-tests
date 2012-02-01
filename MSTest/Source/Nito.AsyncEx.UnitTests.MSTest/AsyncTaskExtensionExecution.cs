using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Nito.AsyncEx.UnitTests
{
    /// <summary>
    /// A test type extension point that takes the AsyncContext passed to [AsyncTestClass] and passes it on to AsyncTestMethodInvoker.
    /// </summary>
    internal sealed class AsyncTaskExtensionExecution : TestExtensionExecution
    {
        /// <summary>
        /// The AsyncContext passed to [AsyncTestClass], or 0 if no AsyncContext was passed.
        /// </summary>
        private readonly AsyncContext testClassAttributeAsyncContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncTaskExtensionExecution"/> class.
        /// </summary>
        /// <param name="testClassAttributeAsyncContext">The AsyncContext passed to [AsyncTestClass], or 0 if no AsyncContext was passed.</param>
        public AsyncTaskExtensionExecution(AsyncContext testClassAttributeAsyncContext)
        {
            this.testClassAttributeAsyncContext = testClassAttributeAsyncContext;
        }

        public override ITestMethodInvoker CreateTestMethodInvoker(TestMethodInvokerContext context)
        {
            return new AsyncTestMethodInvoker(this.testClassAttributeAsyncContext, context);
        }

        public override void Dispose()
        {
        }

        public override void Initialize(TestExecution execution)
        {
        }
    }
}
