using System;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

public class SimpleBoolDisplay : BindableElement, INotifyValueChanged<bool>
{
    public new class UxmlTraits : BindableElement.UxmlTraits
    {
        UxmlStringAttributeDescription name = new(){ name = "Label", defaultValue = "SimpleBoolDisplay" };

        public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
        {
            get { yield break; }
        }

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            ((SimpleBoolDisplay)ve).labelText = name.GetValueFromBag(bag, cc);
        }
    }
    public new class UxmlFactory : UxmlFactory<SimpleBoolDisplay, UxmlTraits> { }

    public static readonly string ussUnityBaseField = "unity-base-field";
    public static readonly string ussClassName = "bool-display";

    private string _labelText = string.Empty;
    public string labelText
    {
        get => _labelText;
        set
        {
            _labelText = value;
            label.text = value;
        }
    }

    private Label label;
    private bool boolValue;

    public SimpleBoolDisplay()
    {
        AddToClassList(ussUnityBaseField);
        AddToClassList(ussClassName);

        label = new Label("default");
        Add(label);
    }

    public void SetValueWithoutNotify(bool newValue)
    {
        boolValue = newValue;
        if (newValue)
        {
            RemoveFromClassList("false");
            AddToClassList("true");
        }
        else
        {
            RemoveFromClassList("true");
            AddToClassList("false");
        }
    }

    public bool value
    {
        get => boolValue;

        set
        {
            if (value == this.value)
            {
                return;
            }

            var previous = this.value;
            SetValueWithoutNotify(value);

            using var evt = ChangeEvent<bool>.GetPooled(previous, value);
            evt.target = this;
            SendEvent(evt);
        }
    }
}
