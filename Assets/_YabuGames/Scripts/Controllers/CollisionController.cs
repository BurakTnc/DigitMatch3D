using System;
using UnityEngine;

namespace _YabuGames.Scripts.Controllers
{
    public class CollisionController : MonoBehaviour
    {
        public bool onMove;
        public bool isMultiplied;
        
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("OperationBox"))
            {
                if (onMove ) 
                    return;
                if(isMultiplied)
                    return;
                Debug.Log("nono");
                var holder = other.gameObject.GetComponent<DigitHolder>();
                holder.PlaceTheDigit(gameObject);
                onMove = true;
            }
            if (other.gameObject.CompareTag("EqualityBox"))
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
