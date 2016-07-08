# NTRU-Sharp
NTRU C# Wrapper

This wrapper implements an interface with [*libntru*] (https://tbuktu.github.io/ntru/) It is currently under development the documentation is coming soon.

This wrapper intends to use only Mono 2.0  libraries to make it compatible with with other software packages such as [*Unity3d*] (http://unity3d.com/).

### TODO
- [x] Generate Keypair
- [x] Encrypt
- [x] Decrypt
- [x] Exporting Keys
- [x] Importing Keys
- [ ] Documentation

## Dependencies

### NTRU C Source

Download NTRU source [here] (https://github.com/tbuktu/libntru)

### Mono

Download and install latest distribution [here] (http://www.mono-project.com/download/)

## Compiling & Testing NTRU-Sharp

Compile the C# code

`mcs Program.cs NTRUWrapper.cs ffi.cs types.cs rand.cs encparams.cs`

execute the test program

`mono Program.exe`

# License

This program is free software: you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later version.
