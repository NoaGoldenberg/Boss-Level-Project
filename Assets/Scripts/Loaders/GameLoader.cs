using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Managers;
using Loaders;
using Debug = UnityEngine.Debug;
using System.Collections;
using Gameplay;
using Random = UnityEngine.Random;

namespace Loaders
{
    public class GameLoader : MonoBehaviour
    {
        
        private void Start()
        {
            StartCoroutine(StartLoadingAsync());
        }

        private IEnumerator StartLoadingAsync()
        {
            yield return new WaitForSeconds(0.1f);
            DontDestroyOnLoad(gameObject);

            LoadCoreSystems();
        }

        private void LoadCoreSystems()
        {
            var coreManager = new CoreManager(OnCoreSystemsLoaded);
            coreManager.LoadManagers();
        }

        private void OnCoreSystemsLoaded(bool isSuccess)
        {
            if (isSuccess)
            {
                LoadMainGameScene();
            }
            else
            {
                Debug.LogError("Core Systems failed to load");
            }
        }

        private void LoadMainGameScene()
        {
            SceneManager.sceneLoaded += OnGameSceneLoaded;
            SceneManager.LoadScene("MainGame");
            
        }

        private void OnGameSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= OnGameSceneLoaded;
            LoadGameEntities();
            FinalizeLoading();
        }

        private void LoadGameEntities()
        {
            CoreManager.instance.LoadPlayer();
            CoreManager.instance.LoadBoss(); 
        }

        
        private void FinalizeLoading()
        {
            Destroy(gameObject);
        }
    }
}
