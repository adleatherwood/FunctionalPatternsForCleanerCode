using System;
using ImperativeTypes;

namespace Imperative
{
    // https://www.slideshare.net/ScottWlaschin/railway-oriented-programming

    public class MyClass
    {
        public MyClass(ILog l, IDb d, ISmtpClient s) => (log, db, smtp) = (l, d, s);
        private readonly ILog log;
        private readonly IDb db;
        private readonly ISmtpClient smtp;

        public string MyMethod(Request request)
        {            
            var isValidated = 
                   !String.IsNullOrWhiteSpace(request.Name) 
                && !String.IsNullOrWhiteSpace(request.Email);

            if (!isValidated)
                return "Request is not valid";
        
            request.Email = request.Email.ToLower(); 
            
            try
            {
                var isUpdated = db.UpdateDatabase(request);             
                if (!isUpdated)
                    return "Customer record not found";
            }
            catch(Exception e)
            {            
                return e.Message;
            }
            
            if (!smtp.SendEmail(request.Email))
            {                            
                log.Error("Customer email not sent");                
            }

            return "Ok";
        } 

        /* Let's take inventory
         * 60-ish words
         *      2 intermediate variables
         *      4 logical operators
         *      4 conditional flow control statements
         *      1 variable mutation
         * All to say this
         *      validate the request
         *      normalize request data
         *      update the database
         *      send an email
         *      log errors and return a result
         * (18 words)
         * How testable is this method?
         * How maintainable is it?
         */
        public string MyMethodWithComments(Request request)
        {
            // How do i test this wihtout mocking the db & smtp clients?
            // Won't this be used anywhere else?
            var isValidated = 
                   !String.IsNullOrWhiteSpace(request.Name) 
                && !String.IsNullOrWhiteSpace(request.Email);

            if (!isValidated)
                return "Request is not valid";

            // Won't this be used anywhere else?
            // This function is changing the callers data?
            request.Email = request.Email.ToLower(); 
                        
            try
            {
                // Testing this function requires a db mock, yay!
                var isUpdated = db.UpdateDatabase(request);
                
                if (!isUpdated)
                    return "Customer record not found";
            }
            catch(Exception e)
            {            
                return e.Message;
            }
            
            // None of this is testable, it's all IO and in my way.
            if (!smtp.SendEmail(request.Email))
            {                          
                // Another hidden dependency.
                log.Error("Customer email not sent");                
            }

            return "Ok";
        }        
    }
}