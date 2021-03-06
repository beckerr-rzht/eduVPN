# Development of eduVPN and Let's Connect! Clients for Windows


## Folders

- _bin_: Automation scripts
   - _Debug_: Debug binaries
   - _Release_: Release binaries
   - _OpenVPN_: OpenVPN upstream binaries
   - _Setup_: MSI packages and EXE installer
- _eduEd25519_: libsodium wrapper providing Ed25519 signing and verifying support to C#/.NET
- _eduJSON_: Lightweight JSON parser
- _eduOAuth_: OAuth 2.0 library
- _eduOpenVPN_: OpenVPN IPC for C#/.NET
- _eduVPN_: Client logic (Model and View Model)
- _eduVPN.Client_: eduVPN client application (Shell)
- _eduVPN.Resources_: Native resources for Windows MUI - localizable names and descriptions for Start Menu shortcuts
- _eduVPN.Views_: Client UI (View)
- _Install_: Theme files for WiX Standard Bootstrapper Application
- _LetsConnect.Client_: Let's Connect! client application (Shell)
- _OpenVPN.Resources_: Native resources for Windows MUI - localizable names and descriptions for system services names


## Pre-requisites

1. Install [Visual Studio 2019 Community Edition](https://www.visualstudio.com/vs/community/). A minimum set of required features is:
   - Workloads
      - .NET desktop development
      - Desktop development with C++ (required by eduEd25519 and OpenVPN submodules, eduVPN.Resources and OpenVPN.Resources resource projects)
   - Individual components
      - Code tools
         - Git for Windows (if not installed by other means)
      - Compilers, build tools, and runtimes
         - Windows Universal CRT SDK (required by eduEd25519 submodule)
      - SDKs, libraries, and frameworks
         - Windows 8.1 SDK (required by eduEd25519 submodule)
2. Clone the eduVPN project source code _including_ sub-modules from the [eduVPN GitHub repository](https://github.com/Amebis/eduVPN) using `git clone --recurse-submodules https://github.com/Amebis/eduVPN.git eduVPN` command.
3. Install TAP-Windows driver: [official](https://openvpn.net/index.php/open-source/downloads.html) or the one included in the eduVPN source tree in the _bin\Setup_ folder.
4. Install .NET Framework 3.5.x: can be installed from _Control Panel_ » _Programs and Features_ » _Turn Windows features on or off_ (required by WiX Toolset).
5. Install [WiX Toolset 3.14 or later](https://wixtoolset.org/releases/v3-14-0-3205/) (required for MSI and EXE installer packaging).


### Code Signing

In order to have the build process digitally sign the release output files, one should provide the following:

1. A signing certificate installed in the building user’s certificate store.
2. The following variables in the environment:
   - `ManifestCertificateThumbprint` - set the value to certificate’s SHA1 thumbprint (hexadecimal, without spaces, e.g. `bc0d8da45f9eeefcbe4e334e1fc262804df88d7e`).
   - `ManifestTimestampRFC3161Url` - set the value to URL used to perform timestamp signature (e.g. `http://sha256timestamp.ws.symantec.com/sha256/timestamp`, `http://timestamp.digicert.com` etc.). In order to perform the timestamp signing successfully, the computer running the build should be online and able to access this URL.


## Building and Packaging


### General Building Guidelines

All `nmake` commands should be invoked from the _x64 Native Tools Command Prompt for VS 2017_ (_Start_ » _All Programs_ » _Visual Studio 2017_ » _Visual Studio Tools_ » _VC_ » _x64 Native Tools Command Prompt for VS 2017_). x86 build environment can be used when the 32-bit version is preferred.
This command prompt sets all Visual Studio 2017 environment variables required by the command line building.

`nmake register` and `nmake unregister` require elevation. Start the _x64 Native Tools Command Prompt for VS 2017_ elevated (_Start_ » _All Programs_ » _Visual Studio 2017_ » _Visual Studio Tools_ » _VC_ » Right click _x64 Native Tools Command Prompt for VS 2017_ » _Run as Administrator_). Other `nmake` commands can be run from a non-elevated _x64 Native Tools Command Prompt for VS 2017_.

Before pulling a new version of the eduVPN source code from the GitHub a `nmake unregister` and `nmake clean` is strongly recommended. You can run both commands combined as `nmake unregister clean` in a single elevated _x64 Native Tools Command Prompt for VS 2017_.


### Testing and Debugging

#### Initial Registration

The registration prepares the working environment for the eduVPN and Let's Connect! clients for testing on the build/development computer. It performs the following:
- Prepares local OpenVPN binaries.
- Installs and starts OpenVPN Interactive Service.
- Builds the Debug version of the eduVPN and Let's Connect! clients.
- Creates Start Menu shortcuts.

1. Start the _x64 Native Tools Command Prompt for VS 2017_ elevated (_Start_ » _All Programs_ » _Visual Studio 2017_ » _Visual Studio Tools_ » _VC_ » Right click _x64 Native Tools Command Prompt for VS 2017_ » _Run as Administrator_).
2. `cd` to the project folder - the one where `eduVPN.sln` and `Makefile` files are located.
3. Start the initial build and registration using `nmake Register` command. This command will:
   - Build the Debug version.
   - Create a Start menu eduVPN and Let's Connect! client shortcuts.
   - Install OpenVPN Interactive Service: one instance per client.
4. The clients can now be started using the Start menu shortcut.


#### Debugging Clients

1. Perform the [initial registration](#initial-registration) described above.
2. Open the `eduVPN.sln` file (with VS2017).
3. _Build_ » _Configuration Manager..._: Select desired active solution and platform. Please note _AnyCPU_ is not supported (yet).
4. In the _Solution Explorer_ pane, locate _eduVPN.Client_ or _LetsConnect.Client_ project, right click on it, and _Set as StartUp Project_.
5. _Build_ » _Build Solution_.
6. _Debug_ » _Start Debugging_.


### Building MSI Packages and EXE Installer

1. Start the _x64 Native Tools Command Prompt for VS 2017_ (_Start_ » _All Programs_ » _Visual Studio 2017_ » _Visual Studio Tools_ » _VC_ » _x64 Native Tools Command Prompt for VS 2017_). x86 build environment can be used if required too. Both versions build all MSI packages and EXE installers
2. `cd` to the project folder - the one where `eduVPN.sln` and `Makefile` files are located.
3. Start the MSI build using `nmake Setup` command.
4. The MSI packages and EXE installers will be saved to the `bin\Setup` folder.
