using System.Collections.Generic;
using UnityEngine;

namespace Views.Gameplay
{
    /**
     * Camera view for the game with smooth target following.
     */
    public class GCameraView : MonoBehaviour
    {
        [SerializeField] private Vector3 _offset = Vector3.back * 5f;
        [SerializeField] private Vector3 _focalOffset = Vector3.right * 2f;
        
        private Transform _target;
        
        public void SetTarget(Transform target)
        {
            _target = target;
            transform.rotation = Quaternion.LookRotation(-_offset, Vector3.up);
        }
        
        private void LateUpdate()
        {
            if (_target == null)
                return;
            
            transform.position = Vector3.Lerp(transform.position, _target.transform.position + _focalOffset + _offset, 0.3f);
        }
    }
}