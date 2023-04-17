#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace GitIntegration
{
	[InitializeOnLoad]
	public class SmartMergeRegistrator
	{
		private const string SmartMergeRegistratorEditorPrefsKey = "smart_merge_installed";
		private const int Version = 1;
		private static readonly string VersionKey = $"{Version}_{Application.unityVersion}";

		//Unity calls the static constructor when the engine opens
		static SmartMergeRegistrator()
		{
			string instaledVersionKey = EditorPrefs.GetString(SmartMergeRegistratorEditorPrefsKey);
			if (instaledVersionKey != VersionKey)
				SmartMergeRegister();
		}

		[MenuItem("Tools/Git/SmartMerge registration")]
		private static void SmartMergeRegister()
		{
			try
			{
				string UnityYAMLMergePath = EditorApplication.applicationContentsPath + "/Tools" + "/UnityYAMLMerge.exe";
				Utils.ExecuteGitWithParams("config merge.unityyamlmerge.name \"Unity SmartMerge (UnityYamlMerge)\"");
				Utils.ExecuteGitWithParams(
					$"config merge.unityyamlmerge.driver \"\\\"{UnityYAMLMergePath}\\\" merge -h -p --force --fallback none %O %B %A %A\"");
				Utils.ExecuteGitWithParams("config merge.unityyamlmerge.recursive binary");
				EditorPrefs.SetString(SmartMergeRegistratorEditorPrefsKey, VersionKey);
				Debug.Log($"Succesfuly registered UnityYAMLMerge with path {UnityYAMLMergePath}");
			}
			catch (Exception e)
			{
				Debug.Log($"Fail to register UnityYAMLMerge with error: {e}");
			}
		}

		[MenuItem("Tools/Git/SmartMerge unregistration")]
		private static void SmartMergeUnRegister()
		{
			Utils.ExecuteGitWithParams("config --remove-section merge.unityyamlmerge");
			Debug.Log("Succesfuly unregistered UnityYAMLMerge");
		}
	}
}
#endif