using System;
using ImperativeTypes;
using FunctionalTypes;

namespace PureMonadScenario
{
    public class MyClass
    {        
        /* LET'S PURIFY EVERYTHING!
         * 22 words total -- only 5 above our english requirements
         * This is what's known as 
         *      Happy Path Programming
         *      Railway Oriented Programming
         *      Functional Composition    
         * Every method is static.
         * There's no indirect input or output.
         * There's no null concerns.
         * Every single function is easily testable.
         * All functions are pure and public.
         * Code reads like the functionality it's trying to achieve.
         * Still requires mocks for testing. :P
         */
        public static string MyMethod(Request request, IDb db, ISmtpClient smtp, ILog log)
        {   
            var result = ValidateRequest(request)            
                .Map(CanonicalizeRequest)    
                .Bind(UpdateDatabase, db)
                .Bind(SendEmail, smtp, log);
            
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

        // static with no hidden dependencies = totally testable now
        public static Result<Request> UpdateDatabase(Request request, IDb db)
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

        // static with no hidden dependencies = totally testable now
        public static Result<Request> SendEmail(Request request, ISmtpClient smtp, ILog log)
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