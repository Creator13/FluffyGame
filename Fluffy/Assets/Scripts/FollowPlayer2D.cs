using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fluffy
{
    public class FollowPlayer2D : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float speed;
        [SerializeField] private float zOffsetOverride;
        [SerializeField] private Vector2 XYOffset;

        private float ZOffset => Mathf.Abs(zOffsetOverride) <= 0.001 ? transform.position.z : zOffsetOverride;

        private Vector3 TargetPos
        {
            get
            {
                var targetPos = target.position;
                targetPos.z = ZOffset;
                targetPos.x += XYOffset.x;
                targetPos.y += XYOffset.y;
                return targetPos;
            }
        }

        private void Start()
        {
            transform.position = TargetPos;
        }

        private void FixedUpdate()
        {
            var targetPos = TargetPos;

            if (Vector3.Distance(transform.position, targetPos) < 0.05) return;
            
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * speed);
        }
    }
}
