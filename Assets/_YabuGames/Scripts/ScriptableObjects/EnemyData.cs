using UnityEngine;

namespace _YabuGames.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Phase Data", menuName = "YabuGames/Phase Data", order = 0)]
    public class EnemyData : ScriptableObject
    {
        public GameObject[] phases = new GameObject[3];
        public int[] givenValue = new int[3];

    }
}