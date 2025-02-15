//--------------------------------------------------------------------
//
// This is a Unity behaviour script that demonstrates how to build
// a loading screen that can be used at the start of a game to
// load both FMOD Studio data and Unity data without blocking the
// main thread, using bank names or asset references for
// Addressables: https://docs.unity3d.com/Manual/com.unity.addressables.html.
//
// This document assumes familiarity with Unity scripting. See
// https://unity3d.com/learn/tutorials/topics/scripting for resources
// on learning Unity scripting.
//
// For information on using FMOD example code in your own programs, visit
// https://www.fmod.com/legal
//
//--------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_ADDRESSABLES_EXIST
using UnityEngine.AddressableAssets;
#endif

namespace FMOD
{
    internal class AsyncBankLoader : MonoBehaviour
    {
#if UNITY_ADDRESSABLES_EXIST
        // List of asset references to load if using Addressables
        public List<AssetReference> AssetReferenceBanks = new List<AssetReference>();
#else
        // List of Banks to load
        [BankRef]
        public List<string> Banks = new();
#endif

        // The name of the scene to load and switch to
        public string Scene;

        public void Start()
        {
            StartCoroutine(LoadGameAsync());
        }

        private void Update()
        {
            // Update the loading indication
        }

        private IEnumerator LoadGameAsync()
        {
            // Start an asynchronous operation to load the scene
            AsyncOperation async = SceneManager.LoadSceneAsync(Scene);

            // Don't let the scene start until all Studio Banks have finished loading
            async.allowSceneActivation = false;

#if UNITY_ADDRESSABLES_EXIST
            // Iterate all the asset references and start loading their studio banks
            // in the background, including their audio sample data
            foreach (var bank in AssetReferenceBanks)
            {
                FMODUnity.RuntimeManager.LoadBank(bank, true);
            }
#else
            // Iterate all the Studio Banks and start them loading in the background
            // including the audio sample data
            foreach (string bank in Banks)
            {
                RuntimeManager.LoadBank(bank, true);
            }
#endif

            // Keep yielding the co-routine until all the bank loading is done
            // (for platforms with asynchronous bank loading)
            while (!RuntimeManager.HaveAllBanksLoaded)
            {
                yield return null;
            }

            // Keep yielding the co-routine until all the sample data loading is done
            while (RuntimeManager.AnySampleDataLoading())
            {
                yield return null;
            }

            // Allow the scene to be activated. This means that any OnActivated() or Start()
            // methods will be guaranteed that all FMOD Studio loading will be completed and
            // there will be no delay in starting events
            async.allowSceneActivation = true;

            // Keep yielding the co-routine until scene loading and activation is done.
            while (!async.isDone)
            {
                yield return null;
            }
        }
    }
}