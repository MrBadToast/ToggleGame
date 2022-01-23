using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private float depth;

    [SerializeField] GameObject[] parallaxObjects;

    SpriteRenderer spriteRederer;
    float y_pos;
    private void Awake()
    {
        spriteRederer = GetComponent<SpriteRenderer>();

    }

    private void Start()
    {
        y_pos = transform.position.y;
    }

    public void LateUpdate()
    {
        transform.localPosition = new Vector3(Camera.main.transform.position.x / -depth, y_pos) + Vector3.forward * 10f;
    }

}