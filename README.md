# DependencyTracer

DependencyTracer is a command-line tool that analyzes your C# or VB.NET solution, and investigate the caller of specified method.

## Prerequisites

* .NET Framework 4.8
* Visual Studio

## Usage

List all methods where `HogeClass::PiyoMethod` is called directly.

```
DependencyTracer.exe -s C:\Path\To\MySolution.sln -c "MyCompany.MyProduct.HogeClass" -m "PiyoMethod"
```

List all methods where `HogeClass::PiyoMethod` is called directly or indirectly.

```
DependencyTracer.exe -s C:\Path\To\MySolution.sln -c "MyCompany.MyProduct.HogeClass" -m "PiyoMethod" -A
```

See `DependencyTracer.exe --help` for more usage.


## License

DependencyTracer is under MIT License. See `LICENSE.txt` for more information.

## Contact

Project Link: [https://github.com/sashimi343/DependencyTracer](https://github.com/sashimi343/DependencyTracer)
