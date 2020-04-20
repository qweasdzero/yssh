using UnityEngine;

namespace StarForce
{
    public abstract class IndexBinding<T> : NumericBinding
    {
        [SerializeField] private T m_DefaultValue = default(T);

        [SerializeField] private T[] m_Values;

        protected override bool IsSuitableProperty()
        {
            return base.IsSuitableProperty() && Property is Property<int>;
        }

        protected sealed override void ApplyNewValue(double value)
        {
            int index = (int) System.Math.Floor(value);
            T newValue;
            if (m_Values == null || index < 0 || index > m_Values.Length)
            {
                newValue = m_DefaultValue;
            }
            else
            {
                newValue = m_Values[index];
            }

#if NOT_USE_UI_THREAD
            ApplyNewValue(newValue);
#else
            MainThreadDispatcher.DispatchToMainThread(() => { ApplyNewValue(newValue); });
#endif
        }

        protected abstract void ApplyNewValue(T value);

        protected override void OnEditorValue()
        {
            base.OnEditorValue();
            if (m_Values == null)
            {
                m_Values = new T[] {};
            }
        }
    }
}