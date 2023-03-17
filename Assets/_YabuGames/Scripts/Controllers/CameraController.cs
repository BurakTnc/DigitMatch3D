using System;
using _YabuGames.Scripts.Signals;
using UnityEngine;

namespace _YabuGames.Scripts.Controllers
{
    public class CameraController : MonoBehaviour
    {
        private GameObject _confetti;

        private void Awake()
        {
            _confetti = transform.GetChild(0).gameObject;
        }

        private void OnEnable()
        {
            CoreGameSignals.Instance.OnLevelWin += Win;
        }

        private void OnDisable()
        {
            CoreGameSignals.Instance.OnLevelWin -= Win;
        }

        private void Win()
        {
            _confetti.SetActive(false);
            _confetti.SetActive(true);
        }
    }
}
