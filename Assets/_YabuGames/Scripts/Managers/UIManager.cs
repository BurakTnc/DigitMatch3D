using System;
using _YabuGames.Scripts.Signals;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _YabuGames.Scripts.Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;
        
        [SerializeField] private GameObject mainPanel, gamePanel, winPanel, losePanel, storePanel;
        [SerializeField] private TextMeshProUGUI[] moneyText;
        [SerializeField] private Image[] icons = new Image[3];
        [SerializeField] private Sprite currentIcon, passedIcon, neutralIcon;
        [SerializeField] private Image progressBar;
        [SerializeField] private GameObject retryButton;


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

        }

        private void OnEnable()
        {
            Subscribe();
        }

        private void OnDisable()
        {
            UnSubscribe();
        }

        private void Start()
        {
            SetMoneyTexts();
            SetIcons();
        }

        #region Subscribtions
        private void Subscribe()
                {
                    CoreGameSignals.Instance.OnGameWin += LevelWin;
                    CoreGameSignals.Instance.OnLevelFail += LevelLose;
                    CoreGameSignals.Instance.OnGameStart += OnGameStart;
                    CoreGameSignals.Instance.OnLevelWin += SetIcons;
                    CoreGameSignals.Instance.OnMistake += Mistake;

                }
        
                private void UnSubscribe()
                {
                    CoreGameSignals.Instance.OnGameWin -= LevelWin;
                    CoreGameSignals.Instance.OnLevelFail -= LevelLose;
                    CoreGameSignals.Instance.OnGameStart -= OnGameStart;
                    CoreGameSignals.Instance.OnLevelWin -= SetIcons;
                    CoreGameSignals.Instance.OnMistake -= Mistake;
                }

        #endregion
        
        private void OnGameStart()
        {
            mainPanel.SetActive(false);
            gamePanel.SetActive(true);
            SetIcons();
        }

        private void Mistake()
        {
            retryButton.SetActive(true);
        }
        private void SetIcons()
        {
            var id = LevelManager.Instance.levelID;
            retryButton.SetActive(false);
            switch (id)
            {
                case 0:
                    icons[0].sprite = currentIcon;
                    icons[1].sprite = neutralIcon;
                    icons[2].sprite = neutralIcon;
                    progressBar.fillAmount = 0;
                    break;
                case 1:
                    icons[0].transform.DOScale(Vector3.one * 1.2f, .4f).SetLoops(2, LoopType.Yoyo);
                    icons[0].sprite = passedIcon;
                    icons[1].sprite = currentIcon;
                    icons[2].sprite = neutralIcon;
                    progressBar.DOFillAmount(.5f, 2f).SetEase(Ease.InSine);
                    break;
                case 2:
                    icons[0].sprite = passedIcon;
                    icons[1].transform.DOScale(Vector3.one * 1.2f, .4f).SetLoops(2, LoopType.Yoyo);
                    icons[1].sprite = passedIcon;
                    icons[2].sprite = currentIcon;
                    progressBar.DOFillAmount(1f, 2f).SetEase(Ease.InSine);
                    break;

            }
        }
        private void SetMoneyTexts()
        {
            if (moneyText.Length <= 0) return;

            foreach (var t in moneyText)
            {
                if (t)
                {
                    t.text = "$" + GameManager.Instance.GetMoney();
                }
            }
        }
        private void LevelWin()
        {
            gamePanel.SetActive(false);
            winPanel.SetActive(true);
            HapticManager.Instance.PlaySuccessHaptic();
        }

        private void LevelLose()
        {
            gamePanel.SetActive(false);
            gamePanel.SetActive(true);
            HapticManager.Instance.PlayFailureHaptic();
        }

        public void PlayButton()
        {
            CoreGameSignals.Instance.OnGameStart?.Invoke();
            HapticManager.Instance.PlaySelectionHaptic();
        }

        public void MenuButton()
        {
            mainPanel.SetActive(true);
            HapticManager.Instance.PlayLightHaptic();
        }

        public void NextButton()
        {
            CoreGameSignals.Instance.OnLevelLoad?.Invoke();
            HapticManager.Instance.PlaySelectionHaptic();
        }

        public void ResetButton()
        {
            LevelManager.Instance.ResetPhase();
        }

        public void RetryButton()
        {
            CoreGameSignals.Instance.OnLevelLoad?.Invoke();
            HapticManager.Instance.PlaySelectionHaptic();
        }
    }
}
