using System;
using UnityEngine;
using Gameplay;
using Managers.Pool;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;


namespace Managers
{
    public class CoreManager
    {
        private Action<bool> _onComplete;
        public static CoreManager instance;
        public Player player { get; private set; }
        public BossEnemy boss { get; private set; }
        

        public PoolManager PoolManager{ get; private set; }

        public CoreManager(Action<bool> onComplete)
        {
            if (instance != null)
            {
                Debug.LogException(new Exception("CoreManager already exists!"));
                return;
            }

            instance = this;
            _onComplete = onComplete;
            
            
        }
        
        public void LoadBoss()
        {
            var originalBoss = Resources.Load<BaseEnemy>("Boss");
            if (Camera.main == null) return;
            var pos = (Vector2)Camera.main.transform.position;
            boss =  (BossEnemy)Object.Instantiate(originalBoss);
            boss.Init();
        }

        public void LoadPlayer()
        {
            var originalPlayer = Resources.Load<Player>("Player");
            if (Camera.main == null) return;
            var pos = (Vector2)Camera.main.transform.position;
            player = Object.Instantiate(originalPlayer);
        }

        public void LoadManagers()
        {
            PoolManager = new();
            _onComplete?.Invoke(true);
        }

    }
}
