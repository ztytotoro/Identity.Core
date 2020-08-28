using Microsoft.Extensions.DependencyInjection;
using System;

namespace Module
{
    public class TestModule : IModule
    {
        public void ConfigureServices(IServiceCollection services) {
            Console.WriteLine("module hit");
        }
    }
}
