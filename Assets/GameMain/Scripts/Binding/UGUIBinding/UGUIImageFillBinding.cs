using System;
using UnityEngine;
using UnityEngine.UI;

namespace StarForce
{
    [RequireComponent(typeof(Image))]
    [DisallowMultipleComponent]
    public class UGUIImageFillBinding : NumericBinding
    {
        [SerializeField, InspectorReadOnly] private Image m_Image;
        protected override void ApplyNewValue(double value)
        {
            m_Image.fillAmount = (float)value;
        }

        protected override void OnEditorValue()
        {
            base.OnEditorValue();
            if (m_Image == null)
            {
                m_Image = gameObject.GetComponent<Image>();
            }
        }
    }
}