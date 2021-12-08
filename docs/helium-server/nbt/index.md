# Helium.NBT

Helium.NBT is a fast and versatile NBT library designed for use within the Helium Project. As such, it is licensed under the Helium Toolchain Common License. Like every other part of Helium's APIs, everything that does not strictly need to be `private`, `protected` or `internal` is marked `public`. Every part of it is use-at-your-own-risk, save those explicitly stated below. 

---

There are no plans to support Bedrock Edition NBT, however, feel free to open a pull request implementing Bedrock Edition NBT.

## Usage

Currently, Helium.NBT supports reading and writing binary NBT to and from a Compound. The two methods to be used for this are `NbtCompoundToken BinaryNbtReader#ReadCompound()` and `void BinaryNbtWriter#WriteCompound(NbtCompoundToken)`. All other exposed methods are use-at-your-own-risk. 

The library also supports stringified NBT. The methods in question are `NbtCompoundToken StringifiedNbtReader#ReadCompound(String)`, `NbtCompoundToken StringifiedNbtReader#ReadCompound(Byte[])` and `Byte[] StringifiedNbtWriter#WriteCompound(NbtCompoundToken)`

## Feature set

- [x] binary NBT to Compound
- [x] Compound to binary NBT
- [x] stringified NBT to Compound
- [x] Compound to stringified NBT
- [ ] binary NBT to any C# class (reflective)
- [ ] any C# class to binary NBT (reflective)
- [ ] stringified NBT to any C# class (reflective)
- [ ] any C# class to stringified NBT (reflective)
- [ ] binary NBT to any C# class (source-generated)
- [ ] any C# class to binary NBT (source-generated)
- [ ] stringified NBT to any C# class (source-generated)
- [ ] any C# class to stringified NBT (source-generated)

## [API Reference](./reference.md)

## See also

- [`BinaryNbtReader`](./ref/binarynbtreader.md)
- [`BinaryNbtWriter`](./ref/binarynbtwriter.md)
- [`NbtCompoundToken`](./ref/nbtcompoundtoken.md)
- [`StringifiedNbtReader`](./ref/stringifiednbtreader.md)
- [`StringifiedNbtWriter`](./ref/stringifiednbtwriter.md)
