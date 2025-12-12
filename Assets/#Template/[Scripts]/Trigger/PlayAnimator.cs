using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DancingLineFanmade.Trigger
{
    [Serializable]
    public class SingleAnimator
    {
        public Animator animator;
        public bool dontRevive;

        private float progress;
        internal bool played;

        private bool playState;

        public void IntiAnimator()
        {
            animator.speed = 0f;
            played = false;
        }

        public void PlayAnimator()
        {
            animator.speed = 1f;
            played = true;
        }

        public void StopAnimator()
        {
            animator.speed = 0f;
        }

        public void SetState()
        {
            animator.Play(animator.GetCurrentAnimatorClipInfo(0)[0].clip.name, 0, progress);
            played = playState;
        }

        public void GetState()
        {
            progress = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            playState = played;
        }
    }

    [DisallowMultipleComponent, RequireComponent(typeof(Collider))]
    public class PlayAnimator : MonoBehaviour
    {
        [SerializeField, TableList] internal List<SingleAnimator> animators = new();

        private void Start()
        {
            foreach (var a in animators)
            {
                a.IntiAnimator();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) 
                return;
            foreach (var a in animators.Where(a => !a.played))
            {
                a.PlayAnimator();
            }
        }
    }
}