using System.Linq;
using Cradiator.Config;
using Cradiator.Model;
using NUnit.Framework;

namespace Cradiator.Tests.Services
{
	[TestFixture]
	public class BuildDataTransformer_Tests
	{
		CruiseAddress _cruiseAddress;
		IConfigSettings _configSettings;
		BuildDataTransformer _transformer;

		[SetUp]
		public void SetUp()
		{
			_cruiseAddress = new CruiseAddress("http://valid/XmlStatusReport.aspx");
			_configSettings = new ConfigSettings { URL = _cruiseAddress.Url };
			_transformer = new BuildDataTransformer(_configSettings);
		}

		[Test]
		public void build_status_is_building_if_building_and_last_build_status_was_failure()
		{
			const string xml =
				@"<Projects>
					<Project name='FooProject' category='' activity='Building' lastBuildStatus='Failure' lastBuildLabel='292' lastBuildTime='2007-11-16T15:03:46.358374-05:00' nextBuildTime='2007-11-16T15:31:00.2683768-05:00' webUrl='http://foo/ccnet'/>
                </Projects>";

			var projectStatuses = _transformer.Transform(xml);
			Assert.That(projectStatuses.First().CurrentState, Is.EqualTo(ProjectStatus.BUILDING));
		}

		[Test]
		public void build_status_is_building_if_building_and_last_build_status_was_success()
		{
			const string xml =
				@"<Projects>
					<Project name='FooProject' category='' activity='Building' lastBuildStatus='Success' lastBuildLabel='292' lastBuildTime='2007-11-16T15:03:46.358374-05:00' nextBuildTime='2007-11-16T15:31:00.2683768-05:00' webUrl='http://foo/ccnet'/>
                </Projects>";

			var projectStatuses = _transformer.Transform(xml);
			Assert.That(projectStatuses.First().CurrentState, Is.EqualTo(ProjectStatus.BUILDING));
		}

		[Test]
		public void build_status_is_failure_if_sleeping_and_last_build_status_was_failure()
		{
			const string xml =
				@"<Projects>
					<Project name='FooProject' category='' activity='Sleeping' lastBuildStatus='Failure' lastBuildLabel='292' lastBuildTime='2007-11-16T15:03:46.358374-05:00' nextBuildTime='2007-11-16T15:31:00.2683768-05:00' webUrl='http://foo/ccnet'/>
                </Projects>";

			var projectStatuses = _transformer.Transform(xml);

			Assert.That(projectStatuses.First().CurrentState, Is.EqualTo(ProjectStatus.FAILURE));
		}

		[Test]
		public void build_status_is_success_if_sleeping_and_last_build_status_was_success()
		{
			const string xml =
				@"<Projects>
					<Project name='FooProject' category='' activity='Sleeping' lastBuildStatus='Success' lastBuildLabel='292' lastBuildTime='2007-11-16T15:03:46.358374-05:00' nextBuildTime='2007-11-16T15:31:00.2683768-05:00' webUrl='http://foo/ccnet'/>
                  </Projects>";

			var projectStatuses = _transformer.Transform(xml);

			Assert.That(projectStatuses.First().CurrentState, Is.EqualTo(ProjectStatus.SUCCESS));
		}

		const string SimilarProjectXml =
				@"<Projects>
					<Project name='FooProject' category='' activity='Sleeping' lastBuildStatus='Success' lastBuildLabel='292' lastBuildTime='2007-11-16T15:03:46.358374-05:00' nextBuildTime='2007-11-16T15:31:00.2683768-05:00' webUrl='http://foo/ccnet'/>
					<Project name='BarProject' category='' activity='Sleeping' lastBuildStatus='Failure' lastBuildLabel='8' lastBuildTime='2007-11-16T05:00:00.2127436-05:00' nextBuildTime='2007-11-17T05:00:00-05:00' webUrl='http://foo/ccnet'/>
					<Project name='FunProject' category='' activity='Sleeping' lastBuildStatus='Failure' lastBuildLabel='39' lastBuildTime='2007-11-16T05:50:00.1105168-05:00' nextBuildTime='2007-11-17T05:50:00-05:00' webUrl='http://foo/ccnet'/>
				</Projects>";

		[Test]
		public void CanFilter_ProjectName_With_BeginsWith_RegEx()
		{
			_configSettings.ProjectNameRegEx = "^F.*";
			_transformer = new BuildDataTransformer(_configSettings);

			var projectStatuses = _transformer.Transform(SimilarProjectXml);

			Assert.That(projectStatuses.Count(), Is.EqualTo(2));
			Assert.That(projectStatuses.First().Name, Is .EqualTo("FooProject"));
			Assert.That(projectStatuses.Skip(1).First().Name, Is.EqualTo("FunProject"));
		}

		[Test]
		public void CanFilter_ProjectName_With_OR_RegEx()
		{
			_configSettings.ProjectNameRegEx = "FooProject|BarProject";
			_transformer = new BuildDataTransformer(_configSettings);

			var projectStatuses = _transformer.Transform(SimilarProjectXml);

			Assert.That(projectStatuses.Count(), Is.EqualTo(2));
			Assert.That(projectStatuses.First().Name, Is.EqualTo("FooProject"));
			Assert.That(projectStatuses.Skip(1).First().Name, Is.EqualTo("BarProject"));
		}

		const string ThreeProjectsProjectXml =
				@"<Projects>
					<Project name='FooProject' category='' activity='Sleeping' lastBuildStatus='Success' lastBuildLabel='292' lastBuildTime='2007-11-16T15:03:46.358374-05:00' nextBuildTime='2007-11-16T15:31:00.2683768-05:00' webUrl='http://foo/ccnet'/>
					<Project name='BarProject' category='' activity='Sleeping' lastBuildStatus='Failure' lastBuildLabel='8' lastBuildTime='2007-11-16T05:00:00.2127436-05:00' nextBuildTime='2007-11-17T05:00:00-05:00' webUrl='http://foo/ccnet'/>
					<Project name='One_More_Project' category='' activity='Sleeping' lastBuildStatus='Failure' lastBuildLabel='39' lastBuildTime='2007-11-16T05:50:00.1105168-05:00' nextBuildTime='2007-11-17T05:50:00-05:00' webUrl='http://foo/ccnet'/>
				</Projects>";

		[Test]
		public void CanFilter_ProjectName_With_PlainProjectName_AsRegEx()
		{
			_configSettings.ProjectNameRegEx = "BarProject";
			_transformer = new BuildDataTransformer(_configSettings);
			var projectStatuses = _transformer.Transform(ThreeProjectsProjectXml);

			Assert.That(projectStatuses.Count(), Is.EqualTo(1));
			Assert.That(projectStatuses.First().Name, Is.EqualTo("BarProject"));
		}

		[Test]
		public void CanTransform_MultipleProjects_WithNoFiltering()
		{
			var projectStatuses = _transformer.Transform(ThreeProjectsProjectXml);
			Assert.That(projectStatuses.Count(), Is.EqualTo(3));
			Assert.That(projectStatuses.First().Name, Is.EqualTo("FooProject"));
			Assert.That(projectStatuses.Skip(1).First().Name, Is.EqualTo("BarProject"));
			Assert.That(projectStatuses.Skip(2).First().Name, Is.EqualTo("One_More_Project"));
		}

		[Test]
		public void RegExFilter_IsUpdated_After_ConfigUpdated()
		{
			_configSettings.ProjectNameRegEx = "FooProject|BarProject";
			_transformer = new BuildDataTransformer(_configSettings);
			var projectStatuses = _transformer.Transform(SimilarProjectXml);
			Assert.That(projectStatuses.Count(), Is.EqualTo(2));

			// notify of config change and fetch again
			var newSettings = new ConfigSettings { ProjectNameRegEx = "BarProject", URL = _cruiseAddress.Url };
			_transformer.ConfigUpdated(newSettings);
			projectStatuses = _transformer.Transform(SimilarProjectXml);
			Assert.That(projectStatuses.Count(), Is.EqualTo(1));
			Assert.That(projectStatuses.First().Name, Is.EqualTo("BarProject"));
		}

		[Test]
		public void CurrentMessage_IsSet_IfPresent_InXml()
		{
			const string xml =
				@"<Projects>
					<Project name='FooProject' CurrentMessage='A message' category='' />
                </Projects>";

			var projectStatuses = _transformer.Transform(xml);

			Assert.That(projectStatuses.First().CurrentMessage, Is.EqualTo("A message"));
		}

        [Test]
        public void CanHandleNull()
        {
            var projectStatuses = _transformer.Transform(null);

            Assert.That(projectStatuses.Count(), Is.EqualTo(0));
        }

		[Test]
		public void CanFilter_Category()
		{
			const string xml =
				@"<Projects>
					<Project name='ImportantProject' category='Important' />
					<Project name='LowPriorityProject' category='LowPriority'/>
                </Projects>";

			_configSettings.CategoryRegEx = "Important";
			_transformer = new BuildDataTransformer(_configSettings);
			var projectStatuses = _transformer.Transform(xml);

			Assert.That(projectStatuses.Count(), Is.EqualTo(1));
			Assert.That(projectStatuses.First().Name, Is.EqualTo("ImportantProject"));
		}
	}
}