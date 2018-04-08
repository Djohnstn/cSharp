using System.EnterpriseServices;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("HelloDCOM1")]
[assembly: AssemblyDescription("Sample DCOM app")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("HelloWorld")]
[assembly: AssemblyProduct("HelloDCOM1")]
[assembly: AssemblyCopyright("Copyright © Nobody 2018")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

//Those are additional attributes that have to be added to the project
[assembly: ApplicationName("HelloDCOM")]
[assembly: ApplicationActivation(ActivationOption.Server)]
//[assembly: AssemblyKeyFile(@"C:\Users\David\Documents\_KeyFiles\HelloDCOM\David.pfx")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(true)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("79b4b3de-2c3a-4405-bf7e-2d78f69186a2")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
