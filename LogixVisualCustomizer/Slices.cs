using BaseX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogixVisualCustomizer
{
    internal static class Slices
    {
        public static float4 GetBottomBorders(float4 verticalSlices, float4 horizontalSlices)
        {
            return new float4(verticalSlices.GetFirstLength(verticalSlices.GetLength()), horizontalSlices.GetFirstLength(horizontalSlices.GetFirstHalfLength()),
                              verticalSlices.GetSecondLength(verticalSlices.GetLength()), 0);
        }

        public static Rect GetBottomRect(float4 verticalSlices, float4 horizontalSlices)
        {
            return new Rect(verticalSlices[0], horizontalSlices[0],
                            verticalSlices[3] - verticalSlices[0], horizontalSlices.GetMiddle() - horizontalSlices[0]);
        }

        public static float GetCenterLength(this float4 slices, float factor = 1)
        {
            return (slices[2] - slices[1]) / factor;
        }

        public static float GetFirstHalfLength(this float4 slices)
        {
            return slices.GetMiddle() - slices[0];
        }

        public static float GetFirstLength(this float4 slices, float factor = 1)
        {
            return (slices[1] - slices[0]) / factor;
        }

        public static float4 GetFullBorders(float4 verticalSlices, float4 horizontalSlices)
        {
            return new float4(verticalSlices.GetFirstLength(verticalSlices.GetLength()), horizontalSlices.GetFirstLength(horizontalSlices.GetLength()),
                              verticalSlices.GetSecondLength(verticalSlices.GetLength()), horizontalSlices.GetSecondLength(horizontalSlices.GetLength()));
        }

        public static Rect GetFullRect(float4 verticalSlices, float4 horizontalSlices)
        {
            return new Rect(verticalSlices[0], horizontalSlices[0],
                            verticalSlices.GetLength(), horizontalSlices.GetLength());
        }

        public static float4 GetHorizontalMiddleBorders(this float4 horizontalSlices)
        {
            return new float4(0, horizontalSlices.GetFirstLength(horizontalSlices.GetLength()),
                              0, horizontalSlices.GetSecondLength(horizontalSlices.GetLength()));
        }

        public static Rect GetHorizontalMiddleRect(float4 verticalSlices, float4 horizontalSlices)
        {
            return new Rect(verticalSlices.GetMiddle(), horizontalSlices[0], 0, horizontalSlices.GetLength());
        }

        public static float4 GetLeftBorders(float4 verticalSlices, float4 horizontalSlices)
        {
            return new float4(verticalSlices.GetFirstLength(verticalSlices.GetFirstHalfLength()), horizontalSlices.GetFirstLength(horizontalSlices.GetLength()),
                              0, horizontalSlices.GetSecondLength(horizontalSlices.GetLength()));
        }

        public static Rect GetLeftRect(float4 verticalSlices, float4 horizontalSlices)
        {
            return new Rect(verticalSlices[0], horizontalSlices[0],
                            verticalSlices.GetMiddle() - verticalSlices[0], horizontalSlices.GetLength());
        }

        public static float GetLength(this float4 slices)
        {
            return slices[3] - slices[0];
        }

        public static float GetMiddle(this float4 slices)
        {
            return (slices[1] + slices[2]) / 2;
        }

        public static float4 GetRightBorders(float4 verticalSlices, float4 horizontalSlices)
        {
            return new float4(0, horizontalSlices.GetFirstLength(horizontalSlices.GetLength()),
                              verticalSlices.GetSecondLength(verticalSlices.GetSecondHalfLength()), horizontalSlices.GetSecondLength(horizontalSlices.GetLength()));
        }

        public static Rect GetRightRect(float4 verticalSlices, float4 horizontalSlices)
        {
            var vMiddle = verticalSlices.GetMiddle();

            return new Rect(vMiddle, horizontalSlices[0], verticalSlices[3] - vMiddle, horizontalSlices.GetLength());
        }

        public static float GetSecondHalfLength(this float4 slices)
        {
            return slices[3] - slices.GetMiddle();
        }

        public static float GetSecondLength(this float4 slices, float factor = 1)
        {
            return (slices[3] - slices[2]) / factor;
        }

        public static float4 GetTopBorders(float4 verticalSlices, float4 horizontalSlices)
        {
            return new float4(verticalSlices.GetFirstLength(verticalSlices.GetLength()), 0,
                              verticalSlices.GetSecondLength(verticalSlices.GetLength()), horizontalSlices.GetSecondLength(horizontalSlices.GetSecondHalfLength()));
        }

        public static Rect GetTopRect(float4 verticalSlices, float4 horizontalSlices)
        {
            var hMiddle = horizontalSlices.GetMiddle();

            return new Rect(verticalSlices[0], hMiddle, verticalSlices.GetLength(), horizontalSlices[3] - hMiddle);
        }

        public static float4 GetVerticalMiddleBorders(this float4 verticalSlices)
        {
            return new float4(verticalSlices.GetFirstLength(verticalSlices.GetLength()), 0,
                              verticalSlices.GetSecondLength(verticalSlices.GetLength()), 0);
        }

        public static Rect GetVerticleMiddleRect(float4 verticleSlices, float4 horizontalSlices)
        {
            return new Rect(verticleSlices[0], horizontalSlices.GetMiddle(), verticleSlices.GetLength(), 0);
        }
    }
}