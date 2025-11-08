using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorFunctions
{
	// Shortcuts
	private class EditorShortcuts : Editor
	{
		// ToDo: Switching scenes
		//private void OnSceneGUI()
		//{
		//	Event e = Event.current;
		//	switch (e.type)
		//	{
		//		case EventType.KeyDown:
		//			switch (e.keyCode)
		//			{
		//				case KeyCode.LeftShift:
		//					if (e.keyCode == KeyCode.Alpha1)
		//					{
		//						//SceneManager
		//					}
		//					break;
		//			}
		//			break;
		//	}
		//}
	}

	private const string materialsPath = "Assets/Art/Materials/";
	private const string texturesPath = "Assets/Art/Textures";
	private const string mat = ".mat";
	private const string png = ".png";
	private const string texturesString = "Textures/";
	private const string materialsString = "Materials/";
	private const string baseColor = "_BaseColor";
	private const string baseColorMap = "_BaseColorMap";
	private const string mask = "_MaskMap";
	private const string normalMap = "_NormalMap";
	private const string normal = "_Normal";
	private const char slash = '/';
	[MenuItem("Rooms/Create Material And Assign Textures", false, 0)]
	public static void CreateMaterialAndAssignTextures()
	{
		var selected = Selection.gameObjects;
		for (int i = 0; i < selected.Length; i++)
		{
			string objectName = selected[i].name;
			string fullPath = materialsPath + objectName + mat;
			Material newMaterial = new Material(Shader.Find("HDRP/Lit"));
			AssetDatabase.CreateAsset(newMaterial, fullPath);
			Material asset = (Material)AssetDatabase.LoadAssetAtPath(fullPath, typeof(Material));
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

			AssignTextures();

			var renderer = selected[i].GetComponent<MeshRenderer>();
			if (renderer != null)
			{
				renderer.material = asset;
			}

			Selection.activeObject = asset;

			void AssignTextures()
			{
				if (asset == null)
				{
					Debug.Log(string.Format("<color=red><b>{0}</b></color>", "No material at path: " + fullPath));
				}
				else
				{
					asset.EnableKeyword("_NORMALMAP");
					AssignTexture(texturesPath + objectName + baseColor + png, baseColorMap);
					AssignTexture(texturesPath + objectName + mask + png, mask);
					AssignTexture(texturesPath + objectName + normal + png, normalMap);
				}
			}
			void AssignTexture(string path, string whichMap)
			{
				Texture2D tex = (Texture2D)AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D));
				if (path.Contains(normal))
				{
					TextureImporter importer = (TextureImporter)AssetImporter.GetAtPath(path);
					TextureImporterSettings settings = new TextureImporterSettings();
					importer.ReadTextureSettings(settings);
					settings.textureType = TextureImporterType.NormalMap;
					importer.SetTextureSettings(settings);
					importer.SaveAndReimport();
				}
				if (tex == null)
				{
					Debug.Log(string.Format("<color=red><b>{0}</b></color>", "No texture at path: " + path));
				}
				else
				{
					asset.SetTexture(whichMap, tex);
				}
			}
		}
	}
	[MenuItem("GameObject/Find Textures", false, 0)]
	public static void FindMaterialAndAssignTextures()
	{
		foreach (var s in Selection.gameObjects)
		{
			var renderer = s.GetComponent<Renderer>();
			if (renderer)
			{
				AssignTextures(renderer.sharedMaterial);
			}
		}
	}

	// FixMe: todo: merge methods

	[MenuItem("GameObject/Create Materials and Find Textures", false, 0)]
	public static void CreateMaterialsAndFindTextures()
	{
		if (Selection.objects.Length <= 0) return;
		var selected = Selection.gameObjects;
		var shader = Shader.Find("HDRP/Lit");
		List<Material> tempList = new List<Material>();
		for (int i = 0; i < selected.Length; i++)
		{
			string objectName = selected[i].name;

			var textures = FindTextures(objectName);
			if (textures.Count <= 0)
			{
				Debug.Log(string.Format("<color=red><b>{0}</b></color>", "Can't find textures"));
				continue;
			}
			Material newMaterial = new Material(shader);
			var texturePath = AssetDatabase.GetAssetPath(textures[0]);
			var materialPath = texturePath.Replace(texturesString, materialsString);
			materialPath = materialPath.Replace(slash + textures[0].name + png, string.Empty);
			materialPath = UpdateToValidFolder(materialPath);
			string fullPath = materialPath += slash + objectName + mat;

			AssetDatabase.CreateAsset(newMaterial, fullPath);
			Debug.Log(string.Format("<color=#abd97e><b>{0}</b></color>", "Created Material in " + fullPath));

			Material asset = (Material)AssetDatabase.LoadAssetAtPath(fullPath, typeof(Material));
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

			asset.SetTexture(baseColorMap, textures[0]);
			asset.SetTexture(mask, textures[1]);
			asset.EnableKeyword("_NORMALMAP");
			asset.SetTexture(normalMap, textures[2]);

			var renderers = selected[i].GetComponentsInChildren<Renderer>();
			if (renderers != null && renderers.Length > 0)
			{
				foreach (var renderer in renderers)
				{
					renderer.sharedMaterial = asset;
				}
			}
			tempList.Add(asset);
		}

		Selection.objects = tempList.ToArray();
		//			EditorGUIUtility.PingObject(asset.GetInstanceID());
	}

	public static string UpdateToValidFolder(string path)
	{
		if (AssetDatabase.IsValidFolder(path) == false)
		{
			var splitted = path.Split(new char[] { slash }, System.StringSplitOptions.RemoveEmptyEntries);
			var currentFolderToTry = string.Empty;
			for (int i = 0; i < splitted.Length; i++)
			{
				if (i >= splitted.Length - 1)
				{
					continue;
				}
				else
				{
					if (i == 0)
					{
						currentFolderToTry += splitted[i];
					}
					else
					{
						currentFolderToTry += slash + splitted[i];
					}
				}
			}
			return UpdateToValidFolder(currentFolderToTry);
		}
		return path;
	}

	private static List<Texture2D> FindTextures(string objectName)
	{
		List<Texture2D> tempList = new List<Texture2D>();
		var listOfGuids = new List<string>();

		FindTexture(objectName + baseColor, baseColorMap);
		FindTexture(objectName + mask, mask);
		FindTexture(objectName + normal, normalMap);

		return tempList;

		void FindTexture(string textureName, string whichMap)
		{
			Texture2D tex = null;
			IterateThroughSubFolders(texturesPath, textureName);
			for (int i = 0; i < listOfGuids.Count; i++)
			{
				var realPath = AssetDatabase.GUIDToAssetPath(listOfGuids[i]);
				tex = (Texture2D)AssetDatabase.LoadAssetAtPath(realPath, typeof(Texture2D));
				if (textureName.Contains(normal))
				{
					TextureImporter importer = (TextureImporter)AssetImporter.GetAtPath(realPath);
					TextureImporterSettings settings = new TextureImporterSettings();
					importer.ReadTextureSettings(settings);
					settings.textureType = TextureImporterType.NormalMap;
					importer.SetTextureSettings(settings);
					importer.SaveAndReimport();
				}
			}
			listOfGuids.Clear();
			if (tex == null)
			{
				Debug.Log(string.Format("<color=red><b>{0}</b></color>", "No texture found: " + textureName));
			}
			else
			{
				tempList.Add(tex);
			}
		}

		void IterateThroughSubFolders(string folderName, string tName)
		{
			TryAddingToList(folderName);
			foreach (var sub in AssetDatabase.GetSubFolders(folderName))
			{
				if (AssetDatabase.GetSubFolders(sub).Length == 0)
				{
					TryAddingToList(sub);
				}
				else
				{
					IterateThroughSubFolders(sub, tName);
				}
			}
			void TryAddingToList(string folder)
			{
				var guids = AssetDatabase.FindAssets(tName, new string[] { folder });
				if (guids.Length > 0)
				{
					listOfGuids.Add(guids[0]);
				}
			}
		}
	}

	[MenuItem("CONTEXT/Material/Find Textures", false, 0)]
	[MenuItem("Tools/Dwarves/Find textures for selected materials")]
	public static void TryAssigningTextures()
	{
		if (Selection.objects.Length <= 0) return;
		Material asset = null;
		string path = string.Empty;

		var selected = Selection.assetGUIDs;
		for (int i = 0; i < selected.Length; i++)
		{
			path = AssetDatabase.GUIDToAssetPath(selected[i]);
			asset = (Material)AssetDatabase.LoadAssetAtPath(path, typeof(Material));
			if (asset != null)
			{
				AssignTextures(asset);
			}
			else
			{
				Debug.Log(string.Format("<color=red><b>{0}</b></color>", i + " of selected assets is not a material"));
			}
		}
		Selection.objects = null;
	}

	private static void AssignTextures(Material mat)
	{
		var listOfArrays = new List<string[]>();

		AssignTexture(mat.name + baseColor, baseColorMap);
		AssignTexture(mat.name + mask, mask);
		mat.EnableKeyword("_NORMALMAP");
		AssignTexture(mat.name + normal, normalMap);

		void AssignTexture(string textureName, string whichMap)
		{
			Texture2D tex = null;
			IterateThroughSubFolders(texturesPath, textureName);
			for (int i = 0; i < listOfArrays.Count; i++)
			{
				Debug.Log(string.Format("<color=white><b>{0}</b></color>", "processing: " + listOfArrays[i][0]));
				var realPath = AssetDatabase.GUIDToAssetPath(listOfArrays[i][0]);
				tex = (Texture2D)AssetDatabase.LoadAssetAtPath(realPath, typeof(Texture2D));
				if (textureName.Contains(normal))
				{
					TextureImporter importer = (TextureImporter)AssetImporter.GetAtPath(realPath);
					TextureImporterSettings settings = new TextureImporterSettings();
					importer.ReadTextureSettings(settings);
					settings.textureType = TextureImporterType.NormalMap;
					importer.SetTextureSettings(settings);
					importer.SaveAndReimport();
				}
			}
			listOfArrays.Clear();
			if (tex == null)
			{
				Debug.Log(string.Format("<color=red><b>{0}</b></color>", "No texture found: " + textureName));
			}
			else
			{
				mat.SetTexture(whichMap, tex);
				Selection.activeObject = mat;
				EditorGUIUtility.PingObject(mat.GetInstanceID());
			}
		}

		void IterateThroughSubFolders(string folderName, string tName)
		{
			TryAddingToList(folderName);
			foreach (var sub in AssetDatabase.GetSubFolders(folderName))
			{
				if (AssetDatabase.GetSubFolders(sub).Length == 0)
				{
					TryAddingToList(sub);
				}
				else
				{
					IterateThroughSubFolders(sub, tName);
				}
			}
			void TryAddingToList(string folder)
			{
				var guids = AssetDatabase.FindAssets(tName, new string[] { folder });
				if (guids.Length > 0)
				{
					listOfArrays.Add(guids);
					Debug.Log(string.Format("<color=#abd97e><b>{0}</b></color>", "found texture in: " + folder));
				}
			}
		}
	}

	private const string miniMapPath = "Assets/Graphics/Sprites/MiniMap/";
	private const string mapLayerName = "Map";
	private const string prefabString = ".prefab";
	[MenuItem("Rooms/Find and Assign map")]
	public static void FindAndAssignMap()
	{
		if (Selection.objects.Length <= 0) return;
		var selected = Selection.gameObjects;
		foreach (var s in selected)
		{
			string assetPath = miniMapPath + s.name + png;
			Sprite asset = (Sprite)AssetDatabase.LoadAssetAtPath(assetPath, typeof(Sprite));
			if (asset == null)
			{
				Debug.Log(string.Format("<color=red><b>{0}</b></color>", "No sprite at path: " + assetPath));
			}
			else
			{
				var mapObject = new GameObject("MiniMap Sprite");
				mapObject.layer = LayerMask.NameToLayer(mapLayerName);
				var mapObjectTransform = mapObject.transform;
				mapObjectTransform.parent = s.transform;
				mapObjectTransform.localPosition = new Vector3(7.5f, 0, -7.5f);
				mapObjectTransform.localRotation = Quaternion.Euler(90, 0, 0);
				mapObjectTransform.localScale = Vector3.one * 2;
				var spriteRenderer = mapObject.AddComponent<SpriteRenderer>();
				spriteRenderer.sprite = asset;
				//Selection.activeObject = mapObject;
				PrefabUtility.ApplyPrefabInstance(s, InteractionMode.AutomatedAction);
				Selection.activeObject = mapObject;
			}
		}
		Selection.objects = null;
	}

	private const string prefabsPath = "Assets/Prefabs/Dungeon/Caverns/new/";
	private const string end = "end";

	private void MakePrefab(GameObject prefab)
	{
		PrefabUtility.SaveAsPrefabAssetAndConnect(prefab, prefabsPath + prefab.name + prefabString, InteractionMode.AutomatedAction);
	}

	public static GameObject PutMeshAsChild(GameObject s)
	{
		GameObject parent = new GameObject(s.name);
		parent.transform.position = s.transform.position;
		s.transform.SetParent(parent.transform);
		return parent;
	}

}

public class DistributeObjects : EditorWindow
{
	private float offset;
	private bool groupEnabled;
	private bool xBool;
	private bool yBool;
	private bool zBool;

	void OnGUI()
	{
		GUILayout.Label("Base Settings", EditorStyles.boldLabel);
		offset = EditorGUILayout.FloatField(offset);
		GUILayout.Space(25);

		groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
		xBool = EditorGUILayout.Toggle("Use X", xBool);
		yBool = EditorGUILayout.Toggle("Use Y", yBool);
		zBool = EditorGUILayout.Toggle("Use Z", zBool);
		EditorGUILayout.EndToggleGroup();
		if (GUILayout.Button("Distribute!"))
		{
			Distribute();
		}
	}

	[MenuItem("Tools/Dwarves/Distribute Objects")]
	static void Init()
	{
		DistributeObjects window = (DistributeObjects)GetWindow(typeof(DistributeObjects));
		window.Show();
	}

	void Distribute()
	{
		GameObject[] objs = Selection.gameObjects;
		var offsetSoFar = 0f;
		for (int i = 0; i < objs.Length; i++)
		{
			var t = objs[i].transform;
			var size = objs[i].GetComponentInChildren<MeshFilter>().sharedMesh.bounds.size;
			if (xBool)
			{
				var val = size.x + offset + offsetSoFar;
				t.position = new Vector3(val, 0, 0);
				offsetSoFar += offset + size.x;
			}
			if (yBool)
			{
				var val = size.y + offset + offsetSoFar;
				t.position = new Vector3(0, val, 0);
				offsetSoFar += offset + size.y;
			}
			if (zBool)
			{
				var val = size.z + offset + offsetSoFar;
				t.position = new Vector3(0, 0, val);
				offsetSoFar += offset + size.z;
			}
		}
	}


}
