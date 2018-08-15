using System;
using ImperativeTypes;
using FunctionalTypes;

namespace FunctionalMonadScenario
{    
    public class MyClassTests
    {        
        public void TestTheWholeThingBecauseWeCan()
        {
            var request = new Request(1, "Yossarian", "catch-22@gmail.com");
            var actual = MyClass.MyMethod(request, r => true, r => true, r => {});

            Assert.AreEqual("Ok", actual);
        }        
    }
}