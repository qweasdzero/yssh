using StarForce;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace StarForce
{
    /// <summary>
    /// 图片拖拽脚本
    /// </summary>
    public class ImageDrag : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        private Image m_Image;

        private void Awake()
        {
            m_Image = GetComponent<Image>();
            m_Image.enabled = false;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            m_Image.enabled = true;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            m_Image.enabled = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = GameEntry.UI.UICamera.ScreenToWorldPoint(eventData.position);
        }
    }
}