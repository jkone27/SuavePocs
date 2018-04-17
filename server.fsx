#r "packages/Suave/lib/netstandard1.6/Suave.dll"
#r "packages/Suave.Swagger/lib/Suave.Swagger.dll"
#r "packages/Newtonsoft.Json/lib/netstandard2.0/Newtonsoft.Json.dll"
#load "types.fs"
 
open Suave.Swagger
open Rest
open FunnyDsl
open Swagger
open Suave
open Suave.Operators
open Suave.Filters
open Suave.Writers
open Suave.Successful
open System
open Types

let now signature : WebPart =
  fun (x : HttpContext) ->
    async {

      let response = { 
          LocalTime = DateTime.Now 
          Signature = signature 
          }
      // The MODEL helper checks the "Accept" header 
      // and switches between XML and JSON format
      return! MODEL response x
    }

let route1 = swagger {
    // syntax 1
    for route in posting <| urlFormat "/time/%s" now  do
      yield description Of route is "What time is it?"
      yield parameter "signature" Of route (fun p -> { p with Type = (Some typeof<MyTime>); In=Body })
  }

let homeRedirect =
    GET >=> path "/" >=> Redirection.redirect  "http://localhost:8080/swagger/v3/ui/index.html"

choose [ 
    homeRedirect
    route1.App ;
] 
|>
startWebServer defaultConfig