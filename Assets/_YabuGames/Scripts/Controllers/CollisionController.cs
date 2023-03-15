using System;
using UnityEngine;

namespace _YabuGames.Scripts.Controllers
{
    public class CollisionController : MonoBehaviour
    {
        public bool onMove;
        
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("OperationBox"))
            {
                if (onMove) 
                    return;
                var holder = other.gameObject.GetComponent<DigitHolder>();
                holder.PlaceTheDigit(gameObject);
                onMove = true;
            }
        }
    }
}
