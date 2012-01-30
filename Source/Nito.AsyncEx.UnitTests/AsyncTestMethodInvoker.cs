using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Reflection;

namespace Nito.AsyncEx.UnitTests
{
    /// <summary>
    /// Invokes a test method within a particular asynchronous context.
    /// </summary>
    internal sealed class AsyncTestMethodInvoker : ITestMethodInvoker
    {
        /// <summary>
        /// The test method context, including the actual test method to run.
        /// </summary>
        private readonly TestMethodInvokerContext context;

        /// <summary>
        /// The asynchronous context chosen to execute the test method. This context will wait until all asynchronous operations have completed, and will propogate asynchronous exceptions.
        /// </summary>
        private readonly Action<Action> executer;

        /// <summary>
        /// Determine the asynchronous context to use.
        /// </summary>
        /// <param name="testClassAttributeAsyncContext">The AsyncContext passed to [AsyncTestClass], or 0 if no AsyncContext was passed.</param>
        /// <param name="invokerContext">The test method context, including the actual test method to run.</param>
        /// <returns>The chosen asynchronous context.</returns>
        private static AsyncContext GetAsyncContext(AsyncContext testClassAttributeAsyncContext, TestMethodInvokerContext invokerContext)
        {
            var testMethod = invokerContext.TestMethodInfo;

            // Check for an [AsyncContext] attribute on the test method.
            var contextAttribute = testMethod.GetCustomAttributes(typeof(AsyncContextAttribute), true).OfType<AsyncContextAttribute>().SingleOrDefault();
            if (contextAttribute != null)
            {
                invokerContext.TestContext.WriteLine("Asynchronous context set by [AsyncContext] on test method.");
                return contextAttribute.AsyncContext;
            }

            // Check for an AsyncContext property passed into [AsyncTestClass].
            if (testClassAttributeAsyncContext != 0)
            {
                invokerContext.TestContext.WriteLine("Asynchronous context set by AsyncContext passed to [TestClass] on test class.");
                return testClassAttributeAsyncContext;
            }

            // Check for an [AsyncContext] attribute on the assembly.
            var assemblyAttribute = testMethod.Module.Assembly.GetCustomAttributes(typeof(AsyncContextAttribute), true).OfType<AsyncContextAttribute>().SingleOrDefault();
            if (assemblyAttribute != null)
            {
                invokerContext.TestContext.WriteLine("Asynchronous context set by [AsyncContext] on assembly.");
                return assemblyAttribute.AsyncContext;
            }

            // Default to General.
            return AsyncContext.General;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncTestMethodInvoker"/> class.
        /// </summary>
        /// <param name="testClassAttributeAsyncContext">The AsyncContext passed to [AsyncTestClass], or 0 if no AsyncContext was passed.</param>
        /// <param name="context">The test method context, including the actual test method to run.</param>
        public AsyncTestMethodInvoker(AsyncContext testClassAttributeAsyncContext, TestMethodInvokerContext context)
        {
            this.context = context;
            var asyncContext = GetAsyncContext(testClassAttributeAsyncContext, context);
            if (asyncContext == AsyncContext.General)
                this.executer = GeneralThreadAffineContext.Run;
            else if (asyncContext == AsyncContext.WindowsForms)
                this.executer = WindowsFormsContext.Run;
            else if (asyncContext == AsyncContext.WindowsPresentationFoundation)
                this.executer = WpfContext.Run;
            else if (asyncContext == AsyncContext.None)
                this.executer = action => action();
            else
                throw new InvalidOperationException("Invalid async test context " + asyncContext);
            this.context.TestContext.WriteLine("Executing in asynchronous context " + asyncContext + ".");
        }

        /// <summary>
        /// Invokes the test method within the chosen asynchronous context.
        /// </summary>
        public TestMethodInvokerResult Invoke(params object[] parameters)
        {
            TestMethodInvokerResult ret = null;
            try
            {
                this.executer(() => { ret = this.context.InnerInvoker.Invoke(parameters); });
            }
            catch (Exception ex)
            {
                // TestMethodInvokerContext.InnerInvoker will capture exceptions thrown by the test method (synchronous errors).
                // We need to capture exceptions thrown by the asynchronous context as well (asynchronous errors).
                ret = new TestMethodInvokerResult { Exception = ex };
            }
            return ret;
        }
    }
}
