# Helium.Data.Castle

Helium.Data.Castle is a fast and versatile serialization library for the Castle binary data format, conceived specifically for use in Helium. It is licensed under the Helium Toolchain Common License.

[Castle](./castle-spec.md) is specifically designed to be fast to deserialize. As such, serializers are difficult to write and the format does not attempt to save disk space or memory space.

## Feature set

### Serialization

- [x] Castle data to in-memory representation
- [x] In-memory representation to Castle data
- [ ] Castle data to C# types
- [ ] C# types to Castle data

### Document Object Model

*Not implemented yet.*

## [API Reference](./reference.md)

## See also

- [CastleConverter](./ref/castleconverter.md)
- [BinaryCastleReader](./ref/binarycastlereader.md)
- [BinaryCastleWriter](./ref/binarycastlewriter.md)
- [CastleRootToken](./ref/castleroottoken.md)
- [CastleCompoundToken](./ref/castlecompoundtoken.md)