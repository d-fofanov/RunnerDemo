using System;
using System.Collections;
using System.Collections.Generic;
using RX;
using UnityEngine;

namespace Utils
{
    /**
     * Time host to provide Unity time services to non-MonoBehaviour classes.
     */
    public class TimeUtil : MonoBehaviour
    {
        public static TimeUtil Create()
        {
            var go = new GameObject("TimeUtil");
            DontDestroyOnLoad(go);
            return go.AddComponent<TimeUtil>();
        }
        
        private readonly Subject<float> _updateActions = new();
        private readonly Subject<float> _fixedUpdateAction = new();
        private readonly HashSet<Action> _runNextFrame = new();
        
        public Subscription AddUpdateAction(Action<float> action)
        {
            return _updateActions.Subscribe(action);
        }
        
        public Subscription AddFixedUpdateAction(Action<float> action)
        {
            return _fixedUpdateAction.Subscribe(action);
        }

        public void RunDelayed(float delay, Action action)
        {
            StartCoroutine(DelayCoroutine(delay, action));
        }

        public void RunNextFrame(Action action)
        {
            _runNextFrame.Add(action);
        }

        private IEnumerator DelayCoroutine(float delay, Action action)
        {
            yield return new WaitForSeconds(delay);
            action?.Invoke();
        }
        
        private void Update()
        {
            foreach (var action in _runNextFrame)
            {
                try
                {
                    action?.Invoke();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
            _runNextFrame.Clear();
            
            _updateActions.OnNext(Time.deltaTime);
        }
        
        private void FixedUpdate()
        {
            _fixedUpdateAction.OnNext(Time.fixedDeltaTime);
        }
        
        private void OnDestroy()
        {
            _runNextFrame.Clear();
        }
    }
}