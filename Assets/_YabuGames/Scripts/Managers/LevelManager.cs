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
        
        private float _xOffset=.4f;

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
            
            var firstDigitObj = Instantiate(Resources.Load<GameObject>($"Spawnables/{firstDigit}"), parent.transform);
            var secondDigitObj = Instantiate(Resources.Load<GameObject>($"Spawnables/{secondDigit}"), parent.transform);

            firstDigitObj.GetComponent<MeshCollider>().enabled = false;
            secondDigitObj.GetComponent<MeshCollider>().enabled = false;
            
            firstDigitObj.transform.SetLocalPositionAndRotation(new Vector3(-_xOffset, 0, 0), Quaternion.Euler(0, 180, 0));
            secondDigitObj.transform.SetLocalPositionAndRotation(new Vector3(_xOffset, 0, 0), Quaternion.Euler(0, 180, 0));
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
        }
        
        private void UnSubscribe()
        {
            CoreGameSignals.Instance.OnSave -= Save;
            CoreGameSignals.Instance.OnLevelWin -= LevelWin;
        }

        #endregion
        
        private void GetValues()
        {
            levelID = PlayerPrefs.GetInt("levelID", 1);
        }

        private void LevelWin()
        {
           // if(false) return;
            levelID++;
        }

        private void Save()
        {
            PlayerPrefs.SetInt("levelID",levelID);
        }
        
    }
}