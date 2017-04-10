## Project Description
This project allows writing unit tests using the async/await keywords, without having to provide your own asynchronous context for each test.

## Installation

1. Install the [NuGet package](https://nuget.org/packages/AsyncUnitTests-MSTest) _while running VS as Administrator_. If this is your first time installing the package on your machine, you'll have to restart Visual Studio.
2. Download the [Async Unit Test item template](item-template.md), which will give you an alternative to the non-async-friendly Unit Test item template.

## Documentation

See the [Why Do I Need This](Why Do I Need This.md), [Getting Started](Getting Started.md), [Optional Component](Optional Component.md), and [Advanced Usage](Advanced Usage.md) pages.

## Future Directions

VS2012 officially supports asynchronous unit tests (you just have to make sure they return `Task`, not `void`), so this project is now in just a maintenance state.