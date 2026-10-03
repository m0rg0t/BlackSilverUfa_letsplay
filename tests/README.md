# Video ID regression checks

Run from repository root with the .NET 10 SDK:

```sh
dotnet run --project tests/Portable.Tests/Portable.Tests.csproj --configuration Release
```

Links the complete production ItemDetailPage.xaml.cs with explicit test-only substitutes for WinRT, generated XAML, Callisto, MyToolkit and data loading. Tests invoke only the actual private URL parser through a test partial class. Network, launching and playback substitutes throw if called. Covers v after a value containing v, normal watch URLs, extra parameters, fragments, uppercase V, raw IDs and malformed lengths/characters. Original source returns the whole URL for feature=video&v=AbCdEf12_-3.

The harness has no NuGet package dependencies and clears package sources in its own NuGet.Config. It is a separate modern test host, not a port or a whole-app build. Stubs are test-only and are not included in the legacy project. Only synthetic fixtures are used; no HTTP requests, device access, capture, user data or credentials.

Still unvalidated: Windows Store XAML/WinRT SDK, Callisto/MyToolkit restore, real playback and the retired YouTube GData service. Original target, dependency manifests and binary references are preserved. Dependency upgrades require a compatible historical restore/build baseline; this PR does not claim modernized dependencies or a functioning remote service.
