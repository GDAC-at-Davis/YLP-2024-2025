using UnityEngine;
using UnityEditor;
using System.IO;
using CharacterScripts;

public class CharacterCreatorWizard : EditorWindow
{
    // Important variable that user will select
    bool tog3D = false;
    string characterName = "";

    bool isComplete = false;

    [MenuItem("GDAC YLP/Character Creator Wizard")]
    public static void ShowWindow()
    {
	    CharacterCreatorWizard c = (CharacterCreatorWizard)GetWindow(typeof(CharacterCreatorWizard));
	    c.minSize = new Vector2(300, 200);
	    c.maxSize = new Vector2(300, 300);
    }
    
    private void OnGUI()
    {
	    if (isComplete)
	    {
		    GUILayout.Label("Files will take time to be created");
	    }
	    else 
	    {
		    GUILayout.Label("This tool automatically creates the necessary template folder for a fighter");
		    GUILayout.FlexibleSpace();
		    characterName = EditorGUILayout.TextField("Character Name:", characterName);
		    tog3D = EditorGUILayout.Toggle("3D:", tog3D);
		    GUILayout.FlexibleSpace();
		    if (GUILayout.Button("Complete"))
		    {
			if (!string.IsNullOrWhiteSpace(characterName))
			{
				Debug.Log("All three steps have been completed");
				isComplete = true;
				CreateFiles();
			}
			else 
			{
				Debug.Log("character requires a name");
			}
		    }
	    }
    }

    // The functions that does the leg work of creating all of the files
    private void CreateFiles()
    {
	    // create base file for character 
	    string rootpath = Application.dataPath + "/Fighters/" + characterName;
	    Directory.CreateDirectory(rootpath);

	    // create subdirectory files for character.
	    Directory.CreateDirectory(rootpath + "/Timelines");
	    Directory.CreateDirectory(rootpath + "/States");
	    Directory.CreateDirectory(rootpath + "/Sprite");
	    Directory.CreateDirectory(rootpath + "/Animations");
	    Directory.CreateDirectory(rootpath + "/Scripts");
	    Directory.CreateDirectory(rootpath + "/Prefabs");
	    Directory.CreateDirectory(rootpath + "/Materials");
	    Directory.CreateDirectory(rootpath + "/Shaders");
	    if (tog3D)
	    {
		    Directory.CreateDirectory(rootpath + "/Models");
	    }

	    // create blank CharacterSO and place in directory
	    ScriptableObject newSO = ScriptableObject.CreateInstance("CharacterSO");
	    AssetDatabase.CreateAsset(newSO, "Assets/Fighters/" + characterName + "/" + characterName + "SO.asset");

	    // copy prefab template
	    File.Copy(Application.dataPath + "/Prefabs/FighterBase.prefab", rootpath + "/" + characterName + ".prefab");
    }

}
