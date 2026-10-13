# MeshOptimizer .NET

Automated multiplatform .NET bindings for [zeux/meshoptimizer](https://github.com/zeux/meshoptimizer) targeting `win-x64`, `win-arm64`, `linux-x64`, `linux-arm64`, `osx-x64`, and `osx-arm64`.

## Repository Architecture
- `MeshOptimizer.slnx`: Visual Studio solution file using the modern XML format.
- `MeshOptimizer/`:
  - `MeshOptimizer.csproj`: Configured for .NET 10 with unsafe blocks and `Content` items mapping `runtimes/{rid}/native/` to output/package targets.
  - `MeshOptimizer.cs`: Hand-written, allocation-free safe wrapper using `Span<T>` and `ReadOnlySpan<T>`.
  - `Native/MeshOptimizer.Bindings.cs`: Source-generated `[LibraryImport]` P/Invoke bindings.
- `BindingGenerator/`: Standalone generator tool powered by `CppAst` that inspects `meshoptimizer.h`, strips `meshopt_`, emits PascalCase method names, and maps native structs and enums.
- `MeshOptimizer.Tests/`: XUnit unit tests verifying mesh remap and vertex cache optimization.
- `.github/workflows/sync-and-build.yml`: CI workflow checking upstream releases, cross-compiling native binaries across all 6 architectures, generating bindings, and publishing GitHub release bundles.
