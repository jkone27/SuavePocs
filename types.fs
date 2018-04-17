module Types
open System


type Person = Good of name: string | Bad of name: string

type MyTime = { 
  LocalTime : DateTime
  Signature : string 
}