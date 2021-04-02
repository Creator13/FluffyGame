/*
MIT License

Copyright (c) 2019 Prashant Singh

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 */


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
