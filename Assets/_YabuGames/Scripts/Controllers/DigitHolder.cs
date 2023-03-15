using System;
using DG.Tweening;
using UnityEngine;

namespace _YabuGames.Scripts.Controllers
{
    public class DigitHolder: MonoBehaviour
    {
        [SerializeField] private Transform firstDigitPlace, secondDigitPlace;

        private GameObject _firstObj, _secondObj;
        private int _firstDigit, _secondDigit;
        private int _digitCount;
        private OperationController _operationController;

        private void Awake()
        {
            _operationController =GetComponent<OperationController>();
        }

        public void PlaceTheDigit(GameObject digit)
        {
            if (_digitCount<1)
            {
                _firstObj = digit;
                _firstDigit = digit.GetComponent<DigitController>().digitValue;
                digit.transform.DOKill();
                digit.transform.DORotate(new Vector3(0, -180, 0), .5f).SetEase(Ease.OutBack);
                digit.transform.DOMove(firstDigitPlace.position + Vector3.up, .3f)
                    .OnComplete(() => LastMove(digit, firstDigitPlace));
                _digitCount++;
            }
            else
            {
                _secondObj = digit;
                _secondDigit = digit.GetComponent<DigitController>().digitValue;
                digit.transform.DOKill();
                digit.transform.DORotate(new Vector3(0, -180, 0), .5f).SetEase(Ease.OutBack);
                digit.transform.DOMove(secondDigitPlace.position + Vector3.up, .3f)
                    .OnComplete(() => LastMove(digit, secondDigitPlace));
                _digitCount++;
            }
            
        }

        private void LastMove(GameObject digit,Transform place)
        {
            digit.transform.DOMove(place.position, .5f).SetEase(Ease.OutBack).OnComplete(StartCalculation);
        }

        private void StartCalculation()
        {
            if (_digitCount>=2)
            {
                _operationController.firstValue = _firstDigit;
                _operationController.secondValue = _secondDigit;
                _digitCount = 0;
                _firstObj.transform.DOScale(Vector3.zero, .5f).SetEase(Ease.InBack);
                _secondObj.transform.DOScale(Vector3.zero, .5f).SetEase(Ease.InBack)
                    .OnComplete(_operationController.StartCalculate);
               // _operationController.StartCalculate();
            }
            
        }
    }
}