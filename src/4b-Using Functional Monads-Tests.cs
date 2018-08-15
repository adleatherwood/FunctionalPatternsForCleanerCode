using System;
using ImperativeTypes;
using FunctionalTypes;

namespace ImpureMonadScenario
{    
    public class MyClassTests
    {        
        public void TestTheWholeThingBecauseWeCan()
        {
            // Still noisy            
            var log = new MockLog();
            var db = new MockDb();
            var smtp = new MockSmtp();
            var myClass = new MyClass(log, db, smtp);
            var request = new Request(1, "Yossarian", "catch-22@gmail.com");

            // I made my UpdateDatabase call internal so I could test it.
            // I don't want production callers to do this, because it's not what my class is for.
            // class changes made for testing is an unacceptable compromise!
            // I also still had to string up ALL of the class dependencies just to test this.
            // This function doesn't even use a logger.
            // Passing a null logger into the constructor might work today, but possibly not tomorrow (brittle tests).
            // But still... I don't give a damn about that smtp thing.  But i have to make one every time.
            // Dependency management digression here ---> ...
            var actual = myClass.UpdateDatabase(request);

            Assert.AreEqual("Ok", actual);
        }

        private class MockDb : IDb
        {
            public bool UpdateDatabase(Request r)
            {
                return true;
            }
        }

        private class MockLog : ILog
        {
            public void Error<T>(T e)
            {                
            }
        }

        private class MockSmtp : ISmtpClient
        {
            public bool SendEmail(string email)
            {
                return true;
            }
        }
    }
}