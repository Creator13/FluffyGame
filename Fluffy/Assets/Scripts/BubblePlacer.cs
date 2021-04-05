/*
The MIT License (MIT)

Copyright (c) 2015-2021 Secret Lab Pty. Ltd. and Yarn Spinner contributors.

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

----

MIT license applies to parts of this file: WorldToAnchoredPosition()
Modified by Casper van Battum (copyright 2021).

*/

using UnityEngine;
using UnityEngine.UI;

public class BubblePlacer : MonoBehaviour
{
    // this script assumes you are using a full-screen Unity UI canvas along with a full-screen game camera
    private Camera worldCamera;

    public Canvas canvas;
    public CanvasScaler canvasScaler;

    private void Awake()
    {
        worldCamera = Camera.main;
    }

    public void PlaceBubble(RectTransform bubbleTransform, Vector3 worldPos, float bubbleMargin)
    {
        bubbleTransform.anchoredPosition = WorldToAnchoredPosition(bubbleTransform, worldPos, bubbleMargin);
    }

    /// <summary>
    /// Calculates where to put dialogue bubble based on worldPosition and any desired screen margins. Ensure
    /// "constrainToViewportMargin" is between 0.0f-1.0f (% of screen) to constrain to screen, or value of -1 lets
    /// bubble go off-screen.
    /// </summary>
    private Vector2 WorldToAnchoredPosition(RectTransform bubble, Vector3 worldPos,
        float constrainToViewportMargin = -1f)
    {
        var screenPos = Vector2.zero;

        Camera canvasCamera = worldCamera;
        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            // Canvas "Overlay" mode is special case for ScreenPointToLocalPointInRectangle (see the Unity docs)
            canvasCamera = null;
        }

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            bubble.parent
                  .GetComponent<RectTransform>(), // calculate local point inside parent... NOT inside the dialogue bubble itself
            worldCamera.WorldToScreenPoint(worldPos),
            canvasCamera,
            out screenPos
        );

        // to force the dialogue bubble to be fully on screen, clamp the bubble rectangle within the screen bounds
        if (constrainToViewportMargin >= 0f)
        {
            // because ScreenPointToLocalPointInRectangle is relative to a Unity UI RectTransform,
            // it may not necessarily match the full screen resolution (i.e. CanvasScaler)

            // it's not really in world space or screen space, it's in a RectTransform "UI space"
            // so we must manually convert our desired screen bounds to this UI space

            bool useCanvasResolution = canvasScaler != null &&
                                       canvasScaler.uiScaleMode != CanvasScaler.ScaleMode.ConstantPixelSize;
            Vector2 screenSize = Vector2.zero;
            screenSize.x = useCanvasResolution ? canvasScaler.referenceResolution.x : Screen.width;
            screenSize.y = useCanvasResolution ? canvasScaler.referenceResolution.y : Screen.height;

            // calculate "half" values because we are measuring margins based on the center, like a radius
            var halfBubbleWidth = bubble.rect.width / 2;
            var halfBubbleHeight = bubble.rect.height / 2;

            // to calculate margin in UI-space pixels, use a % of the smaller screen dimension
            var margin = screenSize.x < screenSize.y
                ? screenSize.x * constrainToViewportMargin
                : screenSize.y * constrainToViewportMargin;

            // finally, clamp the screenPos fully within the screen bounds, while accounting for the bubble's rectTransform anchors
            screenPos.x = Mathf.Clamp(
                screenPos.x,
                margin + halfBubbleWidth - bubble.anchorMin.x * screenSize.x,
                -(margin + halfBubbleWidth) - bubble.anchorMax.x * screenSize.x + screenSize.x
            );
            screenPos.y = Mathf.Clamp(
                screenPos.y,
                margin + halfBubbleHeight - bubble.anchorMin.y * screenSize.y,
                -(margin + halfBubbleHeight) - bubble.anchorMax.y * screenSize.y + screenSize.y
            );
        }

        return screenPos;
    }
}
