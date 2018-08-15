# Functional Patterns for Cleaner Code

The slide presentation can be found [here](https://drive.google.com/open?id=1cki8fN67q-FsAkbPHMO43VCmsSCmx2ZF1jVFlZ2j5uI).

The presentation covers various functional patterns that can be brought into the OOP paradigm with the goal of achieving code that is easier to understand, test, and maintain.  Topics covered:

* Nulls
* Exceptions
* The Law of Demeter
* Indirect Input/Output

The code sample is a step-by-step take on Scott Wlaschin's [Railway Oriented Programming](https://www.slideshare.net/ScottWlaschin/railway-oriented-programming) example.  The code has been rewritten several times in the following stages:

* Imperative
* Declarative
* Using the Result<T> data type
* Using the Result<T> as a monad
* Using only Pure functions
* Replacing interfaces with functions
* A version in F# for kicks

