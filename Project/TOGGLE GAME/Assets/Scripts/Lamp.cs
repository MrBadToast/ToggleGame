using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Lamp : MonoBehaviour
{
    [SerializeField] private float dragDistance = 20f;

    private bool IsLightControlEnabled = true;
    [SerializeField]
    private bool isLightOn = true;

    private Vector2 mouseDownPosition;
    private Vector2 mouseUpPosition;

    //public delegate void LampOn();
    //public delegate void LampOff();

    //public LampOn onLampActivated;
    //public LampOff onLampDeactivated;

    public LineRenderer lampString;
    public Volume ppVolume;

    private void Update()
    {
        if (!IsLightControlEnabled) return;

        if (Input.GetMouseButtonDown(0))
            mouseDownPosition = Input.mousePosition;

        if (Input.GetMouseButtonUp(0))
        {
            mouseUpPosition = Input.mousePosition;

            if (mouseUpPosition.y- mouseDownPosition.y < -dragDistance)
            {
                if (ppVolume.profile.TryGet<ColorCurves>(out var cc))
                {
                    if (IsLightControlEnabled)
                    {
                        if (!isLightOn)
                        {
                            isLightOn = true;
                            cc.active = false;

                            StageMaster.Instance.SwitchMapState(true);

                            GetComponent<SimpleSoundModule>().Play("Light");

                        }
                        else
                        {
                            isLightOn = false;
                            cc.active = true;

                            StageMaster.Instance.SwitchMapState(false);

                            GetComponent<SimpleSoundModule>().Play("Light");
                        }
                    }
                }
            }
        }

        Debug.DrawLine(mouseDownPosition, Input.mousePosition, Color.magenta);

        Camera mainCamera;
        mainCamera = Camera.main;

        Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        const float switchOnLength = -1.2f;
        const float switchOffLength = -2.5f;

        if (Input.GetMouseButton(0))
        {
            if(isLightOn)
            lampString.SetPosition(1, Vector2.up * (Mathf.Clamp(mouseWorldPos.y - mainCamera.ScreenToWorldPoint(mouseDownPosition).y, switchOffLength - 0.25f, switchOnLength)));
            else
                lampString.SetPosition(1, Vector2.up * (Mathf.Clamp(mouseWorldPos.y - mainCamera.ScreenToWorldPoint(mouseDownPosition).y, switchOffLength - 0.25f, switchOffLength)));
        }
        else
        {
            if (isLightOn)
                lampString.SetPosition(1, Vector3.up * Mathf.Lerp(lampString.GetPosition(1).y, switchOnLength, 0.1f));
            else
                lampString.SetPosition(1, Vector3.up * Mathf.Lerp(lampString.GetPosition(1).y, switchOffLength, 0.1f));
        }
    }
}
