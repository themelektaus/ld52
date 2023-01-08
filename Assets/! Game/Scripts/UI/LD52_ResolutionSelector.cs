using System.Collections.Generic;
using System.Linq;

using UnityEngine;

#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEditor;
#endif

namespace Prototype
{
    public class LD52_ResolutionSelector : MonoBehaviour
    {
        [SerializeField] LD52_ResolutionSelectorItem itemTemplate; 

        public readonly List<LD52_ResolutionSelectorItem> items = new();

#if UNITY_EDITOR
        static Assembly GetAssembly()
        {
            return typeof(Editor).Assembly;
        }

        static Type GetGameView()
        {
            return GetAssembly().GetType("UnityEditor.GameView");
        }

        static EditorWindow GetGameViewWindow(Type gameView)
        {
            return EditorWindow.GetWindow(gameView);
        }

        static Type GetGameViewSizes()
        {
            return GetAssembly().GetType("UnityEditor.GameViewSizes");
        }

        static object GetGameViewSizesInstance(Type gameViewSizes)
        {
            var singleton = typeof(ScriptableSingleton<>).MakeGenericType(gameViewSizes);
            return singleton.GetProperty("instance").GetValue(null, null);
        }

        static int GetIndex()
        {
            var gameView = GetGameView();
            var window = GetGameViewWindow(gameView);
            var property = window.GetType().GetProperty("selectedSizeIndex");
            return (int) property.GetValue(window);
        }

        static void SetIndex(int index)
        {
            var gameView = GetGameView();
            var window = GetGameViewWindow(gameView);
            var method = gameView.GetMethod("SizeSelectionCallback");
            method.Invoke(window, new object[] { index, null });
        }

        static List<string> GetEntries()
        {
            var gameViewSizes = GetGameViewSizes();
            var gameViewSizesInstance = GetGameViewSizesInstance(gameViewSizes);
            var group = gameViewSizes.GetMethod("GetGroup").Invoke(gameViewSizesInstance, new object[] { 0 });
            var texts = group.GetType().GetMethod("GetDisplayTexts");
            return (texts.Invoke(group, null) as string[]).ToList();
        }

        void Start()
        {
            var entries = GetEntries();

            var index = GetIndex();
            var i = 0;

            foreach (var entry in entries)
            {
                var item = itemTemplate.gameObject
                    .Instantiate(itemTemplate.transform.parent)
                    .GetComponent<LD52_ResolutionSelectorItem>();

                item.text.text = entry;

                items.Add(item);

                if (index == i)
                    item.Select();

                int _i = i++;
                item.onClick.AddListener(() => SetIndex(_i));
            }

            itemTemplate.gameObject.Destroy();
        }
#else
        void Start()
        {
            var entries = Screen.resolutions
                .Select(x => (x.width, x.height))
                .Distinct()
                .ToList();

            var index = entries.FindIndex(item =>
                item.width == Screen.width &&
                item.height == Screen.height
            );

            var i = 0;

            foreach (var (width, height) in entries)
            {
                var item = itemTemplate.gameObject
                    .Instantiate(itemTemplate.transform.parent)
                    .GetComponent<LD52_ResolutionSelectorItem>();

                item.text.text = $"{width} x {height}";

                items.Add(item);

                if (index == i)
                    item.Select();

                int _i = i++;
                item.onClick.AddListener(() =>
                {
                    var (width, height) = entries[_i];
                    Screen.SetResolution(width, height, Screen.fullScreen);
                });
            }

            itemTemplate.gameObject.Destroy();
        }
#endif
    }
}