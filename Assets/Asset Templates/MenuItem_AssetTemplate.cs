using QuickMenu;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace AssetTemplates
{
    public class MenuItem_AssetTemplate : IMenuItem
    {
        [AddQuickMenuItems]
        public static IEnumerable<IMenuItem> _AddQuickMenuItems()
        {
            foreach (var assetTemplate in assetTemplates)
                yield return new MenuItem_AssetTemplate(assetTemplate);
        }

        static HashSet<AssetTemplate> _assetTemplates;
        public static HashSet<AssetTemplate> assetTemplates
        {
            get
            {
                if (_assetTemplates is null)
                    _assetTemplates = new();

                if (_assetTemplates.Count == 0)
                {
                    _assetTemplates = AssetDatabase.FindAssets($"t:{typeof(AssetTemplate).FullName}")
                        .Select(x => AssetDatabase.GUIDToAssetPath(x))
                        .Select(x => AssetDatabase.LoadAssetAtPath<AssetTemplate>(x))
                        .ToHashSet();
                }

                return _assetTemplates;
            }
        }
        
        public bool visible => true;
        public int priority => 0;

        public string title => assetTemplate?.title ?? null;
        public string description => assetTemplate?.description ?? null;

        public string category => "Template";
        public string subCategory => null;

        readonly AssetTemplate assetTemplate;
        readonly AssetTemplate.Variable[] variables;

        public MenuItem_AssetTemplate(AssetTemplate assetTemplate)
        {
            this.assetTemplate = assetTemplate;
            variables = assetTemplate.variables.ToArray();
        }

        public bool Validation(Context context)
        {
            return context.isProjectBrowserWithAsset;
        }

        public bool Command(Context context)
        {
            foreach (var variable in variables)
            {
                if (variable.required && string.IsNullOrEmpty(variable.value))
                {
                    Debug.LogError($"Variable \"{variable.name}\" is empty but is required");
                    return false;
                }
            }

            var fileTransfers = assetTemplate.GenerateFileTransfers(context.assetFolder, variables);
            if (fileTransfers.Count == 0)
            {
                Debug.LogError($"There are no files to create");
                return false;
            }

            foreach (var fileTransfer in fileTransfers)
            {
                if (File.Exists(fileTransfer.target))
                {
                    Debug.LogError($"File already exists: {fileTransfer.target}");
                    return false;
                }
            }

            foreach (var fileTransfer in fileTransfers)
                new FileInfo(fileTransfer.target).Directory.Create();

            foreach (var fileTransfer in fileTransfers)
                File.WriteAllText(fileTransfer.target, fileTransfer.contents);

            AssetDatabase.Refresh();

            var asset = AssetDatabase.LoadAssetAtPath(fileTransfers[0].target, typeof(Object));
            AssetDatabase.OpenAsset(asset);

            return true;
        }

        public IEnumerable<VisualElement> GetParameterFields()
        {
            for (int i = 0; i < variables.Length; i++)
            {
                var _i = i;

                var field = new TextField
                {
                    label = variables[_i].name,
                    value = variables[_i].value,
                    multiline = variables[_i].multilineValue
                };

                field.RegisterValueChangedCallback(
                    e => variables[_i].value = e.newValue
                );

                if (variables[_i].multilineValue)
                    field.style.minHeight = 40;

                yield return field;
            }
        }
    }
}