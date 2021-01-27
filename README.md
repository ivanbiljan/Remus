# Remus

[![Build Status](https://dev.azure.com/ibiljan/Remus/_apis/build/status/ivanbiljan.Remus?branchName=master)](https://dev.azure.com/ibiljan/Remus/_build/latest?definitionId=1&branchName=master) [![codecov](https://codecov.io/gh/ivanbiljan/Remus/branch/development/graph/badge.svg?token=ET6QQZLJ4E)](https://codecov.io/gh/ivanbiljan/Remus)

Remus is a cross-platform, nix-style command-line parsing library. It is designed with simplicity, flexibility, and testability in mind while handling most basic (as well as advanced) use-cases.  It acts as a middleman between your application and the end-user, and aims to facilitate command interpretation in interactive systems.

## Features
 * Cross-platform
 * Extensible
 * Out of the box CLI argument parsing
 * Method/attribute based command definitions
 * Supports command nesting (i.e, subcommands)
 * Command overload resolution
 * Includes type parsers for all primtive types by default
 * Customizable type parsers

## Getting started
Unlike other declarative CLI frameworks, Remus relies on an attribute-based approach to define commands. The motivation behind this is to allow you to write less boilerplate and get things done faster.
Core functionality is provided through Remus' `ICommandService`. In order to use Remus, you must first configure it via Microsoft's built-in service container, `IServiceProvider`. E.g:

```csharp
using System;
using Remus;
using Remus.Extensions;

namespace Remus.Example {
    internal sealed class Program {
        public static void Main(string[] args) {
            using var host = CreateHostBuilder(args).Build();
            /* code */
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                    services.AddRemus());
    }
}
```

Remus registers commands on a per object basis. This allows command handlers to depend on object state. The registration process is as follows:
 * Create a new `class` containing the commands you wish to implement
 * Within your application's setup, fetch an `ICommandService` instance via `IServiceProvider.GetService<T>()`
 * Initialize a new instance of the type containing the commands
 * Invoke `ICommandService.Register(obj)`, passing the newly created instance as the argument

The following example defines a `print` command that outputs `Hello, World` to stdout.

```csharp
using System;
using Remus;

namespace Remus.Example {
    internal sealed class Commands {
        [CommandHandler("print", "Prints \"Hello, World\" to stdout.")]
        public void Print() {
            Console.WriteLine("Hello, World");
        }
    }

    internal sealed class Program {
        public static void Main(string[] args) {
        }

        private static void Setup() {
            using var host = CreateHostBuilder(args).Build();
            var commandService = host.Services.
        }
    }
}
```

> Note: since we allow commands to depend on the enclosing instance's state, static methods do not make sense in this context and are omitted from the registration process.