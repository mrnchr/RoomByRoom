using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using static System.StringComparison;
using static System.StringSplitOptions;

namespace RoomByRoom.Testing.EditorMode
{
  public class CodeConventionsTesting
  {
    private static readonly char[] _whitespaces = { ' ', '\t', '\r', '\n' };

    private static readonly string[] _typeStrings =
    {
      "class",
      "struct",
      "enum",
      "interface"
    };

    private static IEnumerable<string> CSharpFilePaths =>
      AssetDatabase
        .FindAssets("t:TextAsset", new[] { "Assets" })
        .Select(AssetDatabase.GUIDToAssetPath)
        .Where(path => path.EndsWith(".cs", InvariantCultureIgnoreCase));

    [TestCaseSource(nameof(CSharpFilePaths))]
    public void FilesShouldContainClassInNamespace(string assetPath)
    {
      string[] classText = AssetDatabase
        .LoadAssetAtPath<TextAsset>(assetPath)
        .text.Split(_whitespaces, RemoveEmptyEntries);

      foreach (string type in _typeStrings)
      {
        if (classText.Contains(type))
          classText.Should().ContainInOrder("namespace", type);
      }
    }
  }
}