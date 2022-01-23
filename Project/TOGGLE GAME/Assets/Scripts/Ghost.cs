using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Ghost : SwitchObject_Base
{
    [SerializeField] private float speed;
    [SerializeField] private SkeletonAnimation anim;

    private SimpleSoundModule soundModule;

    private bool enabled = false;

    private bool isAppeared = false;

    private void Update()
    {
        if(enabled)
        {
            if(isAppeared)
            {
                transform.Translate((PlayerBehavior.Instance.transform.position - transform.position).normalized * speed);
            }
        }
    }

    public override void OnActivate()
    {
        if (!enabled) return;
        isAppeared = false;
        anim.AnimationName = "Disolve";
        soundModule.Play("Disolve");


    }

    public override void OnDeactivate()
    {
        if (!enabled) return;

        isAppeared = true;
        anim.AnimationName = "Appear";
        soundModule.Play("Apeear");

    }

    public void SetEnabled(bool value)
    {
        enabled = value;
    }

}
