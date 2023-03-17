using System;
using _YabuGames.Scripts.Enums;
using _YabuGames.Scripts.Managers;
using _YabuGames.Scripts.Signals;
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
        private Camera _cam;

        private void Awake()
        {
            if (gameObject.TryGetComponent(out OperationController controller))
            {
                _operationController = controller;
            }
            _cam=Camera.main;
        }

        public void PlaceTheDigit(GameObject digit)
        {
            if (_digitCount<1)
            {
                var desiredPos = firstDigitPlace.position + Vector3.up;
                _firstObj = digit;
                _firstDigit = digit.GetComponent<DigitController>().digitValue;
                digit.transform.DOKill();
                digit.transform.DORotate(new Vector3(0, -180, 0), .5f).SetEase(Ease.OutBack);
                digit.transform.DOMove(desiredPos, .3f)
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
            var collisionController = digit.GetComponent<CollisionController>();
            var desiredPos = place.position;
            if (collisionController.isMultiplied)
            {
                desiredPos.x += .5f;
            }
            digit.transform.DOMove(desiredPos, .5f).SetEase(Ease.OutBack).OnComplete(StartCalculation);
            if(holderMode==DigitHolderMode.OperationBox) 
                return;
            digit.transform.DORotate(new Vector3(-45, -180, 0), .4f).SetEase(Ease.OutBack);
        }

        private void Win()
        {
            _digitCount = 0;
            CoreGameSignals.Instance.OnLevelWin?.Invoke();
        }

        private void Lose()
        {
            _cam.DOShakeRotation(.5f, Vector3.one, 10, 1, true);
            CoreGameSignals.Instance.OnMistake?.Invoke();
            var oldPosition1 = _firstObj.GetComponent<GrabController>().startPosition;
            _firstObj.transform.DOMove(oldPosition1, .5f).SetEase(Ease.OutBack);
            _firstObj.transform.DORotate(new Vector3(-45, -180, 0), .5f).SetEase(Ease.OutBack);
            if (_digitCount > 1)
            {
                var oldPosition2 = _secondObj.GetComponent<GrabController>().startPosition;
                _secondObj.transform.DORotate(new Vector3(-45, -180, 0), .5f).SetEase(Ease.OutBack).SetDelay(.2f);
                _secondObj.transform.DOMove(oldPosition2, .5f).SetEase(Ease.OutBack).SetDelay(.2f);
            }
            
            _digitCount = 0;
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
                            Win();
                        }
                        else
                        {
                            Lose();
                        }
                    }
                    else
                    {
                        if (_firstDigit < 10) 
                            return;
                        var takenValue = _firstDigit;
                        if (takenValue==LevelManager.Instance.givenValue)
                        {
                            Win();
                        }
                        else
                        {
                            Lose();
                        }
                    }
                    break;
                
                default:
                    break;
            }
            
            
        }
    }
}