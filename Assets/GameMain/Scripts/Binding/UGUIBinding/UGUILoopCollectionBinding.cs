using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

#pragma warning disable 0649
namespace StarForce
{
    public class UGUILoopCollectionBinding : ItemCollectionBinding
    {
        private LinkedList<ItemModelView> m_ItemModelViews = new LinkedList<ItemModelView>();

        [SerializeField] private Vector2 m_ItemSize = new Vector2(100f, 100f);

        [SerializeField] private ScrollRect m_ScrollRect;

        [SerializeField] private RectTransform m_Viewport;

        [SerializeField] private RectTransform m_Content;

        [SerializeField] private RectTransform.Axis m_Axis;

        [SerializeField] private RectOffset m_Padding;

        [SerializeField] private float m_Spacing;

        //[SerializeField] private TextAnchor m_ChildAlignment;

        [SerializeField] private int m_CacheCount;

        [SerializeField, InspectorReadOnly] private float m_Extents;

        [NonSerialized] private RectTransform m_Rect;

        public RectTransform RectTransform
        {
            get
            {
                if (m_Rect == null)
                {
                    m_Rect = GetComponent<RectTransform>();
                }

                return m_Rect;
            }
        }

        protected override bool Bind()
        {
            if (base.Bind())
            {
                m_ScrollRect.onValueChanged.AddListener(UpdateContext);
                SetContent();
                return true;
            }

            return false;
        }

        protected override void Unbind()
        {
            m_ScrollRect.onValueChanged.RemoveListener(UpdateContext);
            base.Unbind();
        }

        private int AxisSign
        {
            get
            {
                if (m_Axis == RectTransform.Axis.Horizontal)
                {
                    return 1;
                }
                else if (m_Axis == RectTransform.Axis.Vertical)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
        }

        private Vector3 GetAxisVector3(float value)
        {
            if (m_Axis == RectTransform.Axis.Horizontal)
            {
                return new Vector3(value, 0, 0);
            }
            else if (m_Axis == RectTransform.Axis.Vertical)
            {
                return new Vector3(0, value, 0);
            }
            else
            {
                return Vector3.zero;
            }
        }

        protected override void OnItemClear()
        {
            foreach (var itemModelView in m_ItemModelViews)
            {
                itemModelView.ClearAllCache();
            }

            SetContentSize();

            float value = 0;

            if (m_Axis == RectTransform.Axis.Horizontal)
            {
                value = m_Padding.left + (m_ItemSize[(int) m_Axis] + m_Spacing) / 2f;
            }
            else if (m_Axis == RectTransform.Axis.Vertical)
            {
                value = m_Padding.top - (m_ItemSize[(int) m_Axis] + m_Spacing) / 2f;
            }

            m_ItemModelViews.First.Value.transform.localPosition = GetAxisVector3(value);

            int index = 0;
            foreach (var itemModelView in m_ItemModelViews)
            {
                SetItemPosition(itemModelView.transform as RectTransform, index);
                itemModelView.ClearAllCache();
                itemModelView.SetContext(itemModelView.Index, Collection.GetBaseItem(itemModelView.Index));
                ++index;
            }
        }

        protected override void OnItemRemove(int index)
        {
            SetContentSize();
            foreach (var itemModelView in m_ItemModelViews)
            {
                float value = AxisSign * (m_ItemSize[(int) m_Axis] + m_Spacing) / 2;
                itemModelView.transform.localPosition += GetAxisVector3(value);
                itemModelView.ClearAllCache();
                itemModelView.SetContext(itemModelView.Index, Collection.GetBaseItem(itemModelView.Index));
            }
        }

        protected override void OnItemInsert(int index, ItemContext itemContext)
        {
            SetContentSize();
            foreach (var itemModelView in m_ItemModelViews)
            {
                float value = AxisSign * (m_ItemSize[(int) m_Axis] + m_Spacing) / 2;
                itemModelView.transform.localPosition -= GetAxisVector3(value);
                itemModelView.ClearAllCache();
                itemModelView.SetContext(itemModelView.Index, Collection.GetBaseItem(itemModelView.Index));
            }
        }

        private void UpdateContext(Vector2 normalizedPosition)
        {
            if (m_ItemModelViews.Count <= 0)
            {
                return;
            }
            //TODO:考虑缩减遍历个数
            foreach (var itemModelView in m_ItemModelViews)
            {
                UpdatePosition(itemModelView);
            }
        }

        /// <summary>
        /// 更新Item的位置
        /// </summary>
        /// <param name="itemModelView">ItemModelView</param>
        /// <returns>当水平滑动时,左边界返回1,右边界返回-1,未更新返回0,当垂直滑动时,上边界返回-1,下边界返回1,未更新返回0</returns>
        private int UpdatePosition(ItemModelView itemModelView)
        {
            Transform itemTrans = itemModelView.transform;
            float viewPos = m_Content.InverseTransformPoint(m_Viewport.position)[(int) m_Axis];
            float itemPos = m_Content.InverseTransformPoint(itemTrans.position)[(int) m_Axis];
            float distance = itemPos - viewPos;

            if (distance > m_Extents || distance < -m_Extents)
            {
                int sign = distance > m_Extents ? -1 : 1;

                float moveExtents = m_Extents - m_ItemSize[(int) m_Axis] * 0.5f;

                int index = itemModelView.Index;

                ItemContext itemContext = null;

                if (m_Axis == RectTransform.Axis.Horizontal)
                {
                    index += sign * m_CacheCount;
                }
                else
                {
                    index -= sign * m_CacheCount;
                }

                itemContext = Collection.GetBaseItem(index);

                if (itemContext == null)
                {
                    return 0;
                }

                float value = moveExtents * 2 + m_Spacing;
                itemTrans.localPosition += sign * GetAxisVector3(value);

                itemModelView.ClearAllCache();
                itemModelView.SetContext(index, itemContext);
                //Log.Debug("{0} : {1}", m_Axis, sign);
                return sign;
            }

            return 0;
        }

        private Vector2 GetLocalPosition(int index)
        {
            Vector2 localPosition = Vector2.zero;
            float size = m_ItemSize[(int) m_Axis];
            Vector2 dir = Vector2.zero;
            float mic = 0;
            if (m_Axis == RectTransform.Axis.Horizontal)
            {
                dir = Vector2.right;
                mic = m_Padding.left + 0.5f * size;
            }
            else if (m_Axis == RectTransform.Axis.Vertical)
            {
                dir = Vector2.down;
                mic = m_Padding.top + 0.5f * size;
            }

            if (m_ItemModelViews.Count == 0)
            {
                localPosition = dir * (mic + index * (size + m_Spacing));
            }
            else
            {
                Vector3 vector3 = m_ItemModelViews.First.Value.transform.localPosition;
                Vector2 pos = new Vector2(vector3.x, vector3.y);
                localPosition = pos + dir * index * (size + m_Spacing);
            }

            return localPosition;
        }

        private ItemModelView InstantiateItem(int index)
        {
            GameObject itemObject = Instantiate(Template, transform);
            RectTransform rectTransform = itemObject.GetComponent<RectTransform>();
            rectTransform.localScale = Vector3.one;
            rectTransform.anchorMax = rectTransform.anchorMin = Vector2.one / 2f;
            rectTransform.sizeDelta = m_ItemSize;
            SetItemPosition(rectTransform, index);
            rectTransform.transform.SetSiblingIndex(index);
            ItemModelView modelView = itemObject.GetOrAddComponent<ItemModelView>();
            modelView.TemplateName = Template.name;
            m_ItemModelViews.AddLast(modelView);
            return modelView;
        }

        private void SetItemPosition(RectTransform rectTransform, int index)
        {
            rectTransform.localPosition = GetLocalPosition(index);
        }

        private void SetContent()
        {
            SetContentSize();

            InitTemplate();
        }

        private void SetContentSize()
        {
            float size = 0;

            if (m_Axis == RectTransform.Axis.Horizontal)
            {
                size += m_Padding.horizontal;
            }
            else if (m_Axis == RectTransform.Axis.Vertical)
            {
                size += m_Padding.vertical;
            }

            for (int i = 0; i < Collection.Count; i++)
            {
                size += m_ItemSize[(int) m_Axis];
                if (i < Collection.Count - 1)
                {
                    size += m_Spacing;
                }
            }

            RectTransform.SetSizeWithCurrentAnchors(m_Axis, size);

            float itemSize = m_ItemSize[(int) m_Axis];
            m_Extents = ((itemSize + m_Spacing) * m_CacheCount - m_Spacing + itemSize) * 0.5f;
        }

        private void InitTemplate()
        {
            for (int i = 0; i < m_CacheCount; i++)
            {
                ItemModelView itemModelView = InstantiateItem(i);
                itemModelView.SetContext(i, Collection.GetBaseItem(i));
            }
        }

        protected override void OnEditorValue()
        {
            base.OnEditorValue();

            if (m_ScrollRect == null)
            {
                m_ScrollRect = GetComponentInParent<ScrollRect>();
            }

            if (m_Padding == null)
            {
                m_Padding = new RectOffset();
                //m_ChildAlignment = TextAnchor.UpperCenter;
            }

            if (m_Viewport == null && m_ScrollRect != null)
            {
                m_Viewport = m_ScrollRect.viewport;
            }

            if (m_Content == null && m_ScrollRect != null)
            {
                m_Content = m_ScrollRect.content;
            }
        }
    }
}