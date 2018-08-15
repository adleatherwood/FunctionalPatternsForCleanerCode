using System;
using ImperativeTypes;

namespace Imperative
{    
    public class MyClassTests
    {        
        public void TestTheWholeThingBecauseWeCan()
        {
            // This noise will be in every one of my tests.
            // I can tuck it somewhere common, but we can also get rid of it entirely.
            var log = new MockLog(); // or new Mock<ILog>().Error((r) => true)... sucks either way.
            var db = new MockDb();   // there's undoubtedly more dependencies to be created here.
            var smtp = new MockSmtp();
            var myClass = new MyClass(log, db, smtp);

            // All of that setup, just to get to here
            var request = new Request(1, "Yossarian", "catch-22@gmail.com");
            var actual = myClass.MyMethod(request);

            // MyMethod is the only testable unit here.
            // All of this setup is required for every test.
            // I can't test the validation logic independently.
            
            Assert.AreEqual("Ok", actual);
        }

        // Mocking frameworks can help, often they produce heinous looking or bad tests.
        // I would need an all new set of mocks for each error scenario.
        // All mocking frameworks do is run your test function in a dynamic IWhatever.
        // None the class abstractions & dynamic whatever is required.
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