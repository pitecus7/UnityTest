using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[StructLayout(LayoutKind.Sequential)]
public struct TwoStrings
{
    public string str1;
    public string str2;
    public string concatenated;
}

public class Concatenater : MonoBehaviour
{
    public delegate void ResponseDelegate(TwoStrings s);

    [DllImport("NATIVECPPLIBRARY", EntryPoint = "ConcatenateValues", CallingConvention = CallingConvention.StdCall)]
    public static extern void ConcatenateValues(TwoStrings _twoStrings, ResponseDelegate response);

    public void ConcatenateValues(string valueOne, string valueTwo, ResponseDelegate callback)
    {
        ConcatenateValues(new TwoStrings()
        {
            str1 = valueOne,
            str2 = valueTwo
        }, callback);
    }
}
