/*
using UnityEngine;
using UnityEditor;
using System.Collections;

public class MakeDifficultySetting {

	[MenuItem("Assets/Create/Difficulty Setting")]
	public static void CreateMyAsset()
	{
		DifficultySetting asset = ScriptableObject.CreateInstance<DifficultySetting>();

		AssetDatabase.CreateAsset(asset, "Assets/Data/DifficultySetting.asset");

		AssetDatabase.SaveAssets();

		EditorUtility.FocusProjectWindow();

		Selection.activeObject = asset;
	}
}
*/
