using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnityTestNativePlugin : MonoBehaviour
{
    [SerializeField] private TMP_InputField firstInputField;
    [SerializeField] private TMP_InputField secondInputField;
    [SerializeField] private TMP_InputField resultInputField;

    [SerializeField] private Concatenater concatenater;
    // Start is called before the first frame update
    void Start()
    {
        if (concatenater == null)
        {
            concatenater = GetComponent<Concatenater>();
        }
    }

    public void ResultAction()
    {
        concatenater.ConcatenateValues(firstInputField.text, secondInputField.text, (response) =>
        {
            resultInputField.text = response.concatenated;
        });
    }
}
