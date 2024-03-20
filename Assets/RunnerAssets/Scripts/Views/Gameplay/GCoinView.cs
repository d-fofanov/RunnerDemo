using UnityEngine;

namespace Views.Gameplay
{
    /**
     * Coin view for the gameplay.
     */
    public class GCoinView : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed;
        
        private void Update()
        {
            transform.rotation = Quaternion.Euler(90, Time.time * _rotationSpeed, 0f);
        }
    }
}