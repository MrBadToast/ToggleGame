using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SwitchableGround : SwitchObject_Base
{
    public GameObject activeGround;
    public GameObject deactiveGround;

    public bool Inverted = false;

    protected override void Start()
    {
        base.Start();

        if (!Inverted)
        {
            activeGround.SetActive(true);
            deactiveGround.SetActive(false);
        }
        else
        {
            activeGround.SetActive(false);
            deactiveGround.SetActive(true);
        }
    }

    public override void OnActivate()
    {
        if (!Inverted)
        {
            activeGround.SetActive(true);
            deactiveGround.SetActive(false);
        }
        else
        {
            activeGround.SetActive(false);
            deactiveGround.SetActive(true);
        }
    }

    public override void OnDeactivate()
    {
        if (!Inverted)
        {
            activeGround.SetActive(false);
            deactiveGround.SetActive(true);
        }
        else
        {
            activeGround.SetActive(true);
            deactiveGround.SetActive(false);
        }
    }
}
