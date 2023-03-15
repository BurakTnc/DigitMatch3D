using System;
using _YabuGames.Scripts.Enums;
using _YabuGames.Scripts.Managers;
using DG.Tweening;
using UnityEngine;

namespace _YabuGames.Scripts.Controllers
{
    public class DigitHolder: MonoBehaviour
    {
        [SerializeField] private Transform firstDigitPlace, secondDigitPlace;
        [SerializeField] private DigitHolderMode holderMode;
        [SerializeField] private Transform calculatePosition;

        private Animator _animator;
        private GameObject _firstObj, _secondObj;
        private int _firstDigit, _secondDigit;
        private int _digitCount;
        private OperationController _operationController;

        private void Awake()
        {
            if (gameObject.TryGetComponent(out OperationController controller))
            {
                _operationController = controller;
            }
            
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
            if(holderMode==DigitHolderMode.OperationBox) 
                return;
            digit.transform.DORotate(new Vector3(-45, -180, 0), .4f).SetEase(Ease.OutBack);
        }

        private void StartCalculation()
        {
            switch (holderMode)
            {
                case DigitHolderMode.OperationBox:
                    if (_digitCount>=2)
                    {
                        _operationController.firstValue = _firstDigit;
                        _operationController.secondValue = _secondDigit;
                        _digitCount = 0;
                        _firstObj.transform.DOScale(Vector3.zero, .5f).SetEase(Ease.InBack);
                        _firstObj.transform.DOMove(calculatePosition.position, .5f).SetEase(Ease.InBack);
                        _secondObj.transform.DOScale(Vector3.zero, .5f).SetEase(Ease.InBack)
                            .OnComplete(_operationController.StartCalculate);
                        _secondObj.transform.DOMove(calculatePosition.position, .5f).SetEase(Ease.InBack);
                    }
                    break;
                case DigitHolderMode.EqualityCheck:
                    
                    if (_firstDigit<10 && _digitCount>1)
                    {
                        var takenValue = (_firstDigit * 10) + _secondDigit;
                        if (takenValue==LevelManager.Instance.givenValue)
                        {
                            Debug.Log("helaal");
                        }
                        else
                        {
                            Debug.Log("yarrag");
                        }
                    }
                    else
                    {
                        if (_firstDigit < 10) 
                            return;
                        var takenValue = _firstDigit;
                        if (takenValue==LevelManager.Instance.givenValue)
                        {
                            Debug.Log("helaal");
                        }
                        else
                        {
                            Debug.Log("yarrag");
                        }
                    }
                    break;
                
                default:
                    break;
            }
            
            
        }
    }
}