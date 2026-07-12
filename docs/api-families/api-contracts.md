# API contracts

`PolyhydraGames.Core.Models.Api` provides `ApiOperationResult`, `ApiOperationResult<T>`, readiness models, validation issues, and paging/sorting/filtering contracts. These types carry data only; they do not execute requests or enforce an application's authorization and validation policies.

Use `ApiOperationResult<T>` when an operation returns a typed value plus success state, message, status code, and validation issues. Use the paging contracts to describe a requested page, sort order, and filter rules across an application boundary.
