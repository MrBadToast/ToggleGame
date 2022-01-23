using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class Ghost : SwitchObject_Base
{
    [SerializeField] private float speed;
    [SerializeField] private Animator anim;
    [SerializeField] private Volume ppVolume;

    private SimpleSoundModule soundModule;

    [SerializeField]
    private bool enabled = false;

    private bool isAppeared = false;

    private void Awake()
    {
        soundModule = GetComponent<SimpleSoundModule>();
    }

    private void Update()
    {
        if (enabled)
        {
            if (isAppeared)
            {
                transform.position += ((PlayerBehavior.Instance.transform.position - transform.position).normalized * speed);
            }

            if (transform.position.x > PlayerBehavior.Instance.transform.position.x)
            {
                transform.rotation = Quaternion.Euler(Vector3.up * 180f);
            }
            else if (transform.position.x < PlayerBehavior.Instance.transform.position.x)
            {
                transform.rotation = Quaternion.Euler(Vector3.up * 0f);
            }
        }

        //if (ppVolume.profile.TryGet<ChromaticAberration>(out var ca))
        //{
        //    if (Vector2.Distance(transform.position, PlayerBehavior.Instance.transform.position) < 10f)
        //    {
        //        ca.Override( Mathf.Lerp(4f, 0f, 10 / Vector2.Distance(transform.position, PlayerBehavior.Instance.transform.position)));
        //    }

        //    ca.intensity.value = 0.0f;
        //}
    }
    

    public override void OnActivate()
    {
        if (!enabled) return;
        isAppeared = false;
        anim.SetBool("Disolve",true);
        anim.SetBool("Appear", false);
        soundModule.Play("Disolve");
        GetComponent<BoxCollider2D>().enabled = false;


    }

    public override void OnDeactivate()
    {
        if (!enabled) return;
        isAppeared = true;
        anim.SetBool("Appear",true);
        anim.SetBool("Disolve", false);
        soundModule.Play("Appear");
        GetComponent<BoxCollider2D>().enabled = true;
    }

    public void SetEnabled(bool value)
    {
        enabled = value;
    }

}
