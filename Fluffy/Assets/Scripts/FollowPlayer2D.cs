using CameraBounding;
using UnityEngine;

namespace Fluffy
{
    [RequireComponent(typeof(Camera))]
    public class FollowPlayer2D : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float speed;
        [SerializeField] private float zOffsetOverride;
        [SerializeField] private Vector2 XYOffset;

        [SerializeField] private CameraBounds2D bounds;

        //TODO This could potentially be made more efficient by caching this value. It's unlikely to change while playing in editor and impossible to change in prod
        private float ZOffset => Mathf.Abs(zOffsetOverride) <= 0.001 ? transform.position.z : zOffsetOverride;

        private Vector3 TargetPos
        {
            get
            {
                var targetPos = target.position;
                targetPos.z = ZOffset;
                targetPos.x += XYOffset.x;
                targetPos.y += XYOffset.y;

                targetPos.x = Mathf.Clamp(targetPos.x, bounds.maxXlimit.x, bounds.maxXlimit.y);
                targetPos.y = Mathf.Clamp(targetPos.y, bounds.maxYlimit.x, bounds.maxYlimit.y);

                return targetPos;
            }
        }

        private void Awake()
        {
            bounds.Initialize(GetComponent<Camera>());
        }

        private void Start()
        {
            transform.position = TargetPos;
        }

        private void FixedUpdate()
        {
            var targetPos = TargetPos;

            if (Vector3.Distance(transform.position, targetPos) < 0.001) return;

            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * speed);
        }
    }
}
