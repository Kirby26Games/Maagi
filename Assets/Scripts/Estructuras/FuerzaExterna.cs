using System;
using System.Collections;
using UnityEngine;

[Serializable]
public struct FuerzaExterna
{
    public Vector3 Fuerza;
    public Vector3 FuerzaActual;
    public float Tiempo;

    public void Contador()
    {
        FuerzaActual -= (Fuerza/Tiempo)*Time.deltaTime;
    }
}
