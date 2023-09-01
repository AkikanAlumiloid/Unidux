using System;
using System.Collections.Generic;
using System.Linq;

namespace Unidux.SceneTransition
{
    public interface ISceneConfig<TScene, TPage>
    {
        IDictionary<TScene, int> CategoryMap { get; }

        IDictionary<TPage, TScene[]> PageMap { get; }

        IDictionary<TPage, TScene> ActiveSceneMap { get; }
    }

    public static class ISceneConfigExtension
    {
        public static IEnumerable<TScene> GetPageScenes<TScene, TPage>(
            this ISceneConfig<TScene, TPage> config)
        {
            return config.CategoryMap
                .Where(entry => entry.Value == SceneCategory.Page)
                .Select(entry => entry.Key);
        }

        public static TScene GetActiveSceneFromPage<TScene, TPage>(
            this ISceneConfig<TScene, TPage> config, TPage targetPage)
            where TPage : Enum
            where TScene : Enum
        {

            KeyValuePair<TPage, TScene[]> map;
            try
            {
                map = config
                .PageMap
                .Where(x => Comparer<TPage>.Default.Compare(x.Key, targetPage) == 0)
                .First();
            }
            catch (InvalidOperationException e)
            {
                UnityEngine.Debug.LogWarning(e);
                return default;
            }

            IDictionary<TPage, TScene> act = config.ActiveSceneMap;
            TScene ret = default;
            foreach (KeyValuePair<TPage, TScene> child in act)
            {
                foreach (TScene mapValueChild in map.Value)
                {
                    if (Comparer<TScene>.Default.Compare(mapValueChild, child.Value) == 0)
                    {
                        ret = mapValueChild;
                    }
                }
            }
            
            return ret;
        }
    }
}