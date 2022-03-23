using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtObject : MonoBehaviour
{
    [SerializeField] GameObject objectToLookAt;
    [SerializeField] bool lockX;
    [SerializeField] bool lockY;
    [SerializeField] bool lockZ;

    Vector3 targetPosition;

    // Update is called once per frame
    void Update()
    {
        if (objectToLookAt == null) return;

        targetPosition = objectToLookAt.transform.position;

        //lock the rotation by forcing the target position to be the same as ours, e.g. same height. This will stop rotation in that axis
        if (lockX) targetPosition.x = this.gameObject.transform.localPosition.x;
        if (lockY) targetPosition.y = this.gameObject.transform.localPosition.y;
        if (lockZ) targetPosition.z = this.gameObject.transform.localPosition.z;

        this.transform.LookAt(targetPosition);

    }
}
