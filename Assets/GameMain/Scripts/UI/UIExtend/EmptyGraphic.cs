﻿using UnityEngine;
using UnityEngine.UI;

namespace StarForce
{
    public class EmptyGraphic : Graphic
    {
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
        }

#if UNITY_EDITOR
        private Vector3[] worldCorners = new Vector3[4];

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;

            Gizmos.matrix = Matrix4x4.identity;
            
            RectTransform rectTransform = transform as RectTransform;
            rectTransform.GetWorldCorners(worldCorners);
            for (int i = 0; i < 4; i++)
            {
                Vector3 form = worldCorners[i];
                Vector3 to = worldCorners[(i + 1) % 4];
                Gizmos.DrawLine(form, to);
            }
        }
#endif
    }
}