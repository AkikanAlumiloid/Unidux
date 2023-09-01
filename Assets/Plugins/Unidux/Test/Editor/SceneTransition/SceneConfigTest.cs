using System.Collections.Generic;
using NUnit.Framework;

namespace Unidux.SceneTransition
{
    public class SceneConfigTest
    {
        [Test]
        public void GetPageScenesTest()
        {
            var config = new SampleConfig();
            Assert.AreEqual(new[]
            {
                SampleScene.PageScene1,
                SampleScene.PageScene2,
                SampleScene.PageActiveScene3,
                SampleScene.PageActiveScene4,
            }, config.GetPageScenes());
        }

        [Test]
        public void GetActiveSceneFromPageTest()
        {
            var config = new SampleConfig();
            Assert.AreEqual(
                SampleScene.PageActiveScene3,
                config.GetActiveSceneFromPage(SamplePage.Page1)
                );
            Assert.AreNotEqual(
                SampleScene.PageScene1,
                config.GetActiveSceneFromPage(SamplePage.Page1)
                );
            Assert.AreNotEqual(
                SampleScene.ModalScene1,
                config.GetActiveSceneFromPage(SamplePage.Page1)
                );
            Assert.AreEqual(
                SampleScene.PageActiveScene4,
                config.GetActiveSceneFromPage(SamplePage.Page2)
                );
        }

        enum SampleScene
        {
            PageScene1,
            PageScene2,
            ModalScene1,
            PageActiveScene3,
            PageActiveScene4,
        }

        enum SamplePage
        {
            Page1,
            Page2,
        }

        class SampleConfig : ISceneConfig<SampleScene, SamplePage>
        {
            public IDictionary<SampleScene, int> CategoryMap
            {
                get
                {
                    return new Dictionary<SampleScene, int>()
                    {
                        {SampleScene.PageScene1, SceneCategory.Page},
                        {SampleScene.PageScene2, SceneCategory.Page},
                        {SampleScene.ModalScene1, SceneCategory.Modal},
                        {SampleScene.PageActiveScene3, SceneCategory.Page },
                        {SampleScene.PageActiveScene4, SceneCategory.Page },
                    };
                }
            }

            public IDictionary<SamplePage, SampleScene[]> PageMap
            {
                get
                {
                    return new Dictionary<SamplePage, SampleScene[]>()
                    {
                        {SamplePage.Page1, new[] {SampleScene.PageScene1, SampleScene.PageActiveScene3}},
                        {SamplePage.Page2, new[] {SampleScene.PageActiveScene4, SampleScene.PageScene2}},
                    };
                }
            }

            public IDictionary<SamplePage, SampleScene> ActiveSceneMap
            {
                get
                {
                    return new Dictionary<SamplePage, SampleScene>()
                    {
                        { SamplePage.Page1, SampleScene.PageActiveScene3 },
                        { SamplePage.Page2, SampleScene.PageActiveScene4 },
                    };
                }
            }
        }
    }
}