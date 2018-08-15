using System;
using ImperativeTypes;

namespace FunctionalTypes
{
    public class MyClass
    {        
        private readonly ILog log;
        private readonly IDb db;
        private readonly ISmtpClient smtp;
        
        /* LET'S TRY USING THAT RESULT THING
         * 34 words-ish - not much better, 
         * But now there's a lot of repetition in the code that can perhaps be dealt with.
         * Perhaps with a Monad!
         */
        public string MyMethod(Request request)
        {            
            var result = ValidateRequest(request);
            if (!result.WasSuccessful)  // Pete & Repeat
                return result.Error;

            request = CanonicalizeRequest(request);        

            result = UpdateDatabase(request);
            if (!result.WasSuccessful)  // Pete & Repeat
                return result.Error;

            result = SendEmail(request);
            if (!result.WasSuccessful)  // Pete & Repeat
                log.Error(result.Error);

            return "Ok";
        } 

        /* Now Validate can return either a success or an error  */
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

        /* The kind of failure that can occur is being stated in the function signature */
        public Result<Request> UpdateDatabase(Request request)
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

        /* I don't have to incur try-catches in my calling code */
        public Result<Request> SendEmail(Request request)
        {            
            if (!smtp.SendEmail(request.Email))            
                return "Customer email not sent";            
            else
                return request;            
        }
    }
}