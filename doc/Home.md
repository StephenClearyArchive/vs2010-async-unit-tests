**Project Description**
This project allows writing unit tests using the async/await keywords, without having to provide your own asynchronous context for each test.

**Installation**

# Install the [NuGet package](https://nuget.org/packages/AsyncUnitTests-MSTest) _while running VS as Administrator_. If this is your first time installing the package on your machine, you'll have to restart Visual Studio.
# Download the [Async Unit Test item template](http://asyncunittests.codeplex.com/releases/view/81565), which will give you an alternative to the non-async-friendly Unit Test item template.

**Documentation**

See the [Why Do I Need This](Why-Do-I-Need-This), [Getting Started](Getting-Started), [Optional Component](Optional-Component), and [Advanced Usage](Advanced-Usage) pages.

**Future Directions**

VS2012 officially supports asynchronous unit tests (you just have to make sure they return {{Task}}, not {{void}}), so this project is now in just a maintenance state.