# Helium Server System Requirements

Helium is designed to run on as much hardware as possible, with as little trade-offs as possible. However, this is not always possible. With its attempt to be as fast as possible, Helium oftentimes finds itself stuck between more speed and dropping support for certain platforms.

## CPU requirements

Requirement | Recommended | Supported | Unsupported
--------- | ----------- | --------- | -----------
CPU Architecture | x64 or aarch64 | x86 | ia64, aarch32, ...
Endianness | Little Endian | Little Endian | Big Endian
CPU Microcode Extensions for x64 | AVX2, SSSE3 | any | -
CPU Microcode Extensions for aarch64 | Neon | any | -

Leveraging AVX-512, whenever .NET implements support, is up for discussion as the extension is still not widely implemented and most implementing CPUs, most notably Intel Alder Lake, disable the extension by default. The same applies to Helium (fittingly) on aarch64 CPUs.

## OS requirements

OS | Support | Notes
-- | ------- | -----
MacOS | Unsupported | - 
Linux | Partial Support | Requires `libmsquic` to be installed on the system
Windows 10 and older | Unsupported | Can be forced to function by manually changing the `HTTP.sys` driver.
Windows 11 Build 22000 and newer | Supported | -
Windows Server 2019 and older | Unsupported | -
Windows Server 2022 and newer | Supported | -

MacOS' invasive policies make Helium impossible to run on MacOS without rather egregious manual effort, rendering it unsupported.

On Linux, QUIC is not yet standard, with the specification not being implemented by OpenSSL 3.0. `libmsquic` has to be installed manually on Linux machines for the time being. Once mainstream OpenSSL implements QUIC, Helium will support Linux first-class.

On Windows, both client and server, `HTTP.sys` only implements QUIC as of Windows 11 Build 22000 and Windows Server 2022, respectively. Older versions are therefore unsupported, although using a non-standard, updated driver may work.