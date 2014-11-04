Release Notes
=============

## 0.12.0

- **Breaking**: Changed the license to Apache 2.0
- Major project reorganization
- Switched from Rake/Albacore to PVCBuild for builds.=

## 0.11.1

- Fixed a bad cast in `ConcatenatedTransformation`

## 0.11.0

- **Breaking**: Concatenated transformations no longer expose their transformations through the `Transformations` property but instead implement `IList<ITransformation>`.
- **Breaking**: `LogExpression` has been renamed to `NaturalLogExpression` while a new arbitrary base `LogExpression` uses that name. The correct `NaturalLogExpression` will still be selected for "log"  or the new "ln" alias when requesting the unary function. A binary request for "log" will give a `NaturalLogExpression`.
- **Breaking**: Some constructors that previously accepted `IEnumerable<T>` or some other collection type now only accept `T[]`. 
- Major internal refactoring.
- Code contract changes and fixes.
- Reduced usage of `ReadOnlyCollection` in favor of `IList<>` or arrays.
- New arbitrary base log expression: `NaturalLogExpression`.
- Improved handling of return types for generated delegates for generic operations.
- Improved safety of generic vector and point types.

## 0.10.0

- **Breaking**: Transformation interfaces have a `object TransformValue(object)` signature and provide information on the types it can transform to and from.


## 0.9.0

- **Breaking**: All interfaces and some classes moved to different namespaces
- Polygon intersection performance increases, roughly 15% faster (crudely measured)
- A few polygon intersection algorithm bug fixes
- Many additions and fixes for code contracts
- Polygon intersection algorithm code refactor
- Code formatting changed from tabs to spaces. The battle is over, I give in!
- Code changes for portability across framework versions

## 0.8.5

- Polygon intersection crossing algorithm bug fix
- Code changes for portability across framework versions

## 0.8.1

- Documentation Fixes
- Ports for:
	- windows8
	- windowsphone8
	- sl4-windowsphone71
	- sl4
	- sl5
- Code changes for portability across framework versions

## 0.8.0

- Distributing CodeContracts with the NuGet package

## 0.7.1

- Some new segment/line/ray intersections
- Delay signing enabled for builds

## 0.7.0

- Added some Linq Expressions for various trig functions