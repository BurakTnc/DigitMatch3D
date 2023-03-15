using System;
using _YabuGames.Scripts.Enums;
using UnityEngine;

namespace _YabuGames.Scripts.Controllers
{
    public class OperationController : MonoBehaviour
    {
        [HideInInspector] public int firstValue, secondValue;
        
        [SerializeField] private OperationMode operationMode;
        [SerializeField] private Transform extractPosition;

        private const float XOffset = .4f;


        private int Calculate()
        {
            switch (operationMode)
            {
                case OperationMode.Addition:
                    return firstValue + secondValue;

                case OperationMode.Subtraction:
                    return Mathf.Abs(firstValue - secondValue);
   
                case OperationMode.Multiplication:
                    return firstValue * secondValue;
                
                case OperationMode.Division:
                    if (firstValue>secondValue)
                    {
                        return (int)(firstValue / secondValue);
                    }
                    return (int)(secondValue / firstValue);
            }

            return 0;
        }

        private void Execute(int calculatedValue)
        {
            var firstDigit = (int)calculatedValue / 10;
            var secondDigit = calculatedValue % 10;
            var parent = new GameObject();
            var firstDigitObj = Instantiate(Resources.Load<GameObject>($"Spawnables/{firstDigit}"), parent.transform);
            var secondDigitObj = Instantiate(Resources.Load<GameObject>($"Spawnables/{secondDigit}"), parent.transform);
            
            firstDigitObj.transform.SetLocalPositionAndRotation(new Vector3(-XOffset, 0, 0), Quaternion.Euler(0, 180, 0));
            secondDigitObj.transform.SetLocalPositionAndRotation(new Vector3(XOffset, 0, 0), Quaternion.Euler(0, 180, 0));
            parent.transform.position = extractPosition.position;
            parent.name = calculatedValue.ToString();
            
            var boxCollider = parent.AddComponent<BoxCollider>();
            boxCollider.center = new Vector3(.4f, .4f, 0);
            boxCollider.size = new Vector3(1.5f, 1, 1);
            parent.AddComponent<GrabController>();
            var digitController = parent.AddComponent<DigitController>();
            digitController.digitValue = calculatedValue;
        }

        public void StartCalculate()
        {
            Execute(Calculate());
        }
    }
}