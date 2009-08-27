using System;

namespace Cradiator.Services
{
	[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)] 
	public class InjectBuildBusterAttribute : Attribute
	{ }

	[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
	public class InjectBuildBusterImageDecoratorAttribute : Attribute
	{ }

	[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
	public class InjectBuildBusterFullNameDecoratorAttribute : Attribute
	{ }
}