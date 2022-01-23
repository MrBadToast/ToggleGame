using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSwitchableObject : SwitchObject_Base
{
    public Sprite positive;
    public Sprite negative;

    public bool Inverted;

    private SpriteRenderer spriteRenderer;

    protected override void Start()
    {
        base.Start();

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (!Inverted) spriteRenderer.sprite = positive;
        else spriteRenderer.sprite = negative;

    }

    public override void OnActivate()
    {
        spriteRenderer.sprite = positive;

    }

    public override void OnDeactivate()
    {
        spriteRenderer.sprite = negative;
    }
}
