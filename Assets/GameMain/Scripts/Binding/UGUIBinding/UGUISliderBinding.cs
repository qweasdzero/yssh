using System;
using UnityEngine;
using UnityEngine.UI;

namespace StarForce
{
    [RequireComponent(typeof(Slider))]
    [DisallowMultipleComponent]
    public class UGUISliderBinding : NumericBinding
    {
        [SerializeField, InspectorReadOnly] private Slider m_Slider;

        protected override bool Bind()
        {
            m_Slider.onValueChanged.AddListener(OnApplyInputValue);
            return base.Bind();
        }

        protected override void Unbind()
        {
            base.Unbind();
            m_Slider.onValueChanged.RemoveListener(OnApplyInputValue);
        }

        private void OnApplyInputValue(float value)
        {
            ApplyInputValue(value);
        }

        protected override void ApplyNewValue(double value)
        {
            m_Slider.value = Mathf.Clamp((float) value, m_Slider.minValue, m_Slider.maxValue);
        }

        protected override void OnEditorValue()
        {
            base.OnEditorValue();
            if (m_Slider == null)
            {
                m_Slider = gameObject.GetComponent<Slider>();
            }
        }
    }
}