using System;
using _YabuGames.Scripts.Enums;
using _YabuGames.Scripts.Managers;
using DG.Tweening;
using UnityEngine;

namespace _YabuGames.Scripts.Controllers
{
    public class OperationController : MonoBehaviour
    {
        [HideInInspector] public int firstValue, secondValue;
        
        [SerializeField] private OperationMode operationMode;
        [SerializeField] private Transform extractPosition;
        [SerializeField] private Transform calculatePosition;
        [SerializeField] private AudioClip exitSound, grabSound;
        
        private Animation _animation;
        private Animator _animator;
        private const float XOffset = .4f;


        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _animation = GetComponent<Animation>();
        }
        

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
            _animation.Play();
            var firstDigit = (int)calculatedValue / 10;
            var secondDigit = calculatedValue % 10;
            var parent = new GameObject();
            if (firstDigit==0)
            {
                var obj=Instantiate(Resources.Load<GameObject>($"Spawnables/{secondDigit}"), parent.transform);
                parent.transform.rotation=Quaternion.Euler(-45,-180,0);
                parent.transform.parent = LevelManager.Instance.GetPlatform();
                parent.transform.position = calculatePosition.position;
                parent.transform.DOScale(Vector3.one * 1.2f, .2f).SetLoops(2,LoopType.Yoyo)
                    .OnComplete(() => ExitBox(parent, calculatedValue));
                parent.name = calculatedValue.ToString();
                obj.transform.SetLocalPositionAndRotation(new Vector3(XOffset, 0, 0), Quaternion.identity);
                obj.GetComponent<GrabController>().enabled = false;
                obj.GetComponent<CollisionController>().enabled = false;
                obj.GetComponent<DigitController>().enabled = false;
                var rrb = parent.AddComponent<Rigidbody>();
                rrb.isKinematic = true;
                var ggrabController = parent.AddComponent<GrabController>();
                ggrabController.startPosition = extractPosition.position;
                ggrabController.grabSound = grabSound;
                var ccollisionController = parent.GetComponent<CollisionController>();
                ccollisionController.isMultiplied = true;
                var digitController = parent.AddComponent<DigitController>();
                digitController.digitValue = calculatedValue;
                return;
                
            }
            var firstDigitObj = Instantiate(Resources.Load<GameObject>($"Spawnables/{firstDigit}"), parent.transform);
            var secondDigitObj = Instantiate(Resources.Load<GameObject>($"Spawnables/{secondDigit}"), parent.transform);
            parent.transform.rotation=Quaternion.Euler(-45,-180,0);
            parent.transform.parent = LevelManager.Instance.GetPlatform();
            firstDigitObj.transform.SetLocalPositionAndRotation(new Vector3(XOffset, 0, 0), Quaternion.identity);
            secondDigitObj.transform.SetLocalPositionAndRotation(new Vector3(-XOffset, 0, 0), Quaternion.identity);
            parent.transform.position = calculatePosition.position;
            parent.transform.DOScale(Vector3.one * 1.2f, .2f).SetLoops(2,LoopType.Yoyo)
                .OnComplete(() => ExitBox(parent, calculatedValue));
            parent.name = calculatedValue.ToString();

            firstDigitObj.GetComponent<GrabController>().enabled = false;
            firstDigitObj.GetComponent<CollisionController>().enabled = false;
            firstDigitObj.GetComponent<DigitController>().enabled = false;
            secondDigitObj.GetComponent<GrabController>().enabled = false;
            secondDigitObj.GetComponent<CollisionController>().enabled = false;
            secondDigitObj.GetComponent<DigitController>().enabled = false;

            var rb = parent.AddComponent<Rigidbody>();
            rb.isKinematic = true;

            var grabController = parent.AddComponent<GrabController>();
            grabController.startPosition = extractPosition.position;
            grabController.grabSound = grabSound;
            var collisionController = parent.GetComponent<CollisionController>();
            collisionController.isMultiplied = true;


        }

        private void ExitBox(GameObject obj, int calculatedValue)
        {
            obj.transform.DOMove(extractPosition.position, .4f).SetEase(Ease.OutBack)
                .OnComplete(() => OpenCollider(obj,calculatedValue));
            var grabController = obj.GetComponent<GrabController>();
            grabController.startPosition = extractPosition.position;
            AudioSource.PlayClipAtPoint(exitSound,Camera.main.transform.position);

        }

        private void OpenCollider(GameObject obj,int calculatedValue)
        {
            var boxCollider = obj.AddComponent<BoxCollider>();
            boxCollider.enabled = true;
            boxCollider.center = new Vector3(-.4f, .4f, 0);
            boxCollider.size = new Vector3(1.5f, 1, 1);
            var digitController = obj.AddComponent<DigitController>();
            digitController.digitValue = calculatedValue;
            
        }
        public void StartCalculate()
        {
            Execute(Calculate());
        }
    }
}