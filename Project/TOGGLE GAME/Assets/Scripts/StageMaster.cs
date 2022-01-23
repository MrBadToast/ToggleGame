using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class StageMaster : MonoBehaviour
{
    private static StageMaster instance;

    public static StageMaster Instance => instance;

    public delegate void LampOn();
    public delegate void LampOff();

    public LampOn onLampActivated;
    public LampOff onLampDeactivated;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private bool isStageInverted = false;
    public bool IsStageInverted { get { return isStageInverted; } }

    [SerializeField]
    private Volume ppVolume;

    public void SwitchMapState(bool value)
    {
        if(value)
        {
            if (onLampActivated != null)
                onLampActivated.Invoke();
        }
        else
        {
            if (onLampDeactivated != null)
                onLampDeactivated.Invoke();
        }
    }

    private void Update()
    {
        
    }
}
