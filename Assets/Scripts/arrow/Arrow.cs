using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private Transform tip;

    public bool isInAir;

    [SerializeField]
    private new Rigidbody rigidbody;

    private Quaternion lastRotation;

    private void FixedUpdate() {
        if (!isInAir) {
            return;
        }

        if (rigidbody.velocity == Vector3.zero) {
            return;
        }

        transform.rotation = Quaternion.LookRotation(rigidbody.velocity, transform.up);
        lastRotation = transform.rotation;
    }

    public void Release(float pullAmount) {
        isInAir = true;
        rigidbody.useGravity = true;
        rigidbody.isKinematic = false;
        rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;

        Vector3 force = transform.forward * (pullAmount * speed);
        rigidbody.AddForce(force);

    }

    private void OnCollisionEnter(Collision collision) {
        rigidbody.Sleep();
        rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        isInAir = false;
        rigidbody.useGravity = false;
        rigidbody.isKinematic = true;
        GetComponent<Collider>().enabled = false;

        transform.parent = collision.gameObject.transform;
        transform.rotation = lastRotation;
    }
}
