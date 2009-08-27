using System.Reflection;
using System.Windows;
using Cradiator.Model;
using NUnit.Framework;

namespace Cradiator.Tests.Model
{
	[TestFixture]
	public class SkinResourceController_Tests
	{
		[Test]
		public void CanGet_AlreadyLoaded_Resource()
		{
			var skin1 = new Skin("Stack"); // these tests are reliant on xaml file names in the main assembly (Skin folder)
			var skin2 = new Skin("Grid");

			Application.ResourceAssembly = Assembly.GetAssembly(typeof (Skin));

			var skinResourceLoader = new SkinResourceLoader();

			var resourceSkin1 = skinResourceLoader.LoadOrGet(skin1);
			var resourceSkin2 = skinResourceLoader.LoadOrGet(skin2);

			Assert.That(skinResourceLoader.LoadOrGet(skin2), Is.SameAs(resourceSkin2));
			Assert.That(skinResourceLoader.LoadOrGet(skin1), Is.SameAs(resourceSkin1));
		}
	}
}