using UnityEngine;

namespace StarForce
{
    public abstract class ColorBinding : PropertyBinding
    {
        private static readonly Color DefaultColor = Color.white;

        protected override bool IsSuitableProperty()
        {
            return base.IsSuitableProperty() && (Property is Property<Color> || Property is Property<Color32>);
        }

        protected override bool Bind()
        {
            bool isbind = base.Bind();
            if (!isbind)
            {
                if (Property is Property<Color>)
                {
                    (Property as Property<Color>).SetValue(DefaultColor);
                }
                else if (Property is Property<Color32>)
                {
                    (Property as Property<Color32>).SetValue(DefaultColor);
                }
            }

            return isbind;
        }

        protected sealed override void OnChange()
        {
            if (IgnoreChanges) return;
            var newColor = Property == null ? DefaultColor : GetColorValue(Property);
#if NOT_USE_UI_THREAD
            ApplyNewValue(newColor);
#else
            MainThreadDispatcher.DispatchToMainThread(() => { ApplyNewValue(newColor); });
#endif
        }

        protected abstract void ApplyNewValue(Color newColor);
    }
}