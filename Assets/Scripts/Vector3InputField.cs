using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Vector3InputField : MonoBehaviour
{
    public Action<Vector3> OnValueChanged;

    [SerializeField] private TMP_InputField xValueInputfield;
    [SerializeField] private TMP_InputField yValueInputfield;
    [SerializeField] private TMP_InputField zValueInputfield;

    private float valueX;
    private float valueY;
    private float valueZ;

    void Awake()
    {
        xValueInputfield.onValueChanged.AddListener(OnXValueChanged);
        yValueInputfield.onValueChanged.AddListener(OnYValueChanged);
        zValueInputfield.onValueChanged.AddListener(OnZValueChanged);
    }

    private void OnXValueChanged(string arg0)
    {
        float.TryParse(arg0, out valueX);

        OnValueChanged?.Invoke(new Vector3(valueX, valueY, valueZ));
    }

    private void OnYValueChanged(string arg0)
    {
        float.TryParse(arg0, out valueY);

        OnValueChanged?.Invoke(new Vector3(valueX, valueY, valueZ));
    }

    private void OnZValueChanged(string arg0)
    {
        float.TryParse(arg0, out valueZ);

        OnValueChanged?.Invoke(new Vector3(valueX, valueY, valueZ));
    }
}
