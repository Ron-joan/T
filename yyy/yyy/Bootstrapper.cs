using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Stylet;
using StyletIoC;
using yyy.Pages;

namespace yyy
{
    public class Bootstrapper : Bootstrapper<ShellViewModel>
    {
        protected override void ConfigureIoC(IStyletIoCBuilder builder)
        {
            _ = builder.Bind<Dictionary<string, Func<IScreen>>>().ToFactory(c =>
                new Dictionary<string, Func<IScreen>>()
                {
                    {"Shell",() => c.Get<ShellViewModel>() },
                }
            );
        }



        protected override void Configure()
        {
            // Perform any other configuration before the application starts
        }
    }
}
