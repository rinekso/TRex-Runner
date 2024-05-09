using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysFace : MonoBehaviour
{
    [SerializeField]
    bool _useMainCam = false;
    public Transform _target;
    enum Axis
    {
        none, up, right, forward
    }
    [SerializeField]
    Axis _axis;
    [SerializeField]
    bool useParent = true;
    // Start is called before the first frame update
    void Start()
    {
        if(_useMainCam){
            _target = Camera.main.transform;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(_target != null){
            Vector3 dir = Vector3.zero;
            if(useParent){
                Vector3 targetUpdate = transform.parent.InverseTransformPoint(_target.position);
                dir = (targetUpdate-transform.localPosition).normalized;
            }else{
                dir = (_target.position - transform.position).normalized;
            }

            if(_axis == Axis.none)
                transform.LookAt(_target);
            else if(_axis == Axis.up){
                float targetAngle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
                targetAngle = -targetAngle+90;
                transform.localEulerAngles = new Vector3(0,targetAngle,0);
            }else if(_axis == Axis.right){
                float targetAngle = Mathf.Atan2(dir.y, dir.z) * Mathf.Rad2Deg;
                targetAngle = -targetAngle+90;
                transform.localEulerAngles = new Vector3(targetAngle,0,0);
            }else if(_axis == Axis.forward){
                float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                // targetAngle = targetAngle;
                transform.localEulerAngles = new Vector3(0,0,targetAngle);
            }
        }
    }
}
