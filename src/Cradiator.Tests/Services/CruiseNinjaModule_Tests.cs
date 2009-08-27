using Cradiator.App;
using Cradiator.Config;
using Cradiator.Views;
using NUnit.Framework;
using Rhino.Mocks;

namespace Cradiator.Tests.Services
{
    [TestFixture]
    public class CruiseNinjaModule_Tests
    {
        [Test]
        public void CanBoot()
        {
            var view = MockRepository.GenerateStub<ICradiatorView>();
            var configSettings = MockRepository.GenerateStub<IConfigSettings>();
            configSettings.ProjectNameRegEx = ".*";
            configSettings.CategoryRegEx = ".*";
            var boot = new Bootstrapper(configSettings, view);
            boot.CreateKernel();

            configSettings.AssertWasCalled(c=>c.Load());
        }
    }
}