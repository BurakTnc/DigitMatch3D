using System;
using DG.Tweening;
using UnityEngine;

namespace _YabuGames.Scripts.Controllers
{
    [RequireComponent(typeof(CollisionController))]
    public class GrabController : MonoBehaviour
    {
       [HideInInspector] public Vector3 startPosition;
        
        private Vector3 _offset;
        private Camera _camera;
        private CollisionController _collisionController;

        private void Awake()
        {
            _camera=Camera.main;
            _collisionController = GetComponent<CollisionController>();
            startPosition = transform.position;
        }

        private void Start()
        {
            
        }

        private void OnMouseUp()
        {
            _collisionController.onMove = false;
            transform.DOComplete();
            transform.DORotate(new Vector3(-45, -180, 0),.3f).SetEase(Ease.OutBack);
            transform.DOMove(startPosition, .5f).SetEase(Ease.OutBack);
            _collisionController.onMove = false;
        }

        private void OnMouseDown()
        {
            _collisionController.onMove = true;
            transform.DOComplete();
            transform.DORotate(new Vector3(0, -180, 0),.3f).SetEase(Ease.OutBack);
            //transform.DOMoveY(_startPosition.y + .3f, .2f).SetEase(Ease.OutBack).SetRelative(true);
            _offset = Input.mousePosition - _camera.WorldToScreenPoint(transform.position);
        }

        private void OnMouseDrag()
        {
            var desiredPos=_camera.ScreenToWorldPoint(Input.mousePosition - _offset);
            //desiredPos.y += 1f;
            transform.position = desiredPos;
        }
    }
}
