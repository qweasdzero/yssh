using UnityEngine.EventSystems;

namespace StarForce
{
    public abstract class UICustomBehaviour : UIBehaviour
    {
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            OnEditorValue();
        }

        protected override void Reset()
        {
            OnValidate();
        }
#endif

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        protected virtual void OnEditorValue()
        {
        }
    }
}