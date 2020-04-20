using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.UI;

namespace SG1
{
    /// <summary>
    /// 镜像类型
    /// </summary>
    public enum MirrorDir
    {
        Null, //无
        Horizontal, // 水平镜像
        Vertical, // 垂直镜像
        Quater, // 四方镜像
    }

    [AddComponentMenu("UI/ImageCompletion", 111)]
    public class ImageCompletion : Image
    {
        private static readonly Vector2[] s_VertScratch = new Vector2[4];
        private static readonly Vector2[] s_UVScratch = new Vector2[4];
        private List<Vector2[]> Vert = new List<Vector2[]>();
        [SerializeField] private MirrorDir MirrorDir = MirrorDir.Horizontal;

        /// <summary>
        /// 根据不同Image Type选择不同类型
        /// </summary>
        /// <param name="toFill"></param>
        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            
            if (overrideSprite == null)
            {
                base.OnPopulateMesh(toFill);
            }
            else
            {
                switch (type)
                {
                    case Type.Simple:
                        if (!useSpriteMesh)
                        {
                            GenerateSimpleSprite(toFill, preserveAspect);
                            break;
                        }

                        base.OnPopulateMesh(toFill);
                        break;
                    case Type.Sliced:
                        GenerateSlicedSprite(toFill);
                        break;
                    case Type.Tiled:
                    case Type.Filled:
                        base.OnPopulateMesh(toFill);
                        break;
                }
            }
        }

        /// <summary>
        /// Simple类型方法
        /// </summary>
        /// <param name="vh"></param>
        /// <param name="lPreserveAspect"></param>
        private void GenerateSimpleSprite(VertexHelper vh, bool lPreserveAspect)
        {
            Vector4 drawingDimensions = GetDrawingDimensions(lPreserveAspect);
            Vector4 vector4 = !(overrideSprite != null)
                ? Vector4.zero
                : DataUtility.GetOuterUV(overrideSprite);
            Vector2 origin = new Vector2((drawingDimensions.x + drawingDimensions.z) / 2,
                (drawingDimensions.y + drawingDimensions.w) / 2);
            Vector2[] uv =
            {
                new Vector2(vector4.x, vector4.y), new Vector2(vector4.x, vector4.w),
                new Vector2(vector4.z, vector4.y), new Vector2(vector4.z, vector4.w)
            };
            vh.Clear();
            switch (MirrorDir)
            {
                case MirrorDir.Null:
                    NormalSimple(vh, drawingDimensions, uv);
                    break;
                case MirrorDir.Quater:
                    QuaterSimple(vh, drawingDimensions, uv, origin);
                    break;
                case MirrorDir.Horizontal:
                    HorizontalSimple(vh, drawingDimensions, uv, origin);
                    break;
                case MirrorDir.Vertical:
                    VerticalSimple(vh, drawingDimensions, uv, origin);
                    break;
            }
        }

        #region Simple

        /// <summary>
        /// Simple类型普通显示
        /// </summary>
        /// <param name="vh"></param>
        /// <param name="drawingDimensions"></param>
        /// <param name="uv"></param>
        private void NormalSimple(VertexHelper vh, Vector4 drawingDimensions, Vector2[] uv)
        {
            vh.AddVert(new Vector3(drawingDimensions.x, drawingDimensions.y), color, uv[0]);
            vh.AddVert(new Vector3(drawingDimensions.x, drawingDimensions.w), color, uv[1]);
            vh.AddVert(new Vector3(drawingDimensions.z, drawingDimensions.w), color, uv[3]);
            vh.AddVert(new Vector3(drawingDimensions.z, drawingDimensions.y), color, uv[2]);
            vh.AddTriangle(0, 1, 2);
            vh.AddTriangle(0, 2, 3);
        }

        /// <summary>
        /// Simple对角镜像显示
        /// </summary>
        /// <param name="vh"></param>
        /// <param name="drawingDimensions"></param>
        /// <param name="uv"></param>
        /// <param name="origin"></param>
        private void QuaterSimple(VertexHelper vh, Vector4 drawingDimensions, Vector2[] uv, Vector2 origin)
        {
            vh.AddVert(new Vector3(origin.x, origin.y), color, uv[3]);
            vh.AddVert(new Vector3(drawingDimensions.x, origin.y), color, uv[1]);
            vh.AddVert(new Vector3(drawingDimensions.x, drawingDimensions.w), color, uv[0]);
            vh.AddVert(new Vector3(origin.x, drawingDimensions.w), color, uv[2]);
            vh.AddVert(new Vector3(drawingDimensions.z, drawingDimensions.w), color, uv[0]);
            vh.AddVert(new Vector3(drawingDimensions.z, origin.y), color, uv[1]);
            vh.AddVert(new Vector3(drawingDimensions.z, drawingDimensions.y), color, uv[0]);
            vh.AddVert(new Vector3(origin.x, drawingDimensions.y), color, uv[2]);
            vh.AddVert(new Vector3(drawingDimensions.x, drawingDimensions.y), color, uv[0]);
            for (int i = 0; i < vh.currentVertCount - 2; i++)
            {
                vh.AddTriangle(0, i + 1, i + 2);
            }

            vh.AddTriangle(0, vh.currentVertCount - 1, 1);
        }

        /// <summary>
        /// Simple水平镜像显示
        /// </summary>
        /// <param name="vh"></param>
        /// <param name="drawingDimensions"></param>
        /// <param name="uv"></param>
        /// <param name="origin"></param>
        private void HorizontalSimple(VertexHelper vh, Vector4 drawingDimensions, Vector2[] uv, Vector2 origin)
        {
            vh.AddVert(new Vector3(origin.x, drawingDimensions.w), color, uv[3]);
            vh.AddVert(new Vector3(origin.x, drawingDimensions.y), color, uv[2]);
            vh.AddVert(new Vector3(drawingDimensions.x, drawingDimensions.y), color, uv[0]);
            vh.AddVert(new Vector3(drawingDimensions.x, drawingDimensions.w), color, uv[1]);
            vh.AddVert(new Vector3(drawingDimensions.z, drawingDimensions.w), color, uv[1]);
            vh.AddVert(new Vector3(drawingDimensions.z, drawingDimensions.y), color, uv[0]);
            vh.AddTriangle(0, 2, 3);
            vh.AddTriangle(0, 1, 2);
            vh.AddTriangle(0, 4, 5);
            vh.AddTriangle(0, 5, 1);
        }

        /// <summary>
        /// Simple垂直镜像显示
        /// </summary>
        /// <param name="vh"></param>
        /// <param name="drawingDimensions"></param>
        /// <param name="uv"></param>
        /// <param name="origin"></param>
        private void VerticalSimple(VertexHelper vh, Vector4 drawingDimensions, Vector2[] uv, Vector2 origin)
        {
            vh.AddVert(new Vector3(drawingDimensions.x, origin.y), color, uv[1]);
            vh.AddVert(new Vector3(drawingDimensions.z, origin.y), color, uv[3]);
            vh.AddVert(new Vector3(drawingDimensions.z, drawingDimensions.w), color, uv[2]);
            vh.AddVert(new Vector3(drawingDimensions.x, drawingDimensions.w), color, uv[0]);
            vh.AddVert(new Vector3(drawingDimensions.x, drawingDimensions.y), color, uv[0]);
            vh.AddVert(new Vector3(drawingDimensions.z, drawingDimensions.y), color, uv[2]);
            vh.AddTriangle(0, 2, 3);
            vh.AddTriangle(0, 1, 2);
            vh.AddTriangle(0, 4, 5);
            vh.AddTriangle(0, 5, 1);
        }

        #endregion

        /// <summary>
        /// 获取绘图尺寸
        /// </summary>
        /// <param name="shouldPreserveAspect"></param>
        /// <returns></returns>
        private Vector4 GetDrawingDimensions(bool shouldPreserveAspect)
        {
            Vector4 vector4_1 = !(overrideSprite == null)
                ? DataUtility.GetPadding(overrideSprite)
                : Vector4.zero;
            Vector2 spriteSize = !(overrideSprite == null)
                ? new Vector2(overrideSprite.rect.width, overrideSprite.rect.height)
                : Vector2.zero;
            Rect pixelAdjustedRect = this.GetPixelAdjustedRect();
            int num1 = Mathf.RoundToInt(spriteSize.x);
            int num2 = Mathf.RoundToInt(spriteSize.y);
            Vector4 vector4_2 = new Vector4(vector4_1.x / num1, vector4_1.y / num2,
                (num1 - vector4_1.z) / num1, (num2 - vector4_1.w) / num2);
            if (shouldPreserveAspect && (double) spriteSize.sqrMagnitude > 0.0)
                this.PreserveSpriteAspectRatio(ref pixelAdjustedRect, spriteSize);
            vector4_2 = new Vector4(pixelAdjustedRect.x + pixelAdjustedRect.width * vector4_2.x,
                pixelAdjustedRect.y + pixelAdjustedRect.height * vector4_2.y,
                pixelAdjustedRect.x + pixelAdjustedRect.width * vector4_2.z,
                pixelAdjustedRect.y + pixelAdjustedRect.height * vector4_2.w);
            return vector4_2;
        }

        /// <summary>
        /// 保持图片宽高比
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="spriteSize"></param>
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

        /// <summary>
        /// Sliced类型方法
        /// </summary>
        /// <param name="toFill"></param>
        private void GenerateSlicedSprite(VertexHelper toFill)
        {
            if (!hasBorder)
            {
                GenerateSimpleSprite(toFill, false);
            }
            else
            {
                Vector4 outerUv;
                Vector4 innerUv;
                Vector4 Padding;
                if (overrideSprite != null)
                {
                    outerUv = DataUtility.GetOuterUV(overrideSprite);
                    innerUv = DataUtility.GetInnerUV(overrideSprite);
                    Padding = overrideSprite.border;
                }
                else
                {
                    outerUv = Vector4.zero;
                    innerUv = Vector4.zero;
                    Padding = Vector4.zero;
                }

                s_UVScratch[0] = new Vector2(outerUv.x, outerUv.y);
                s_UVScratch[1] = new Vector2(innerUv.x, innerUv.y);
                s_UVScratch[2] = new Vector2(innerUv.z, innerUv.w);
                s_UVScratch[3] = new Vector2(outerUv.z, outerUv.w);
                toFill.Clear();
                Vert.Clear();
                Rect pixelAdjustedRect = GetPixelAdjustedRect(); //以左下顶点为原点绘制图形
                s_VertScratch[0] = pixelAdjustedRect.position;
                switch (MirrorDir)
                {
                    case MirrorDir.Null:
                        NormalSliced(toFill, Padding,pixelAdjustedRect);
                        break;
                    case MirrorDir.Quater:
                        QuaterSliced(toFill, Padding,pixelAdjustedRect);
                        break;
                    case MirrorDir.Horizontal:
                        HorizontalSliced(toFill, Padding,pixelAdjustedRect);
                        break;
                    case MirrorDir.Vertical:
                        VerticalSliced(toFill, Padding,pixelAdjustedRect);
                        break;
                }
            }
        }

        /// <summary>
        /// 调整边框
        /// </summary>
        /// <param name="border"></param>
        /// <param name="adjustedRect"></param>
        /// <returns></returns>
        private Vector4 GetAdjustedBorders(Vector4 border, Rect adjustedRect)
        {
            for (int index = 0; index <= 1; ++index)
            {
                float num1 = border[index] + border[index + 2];
                if (adjustedRect.size[index] < (double) num1 && num1 != 0.0)
                {
                    float num2 = adjustedRect.size[index] / num1;
                    border[index] *= num2;
                    border[index + 2] *= num2;
                }
            }

            return border;
        }

        /// <summary>
        /// 计算顶点位置
        /// </summary>
        /// <param name="Padding"></param>
        /// <param name="pixelAdjustedRect"></param>
        private void SetVertScratch(Vector4 Padding,Rect pixelAdjustedRect)
        {
            Vector4 adjustedBorders = GetAdjustedBorders(Padding / pixelsPerUnit, pixelAdjustedRect); //九宫格内顶点位置
            s_VertScratch[3] = s_VertScratch[0] + pixelAdjustedRect.size;
            s_VertScratch[1] = s_VertScratch[0] + new Vector2(adjustedBorders.x, adjustedBorders.y);
            s_VertScratch[2] = s_VertScratch[3] - new Vector2(adjustedBorders.z, adjustedBorders.w);
        }
        #region Sliced

        /// <summary>
        /// Sliced类型普通显示
        /// </summary>
        /// <param name="toFill"></param>
        /// <param name="pixelAdjustedRect"></param>
        private void NormalSliced(VertexHelper toFill, Vector4 Padding,Rect pixelAdjustedRect)
        {
            SetVertScratch(Padding, pixelAdjustedRect);
            for (int index1 = 0; index1 < 3; ++index1)
            {
                int index2 = index1 + 1;
                for (int index3 = 0; index3 < 3; ++index3)
                {
                    if (fillCenter || index1 != 1 || index3 != 1)
                    {
                        int index4 = index3 + 1;
                        AddQuad(toFill, new Vector2(s_VertScratch[index1].x, s_VertScratch[index3].y),
                            new Vector2(s_VertScratch[index2].x, s_VertScratch[index4].y), color,
                            new Vector2(s_UVScratch[index1].x, s_UVScratch[index3].y),
                            new Vector2(s_UVScratch[index2].x, s_UVScratch[index4].y));
                    }
                }
            }
        }

        /// <summary>
        /// Sliced对角镜像显示
        /// </summary>
        /// <param name="toFill"></param>
        /// <param name="pixelAdjustedRect"></param>
        private void QuaterSliced(VertexHelper toFill, Vector4 Padding,Rect pixelAdjustedRect)
        {
            pixelAdjustedRect = new Rect(
                pixelAdjustedRect.position * 0.5f,
                pixelAdjustedRect.size * 0.5f);
            SetVertScratch(Padding, pixelAdjustedRect);
            Vector2 uvMin;
            Vector2 uvMax;
            for (int index1 = 0; index1 < 3; ++index1)
            {
                int index2 = index1 + 1;
                for (int index3 = 0; index3 < 3; ++index3)
                {
                    if (fillCenter || index1 != 1 || index3 != 1)
                    {
                        int index4 = index3 + 1;
                        uvMin = new Vector2(s_UVScratch[index1].x, s_UVScratch[index3].y);
                        uvMax = new Vector2(s_UVScratch[index2].x, s_UVScratch[index4].y);
                        AddQuad(toFill, new Vector2(s_VertScratch[index1].x, s_VertScratch[index3].y),
                            new Vector2(s_VertScratch[index2].x, s_VertScratch[index4].y), color, uvMin, uvMax);
                        AddQuad(toFill,
                            new Vector2(2 * s_VertScratch[3].x - s_VertScratch[index1].x, s_VertScratch[index3].y),
                            new Vector2(2 * s_VertScratch[3].x - s_VertScratch[index2].x, s_VertScratch[index4].y),
                            color, uvMin, uvMax);
                        AddQuad(toFill,
                            new Vector2(s_VertScratch[index1].x, 2 * s_VertScratch[3].y - s_VertScratch[index3].y),
                            new Vector2(s_VertScratch[index2].x, 2 * s_VertScratch[3].y - s_VertScratch[index4].y),
                            color, uvMin, uvMax);
                        AddQuad(toFill,
                            new Vector2(2 * s_VertScratch[3].x - s_VertScratch[index1].x,
                                2 * s_VertScratch[3].y - s_VertScratch[index3].y),
                            new Vector2(2 * s_VertScratch[3].x - s_VertScratch[index2].x,
                                2 * s_VertScratch[3].y - s_VertScratch[index4].y), color, uvMin, uvMax);
                    }
                }
            }
        }

        /// <summary>
        /// Sliced垂直镜像显示
        /// </summary>
        /// <param name="toFill"></param>
        /// <param name="pixelAdjustedRect"></param>
        private void VerticalSliced(VertexHelper toFill, Vector4 Padding,Rect pixelAdjustedRect)
        {
            pixelAdjustedRect = new Rect(pixelAdjustedRect.x,pixelAdjustedRect.y * 0.5f, pixelAdjustedRect.width,pixelAdjustedRect.height * 0.5f);
            SetVertScratch(Padding, pixelAdjustedRect);
            Vector2 uvMin;
            Vector2 uvMax;
            for (int index1 = 0; index1 < 3; ++index1)
            {
                int index2 = index1 + 1;
                for (int index3 = 0; index3 < 3; ++index3)
                {
                    if (fillCenter || index1 != 1 || index3 != 1)
                    {
                        int index4 = index3 + 1;
                        uvMin = new Vector2(s_UVScratch[index1].x, s_UVScratch[index3].y);
                        uvMax = new Vector2(s_UVScratch[index2].x, s_UVScratch[index4].y);
                        AddQuad(toFill,
                            new Vector2(s_VertScratch[index1].x, s_VertScratch[index3].y),
                            new Vector2(s_VertScratch[index2].x, s_VertScratch[index4].y), color, uvMin, uvMax);
                        AddQuad(toFill,
                            new Vector2(s_VertScratch[index1].x, 2 * s_VertScratch[3].y - s_VertScratch[index3].y),
                            new Vector2(s_VertScratch[index2].x, 2 * s_VertScratch[3].y - s_VertScratch[index4].y),
                            color, uvMin, uvMax);
                    }
                }
            }
        }

        /// <summary>
        /// Sliced水平镜像显示
        /// </summary>
        /// <param name="toFill"></param>
        /// <param name="pixelAdjustedRect"></param>
        private void HorizontalSliced(VertexHelper toFill, Vector4 Padding,Rect pixelAdjustedRect)
        {
            pixelAdjustedRect =new Rect(pixelAdjustedRect.x * 0.5f,
                pixelAdjustedRect.y, pixelAdjustedRect.width * 0.5f,
                pixelAdjustedRect.height);
            SetVertScratch(Padding, pixelAdjustedRect);
            Vector2 uvMin;
            Vector2 uvMax;
            for (int index1 = 0; index1 < 3; ++index1)
            {
                int index2 = index1 + 1;
                for (int index3 = 0; index3 < 3; ++index3)
                {
                    if (fillCenter || index1 != 1 || index3 != 1)
                    {
                        int index4 = index3 + 1;
                        uvMin = new Vector2(s_UVScratch[index1].x, s_UVScratch[index3].y);
                        uvMax = new Vector2(s_UVScratch[index2].x, s_UVScratch[index4].y);
                        AddQuad(toFill,
                            new Vector2(s_VertScratch[index1].x, s_VertScratch[index3].y),
                            new Vector2(s_VertScratch[index2].x, s_VertScratch[index4].y), color, uvMin, uvMax);
                        AddQuad(toFill,
                            new Vector2(2 * s_VertScratch[3].x - s_VertScratch[index1].x, s_VertScratch[index3].y),
                            new Vector2(2 * s_VertScratch[3].x - s_VertScratch[index2].x, s_VertScratch[index4].y),
                            color, uvMin, uvMax);
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// 画出一个面片
        /// </summary>
        /// <param name="vertexHelper"></param>
        /// <param name="posMin"></param>
        /// <param name="posMax"></param>
        /// <param name="color"></param>
        /// <param name="uvMin"></param>
        /// <param name="uvMax"></param>
        private void AddQuad(
            VertexHelper vertexHelper,
            Vector2 posMin,
            Vector2 posMax,
            Color32 color,
            Vector2 uvMin,
            Vector2 uvMax)
        {
            int i0 = AddVert(vertexHelper, new Vector3(posMin.x, posMin.y, 0.0f), color, new Vector2(uvMin.x, uvMin.y));
            int i1 = AddVert(vertexHelper, new Vector3(posMin.x, posMax.y, 0.0f), color, new Vector2(uvMin.x, uvMax.y));
            int i2 = AddVert(vertexHelper, new Vector3(posMax.x, posMax.y, 0.0f), color, new Vector2(uvMax.x, uvMax.y));
            int i3 = AddVert(vertexHelper, new Vector3(posMax.x, posMin.y, 0.0f), color, new Vector2(uvMax.x, uvMin.y));
            vertexHelper.AddTriangle(i0, i1, i2);
            vertexHelper.AddTriangle(i2, i3, i0);
        }

        /// <summary>
        /// 同顶点重复使用
        /// </summary>
        /// <param name="vertexHelper"></param>
        /// <param name="vector2"></param>
        /// <param name="color"></param>
        /// <param name="uv"></param>
        /// <returns></returns>
        private int AddVert(VertexHelper vertexHelper, Vector2 vector2, Color color, Vector2 uv)
        {
            for (int i = 0; i < Vert.Count; i++)
            {
                if (Vert[i][0] == vector2 && Vert[i][1] == uv)
                {
                    return i;
                }
            }
            vertexHelper.AddVert(vector2, color, uv);
            Vert.Add(new[] {vector2, uv});
            return vertexHelper.currentVertCount - 1;
        }
    }
}