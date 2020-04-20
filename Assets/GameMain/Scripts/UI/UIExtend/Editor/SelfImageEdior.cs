using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Events;
using Vector3 = System.Numerics.Vector3;

[CustomEditor(typeof(SelfImage), true)]
[CanEditMultipleObjects]
public class SelfImageEditor : ImageEditor
{
    [SerializeField] //必须要加
    protected float[] m_WorldCorners = new float[2];

    protected SerializedProperty _assetLstProperty;

    protected SerializedProperty m_ImageType;
    protected SerializedProperty m_SliderX;
    protected SerializedProperty m_SliderY;

    private AnimBool m_ShowZoom;
    private AnimBool m_ShowZoomSlider;

    protected override void OnEnable()
    {
        base.OnEnable();
        _assetLstProperty = serializedObject.FindProperty("m_WorldCorners");
        m_ImageType = serializedObject.FindProperty("m_ImageType");
        m_SliderX = serializedObject.FindProperty("m_SliderX");
        m_SliderY = serializedObject.FindProperty("m_SliderY");
        SelfImage.ImageType enumValueIndex = (SelfImage.ImageType) m_ImageType.enumValueIndex;
        this.m_ShowZoom =
            new AnimBool(!m_ImageType.hasMultipleDifferentValues && enumValueIndex == SelfImage.ImageType.Zoom);
        this.m_ShowZoomSlider = new AnimBool(!m_ImageType.hasMultipleDifferentValues &&
                                             enumValueIndex == SelfImage.ImageType.ZoomSlider);
        this.m_ShowZoom.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
        this.m_ShowZoomSlider.valueChanged.AddListener(new UnityAction(((Editor) this).Repaint));
    }

    protected override void OnDisable()
    {
        this.m_ShowZoom.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
        this.m_ShowZoomSlider.valueChanged.RemoveListener(new UnityAction(((Editor) this).Repaint));
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space();
        serializedObject.Update();

        EditorGUILayout.PropertyField(m_ImageType);
        SelfImage target = this.target as SelfImage;
        SelfImage.ImageType enumValueIndex = (SelfImage.ImageType) this.m_ImageType.enumValueIndex;
        bool flag = !this.m_ImageType.hasMultipleDifferentValues &&
                    (enumValueIndex == SelfImage.ImageType.Zoom || enumValueIndex == SelfImage.ImageType.ZoomSlider);
        this.m_ShowZoom.target = !m_ImageType.hasMultipleDifferentValues && enumValueIndex == SelfImage.ImageType.Zoom;
        this.m_ShowZoomSlider.target =
            !m_ImageType.hasMultipleDifferentValues && enumValueIndex == SelfImage.ImageType.ZoomSlider;
        if (EditorGUILayout.BeginFadeGroup(this.m_ShowZoom.faded) && target.hasBorder)
        {
            EditorGUILayout.PropertyField(_assetLstProperty, true);
        }
        EditorGUILayout.EndFadeGroup();

        if (EditorGUILayout.BeginFadeGroup(this.m_ShowZoomSlider.faded) && target.hasBorder)
        {
            EditorGUILayout.Slider(m_SliderX,0,1);
            EditorGUILayout.Slider(m_SliderY,0,1);
        }
        EditorGUILayout.EndFadeGroup();
        
        serializedObject.ApplyModifiedProperties();
    }
}