using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Cradiator.Config;
using Cradiator.Extensions;

namespace Cradiator.Model
{
	public class BuildDataTransformer : IConfigObserver
	{
		Regex _projectNameRegEx;
		Regex _categoryRegEx;

		public BuildDataTransformer(IConfigSettings configSettings)
		{
			SetRegex(configSettings);
			configSettings.AddObserver(this);
		}

		public IEnumerable<ProjectStatus> Transform(string xml)
		{
			if (string.IsNullOrEmpty(xml)) return new List<ProjectStatus>();

			return from project in XDocument.Parse(xml).Elements("Projects").Elements("Project")
			       let name = project.Attribute("name").GetValue()
				   let category = project.Attribute("category").GetValue()
				   where _projectNameRegEx.Match(name).Success 
				   where _categoryRegEx.Match(category).Success
				   select new ProjectStatus(name)
			              {
			              	CurrentMessage = project.Attribute("CurrentMessage").GetValue(),
			              	LastBuildStatus = project.Attribute("lastBuildStatus").GetValue(),
			              	ProjectActivity = new ProjectActivity(project.Attribute("activity").GetValue())
			              };
		}

		public void ConfigUpdated(ConfigSettings newSettings)
		{
			SetRegex(newSettings);
		}

		void SetRegex(IConfigSettings newSettings) 
		{
			_projectNameRegEx = new Regex(newSettings.ProjectNameRegEx);
			_categoryRegEx = new Regex(newSettings.CategoryRegEx);
		}
	}
}