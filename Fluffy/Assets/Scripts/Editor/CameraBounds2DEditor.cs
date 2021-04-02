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
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace CameraBounding
{
    [CustomEditor(typeof(CameraBounds2D))]
    public class CameraBounds2DEditor : Editor
    {
        CameraBounds2D _bounds;
        readonly BoxBoundsHandle m_BoundsHandle = new BoxBoundsHandle();

        public void OnSceneGUI()
        {
            _bounds = (CameraBounds2D) target;
            Matrix4x4 handleMatrix = _bounds.transform.localToWorldMatrix;
            handleMatrix.SetRow(0, Vector4.Scale(handleMatrix.GetRow(0), new Vector4(1f, 1f, 0f, 1f)));
            handleMatrix.SetRow(1, Vector4.Scale(handleMatrix.GetRow(1), new Vector4(1f, 1f, 0f, 1f)));
            handleMatrix.SetRow(2, new Vector4(0f, 0f, 1f, _bounds.transform.position.z));

            using (new Handles.DrawingScope(handleMatrix))
            {
                m_BoundsHandle.center = _bounds.offset;
                m_BoundsHandle.size = _bounds.scaleBound;
                m_BoundsHandle.SetColor(Color.gray);
                EditorGUI.BeginChangeCheck();
                m_BoundsHandle.DrawHandle();
                Rect rect = new Rect(m_BoundsHandle.center.x - (m_BoundsHandle.size.x / 2),
                    m_BoundsHandle.center.y - (m_BoundsHandle.size.y / 2), m_BoundsHandle.size.x,
                    m_BoundsHandle.size.y);
                Handles.DrawSolidRectangleWithOutline(rect, new Color(1, 1, 1, 0.1f), Color.yellow);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_bounds,
                        string.Format("Modify {0}", ObjectNames.NicifyVariableName(_bounds.GetType().Name)));

                    // test for size change after using property setter in case input data was sanitized
                    Vector2 oldSize = _bounds.scaleBound;
                    _bounds.scaleBound = m_BoundsHandle.size;

                    // because projection of offset is a lossy operation, only do it if the size has actually changed
                    // this check prevents drifting while dragging handle when size is zero (case 863949)
                    if (_bounds.scaleBound != oldSize)
                        _bounds.offset = m_BoundsHandle.center;
                }
            }
        }
    }
}
