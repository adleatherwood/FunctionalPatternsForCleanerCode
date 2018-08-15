module FSharpComparison

open System

type Request = {UserId : int; Name : string; Email : string }
type WriteRequest = Request -> bool
type SendEmail = string -> bool
type LogError = string -> unit


let ValidateRequest (request: Request) =
    if not (String.IsNullOrWhiteSpace(request.Name)) && not (String.IsNullOrWhiteSpace(request.Email)) then
        Ok request;
    else
        Error "Request is not valid";


let CanonicalizeRequest (request: Request) =
    { request with Email = request.Email.ToLower() }


let UpdateDatabase (db: WriteRequest) (request: Request) =
    try    
        let isUpdated = db(request);             
        if isUpdated then
            Ok request;
        else
            Error "Customer record not found";    
    with
    | e -> Error "DB Error: Customer record not updated";    


let SendEmail (smtp : SendEmail) (log: LogError) (request : Request) =
    let error = "Customer email not sent"
    if not (smtp request.Email) then
        log(error)
        Error error
    else
        Ok request

        
let MyMethod(request: Request) (db: WriteRequest) (smtp: SendEmail) (log: LogError) : string =
    let result = 
        request 
        |> ValidateRequest 
        |> Result.map CanonicalizeRequest
        |> Result.bind (UpdateDatabase db) 
        |> Result.bind (SendEmail smtp log)
    
    match result with
    | Ok _ -> "Ok"
    | Error msg -> msg    