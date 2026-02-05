# COIN Number Portability

This repository contains projects that provide a modern, powerful, clean and easy to use, abstraction for COIN's [Number Portability Library](https://www.nuget.org/packages/Vereniging-COIN.Sdk.NP).

## What This Library Solves

COIN's NP SDK exposes the official message contracts, but it is verbose and low level for day-to-day application code. This library wraps the SDK with:

* Strongly typed records for NP messages with idiomatic C# shapes.
* Safer parsing and serialization helpers for the COIN wire formats.
* A simplified `NPClient` to send, receive, and acknowledge messages without dealing with the SDK plumbing directly.

The goal is to keep full compatibility with COIN's SDK while making integration code smaller, clearer, and less error-prone.

## COIN Number Portability, In Short

COIN Number Portability (NP) is the standardized message flow used in the Netherlands to move telephone numbers between providers. Parties exchange structured messages (requests, answers, performed, cancel, etc.) that:

* Initiate a porting request from a recipient provider.
* Validate and approve (or delay/deny) the request by the donor provider.
* Confirm the actual porting completion and related metadata.

This library provides the message models and client abstractions to participate in those NP flows.

* [Coin NP Client](COINNP.Client) - Contains a Number Portability client.
* [Coin NP Entities](COINNP.Entities) - Contains Number Portability messages defined by COIN.

Please see the respective projects for documentation.

## License

Licensed under MIT license. See [LICENSE](LICENSE) for details.

## Thank you's and Attributions

* [Vereniging COIN](https://coin.nl/nl/home) for providing the [.Net SDK](https://gitlab.com/verenigingcoin-public/coin-sdk-dotnet) ([NP SDK](https://gitlab.com/verenigingcoin-public/coin-sdk-dotnet/-/tree/master/number-portability-sdk))
* [Bart van Munster](https://gitlab.com/bart.vanmunster) for his feedback
