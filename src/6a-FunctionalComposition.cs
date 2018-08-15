using System;
using ImperativeTypes;
using FunctionalTypes;

namespace FunctionalMonadScenario
{
    public delegate bool WriteRequest(Request r);
    public delegate bool SendEmail(string email);
    public delegate void LogError(string error);

    public class MyClass
    {        
        /* EVERYTHING IS A FUNCTION NOW...
         * 100% testable - No interface mocking required
         * Pretty darn readable - Very little noise.
         */
        public static string MyMethod(Request request, WriteRequest db, SendEmail smtp, LogError log)
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
        public static Result<Request> UpdateDatabase(Request request, WriteRequest db)
        {
            try
            {
                var isUpdated = db(request);             
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
        public static Result<Request> SendEmail(Request request, SendEmail smtp, LogError log)
        {
            const string error = "Customer email not sent";
            if (!smtp(request.Email))                                        
            {
                log(error);
                return error;                
            }
            else
                return request;            
        }
    }
}