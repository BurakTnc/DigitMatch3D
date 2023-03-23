using System;
using System.Collections;
using System.Collections.Generic;
using _YabuGames.Scripts.Controllers;
using _YabuGames.Scripts.ScriptableObjects;
using _YabuGames.Scripts.Signals;
using DG.Tweening;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace _YabuGames.Scripts.Managers
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance;
        
        public  List<EnemyData> levels=new List<EnemyData>();

        public int levelID;
        [HideInInspector] public int givenValue;
        [SerializeField] private Transform givenValuePosition;
        
        [SerializeField] private GameObject slashIcon;

        private Transform _currentParent;
        private GameObject _oldPlatform;
        private List<GameObject> _currentPhases = new List<GameObject>();
        public int phaseLevel;
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
            Initialize();
        }

        private void ExtractTheGivenValue()
        {

            givenValue = levels[levelID].givenValue[phaseLevel];
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
            CoreGameSignals.Instance.OnMistake += Mistake;
        }
        
        private void UnSubscribe()
        {
            CoreGameSignals.Instance.OnSave -= Save;
            CoreGameSignals.Instance.OnLevelWin -= LevelWin;
            CoreGameSignals.Instance.OnGameStart -= Initialize;
            CoreGameSignals.Instance.OnMistake -= Mistake;
        }

        #endregion
        
        private void GetValues()
        {
            levelID = PlayerPrefs.GetInt("levelID", 0);
            phaseLevel = PlayerPrefs.GetInt("phaseLevel", 0);
        }

        private IEnumerator OpenSlash()
        {
            slashIcon.SetActive(true);
           yield return new WaitForSeconds(1);
           slashIcon.SetActive(false);
        }
        private void Mistake()
        {
            StartCoroutine(OpenSlash());
        }
        private void LevelWin()
        {
            if (phaseLevel==2)
            {
                levelID++;
                phaseLevel = 0;
                if (levelID>=levels.Count)
                {
                    levelID = 0;
                }
                CoreGameSignals.Instance.OnGameWin?.Invoke();
                CoreGameSignals.Instance.OnSave?.Invoke();
                return;
            }
            HapticManager.Instance.PlaySuccessHaptic();
            phaseLevel++;
            _oldPlatform = _currentParent.gameObject;
            CoreGameSignals.Instance.OnSave?.Invoke();
            Initialize();
        }

        private void Initialize()
        {
            var newPlatform = Instantiate(levels[levelID].phases[phaseLevel]);
            _currentParent = newPlatform.transform;
            _currentPhases.Add(newPlatform);
            newPlatform.SetActive(true);
            newPlatform.transform.position = new Vector3(17, -7.69f, 8.23f);
            newPlatform.transform.DOMoveX(3.70f, .7f).SetEase(Ease.OutBack).SetDelay(1.5f);
            
            if(!_oldPlatform) 
                return;
            _oldPlatform.transform.DOMoveX(-14.70f, .7f).SetEase(Ease.InBack).SetDelay(.5f).SetRelative(true);

            if (!_firstDigitObj) 
                return;
            var temp1 = _firstDigitObj;
            var temp2 = _secondDigitObj;
            Destroy(temp1);
            Destroy(temp2);
            ExtractTheGivenValue();
        }

        private void Save()
        {
            PlayerPrefs.SetInt("levelID",levelID);
            PlayerPrefs.SetInt("phaseLevel",phaseLevel);
        }

        public void ResetPhase()
        {
     
            SceneManager.LoadScene(0);
        }

        public Transform GetPlatform()
        {
            return _currentParent;
        }
    }
}