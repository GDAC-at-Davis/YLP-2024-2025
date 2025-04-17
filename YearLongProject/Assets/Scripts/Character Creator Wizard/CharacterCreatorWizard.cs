using UnityEngine;
using UnityEditor;
using System.IO;
using CharacterScripts;
using System.Collections;

public class CharacterCreatorWizard : EditorWindow
{
    // Important variable that user will select
    bool tog3D = false;
    string characterName = "";
    string rootpath;  

    // used for loading UI and indicating the process is complete
    // 
    // PROMPTING: This state is for when the users inputs information about the new character 
    // CREATING: This state is for when the code is creating the files and folder for the characters. 
    // 		 Usually the code is fasting enough that users won't see this states.
    // COMPLETE: This state is for when all the files and folder are finished being created. 
    enum WizardState {PROMPTING, CREATING, COMPLETE};

    WizardState currState = WizardState.PROMPTING;

    [MenuItem("GDAC YLP/Character Creator Wizard")]
    public static void ShowWindow()
    {
	    CharacterCreatorWizard c = (CharacterCreatorWizard)GetWindow(typeof(CharacterCreatorWizard));
	    c.minSize = new Vector2(300, 200);
	    c.maxSize = new Vector2(400, 300);
    }
    
    private void OnGUI()
    {
	    if (currState == WizardState.PROMPTING)
	    {
		    GUILayout.Label("This tool automatically creates the necessary template folder for a fighter");
		    GUILayout.FlexibleSpace();
		    characterName = EditorGUILayout.TextField("Character Name:", characterName);
		    tog3D = EditorGUILayout.Toggle("3D:", tog3D);
		    GUILayout.FlexibleSpace();
		    if (GUILayout.Button("Create"))
		    {
			if (!string.IsNullOrWhiteSpace(characterName))
			{
				Debug.Log("All three steps have been completed");
				currState = WizardState.CREATING;
				CreateFiles();
			}
			else 
			{
				Debug.Log("character requires a name");
			}
		    }
	    }
	    else if (currState == WizardState.CREATING)
	    {
		    GUILayout.Label("Files and folder will take some time to be created");
	    }
	    else if (currState == WizardState.COMPLETE)
	    {
		    GUILayout.Label("Setup Complete!");
		    GUILayout.FlexibleSpace();
		    if (GUILayout.Button("Complete"))
		    {
	    		Close();
		    }
	    }
    }

    // for this class update is used to track if the wizard's creation process was complete.
    void Update()
    {
	    if (currState == WizardState.CREATING)
	    {
		    bool finished = CheckCompletion();
		    if (finished)
		    {
			currState = WizardState.COMPLETE;
		    }
	    }
    }

    // This method check if every necessary file and folder exist at the specificed location
    // This indicates that the wizard had already done it's job
    bool CheckCompletion()
    {
	    bool finished = true;

	    // check if each needed file and folder exists
	    finished = Directory.Exists(rootpath + "/Timelines");
	    finished = File.Exists(rootpath + "/Timelines.meta");
	    finished = Directory.Exists(rootpath + "/States");
	    finished = File.Exists(rootpath + "/States.meta");
	    finished = Directory.Exists(rootpath + "/Sprite");
	    finished = File.Exists(rootpath + "/Sprite.meta");
	    finished = Directory.Exists(rootpath + "/Animations");
	    finished = File.Exists(rootpath + "/Animations.meta");
	    finished = Directory.Exists(rootpath + "/Scripts");
	    finished = File.Exists(rootpath + "/Scripts.meta");
	    finished = Directory.Exists(rootpath + "/Prefabs");
	    finished = File.Exists(rootpath + "/Prefabs.meta");
	    finished = Directory.Exists(rootpath + "/Materials");
	    finished = File.Exists(rootpath + "/Materials.meta");
	    finished = Directory.Exists(rootpath + "/Shaders");
	    finished = File.Exists(rootpath + "/Shaders.meta");
	    if (tog3D)
	    {
		finished = Directory.Exists(rootpath + "/Models");
		finished = File.Exists(rootpath + "/Models.meta");
	    }

	    finished = File.Exists(rootpath + "/" + characterName + "SO.asset");
	    finished = File.Exists(rootpath + "/" + characterName + "SO.asset.meta");
	    finished = File.Exists(rootpath + "/" + characterName + ".prefab");
	    finished = File.Exists(rootpath + "/" + characterName + ".prefab.meta");

	    return finished;
    }


    // This functions creates the file and folders necessary for an individual character 
    private void CreateFiles()
    {
	    // create base file for character 
	    rootpath = Application.dataPath + "/Fighters/" + characterName;
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

	    AssetDatabase.Refresh();
	   
	    // edit scriptable object to include name and prefab
	    ((CharacterSO)newSO).CharacterDisplayName = characterName;
	    GameObject characterPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Fighters/" + characterName + "/" + characterName + ".prefab", typeof(GameObject));
	    if (characterPrefab == null)
	    {
		    Debug.Log("aig4eioagaegeioageoagnaognaoghaoiefhaklecnaejokbvneoabg");
	    }
	    ((CharacterSO)newSO).CharacterPrefab = characterPrefab;

    }

}
