using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class CardInfo : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    [MenuItem("Window/UI Toolkit/CardInfo")]
    public static void ShowExample()
    {
        CardInfo wnd = GetWindow<CardInfo>();
        wnd.titleContent = new GUIContent("CardInfo");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Instantiate UXML
        VisualElement fromUxml = m_VisualTreeAsset.Instantiate();
        fromUxml.Children().First(c => c.name == "OriginalPosition").SetEnabled(false);

        root.Add(fromUxml);
    }

    public void OnSelectionChange()
    {
        var selectedObject = Selection.activeObject.GetComponent<Card>();
        if (selectedObject != null)
        {
            SerializedObject so = new(selectedObject);
            rootVisualElement.Bind(so);
        }
        else
        {
            rootVisualElement.Unbind();
        }
    }
}
