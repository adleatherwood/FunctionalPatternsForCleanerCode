using System;
using ImperativeTypes;

namespace Declarative
{
    public class MyClass
    {
        private readonly ILog log;
        private readonly IDb db;
        private readonly ISmtpClient smtp;

        /* LET'S BREAK IT DOWN INTO STEPS
         * Keywords to get to what this function does?  35
         * Half of the previous version to read to come to an understanding about the function.
         * Validate and Canonicalize are totally pure, testable, and reusable.
         * Update & Send are impure and likely testable with a bit of wonky effort.
         * But we can do better...
         */
        public string MyMethod(Request request)
        {            
            var isValidated = ValidateRequest(request);
            if (!isValidated)
                return "Request is not valid";
        
            request = CanonicalizeRequest(request);
                                    
            var isUpdated = UpdateDatabase(request, out string error);
            if(!isUpdated)
                return error;
            
            SendEmail(request.Email);
            
            return "Ok";
        } 

        /* This is pure and independantly testable now.  And reusable. */
        public static bool ValidateRequest(Request request)
        {
            return !String.IsNullOrWhiteSpace(request.Name) 
                && !String.IsNullOrWhiteSpace(request.Email);
        }

        /* This is pure and independantly testable now.  And reusable. */
        public static Request CanonicalizeRequest(Request request)
        {
            /* Becasue mutability leads to anger and anger leads to hate. */
            return new Request(
                request.UserId, 
                request.Name, 
                request.Email.ToLower());
        }

        /* This is impure and testable given some finagling with moq. */
        public bool UpdateDatabase(Request request, out string error)
        {
            error = null;
            try
            {
                var isUpdated = db.UpdateDatabase(request);             
                if (!isUpdated)
                    error = "Customer record not found";
                return true;
            }
            catch
            {            
                error = "DB Error: Customer record not updated";
                return false;
            }           
        }

        /* This is impure and testable given some finagling with moq. */
        public void SendEmail(string email)
        {
            if (!smtp.SendEmail(email))
            {                            
                log.Error("Customer email not sent");                
            }
        }
    }
}