using UnityEditor;


namespace ArmnomadsGames
{

	[InitializeOnLoad]
	public static class LayerUtils
	{

		static LayerUtils()
		{
			CreateLayer("LaserSword");
		}


		private static void CreateLayer(string layerName)
		{
			SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
			SerializedProperty layers = tagManager.FindProperty("layers");
			bool ExistLayer = false;

			// Check Layer
			for (int i = 8; i < layers.arraySize; i++)
			{
				SerializedProperty layerSP = layers.GetArrayElementAtIndex(i);

				if (layerSP.stringValue == layerName)
				{
					ExistLayer = true;
					break;
				}
			}

			// Create Layer
			for (int j = 8; j < layers.arraySize; j++)
			{
				SerializedProperty layerSP = layers.GetArrayElementAtIndex(j);
				if (layerSP.stringValue == "" && !ExistLayer)
				{
					layerSP.stringValue = layerName;
					tagManager.ApplyModifiedProperties();

					break;
				}
			}
		}

	}

}