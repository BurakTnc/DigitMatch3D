using System;
using DG.Tweening;
using UnityEngine;

namespace _YabuGames.Scripts.Controllers
{
    public class GrabController : MonoBehaviour
    {
        private Vector3 _offset, _startPosition;
        private Camera _camera;

        private void Awake()
        {
            _camera=Camera.main;
        }

        private void Start()
        {
            _startPosition = transform.position;
        }

        private void OnMouseUp()
        {
            transform.DOComplete();
            transform.DOMove(_startPosition, .5f).SetEase(Ease.OutBack);
        }

        private void OnMouseDown()
        {
            transform.DOComplete();
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
