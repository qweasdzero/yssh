using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace SG1
{
    static class EditorExtensions
    {
        [MenuItem("GameObject/Self/ImageCompletion")]
        static void Completion()
        {
            GameObject go = new GameObject("ImageCompletion");
            go.AddComponent<ImageCompletion>();
            SetTransform(go);
        }

        [MenuItem("GameObject/Self/SelfImage")]
        static void NewSelfImage()
        {
            GameObject go = new GameObject("SelfImage");
            go.AddComponent<SelfImage>();
            SetTransform(go);
        }

        [MenuItem("GameObject/Self/SuperInput")]
        static void NewSuperInputField()
        { 
            GameObject superInputField = new GameObject("SuperInputField");
            superInputField.AddComponent<Image>();
            SuperInputField super = superInputField.AddComponent<SuperInputField>();
            RectTransform superinputrect = (RectTransform) superInputField.transform;
            superinputrect.sizeDelta = new Vector2(160, 30);

            GameObject placeholder = new GameObject("Placeholder");
            placeholder.transform.SetParent(superInputField.transform);
            Text placeholdertext = placeholder.AddComponent<Text>();
            RectTransform placeholderrect = (RectTransform) placeholder.transform;
            placeholdertext.color = new Color32(50, 50, 50, 128);
            placeholdertext.fontStyle = FontStyle.Italic;
            placeholdertext.raycastTarget = false;
            placeholderrect.anchorMin = Vector2.zero;
            placeholderrect.anchorMax = Vector2.one;
            placeholderrect.offsetMax = new Vector2(-10, -7);
            placeholderrect.offsetMin = new Vector2(10, 6);
            placeholdertext.text = "Enter text...";

            GameObject text = new GameObject("Text");
            text.transform.SetParent(superInputField.transform);
            Text texttext = text.AddComponent<Text>();
            RectTransform textrect = (RectTransform) text.transform;
            texttext.color = new Color32(50, 50, 50, 255);
            texttext.raycastTarget = false;
            texttext.supportRichText = false;
            textrect.anchorMin = Vector2.zero;
            textrect.anchorMax = Vector2.one;
            textrect.offsetMax = new Vector2(-10, -7);
            textrect.offsetMin = new Vector2(10, 6);

            super.placeholder = placeholdertext;
            super.textComponent = texttext;
            superInputField.layer = 5;
            text.layer = 5;
            placeholder.layer = 5;
            SetTransform(superInputField);
        }

        /// <summary>
        /// 设施生成物体位置
        /// </summary>
        /// <param name="go"></param>
        static void SetTransform(GameObject go)
        {
            if (Selection.activeGameObject == null ||
                Selection.activeGameObject.GetComponentsInParent<Canvas>() == null)
            {
                GameObject canvas = GameObject.Find("UI Form Instances");
                if (!canvas)
                {
                    canvas = new GameObject("Canvas");
                    canvas.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
                    canvas.AddComponent<CanvasScaler>();
                    canvas.AddComponent<GraphicRaycaster>();
                    canvas.transform.SetParent(Selection.activeGameObject.transform);
                }

                go.transform.SetParent(canvas.transform);
            }
            else
            {
                go.transform.SetParent(Selection.activeGameObject.transform);
            }

            go.transform.localScale = Vector3.one;
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            Selection.activeGameObject = go;
        }
    }
}