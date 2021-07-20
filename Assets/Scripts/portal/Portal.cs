using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public bool isOrange;

    public Portal linkedPortal;

    public Camera selfCamera;

    public Texture defaultTexture;

    public bool isReady = false;

    private const string OPEN_PORTAL_TRIGGER = "Open";
    private const string CLOSE_PORTAL_TRIGGER = "Close";
    private const string UNTAGGED = "Untagged";
    public const string BLUE_PORTAL_TAG = "Blue Portal";
    public const string ORANGE_PORTAL_TAG = "Orange Portal";

    public void MakeTeleport(PortalTraveller traveller) {
        if (!isReady) {
            return;
        }
        if (traveller) {
            var linkedWorldMatrix = linkedPortal.transform.localToWorldMatrix;
            var localMatrix = transform.worldToLocalMatrix;
            var travellerWorldMatrix = traveller.transform.localToWorldMatrix;
            var matrix = linkedWorldMatrix * localMatrix * travellerWorldMatrix;

            var pos = matrix.GetColumn(3);
            traveller.Teleport(transform, linkedPortal.transform, pos, matrix.rotation);
        }
    }

    private void CheckSecondPortal()
    {
        string tagToFindObject;

        if (isOrange) {
            tagToFindObject = BLUE_PORTAL_TAG;
        } else {
            tagToFindObject = ORANGE_PORTAL_TAG;
        }

        if (GameObject.FindGameObjectWithTag(tagToFindObject) != null)
        {
            ConnectPortals();
            linkedPortal.ConnectPortals();
        }
    }

    private void ConnectPortals()
    {
        string tagToFindObject;

        if (isOrange) {
            tagToFindObject = BLUE_PORTAL_TAG;
        } else {
            tagToFindObject = ORANGE_PORTAL_TAG;
        }

        linkedPortal = GameObject.FindGameObjectWithTag(tagToFindObject).GetComponent<Portal>();
        isReady = true;
    }

    public void Open()
    {
        GetComponent<Animator>().SetTrigger(OPEN_PORTAL_TRIGGER);
        CheckSecondPortal();

    }

    public void Close()
    {
        tag = UNTAGGED;
        GetComponent<Animator>().SetTrigger(CLOSE_PORTAL_TRIGGER);
        Destroy(gameObject, 0.5f);
    }
}
