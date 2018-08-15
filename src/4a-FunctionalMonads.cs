using System;
using ImperativeTypes;
using FunctionalTypes;

namespace ImpureMonadScenario
{
    public class MyClass
    {
        public MyClass(ILog l, IDb d, ISmtpClient s) => (log, db, smtp) = (l, d, s);
        private readonly ILog log;
        private readonly IDb db;
        private readonly ISmtpClient smtp;

        /* LET'S USE RESULT<T> AS A MONAD
         * 22 words total -- only 5 above our english requirements
         * This is what's known as 
         *      Happy Path Programming
         *      Railway Oriented Programming
         *      Functional Composition    
         * There's no indirect input or output.
         * Every single is easily testable.
         * Code reads like the functionality it's trying to achieve.
         */
        public string MyMethod(Request request)
        {   
            var result = ValidateRequest(request)            
                .Map(CanonicalizeRequest)    
                .Bind(UpdateDatabase)
                .Bind(SendEmail);

            return result.WasSuccessful
                ?  "Ok"
                : result.Error;
        } 

        public static Result<Request> ValidateRequest(Request request)
        {
            if (!String.IsNullOrWhiteSpace(request.Name) && !String.IsNullOrWhiteSpace(request.Email))
                return request;
            else
                return "Request is not valid";
        }

        public static Request CanonicalizeRequest(Request request)
        {
            return new Request(
                request.UserId, 
                request.Name, 
                request.Email.ToLower());
        }

        /* Still impure and tedious to test. private! */
        internal Result<Request> UpdateDatabase(Request request)
        {
            try
            {
                var isUpdated = db.UpdateDatabase(request);             
                if (isUpdated)
                    return request;
                else
                    return "Customer record not found";
            }
            catch
            {            
                return "DB Error: Customer record not updated";
            }
        }

        /* Still impure and tedious to test. private! */
        /* AoP discussion here -> you'd never log in here... */
        private Result<Request> SendEmail(Request request)
        {
            const string error = "Customer email not sent";

            if (!smtp.SendEmail(request.Email))  
            {                                      
                log.Error(error);
                return error;
            }
            else
                return request;            
        }
    }
}