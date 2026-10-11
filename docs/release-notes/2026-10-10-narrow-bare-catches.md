# Narrowed catches and FromCSV continueOnError (#47)

## Summary

- Contract changed: StronglyTypedId Try* methods, SystemTextJsonHelper.TrySerializeObject, MatrixDisplaySettingsSchema.ValidateRoundTrip, CSVExtensions.FromCSV<T>
- Change type: behavioral
- Package version: next 2.0.1.x

## Compatibility

- Package consumers: Exceptions outside the expected parse/serialize types now propagate instead of returning false; FromCSV<T> now honors continueOnError (returns default when true, rethrows when false).
- Project-reference consumers: same as package consumers
- Serialization impact: none
- Nullability or required-field impact: none
- Enum or identifier impact: none

## Migration Notes

- Required consumer change: Only if a custom ID constructor throws an unexpected exception type: fix the constructor.
- Safe rollout order: n/a
- Deprecated member removal target: n/a

## Validation

- `dotnet test Tests/PolyhydraGames.Core.Models.Test.csproj`: passes on this branch.
