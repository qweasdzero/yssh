using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;

[AddComponentMenu("UI/SelfImage", 111)]
public class SelfImage : Image
{
    private static readonly Vector2[] s_VertScratch = new Vector2[4];
    private static readonly Vector2[] s_UVScratch = new Vector2[4];
    [SerializeField] private ImageType m_ImageType = ImageType.Normal;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        if (overrideSprite == null || m_ImageType == ImageType.Normal)
        {
            base.OnPopulateMesh(vh);
        }
        else
        {
            switch (type)
            {
                case Type.Simple:
                    if (!useSpriteMesh)
                    {
                        GenerateSimpleSprite(vh, preserveAspect);
                        break;
                    }

                    base.OnPopulateMesh(vh);
                    break;
                case Type.Sliced:
                    GenerateSlicedSprite(vh);
                    break;
                case Type.Tiled:
                case Type.Filled:
                    base.OnPopulateMesh(vh);
                    break;
            }
        }
    }

    private void GenerateSimpleSprite(VertexHelper vh, bool lPreserveAspect)
    {
        Vector4 drawingDimensions = this.GetDrawingDimensions(lPreserveAspect);
        Vector4 vector4 = !((UnityEngine.Object) overrideSprite != (UnityEngine.Object) null)
            ? Vector4.zero
            : DataUtility.GetOuterUV(overrideSprite);
        drawingDimensions = new Vector4(worldcorners[0].x, worldcorners[0].y, worldcorners[1].x, worldcorners[1].y);
        Color color = this.color;
        vh.Clear();
        vh.AddVert(new Vector3(drawingDimensions.x, drawingDimensions.y), (Color32) color,
            new Vector2(vector4.x, vector4.y));
        vh.AddVert(new Vector3(drawingDimensions.x, drawingDimensions.w), (Color32) color,
            new Vector2(vector4.x, vector4.w));
        vh.AddVert(new Vector3(drawingDimensions.z, drawingDimensions.w), (Color32) color,
            new Vector2(vector4.z, vector4.w));
        vh.AddVert(new Vector3(drawingDimensions.z, drawingDimensions.y), (Color32) color,
            new Vector2(vector4.z, vector4.y));
        vh.AddTriangle(0, 1, 2);
        vh.AddTriangle(2, 3, 0);
    }

    private void GenerateSlicedSprite(VertexHelper toFill)
    {
        if (!this.hasBorder)
        {
            this.GenerateSimpleSprite(toFill, false);
        }
        else
        {
            Vector4 vector4_1;
            Vector4 vector4_2;
            Vector4 vector4_3;
            Vector4 vector4_4;
            if ((UnityEngine.Object) this.overrideSprite != (UnityEngine.Object) null)
            {
                vector4_1 = DataUtility.GetOuterUV(this.overrideSprite);
                vector4_2 = DataUtility.GetInnerUV(this.overrideSprite);
                vector4_3 = DataUtility.GetPadding(this.overrideSprite);
                vector4_4 = this.overrideSprite.border;
            }
            else
            {
                vector4_1 = Vector4.zero;
                vector4_2 = Vector4.zero;
                vector4_3 = Vector4.zero;
                vector4_4 = Vector4.zero;
            }

            Rect pixelAdjustedRect =
                new Rect(worldcorners[0].x, worldcorners[0].y, m_WorldCorners[0], m_WorldCorners[1]);
            Vector4 adjustedBorders = this.GetAdjustedBorders(vector4_4 / this.pixelsPerUnit, pixelAdjustedRect);
            Vector4 vector4_5 = vector4_3 / this.pixelsPerUnit;

            s_VertScratch[0] = new Vector2(vector4_5.x, vector4_5.y);
            s_VertScratch[3] =
                new Vector2(pixelAdjustedRect.width - vector4_5.z, pixelAdjustedRect.height - vector4_5.w);
            s_VertScratch[1].x = adjustedBorders.x;
            s_VertScratch[1].y = adjustedBorders.y;
            s_VertScratch[2].x = pixelAdjustedRect.width - adjustedBorders.z;
            s_VertScratch[2].y = pixelAdjustedRect.height - adjustedBorders.w;
            for (int index = 0; index < 4; ++index)
            {
                s_VertScratch[index].x += pixelAdjustedRect.x;
                s_VertScratch[index].y += pixelAdjustedRect.y;
            }

            s_UVScratch[0] = new Vector2(vector4_1.x, vector4_1.y);
            s_UVScratch[1] = new Vector2(vector4_2.x, vector4_2.y);
            s_UVScratch[2] = new Vector2(vector4_2.z, vector4_2.w);
            s_UVScratch[3] = new Vector2(vector4_1.z, vector4_1.w);
            toFill.Clear();
            for (int index1 = 0; index1 < 3; ++index1)
            {
                int index2 = index1 + 1;
                for (int index3 = 0; index3 < 3; ++index3)
                {
                    if (fillCenter || index1 != 1 || index3 != 1)
                    {
                        int index4 = index3 + 1;
                        AddQuad(toFill, new Vector2(s_VertScratch[index1].x, s_VertScratch[index3].y),
                            new Vector2(s_VertScratch[index2].x, s_VertScratch[index4].y), (Color32) this.color,
                            new Vector2(s_UVScratch[index1].x, s_UVScratch[index3].y),
                            new Vector2(s_UVScratch[index2].x, s_UVScratch[index4].y));
                    }
                }
            }
        }
    }

    private Vector4 GetDrawingDimensions(bool shouldPreserveAspect)
    {
        Vector4 vector4_1 = !((UnityEngine.Object) this.overrideSprite == (UnityEngine.Object) null)
            ? DataUtility.GetPadding(this.overrideSprite)
            : Vector4.zero;
        Vector2 spriteSize = !((UnityEngine.Object) this.overrideSprite == (UnityEngine.Object) null)
            ? new Vector2(this.overrideSprite.rect.width, this.overrideSprite.rect.height)
            : Vector2.zero;
        Rect pixelAdjustedRect = this.GetPixelAdjustedRect();
        int num1 = Mathf.RoundToInt(spriteSize.x);
        int num2 = Mathf.RoundToInt(spriteSize.y);
        Vector4 vector4_2 = new Vector4(vector4_1.x / (float) num1, vector4_1.y / (float) num2,
            ((float) num1 - vector4_1.z) / (float) num1, ((float) num2 - vector4_1.w) / (float) num2);
        if (shouldPreserveAspect && (double) spriteSize.sqrMagnitude > 0.0)
            this.PreserveSpriteAspectRatio(ref pixelAdjustedRect, spriteSize);
        vector4_2 = new Vector4(pixelAdjustedRect.x + pixelAdjustedRect.width * vector4_2.x,
            pixelAdjustedRect.y + pixelAdjustedRect.height * vector4_2.y,
            pixelAdjustedRect.x + pixelAdjustedRect.width * vector4_2.z,
            pixelAdjustedRect.y + pixelAdjustedRect.height * vector4_2.w);
        return vector4_2;
    }

    private void PreserveSpriteAspectRatio(ref Rect rect, Vector2 spriteSize)
    {
        float num1 = spriteSize.x / spriteSize.y;
        float num2 = rect.width / rect.height;
        if ((double) num1 > (double) num2)
        {
            float height = rect.height;
            rect.height = rect.width * (1f / num1);
            rect.y += (height - rect.height) * this.rectTransform.pivot.y;
        }
        else
        {
            float width = rect.width;
            rect.width = rect.height * num1;
            rect.x += (width - rect.width) * this.rectTransform.pivot.x;
        }
    }

    private Vector4 GetAdjustedBorders(Vector4 border, Rect adjustedRect)
    {
        Rect rect = this.rectTransform.rect;
        for (int index = 0; index <= 1; ++index)
        {
            float num1 = border[index] + border[index + 2];
            if ((double) adjustedRect.size[index] < (double) num1 && (double) num1 != 0.0)
            {
                float num2 = adjustedRect.size[index] / num1;
                border[index] *= num2;
                border[index + 2] *= num2;
            }
        }

        return border;
    }

    private static void AddQuad(
        VertexHelper vertexHelper,
        Vector3[] quadPositions,
        Color32 color,
        Vector3[] quadUVs)
    {
        int currentVertCount = vertexHelper.currentVertCount;
        for (int index = 0; index < 4; ++index)
            vertexHelper.AddVert(quadPositions[index], color, (Vector2) quadUVs[index]);
        vertexHelper.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
        vertexHelper.AddTriangle(currentVertCount + 2, currentVertCount + 3, currentVertCount);
    }

    private static void AddQuad(
        VertexHelper vertexHelper,
        Vector2 posMin,
        Vector2 posMax,
        Color32 color,
        Vector2 uvMin,
        Vector2 uvMax)
    {
        int currentVertCount = vertexHelper.currentVertCount;
        vertexHelper.AddVert(new Vector3(posMin.x, posMin.y, 0.0f), color, new Vector2(uvMin.x, uvMin.y));
        vertexHelper.AddVert(new Vector3(posMin.x, posMax.y, 0.0f), color, new Vector2(uvMin.x, uvMax.y));
        vertexHelper.AddVert(new Vector3(posMax.x, posMax.y, 0.0f), color, new Vector2(uvMax.x, uvMax.y));
        vertexHelper.AddVert(new Vector3(posMax.x, posMin.y, 0.0f), color, new Vector2(uvMax.x, uvMin.y));
        vertexHelper.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
        vertexHelper.AddTriangle(currentVertCount + 2, currentVertCount + 3, currentVertCount);
    }

    [SerializeField] private float[] m_WorldCorners = new float[2];
    [SerializeField] private float m_SliderX = 1;
    [SerializeField] private float m_SliderY = 1;

    private Vector2[] worldcorners
    {
        get
        {
            Vector2[] world = new Vector2[2];
            Vector2 pos = rectTransform.rect.center;

            if (m_ImageType == ImageType.Zoom)
            {
                world[0] = pos + new Vector2(-m_WorldCorners[0], -m_WorldCorners[1]) / 2;
                world[1] = pos + new Vector2(m_WorldCorners[0], m_WorldCorners[1]) / 2;
            }

            if (m_ImageType == ImageType.ZoomSlider)
            {
                var rect = rectTransform.rect;
                world[0] = pos + new Vector2(-rect.width * m_SliderX, -rect.height * m_SliderY) / 2;
                world[1] = pos + new Vector2(rect.width * m_SliderX, rect.height * m_SliderY) / 2;
            }


            return world;
        }
    }

    public enum ImageType
    {
        Normal,
        Zoom, //可以调整图片大小不影响点击区域，改成滑轮是否更好？
        ZoomSlider,
    }
}