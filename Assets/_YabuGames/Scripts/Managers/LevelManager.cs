using System;
using System.Collections.Generic;
using _YabuGames.Scripts.Controllers;
using _YabuGames.Scripts.Signals;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _YabuGames.Scripts.Managers
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance;
        public int levelID;
        [HideInInspector] public int givenValue;

        [SerializeField] private List<int> valueList = new List<int>();
        [SerializeField] private Transform givenValuePosition;
        [SerializeField] private GameObject[] levels = new GameObject[3];
        
        private float _xOffset=.4f;
        private GameObject _firstDigitObj, _secondDigitObj;
        private void Awake()
        {
            #region Singleton

            if (Instance != this && Instance != null) 
            {
                Destroy(this);
                return;
            }

            Instance = this;

            #endregion
            
            GetValues();
        }

        private void Start()
        {
            ExtractTheGivenValue();
        }

        private void ExtractTheGivenValue()
        {

            givenValue = valueList[levelID];
            var parent = new GameObject();
            parent.name = "Given Value = " + givenValue;
            var firstDigit = givenValue / 10;
            var secondDigit = givenValue % 10;
            
            _firstDigitObj = Instantiate(Resources.Load<GameObject>($"Spawnables/{firstDigit}"), parent.transform);
            _secondDigitObj = Instantiate(Resources.Load<GameObject>($"Spawnables/{secondDigit}"), parent.transform);

            _firstDigitObj.GetComponent<MeshCollider>().enabled = false;
            _secondDigitObj.GetComponent<MeshCollider>().enabled = false;
            
            _firstDigitObj.transform.SetLocalPositionAndRotation(new Vector3(-_xOffset, 0, 0), Quaternion.Euler(0, 180, 0));
            _secondDigitObj.transform.SetLocalPositionAndRotation(new Vector3(_xOffset, 0, 0), Quaternion.Euler(0, 180, 0));
            parent.transform.SetPositionAndRotation(givenValuePosition.position, givenValuePosition.rotation);

        }

        private void OnEnable()
        {
            Subscribe();
        }

        private void OnDisable()
        {
            UnSubscribe();
        }

        #region Subscribtons

        private void Subscribe()
        {
            CoreGameSignals.Instance.OnSave += Save;
            CoreGameSignals.Instance.OnLevelWin += LevelWin;
            CoreGameSignals.Instance.OnGameStart += Initialize;
        }
        
        private void UnSubscribe()
        {
            CoreGameSignals.Instance.OnSave -= Save;
            CoreGameSignals.Instance.OnLevelWin -= LevelWin;
            CoreGameSignals.Instance.OnGameStart -= Initialize;
        }

        #endregion
        
        private void GetValues()
        {
            levelID = PlayerPrefs.GetInt("levelID", 1);
        }

        private void LevelWin()
        {
            if (levelID==3)
            {
                CoreGameSignals.Instance.OnGameWin?.Invoke();
                return;
            }
            levelID++;
            Initialize();
        }

        private void Initialize()
        {
            foreach (var t in levels)
            {
                t.SetActive(false);
            }
            levels[levelID].SetActive(true);

            if (_firstDigitObj)
            {
                var temp1 = _firstDigitObj;
                var temp2 = _secondDigitObj;
            
                Destroy(temp1);
                Destroy(temp2);
            }
            
            ExtractTheGivenValue();
        }

        private void Save()
        {
            PlayerPrefs.SetInt("levelID",levelID);
        }
        
    }
}