using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices; 

#if DEBUG
	[assembly: AssemblyConfiguration("Debug")]
#elif STAGING
	[assembly: AssemblyConfiguration("Staging")] 
#elif MASTER
	[assembly: AssemblyConfiguration("master")] 
#elif RELEASE
	[assembly: AssemblyConfiguration("Release")] 
#endif

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyProduct("RabbitCache")]
[assembly: AssemblyCompany("Michael Vivet")]
[assembly: NeutralResourcesLanguage("en-US")]
