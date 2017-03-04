using System;
using Xunit;

namespace CqrsFramework.Tests
{
    public class TimerTaskFactoryTests
    {
        public TimerTaskFactoryTests() { }

        [Fact]
        public void Test1()
        {
            CqrsFramework.Tasks.TimerTaskFactory.StartNew(() => "hello world",
                    (result) => result == "hello",
                    new TimeSpan(0, 0, 1),
                    new TimeSpan(0, 0, 3)).Wait();
            Console.WriteLine("Hello World!");
        }
    }
}