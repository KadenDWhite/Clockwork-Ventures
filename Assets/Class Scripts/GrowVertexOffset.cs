using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowVertexOffset : MonoBehaviour
{
    private Material m_material;
    private float offset = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        m_material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        offset += Time.deltaTime;
        m_material.SetFloat("_VertexOffset", offset);
    }
}
