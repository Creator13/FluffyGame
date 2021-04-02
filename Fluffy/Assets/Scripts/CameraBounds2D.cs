using UnityEngine;

namespace CameraBounding
{
    public class CameraBounds2D : MonoBehaviour
    {
        public Vector2 scaleBound = new Vector2(1, 1);
        public Vector2 offset;

        [HideInInspector] public Vector2 maxXlimit;
        [HideInInspector] public Vector2 maxYlimit;

        Camera _camera;

        private void Start()
        {
            var sprite = GetComponent<SpriteRenderer>();
            if (sprite)
            {
                // scaleBound = sprite.bounds.extents * 2;
            }
        }

        public void Initialize(Camera camera)
        {
            _camera = camera;
            CalculateBounds();
        }

        private void Update()
        {
            CalculateBounds();
        }

        private void CalculateBounds()
        {
            var cameraHalfWidth = _camera.aspect * _camera.orthographicSize;
            maxXlimit = new Vector2((transform.position.x + offset.x - (scaleBound.x / 2)) + cameraHalfWidth,
                (transform.position.x + offset.x + (scaleBound.x / 2)) - cameraHalfWidth);
            maxYlimit = new Vector2((transform.position.y + offset.y - (scaleBound.y / 2)) + _camera.orthographicSize,
                (transform.position.y + offset.y + (scaleBound.y / 2)) - _camera.orthographicSize);
        }

        public void OnDrawGizmos()
        {
            Gizmos.color= Color.red;
            Gizmos.DrawWireCube(transform.position + new Vector3(offset.x, offset.y, 0), scaleBound);
            // Gizmos.DrawWireCube();
        }
    }
}
