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
        
        public float AdditionalXOffset { private get; set; }
        public float AdditionalYOffset { private get; set; }

        private Vector3 TargetPos
        {
            get
            {
                var targetPos = target.position;
                targetPos.z = ZOffset;
                targetPos.x += XYOffset.x + AdditionalXOffset;
                targetPos.y += XYOffset.y = AdditionalYOffset;

                targetPos.x = Mathf.Clamp(targetPos.x, Bounds.maxXlimit.x, Bounds.maxXlimit.y);
                targetPos.y = Mathf.Clamp(targetPos.y, Bounds.maxYlimit.x, Bounds.maxYlimit.y);

                return targetPos;
            }
        }

        public CameraBounds2D Bounds
        {
            private get => bounds;
            set
            {
                bounds = value;
                bounds.Initialize();
            }
        }

        private void Awake()
        {
            bounds.Initialize();
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

        public void SetTarget(Transform target)
        {
            this.target = target;
        }
    }
}
