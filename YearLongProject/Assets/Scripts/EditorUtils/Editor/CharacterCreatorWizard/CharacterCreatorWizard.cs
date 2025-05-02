using UnityEngine;
using UnityEditor;
using System.IO;
using CharacterScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using State_Machine_Scripts;
using Timeline.SetTransitionStates;
using Timeline.RigidbodyTween;
using Timeline.LockFlipX;
using Timeline.Hitboxes;
using Timeline.FastFall;
using Hitbox.Emitters;
using Movement;
using Menus;
using UnityEditor.SceneManagement;

public class CharacterCreatorWizard : EditorWindow
{
    // Important variable that user will select
    bool tog3D = false;
    string characterName = "";
    string rootpathAbsolute;
    string rootpathRelative;

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
	    c.maxSize = new Vector2(300, 300);
    }
    
    private void OnGUI()
    {
	    GUIStyle wrappedText = new GUIStyle(GUI.skin.label);
	    wrappedText.wordWrap = true;

	    if (currState == WizardState.PROMPTING)
	    {
		    GUILayout.Label("This tool automatically creates the necessary base folders and files for a fighter", wrappedText);
		    GUILayout.FlexibleSpace();
		    characterName = EditorGUILayout.TextField("Character Name:", characterName);
		    tog3D = EditorGUILayout.Toggle("3D:", tog3D);
		    GUILayout.FlexibleSpace();
		    if (GUILayout.Button("Create"))
		    {
			if (!string.IsNullOrWhiteSpace(characterName))
			{
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
		    GUILayout.Label("Files and folder will take some time to be created", wrappedText);
	    }
	    else if (currState == WizardState.COMPLETE)
	    {
		    GUILayout.Label("Setup Complete!", wrappedText);
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
	    //
	    // sub folders:
	    List<string> folderNames = new List<string> {"Timelines", "States", "Sprite", "Animations", "Scripts", "Prefabs", "Materials", "Shaders"};
	    if (tog3D)
	    {
		    folderNames.Add("Models");
	    }

	    foreach (string folderName in folderNames)
	    {
		    finished = Directory.Exists(rootpathAbsolute + folderName);
		    finished = File.Exists(rootpathAbsolute + folderName + ".meta");
	    }

	    // upper level files
	    List<string> fileNames = new List<string> {characterName + "SO.asset", characterName + ".prefab" };

	    foreach (string fileName in fileNames)
	    {
		    finished = File.Exists(rootpathAbsolute + fileName);
		    finished = File.Exists(rootpathAbsolute + fileName + ".meta");
	    }

	    return finished;
    }


    // This functions creates the file and folders necessary for an individual character 
    private void CreateFiles()
    {
	    // create base file for character 
	    rootpathAbsolute = Application.dataPath + "/Fighters/" + characterName + "/"; // use for c# base methods 
	    rootpathRelative = "Assets/Fighters/" + characterName + "/"; // relative to unity project folder, used for Unity library methods
	    Directory.CreateDirectory(rootpathAbsolute);

	    // create subdirectory files for character.
	    List<string> folderNames = new List<string> {"Timelines", "States", "Sprite", "Animations", "Scripts", "Prefabs", "Materials", "Shaders"};
	    if (tog3D)
	    {
		    folderNames.Add("Models");
	    }
	    foreach (string folderName in folderNames) 
	    {
		    Directory.CreateDirectory(rootpathAbsolute + folderName);
	    } 


	    // Make variant prefab for the character 
	    FileUtil.CopyFileOrDirectory("Assets/Fighters/TheBoxer/TheBoxer.prefab", rootpathRelative + characterName + ".prefab");

	    // Make copy of the fighter test scene 
	    FileUtil.CopyFileOrDirectory("Assets/Scripts/EditorUtils/Editor/CharacterCreatorWizard/DefaultTestScene.unity", rootpathRelative + characterName + "TestScene.unity");
	    
	    // copy over Timeline and States Assets from boxer
	    List<string> stateNames = new List<string> {"Move", "Jump", "Hitstun", "Light", "Dash", "Heavy", "Special"};
	    foreach (string stateName in stateNames)
	    {
		    FileUtil.CopyFileOrDirectory("Assets/Fighters/TheBoxer/States/" + stateName + ".asset", rootpathRelative + "States/" + stateName + ".asset");
	    }
	    
	    List<string> timelineNames = new List<string> {"Hitstun", "Light", "Dash", "Heavy", "Special", "Air", "Idle", "Run"};
	    foreach (string tlName in timelineNames)
	    {
		    FileUtil.CopyFileOrDirectory("Assets/Fighters/TheBoxer/Timelines/" + tlName + ".playable", rootpathRelative + "Timelines/" + tlName + ".playable");
	    }

	    // Refresh Assets so Unity creates .meta files
	    AssetDatabase.Refresh();
	    
	    // Edit Prefab to link it with copied state and timeline files 
	    GameObject newFighter = PrefabUtility.LoadPrefabContents(rootpathRelative + characterName + ".prefab");

	    List<(string, string)> childStateNames = new List<(string, string)> 
	    {
		    ("MoveState/Move", "Run"), ("MoveState/Idle", "Idle"), ("MoveState/Air", "Air"), ("JumpState", "Air"), 
		    ("HitstunState", "Hitstun"), ("LightAttackState", "Light"), ("DashState", "Dash"), ("HeavyAttackState", "Heavy"), ("SpecialAttackState", "Special")
	    };

	    foreach((string, string) csName in childStateNames)
	    {
		    Transform curr = newFighter.transform.Find("Action Manager/" + csName.Item1);
		    var timeline = (PlayableAsset)AssetDatabase.LoadAssetAtPath(rootpathRelative + "Timelines/" + csName.Item2 + ".playable", typeof(PlayableAsset));
		    curr.GetComponent<PlayableDirector>().playableAsset = timeline;

		    // set potential bindings for new timeline 
		    var tracks = ((TimelineAsset)(curr.GetComponent<PlayableDirector>().playableAsset)).GetOutputTracks();
		    foreach (var track in tracks)
		    {
			    if (track is SetTransitionStatesTrack)
			    {
				    curr.GetComponent<PlayableDirector>().SetGenericBinding(track, newFighter.transform.GetComponentInChildren<CharacterActionManager>());
			    }
			    else if (track is RbTweenTrack)
			    {
				    curr.GetComponent<PlayableDirector>().SetGenericBinding(track, newFighter.transform.GetComponentInChildren<CharacterRigidbody2D>());
			    }
			    else if (track is AnimationTrack)
			    {
				    curr.GetComponent<PlayableDirector>().SetGenericBinding(track, newFighter.transform.GetComponentInChildren<Animator>());
			    }
			    else if (track is EnableFastFallTrack)
			    {
				    curr.GetComponent<PlayableDirector>().SetGenericBinding(track, newFighter.transform.GetComponentInChildren<FastFall>());
			    }
			    else if (track is LockFlipXTrack)
			    {
				    curr.GetComponent<PlayableDirector>().SetGenericBinding(track, newFighter.transform.GetComponentInChildren<CharacterFacingDirection>());
			    }
			    else if (track is HitboxTrack)
			    {
				    curr.GetComponent<PlayableDirector>().SetGenericBinding(track, newFighter.transform.GetComponentInChildren<BasicHitboxEmitter>());
			    }
			    else if (track is HitboxTrack)
			    {
				    curr.GetComponent<PlayableDirector>().SetGenericBinding(track, newFighter.transform.GetComponentInChildren<BasicHitboxEmitter>());
			    }
		    }
	    }

	    List<(string, string)> childSONames = new List<(string, string)> 
	    {
		    ("MoveState", "Move"), ("JumpState", "Jump"), 
		    ("HitstunState", "Hitstun"), ("LightAttackState", "Light"), ("DashState", "Dash"), ("HeavyAttackState", "Heavy"), ("SpecialAttackState", "Special")
	    };

	    foreach((string, string) csName in childSONames)
	    {
		    Transform curr = newFighter.transform.Find("Action Manager/" + csName.Item1);
		    var SO = (StateNameSO)AssetDatabase.LoadAssetAtPath(rootpathRelative + "States/" + csName.Item2 + ".asset", typeof(StateNameSO));
		    curr.GetComponent<CharacterState>().stateNameSO = SO; 
	    }

	    PrefabUtility.SaveAsPrefabAsset(newFighter, rootpathRelative + characterName + ".prefab");
	    PrefabUtility.UnloadPrefabContents(newFighter);

	    // create blank CharacterSO and place in directory
	    ScriptableObject newSO = ScriptableObject.CreateInstance("CharacterSO");
	    AssetDatabase.CreateAsset(newSO, rootpathRelative + characterName + "SO.asset");

	    // Refresh Assets so Unity creates .meta files
	    AssetDatabase.Refresh();

	    // Add Character to fighting stage file
	    EditorSceneManager.OpenScene(rootpathRelative + characterName + "TestScene.unity");
	    
	    newFighter = AssetDatabase.LoadAssetAtPath<GameObject>(rootpathRelative + characterName + ".prefab");
	    var newScene = SceneManager.GetSceneByName(characterName + "TestScene");
	    Debug.Log("p " + newFighter + " : " + newScene);
	    GameObject newFighterInstance = (GameObject)(PrefabUtility.InstantiatePrefab(newFighter));
	    Debug.Log("k " + newFighterInstance);
	    SceneManager.MoveGameObjectToScene(newFighterInstance, newScene);
	    EditorSceneManager.MarkSceneDirty(newScene);


	    // edit scriptable object to include name and prefab
	    ((CharacterSO)newSO).CharacterDisplayName = characterName;
	    GameObject characterPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(rootpathRelative + characterName + ".prefab", typeof(GameObject));
	    ((CharacterSO)newSO).CharacterPrefab = characterPrefab;

	    // Link to main roster
	    CharacterSelectRoster mainRoster = (CharacterSelectRoster)AssetDatabase.LoadAssetAtPath("Assets/GameData/MainRoster.asset", typeof(CharacterSelectRoster));
	    mainRoster.AddCharacter(((CharacterSO)newSO));
    }

}
