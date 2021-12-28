using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXProperties : MonoBehaviour
{
    [SerializeField] private VisualEffect beizerCurveVfx;

    [SerializeField] private Vector3InputField initPointVector3InputField;
    [SerializeField] private Vector3InputField endPointVector3InputField;

    // Start is called before the first frame update
    void Start()
    {
        initPointVector3InputField.OnValueChanged += OnInitInputFieldValueChanged;
        endPointVector3InputField.OnValueChanged += OnEndInputFieldValueChanged;
    }

    private void OnInitInputFieldValueChanged(Vector3 obj)
    {
        beizerCurveVfx.SetVector3("InitialPosition", obj);
    }

    private void OnEndInputFieldValueChanged(Vector3 obj)
    {
        beizerCurveVfx.SetVector3("EndPosition", obj);
    }
}
