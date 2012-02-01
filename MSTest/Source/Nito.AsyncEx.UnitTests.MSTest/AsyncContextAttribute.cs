using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Nito.AsyncEx.UnitTests
{
    /// <summary>
    /// Specifies the context in which to run the test method.
    /// </summary>
    public enum AsyncContext
    {
        // Start values at 1, leaving 0 undefined. This allows us to have an unnamed "uninitialized" value (0) to work around the lack of nullable property types in attributes.

        /// <summary>
        /// A framework-independent general thread-affine asynchronous context. This is the default, and should be used whenever possible.
        /// </summary>
        General = 1,

        /// <summary>
        /// A Windows Forms framework, for testing asynchronous components that depend on Control.Invoke/BeginInvoke or an actual message loop.
        /// </summary>
        WindowsForms,

        /// <summary>
        /// A WPF framework, for testing asynchronous components that depend on Dispatcher or an actual message loop.
        /// </summary>
        WindowsPresentationFoundation,

        /// <summary>
        /// No asynchronous context; the test method is invoked synchronously (same as MSTest default behavior). This context may cause asynchronous tests to pass when they should fail. This context should only be used for test methods that depend on a thread-pool context.
        /// </summary>
        None
    }

    /// <summary>
    /// Specifies the context in which to run the test method. If this attribute is applied to the assembly, then this specifies the default context in which to run all test methods in that assembly.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Assembly, AllowMultiple = false)]
    public sealed class AsyncContextAttribute : Attribute
    {
        private readonly AsyncContext asyncContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncContextAttribute"/> class.
        /// </summary>
        /// <param name="asyncContext">Specifies the context in which to run the test method. If this attribute is applied to the assembly, then this specifies the default context in which to run all test methods in that assembly.</param>
        public AsyncContextAttribute(AsyncContext asyncContext)
        {
            this.asyncContext = asyncContext;
        }

        /// <summary>
        /// Specifies the context in which to run the test method. If this attribute is applied to the assembly, then this specifies the default context in which to run all test methods in that assembly.
        /// </summary>
        public AsyncContext AsyncContext { get { return this.asyncContext; } }
    }
}
