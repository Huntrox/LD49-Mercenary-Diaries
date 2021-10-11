#if UNITY_EDITOR
using Newtonsoft.Json;
using System.IO;
using UnityEditor;
#endif
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace HuntroxGames.LD49
{
    [CreateAssetMenu(fileName="New ItemsDatabase",menuName="Assets/ItemsDatabase")]
    public class ItemsDatabase : ScriptableObject
    {

		public List<Item> items = new List<Item>();

		public List<Structure> structures = new List<Structure>();
		public List<string> presets = new List<string>();
		public List<string> names = new List<string>();
		public List<string> buildings = new List<string>();
		public List<string> locations = new List<string>();

		public List<string> mercenaryNames = new List<string>();



#if UNITY_EDITOR
		[SerializeField] private TextAsset itemsJson;
		[SerializeField] private TextAsset structersJson;
		[ContextMenu("SO To Json")]
		public void SoToJson()
		{
			if (itemsJson && !items.IsNullOrEmpty())
			{
				string json = JsonConvert.SerializeObject(items);
				string path = Application.dataPath + "/../" + AssetDatabase.GetAssetPath(itemsJson);
				string directory = Path.GetFullPath(path);
				Debug.Log(directory + '\n'+json);
				File.WriteAllText(directory, json);
			}
			if (structersJson && !structures.IsNullOrEmpty())
			{
				string json = JsonConvert.SerializeObject(structures);
				string path = Application.dataPath + "/../" + AssetDatabase.GetAssetPath(structersJson);
				string directory = Path.GetFullPath(path);
				Debug.Log(directory + '\n' + json);
				File.WriteAllText(directory, json);
			}

		}
		[ContextMenu("Json To So")]
		public void JsonToSo()
		{
			var name = Resources.Load<TextAsset>("names").text.Split('\n').ToList();
			mercenaryNames = name;
		}

		[MenuItem("HuntroxUtils/Project/RefreshAssests", false, 0)]
		public static void RefreshAssets()
		{
			if (Selection.assetGUIDs.Length> 0)
			{
				EditorUtility.SetDirty(Selection.activeObject);
			}
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

		}
		[MenuItem("HuntroxUtils/Test", false, 0)]
		public static void TestCode()
		{
			int recLev = 5;
			int levl = 7;
			float healthModPerLev = 0.2f;
			float diffModifire = 0.2f;

			Debug.Log(Mathf.Clamp((recLev - levl),0,int.MaxValue) * healthModPerLev + diffModifire);
		}
		[MenuItem("HuntroxUtils/TestNames", false, 0)]
		public static void TestNameCode()
		{

			Debug.Log(Utils.Utils.GetRandomMercenaryName());
		}

#endif
	}
}