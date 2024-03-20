using System.Collections.Generic;
using UnityEngine;

namespace Views.Gameplay
{
    /**
     * Camera view for the game with smooth target following.
     */
    public class GCameraView : MonoBehaviour
    {
        [SerializeField] private float _avgTime = 0.15f;

        private readonly struct HistoryRecord
        {
            public readonly Vector3 Point;
            public readonly float Time;

            public HistoryRecord(Vector3 point, float time)
            {
                Point = point;
                Time = time;
            }
        }
        
        private readonly Queue<HistoryRecord> _targetPosHistory = new();
        
        private Transform _target;
        private Vector3 _offset = Vector3.back * 5f;
        
        public void SetTarget(Transform target)
        {
            _targetPosHistory.Clear();
            _target = target;
            transform.rotation = Quaternion.LookRotation(-_offset, Vector3.up);
        }
        
        private void LateUpdate()
        {
            if (_target == null)
                return;
            
            var removeOlderThan = Time.time - _avgTime;
            while (_targetPosHistory.Count > 0 && _targetPosHistory.Peek().Time < removeOlderThan)
                _targetPosHistory.Dequeue();
            
            _targetPosHistory.Enqueue(new HistoryRecord(_target.position,Time.time ));

            var avgPos = Vector3.zero;
            foreach (var record in _targetPosHistory)
                avgPos += record.Point;
            avgPos /= _targetPosHistory.Count;
            
            transform.position = avgPos + _offset;
        }
    }
}