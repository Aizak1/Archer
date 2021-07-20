using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTraveller : MonoBehaviour {

    private new Rigidbody rigidbody;

    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
    }

    public  void Teleport(Transform fromPortal, Transform toPortal, Vector3 pos, Quaternion rot) {
        transform.position = pos;
        transform.rotation = rot;
        var vel = toPortal.TransformVector(fromPortal.InverseTransformVector(rigidbody.velocity));
        rigidbody.velocity = vel;

        var inverseAngularVel = fromPortal.InverseTransformVector(rigidbody.angularVelocity);
        var angVel = toPortal.TransformVector(inverseAngularVel);
        rigidbody.angularVelocity = angVel;
    }

}
