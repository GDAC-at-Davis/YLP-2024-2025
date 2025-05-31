using System.Collections.Generic;
using System.IO;
using CharacterScripts;
using Hitbox.Emitters;
using Menus;
using Movement;
using State_Machine_Scripts;
using Timeline.FastFall;
using Timeline.Hitboxes;
using Timeline.LockFlipX;
using Timeline.RigidbodyTween;
using Timeline.SetTransitionStates;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

public class CharacterCreatorWizard : EditorWindow
{
    // used for loading UI and indicating the process is complete
    // 
    // PROMPTING: This state is for when the users inputs information about the new character 
    // CREATING: This state is for when the code is creating the files and folder for the characters. 
    // 		 Usually the code is fasting enough that users won't see this states.
    // COMPLETE: This state is for when all the files and folder are finished being created. 
    private enum WizardState
    {
        PROMPTING,
        CREATING,
        COMPLETE
    }

    private const string templateCharPrefix = "TemplateChar";

    // Important variable that user will select
    private bool tog3D;
    private string characterName = "";
    private string rootpathAbsolute;
    private string rootpathRelative;

    private WizardState currState = WizardState.PROMPTING;

    // for this class update is used to track if the wizard's creation process was complete.
    private void Update()
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

    private void OnGUI()
    {
        var wrappedText = new GUIStyle(GUI.skin.label);
        wrappedText.wordWrap = true;

        if (currState == WizardState.PROMPTING)
        {
            GUILayout.Label("This tool automatically creates the necessary base folders and files for a fighter",
                wrappedText);
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

    [MenuItem("GDAC YLP/Character Creator Wizard")]
    public static void ShowWindow()
    {
        var c = (CharacterCreatorWizard)GetWindow(typeof(CharacterCreatorWizard));
        c.minSize = new Vector2(300, 200);
        c.maxSize = new Vector2(300, 300);
    }

    // This method check if every necessary file and folder exist at the specificed location
    // This indicates that the wizard had already done it's job
    private bool CheckCompletion()
    {
        var finished = true;

        // check if each needed file and folder exists
        //
        // sub folders:
        var folderNames = new List<string>
            { "Timelines", "States", "Sprite", "Animations", "Scripts", "Prefabs", "Materials", "Shaders" };
        if (tog3D)
        {
            folderNames.Add("Models");
        }

        foreach (string folderName in folderNames)
        {
            finished = Directory.Exists(rootpathAbsolute + folderName);
            finished = File.Exists($"{rootpathAbsolute}{folderName}.meta");
        }

        // upper level files
        var fileNames = new List<string> { $"{characterName}SO.asset", $"{characterName}.prefab" };

        foreach (string fileName in fileNames)
        {
            finished = File.Exists(rootpathAbsolute + fileName);
            finished = File.Exists($"{rootpathAbsolute}{fileName}.meta");
        }

        return finished;
    }

    // This functions creates the file and folders necessary for an individual character 
    private void CreateFiles()
    {
        // create base file for character 
        rootpathAbsolute = $"{Application.dataPath}/Fighters/{characterName}/"; // use for c# base methods 
        rootpathRelative =
            $"Assets/Fighters/{characterName}/"; // relative to unity project folder, used for Unity library methods
        Directory.CreateDirectory(rootpathAbsolute);

        var rpTemplateRelative = $"Assets/Fighters/{templateCharPrefix}/";

        // create subdirectory files for character.
        var folderNames = new List<string>
            { "Timelines", "States", "Sprite", "Animations", "Scripts", "Prefabs", "Materials", "Shaders" };
        if (tog3D)
        {
            folderNames.Add("Models");
        }

        foreach (string folderName in folderNames)
        {
            Directory.CreateDirectory(rootpathAbsolute + folderName);
        }

        // Make variant prefab for the character 
        AssetDatabase.CopyAsset($"{rpTemplateRelative}{templateCharPrefix}.prefab",
            $"{rootpathRelative}{characterName}.prefab");

        // Make copy of the fighter test scene 
        AssetDatabase.CopyAsset("Assets/Scripts/EditorUtils/Editor/CharacterCreatorWizard/DefaultTestScene.unity",
            $"{rootpathRelative}{characterName}TestScene.unity");

        // Copy over state name assets
        var stateNames = new List<string> { "Move", "Jump", "Hitstun", "Light", "Dash", "Heavy", "Special" };
        foreach (string stateName in stateNames)
        {
            string templateCharStateName = templateCharPrefix + stateName;
            string newStateName = ReplaceTemplateNameWithCharacterName(templateCharStateName, characterName);
            AssetDatabase.CopyAsset($"{rpTemplateRelative}States/{templateCharStateName}.asset",
                $"{rootpathRelative}States/{newStateName}.asset");
        }

        // Copy over timeline assets
        var timelineNames = new List<string> { "Hitstun", "Light", "Dash", "Heavy", "Special", "Air", "Idle", "Run" };
        foreach (string tlName in timelineNames)
        {
            string templateCharTlName = templateCharPrefix + tlName;
            string newTlName = ReplaceTemplateNameWithCharacterName(templateCharTlName, characterName);
            AssetDatabase.CopyAsset($"{rpTemplateRelative}Timelines/{templateCharTlName}.playable",
                $"{rootpathRelative}Timelines/{newTlName}.playable");
        }

        // Refresh Assets so Unity creates .meta files
        AssetDatabase.Refresh();

        // Edit Prefab to link it with copied state and timeline files 
        GameObject newFighter = PrefabUtility.LoadPrefabContents($"{rootpathRelative}{characterName}.prefab");

        // Maps state monobehavior gameobject name to the name of the timeline it should use
        // Assigns timelines to state playable directors
        var childStateNames = new List<(string, string)>
        {
            ("MoveState/Move", "Run"),
            ("MoveState/Idle", "Idle"),
            ("MoveState/Air", "Air"),
            ("JumpState", "Air"),
            ("HitstunState", "Hitstun"),
            ("LightAttackState", "Light"),
            ("DashState", "Dash"),
            ("HeavyAttackState", "Heavy"),
            ("SpecialAttackState", "Special")
        };

        foreach ((string, string) csName in childStateNames)
        {
            Transform curr = newFighter.transform.Find($"Action Manager/{csName.Item1}");
            var timeline =
                (PlayableAsset)AssetDatabase.LoadAssetAtPath(
                    $"{rootpathRelative}Timelines/{characterName}{csName.Item2}.playable", typeof(PlayableAsset));
            curr.GetComponent<PlayableDirector>().playableAsset = timeline;

            // set potential bindings for new timeline 
            IEnumerable<TrackAsset> tracks =
                ((TimelineAsset)curr.GetComponent<PlayableDirector>().playableAsset).GetOutputTracks();
            foreach (TrackAsset track in tracks)
            {
                if (track is SetTransitionStatesTrack)
                {
                    curr.GetComponent<PlayableDirector>().SetGenericBinding(track,
                        newFighter.transform.GetComponentInChildren<CharacterActionManager>());
                }
                else if (track is RbTweenTrack)
                {
                    curr.GetComponent<PlayableDirector>().SetGenericBinding(track,
                        newFighter.transform.GetComponentInChildren<CharacterRigidbody2D>());
                }
                else if (track is AnimationTrack)
                {
                    curr.GetComponent<PlayableDirector>().SetGenericBinding(track,
                        newFighter.transform.GetComponentInChildren<Animator>());
                }
                else if (track is EnableFastFallTrack)
                {
                    curr.GetComponent<PlayableDirector>().SetGenericBinding(track,
                        newFighter.transform.GetComponentInChildren<FastFall>());
                }
                else if (track is LockFlipXTrack)
                {
                    curr.GetComponent<PlayableDirector>().SetGenericBinding(track,
                        newFighter.transform.GetComponentInChildren<CharacterFacingDirection>());
                }
                else if (track is HitboxTrack)
                {
                    curr.GetComponent<PlayableDirector>().SetGenericBinding(track,
                        newFighter.transform.GetComponentInChildren<BasicHitboxEmitter>());
                }
            }
        }

        // Assign state name SOs to state monobehaviors
        var childSONames = new List<(string, string)>
        {
            ("MoveState", "Move"),
            ("JumpState", "Jump"),
            ("HitstunState", "Hitstun"),
            ("LightAttackState", "Light"),
            ("DashState", "Dash"),
            ("HeavyAttackState", "Heavy"),
            ("SpecialAttackState", "Special")
        };

        foreach ((string, string) csName in childSONames)
        {
            Transform curr = newFighter.transform.Find($"Action Manager/{csName.Item1}");
            var SO = (StateNameSO)AssetDatabase.LoadAssetAtPath(
                $"{rootpathRelative}States/{characterName}{csName.Item2}.asset",
                typeof(StateNameSO));
            curr.GetComponent<CharacterState>().stateNameSO = SO;
        }

        PrefabUtility.SaveAsPrefabAsset(newFighter, $"{rootpathRelative}{characterName}.prefab");
        PrefabUtility.UnloadPrefabContents(newFighter);

        // create blank CharacterSO and place in directory
        ScriptableObject newSO = CreateInstance<CharacterSO>();
        AssetDatabase.CreateAsset(newSO, $"{rootpathRelative}{characterName}SO.asset");

        // Refresh Assets so Unity creates .meta files
        AssetDatabase.Refresh();

        // Add Character to fighting stage file
        EditorSceneManager.OpenScene($"{rootpathRelative}{characterName}TestScene.unity");

        newFighter = AssetDatabase.LoadAssetAtPath<GameObject>($"{rootpathRelative}{characterName}.prefab");
        Scene newScene = SceneManager.GetSceneByName($"{characterName}TestScene");
        Debug.Log($"p {newFighter} : {newScene}");
        var newFighterInstance = (GameObject)PrefabUtility.InstantiatePrefab(newFighter);
        Debug.Log($"k {newFighterInstance}");
        SceneManager.MoveGameObjectToScene(newFighterInstance, newScene);
        //EditorSceneManager.MarkSceneDirty(newScene);
        EditorSceneManager.SaveScene(newScene);

        // edit scriptable object to include name and prefab
        ((CharacterSO)newSO).CharacterDisplayName = characterName;
        var characterPrefab =
            (GameObject)AssetDatabase.LoadAssetAtPath($"{rootpathRelative}{characterName}.prefab", typeof(GameObject));
        ((CharacterSO)newSO).CharacterPrefab = characterPrefab;

        // Link to main roster
        var mainRoster =
            (CharacterSelectRoster)AssetDatabase.LoadAssetAtPath("Assets/GameData/MainRoster.asset",
                typeof(CharacterSelectRoster));
        mainRoster.AddCharacter((CharacterSO)newSO);
    }

    private string ReplaceTemplateNameWithCharacterName(string templateName, string charName)
    {
        return templateName.Replace(templateCharPrefix, charName);
    }
}