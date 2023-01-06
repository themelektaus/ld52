using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AssetTemplates
{
    [CreateAssetMenu]
    public class AssetTemplate : ScriptableObject
    {
        [Serializable]
        public struct Variable
        {
            public string name;
            public bool required;
            public bool multilineValue;
            [Multiline] public string value;

            public static implicit operator Variable((string name, string value) variable) =>
                new() { name = variable.name, value = variable.value };
        }

        [Serializable]
        public struct FileTransfer
        {
            public string target;
            public string contents;
        }

        public string title;
        public string description;
        public Variable[] variables;

        [SerializeField, Button("Create/Open Template Folder")] bool _Create_Open_Template_Folder;
        void Create_Open_Template_Folder()
        {
            var folder = GetTemplateFolder();
            Directory.CreateDirectory(folder);
            EditorUtility.OpenWithDefaultApp(folder);
        }

        public string GetName()
        {
            if (!string.IsNullOrEmpty(title))
                return title;
            return name;
        }

        public List<FileTransfer> GenerateFileTransfers(string targetPath, IEnumerable<Variable> variables)
        {
            var files = new List<FileTransfer>();
            var sourceFolder = GetTemplateFolder().Replace('/', '\\');
            foreach (var file in Directory.EnumerateFiles(sourceFolder, "*.*", SearchOption.AllDirectories))
            {
                var targetFile = ReplaceVariables(file[sourceFolder.Length..], variables);
                files.Add(new()
                {
                    target = Path.Combine(targetPath, targetFile.Trim('\\')).Replace('/', '\\'),
                    contents = ReplaceVariables(File.ReadAllText(file), variables)
                });
            }
            return files;
        }

        public string GetTemplateFolder()
        {
            var path = AssetDatabase.GetAssetPath(this);
            return Path.Combine(
                path[..(path.Length - Path.GetFileName(path).Length)],
                Path.GetFileNameWithoutExtension(path) + "~"
            );
        }

        public static string ReplaceVariables(string contents, IEnumerable<Variable> variables)
        {
            Dictionary<string, string> replacements = new();

            foreach (var variable in variables.Concat(variables))
            {
                var value = variable.value.Trim();

                if (string.IsNullOrEmpty(value))
                    continue;

                replacements[variable.name] = value;
            }

            foreach (var key in replacements.Keys)
                contents = contents.Replace($"__{key}__", replacements[key]);

            contents = contents
                .Replace("\r\n", "\n")
                .Replace("\r", "\n")
                .Replace("\n", "\r\n");

            return contents;
        }
    }
}