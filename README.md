# OCFF

![Build Status](https://tclasenitvt.visualstudio.com/_apis/public/build/definitions/c1d563f3-554e-400a-af81-041a95682db3/4/badge)
![![NuGet](https://img.shields.io/nuget/v/OCFF.svg)](https://www.nuget.org/packages/OCFF/)

The Source Code for the Reference Implementation of the OCFF (Omnipotent ConfigurationFile Format). This Format enables the user to easily create his own custom behavior for compute and enumeration funcs by implementing the respective interfaces.
Here we have a example ocff-file:

```
#This is a comment
[ExampleHeader]
Some Value

[SecondHeader]
this next word is @ExampleHeader

<BoolHeader>
True

#The next header is a argument for a ComputeVariable named ComputeHeader
[ComputeHeader]
@ExampleHeader

[UsingCompute]
$ComputeHeader

#This Header also takes the Value from the ComputeHeader as Argument
[EnumerationHeader]
$ComputeHeader

#This Section will appear as often as the EnumerationHeader returns a value
[UsingEnumeration]
&EnumerationHeader
```

Use

`Install-Package OCFF`

to install the Nuget-Package.
