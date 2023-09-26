using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using UnityEngine.Networking;
using JetBrains.Annotations;
using Unity.VisualScripting;
using DG.Tweening;
using UnityEditor.Build;

public class ObjectSlicer : MonoBehaviour
{
    public float slicedObjectInitialVelocity = 100;
    public Material slicedMaterial;
    public Transform startSlicingPoint;
    public Transform endSlicingPoint;
    public LayerMask slacebleLayer;
    public VelocityEstimator velocityEstimator;

    public delegate void BoxHitDelegate(float hullVolume,Vector3 hullPosition,BoxColor boxColor);
    public event BoxHitDelegate OnBoxHit;


    private int hullCount;
    void Update()
    {
        RaycastHit hit;
        Vector3 slicingDirection = endSlicingPoint.position - startSlicingPoint.position;
        bool hasHit = Physics.Raycast(startSlicingPoint.position, slicingDirection, out hit, slicingDirection.magnitude, slacebleLayer);
        if (hasHit)
        {
            Slice(hit.transform.gameObject,hit.point, velocityEstimator.GetVelocityEstimate(),hit.collider.gameObject.GetComponent<BoxController>().GetBoxColor);
        }
    }
    void Slice(GameObject target, Vector3 planePosition, Vector3 slicerVelocity,BoxColor boxColor)
    {
        AudioManager.instance.Play("SwordHit");
        Vector3 slicingDirection = endSlicingPoint.position - startSlicingPoint.position;
        Vector3 planeNormal = Vector3.Cross(slicerVelocity, slicingDirection);
        SlicedHull hull = target.Slice(planePosition, planeNormal);
        if (hull != null)
        {
            GameObject upperHull = hull.CreateUpperHull(target,slicedMaterial);
            GameObject lowerHull = hull.CreateLowerHull(target, slicedMaterial);

            CreateSlicedComponent(upperHull);
            CreateSlicedComponent(lowerHull);
            SendScore(boxColor, upperHull);
            upperHull.layer= 7;
            lowerHull.layer = 7;
            Destroy(target);
            FadeHul(upperHull.GetComponent<MeshRenderer>().material, lowerHull.GetComponent<MeshRenderer>().material);
        }
    }
    void FadeHul(Material materialUpper, Material materialLower)
    {
        materialUpper.DOFloat(1, "_Dissolve", 1).SetDelay(0.1f).SetEase(Ease.InSine);
        materialLower.DOFloat(1, "_Dissolve", 1).SetDelay(0.1f).SetEase(Ease.InSine);
    }
    void CreateSlicedComponent(GameObject slicedHull)
    {

        Rigidbody rb = slicedHull.AddComponent<Rigidbody>();
        MeshCollider collider = slicedHull.AddComponent<MeshCollider>();
        collider.convex = true;
        rb.AddExplosionForce(slicedObjectInitialVelocity,slicedHull.transform.position,1);
        Destroy(slicedHull, 1.1f);
    }
    /// <summary>
    /// This is to send the sliced box color, and the volume of the hull to the sliced hull controller
    /// </summary>
    /// <param name="target">the box</param>
    /// <param name="hull">One of the sliced hulls</param>
    private void SendScore(BoxColor boxColor, GameObject hull)
    {
        float hullVolume = MeshVolume.VolumeOfMesh(hull.GetComponent<MeshFilter>().mesh) * 100000;
        OnBoxHit?.Invoke(hullVolume, hull.transform.position, boxColor);
    }
}
