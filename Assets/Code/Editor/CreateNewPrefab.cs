// Creates a prefab at the given path.
// If a prefab already exists it asks if you want to replace it

using UnityEngine;
using UnityEditor;

public class CreateNewPrefab : EditorWindow
{
	[MenuItem("Prefab/Create New Prefab")]
	static void CreatePrefab()
	{
		GameObject[] objs = Selection.gameObjects;

		foreach (GameObject go in objs)
		{
			if (PrefabUtility.IsOutermostPrefabInstanceRoot(go))
			{
				PrefabUtility.UnpackPrefabInstance(go, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
			}
			/*
			var renderer = go.GetComponentInChildren<Renderer>();
			if (renderer)
			{
				if (renderer is MeshRenderer)
				{

				}
				else if (renderer is SkinnedMeshRenderer)
				{

				}
			}
			var texturePath = AssetDatabase.GetAssetPath(textures[0]);
			var materialPath = texturePath.Replace(texturesString, prefabsString);
			materialPath = texturePath.Replace(slash + textures[0].name + png, string.Empty);
			materialPath = EditorFunctions.UpdateToValidFolder(texturePath);
			string fullPath = materialPath += slash + objectName + mat;
			*/

			string localPath = "Assets/" + go.name + ".prefab";
			if (AssetDatabase.LoadAssetAtPath(localPath, typeof(GameObject)))
			{
				if (EditorUtility.DisplayDialog("Are you sure?",
					"The prefab already exists. Do you want to overwrite it?",
					"Yes",
					"No"))
				{
					CreateNew(go, localPath);
				}
			}
			else
			{
				Debug.Log(go.name + " Prefab Created");
				CreateNew(go, localPath);
			}
		}
	}

	// Disable the menu item if no selection is in place
	[MenuItem("Prefab/Create New Prefab", true)]
	static bool ValidateCreatePrefab()
	{
		return Selection.activeGameObject != null;
	}

	static void CreateNew(GameObject obj, string path)
	{
		Object prefab = PrefabUtility.SaveAsPrefabAsset(obj, path);
		PrefabUtility.SaveAsPrefabAssetAndConnect(obj, path, InteractionMode.AutomatedAction);
	}
}