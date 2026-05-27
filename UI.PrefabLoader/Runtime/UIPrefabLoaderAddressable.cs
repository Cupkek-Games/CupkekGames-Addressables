#if UNITY_ADDRESSABLES
using CupkekGames.KeyValueDatabases;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using CupkekGames.Luna;

namespace CupkekGames.AddressableAssets.UI
{
  public abstract class UIPrefabLoaderAddressable<TKey> : PrefabLoaderAddressable<TKey>
  {
    /// <summary>
    /// Instantiate the prefab and immediately trigger its fade-in. The
    /// UI variants of the loader own the "spawn-and-show" auto-fade so
    /// the destination's <c>StartVisible</c> bool stays purely about
    /// born state (no animation baked in). Safe for every born state:
    /// Visible-born views short-circuit inside
    /// <see cref="FadeUIElement.FadeIn"/> via the resolvedStyle guard;
    /// Invisible-born views animate from 0 → 1.
    /// </summary>
    public override GameObject Instantiate(TKey key)
    {
      GameObject instance = base.Instantiate(key);
      if (instance != null && instance.TryGetComponent<UIViewComponent>(out var view))
      {
        view.Show();
      }
      return instance;
    }

    public void FadeOutDestroy(TKey key, float duration = 0.5f)
    {
      List<GameObject> list = GetInstances(key);

      if (list == null)
      {
        return;
      }

      foreach (GameObject go in list)
      {
        UIDocument uiDocument = go.GetComponent<UIDocument>();

        // fade
        FadeUIElement fadeUIElement = new(this, uiDocument.rootVisualElement);
        fadeUIElement.SetDuration(duration);
        fadeUIElement.FadeOut();
      }

      StartCoroutine(DestroyAllOfWithDelay(key, duration));
    }
  }
}
#endif