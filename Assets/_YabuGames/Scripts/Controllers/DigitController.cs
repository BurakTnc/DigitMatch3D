using System;
using UnityEngine;

namespace _YabuGames.Scripts.Controllers
{
    public class DigitController : MonoBehaviour
    {
        public int digitValue;

        private void Start()
        {
            var sayi = 16;
            var onluk = (int) sayi / 10;
            var birlik = sayi % 10;
            var parent = new GameObject();
            var firstDigit = Instantiate(Resources.Load<GameObject>($"Spawnables/{onluk}"),parent.transform);
            var secondDigit = Instantiate(Resources.Load<GameObject>($"Spawnables/{birlik}"),parent.transform);
            parent.transform.position = new Vector3(0, 0, 17);
        }
    }
}
