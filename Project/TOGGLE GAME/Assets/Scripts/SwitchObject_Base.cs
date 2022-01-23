using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchObject_Base : MonoBehaviour
{
    protected virtual void Start()
    {
        Debug.Log(StageMaster.Instance.name);
        StageMaster.Instance.onLampActivated += OnActivate;
        StageMaster.Instance.onLampDeactivated += OnDeactivate;
    }

    protected virtual void OnDestroy()
    {
        StageMaster.Instance.onLampActivated -= OnActivate;
        StageMaster.Instance.onLampDeactivated -= OnDeactivate;
    }

    public void ManualDelegateRemove()
    {
        StageMaster.Instance.onLampActivated -= OnActivate;
        StageMaster.Instance.onLampDeactivated -= OnDeactivate;
    }

    public virtual void OnActivate() { }
    public virtual void OnDeactivate() { }


}
