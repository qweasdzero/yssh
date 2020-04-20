﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SG1
{
    [AddComponentMenu("UI/Effects/Gradient Color")]
    [RequireComponent(typeof(Graphic))]
    public class GradientColor : BaseMeshEffect
    {
        private const int ONE_TEXT_VERTEX = 6;

        [SerializeField] private Color m_colorBottom = Color.white;

        [SerializeField] private Color m_colorLeft = Color.white;

        [SerializeField] private Color m_colorRight = Color.white;

        [SerializeField] private Color m_colorTop = Color.white;

        [SerializeField] [Range(-1f, 1f)] private float m_gradientOffsetHorizontal;

        [SerializeField] [Range(-1f, 1f)] private float m_gradientOffsetVertical;

        [SerializeField] private bool m_splitTextGradient;

        public Color colorTop
        {
            get { return m_colorTop; }
            set
            {
                if (m_colorTop != value)
                {
                    m_colorTop = value;
                    Refresh();
                }
            }
        }

        public Color colorBottom
        {
            get { return m_colorBottom; }
            set
            {
                if (m_colorBottom != value)
                {
                    m_colorBottom = value;
                    Refresh();
                }
            }
        }

        public Color colorLeft
        {
            get { return m_colorLeft; }
            set
            {
                if (m_colorLeft != value)
                {
                    m_colorLeft = value;
                    Refresh();
                }
            }
        }

        public Color colorRight
        {
            get { return m_colorRight; }
            set
            {
                if (m_colorRight != value)
                {
                    m_colorRight = value;
                    Refresh();
                }
            }
        }

        public float gradientOffsetVertical
        {
            get { return m_gradientOffsetVertical; }
            set
            {
                if (m_gradientOffsetVertical != value)
                {
                    m_gradientOffsetVertical = Mathf.Clamp(value, -1f, 1f);
                    Refresh();
                }
            }
        }

        public float gradientOffsetHorizontal
        {
            get { return m_gradientOffsetHorizontal; }
            set
            {
                if (m_gradientOffsetHorizontal != value)
                {
                    m_gradientOffsetHorizontal = Mathf.Clamp(value, -1f, 1f);
                    Refresh();
                }
            }
        }

        public bool splitTextGradient
        {
            get { return m_splitTextGradient; }
            set
            {
                if (m_splitTextGradient != value)
                {
                    m_splitTextGradient = value;
                    Refresh();
                }
            }
        }

        public override void ModifyMesh(VertexHelper vh)
        {
            if (IsActive() == false) return;

            var vList = UIEffectListPool<UIVertex>.Get();

            vh.GetUIVertexStream(vList);

            ModifyVertices(vList);

            vh.Clear();
            vh.AddUIVertexTriangleStream(vList);

            UIEffectListPool<UIVertex>.Release(vList);
        }

        private void ModifyVertices(List<UIVertex> vList)
        {
            if (IsActive() == false || vList == null || vList.Count == 0) return;

            float minX = 0f, minY = 0f, maxX = 0f, maxY = 0f, width = 0f, height = 0;

            UIVertex newVertex;
            for (var i = 0; i < vList.Count; i++)
            {
                if (i == 0 || m_splitTextGradient && i % ONE_TEXT_VERTEX == 0)
                {
                    minX = vList[i].position.x;
                    minY = vList[i].position.y;
                    maxX = vList[i].position.x;
                    maxY = vList[i].position.y;

                    var vertNum = m_splitTextGradient ? i + ONE_TEXT_VERTEX : vList.Count;

                    for (var k = i; k < vertNum; k++)
                    {
                        if (k >= vList.Count) break;
                        var vertex = vList[k];
                        minX = Mathf.Min(minX, vertex.position.x);
                        minY = Mathf.Min(minY, vertex.position.y);
                        maxX = Mathf.Max(maxX, vertex.position.x);
                        maxY = Mathf.Max(maxY, vertex.position.y);
                    }

                    width = maxX - minX;
                    height = maxY - minY;
                }

                newVertex = vList[i];

                Color colorOriginal = newVertex.color;
                var colorVertical = Color.Lerp(m_colorBottom, m_colorTop,
                    (height > 0 ? (newVertex.position.y - minY) / height : 0) + m_gradientOffsetVertical);
                var colorHorizontal = Color.Lerp(m_colorLeft, m_colorRight,
                    (width > 0 ? (newVertex.position.x - minX) / width : 0) + m_gradientOffsetHorizontal);

                newVertex.color = colorOriginal * colorVertical * colorHorizontal;

                vList[i] = newVertex;
            }
        }

        private void Refresh()
        {
            if (graphic != null) graphic.SetVerticesDirty();
        }
    }
}