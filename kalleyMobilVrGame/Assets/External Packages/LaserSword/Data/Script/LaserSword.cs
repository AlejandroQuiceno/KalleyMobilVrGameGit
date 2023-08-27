using System.Collections;
using UnityEngine;
using UnityEngine.Events;


namespace ArmnomadsGames
{

    [System.Serializable]
    public class TriggerEvent : UnityEvent<Collider> { }

    [System.Serializable]
    public class SliceEventOne : UnityEvent<Vector3> { }

    [System.Serializable]
    public class SliceEventTwo : UnityEvent<GameObject, GameObject> { }


    public class LaserSword : MonoBehaviour
    {
        [Space(10)]

        [SerializeField] bool enableAtStart = true;

        [Space(10)]

        [Header("---------- Laser ----------")]
        [SerializeField] [Range(12, 40)] float laserFadeSpeed = 18;
        [SerializeField] float laserShowTime = 0.3f;
        [SerializeField] float laserHideTime = 0.3f;
        [SerializeField] AnimationCurve curveScaleUp;
        [SerializeField] AnimationCurve curveScaleDown;

        [Header("---------- Slice ----------")]
        [SerializeField] bool slicing = true;
        [SerializeField] bool sliceablePieces = true;
        [SerializeField] bool attachRigidBodies = true;
        [SerializeField] bool attachColliders = true;
        [SerializeField] string sliceTag = "Player";
        [SerializeField] Material slicedMaterial;
        [SerializeField] PhysicMaterial slicedPhysicMaterial;

        [Header("---------- Flickering ----------")]
        [SerializeField] bool flickering;
        [SerializeField] [Range(0.01f, 0.05f)] float flickRate = 0.025f;
        [SerializeField] [Range(0.1f, 0.9f)] float flickMin = 0.5f;
        [SerializeField] [Range(1, 8)] float lightMultiplier = 2f;
        [SerializeField] Light laserLight;

        [Header("---------- Lens Flare ----------")]
        [SerializeField] LensFlare contactLensFlare;
        [SerializeField] [Range(0, 50)] float lensMin = 50f;
        [SerializeField] [Range(50, 100)] float lensMax = 70f;

        [Header("---------- Particles ----------")]
        [SerializeField] ParticleSystem lightning_particle;
        [SerializeField] ParticleSystem smoke_particle;

        [Header("---------- Audio ----------")]
        [SerializeField] [Range(0.1f, 0.5f)] float hitSoundCooldown = 0.3f;
        [SerializeField] AudioSource audioIdle;
        [SerializeField] AudioSource audioInstant;
        [SerializeField] AudioClip clipHit;
        [SerializeField] AudioClip clipMove;
        [SerializeField] AudioClip clipShowHide;

        [Header("---------- Events ----------")]
        [SerializeField] TriggerEvent OnTriggerEnter;
        [SerializeField] TriggerEvent OnTriggerStay;
        [SerializeField] TriggerEvent OnTriggerExit;
        [SerializeField] SliceEventOne OnSliceStart;
        [SerializeField] SliceEventTwo OnSliceEnd;


        private bool reset;
        private int laserLayerIndex;
        private float lerpTime;
        private float currentTime;
        private float flickTimer;
        private float hitAudioTimer;
        private LayerMask laserLayerMask;
        private RaycastHit rHit;
        private Vector3 collStartPoint;
        private Vector3 collEndPoint_1;
        private Vector3 collEndPoint_2;
        private Transform boneTarget;
        private Transform laserTrans;
        private Transform laserBone;
        private Transform rigTrans;
        private Transform origin;
        private LaserCollider laserCollider;



        #region MONO BEHAVIOUR
        void Start()
        {
            // Caching References
            laserTrans = transform.Find("Laser");
            rigTrans = laserTrans.Find("rig");
            laserBone = rigTrans.Find("Bone03");
            origin = rigTrans.Find("origin");
            laserCollider = laserTrans.GetComponent<LaserCollider>();

            // Set layers
            laserLayerMask = LayerMask.GetMask("LaserSword");
            laserLayerIndex = LayerMask.NameToLayer("LaserSword");

            // Registering for trigger events
            laserCollider.OnCollEnter += TriggerEnter;
            laserCollider.OnCollStay += TriggerStay;
            laserCollider.OnCollExit += TriggerExit;

            // Disable
            this.enabled = false;
            laserCollider.Enable = false;
            rigTrans.localScale = Vector3.zero;

            CreateTarget();

            if (enableAtStart)
                Enable();
        }

        void Update()
        {
            UpdateLaser();
            CheckHitAudioTime();
        }

        void FixedUpdate()
        {
            if (!flickering)
                return;

            flickTimer += Time.deltaTime;
            if (flickTimer > flickRate)
            {
                flickTimer = 0f;
                float r = Random.Range(flickMin, 1f);

                rigTrans.localScale = new Vector3(r * 1f, 1f, r * 1f);

                if (laserLight != null)
                    laserLight.intensity = r * lightMultiplier;
            }
        }

#if UNITY_EDITOR
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(collStartPoint, 0.1f);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(collEndPoint_1, 0.1f);
            Gizmos.DrawSphere(collEndPoint_2, 0.1f);
        }
#endif

        void TriggerEnter(Collider coll)
        {
            OnTriggerEnter?.Invoke(coll);

            // Other Laser
            if (coll.gameObject.layer == laserLayerIndex)
            {
                if (audioInstant != null)
                {
                    audioInstant.clip = clipHit;
                    audioInstant.Play();
                }
            }

            // Slice
            if (slicing && coll.gameObject.CompareTag(sliceTag))
            {
                Vector3 laserMiddlePos = 0.5f * (laserTrans.position + origin.position);
                collStartPoint = coll.ClosestPointOnBounds(laserMiddlePos);

                OnSliceStart?.Invoke(collStartPoint);

                if (audioInstant != null)
                {
                    audioInstant.clip = clipHit;
                    audioInstant.Play();
                }
            }
        }

        void TriggerStay(Collider coll)
        {
            OnTriggerStay?.Invoke(coll);

            // Other Laser
            if (coll.gameObject.layer == laserLayerIndex)
            {
                if (Physics.Linecast(laserTrans.position, origin.position, out rHit, laserLayerMask))
                {
                    if (contactLensFlare != null)
                    {
                        if (!contactLensFlare.enabled)
                            contactLensFlare.enabled = true;

                        contactLensFlare.brightness = Random.Range(lensMin, lensMax);
                        contactLensFlare.transform.position = rHit.point;
                    }
                }
            }
        }

        void TriggerExit(Collider coll)
        {
            OnTriggerExit?.Invoke(coll);

            // Other Laser
            if (coll.gameObject.layer == laserLayerIndex)
            {
                if (contactLensFlare != null)
                    contactLensFlare.enabled = false;
            }

            // Slice
            if (slicing && coll.gameObject.CompareTag(sliceTag))
            {
                collEndPoint_1 = laserTrans.position;
                collEndPoint_2 = origin.position;
                Plane plane = new Plane(collStartPoint, collEndPoint_1, collEndPoint_2);
                Vector3 center = (collStartPoint + collEndPoint_1 + collEndPoint_2) / 3;

                Material mat = slicedMaterial;
                if (mat == null)
                    mat = coll.GetComponent<Renderer>().material;

                // Slice and get pieces
                GameObject[] gos = MeshSlicer.CutIntoPieces(coll.gameObject, center, plane.normal, mat);

                // Set up Layer and Tag
                gos[0].gameObject.layer = coll.gameObject.layer;
                gos[1].gameObject.layer = coll.gameObject.layer;
                if (sliceablePieces)
                {
                    gos[0].gameObject.tag = coll.gameObject.tag;
                    gos[1].gameObject.tag = coll.gameObject.tag;
                }
                else
                {
                    gos[0].gameObject.tag = "Untagged";
                    gos[1].gameObject.tag = "Untagged";
                }

                // Attach RigidBodies
                if (attachRigidBodies)
                {
                    if (!gos[0].GetComponent<Rigidbody>())
                        gos[0].AddComponent<Rigidbody>();
                    if (!gos[1].GetComponent<Rigidbody>())
                        gos[1].AddComponent<Rigidbody>();
                }

                // Attach Mesh Colliders
                if (attachColliders)
                {
                    MeshCollider mc1 = gos[0].AddComponent<MeshCollider>();
                    MeshCollider mc2 = gos[1].AddComponent<MeshCollider>();
                    mc1.convex = true;
                    mc2.convex = true;
                    if (slicedPhysicMaterial != null)
                        mc1.material = mc2.material = slicedPhysicMaterial;
                }

                OnSliceEnd?.Invoke(gos[0], gos[1]);

                Destroy(coll);
            }
        }
        #endregion


        /// <summary>
        /// Enable the laser.
        /// </summary>
        public void Enable()
        {
            if (this.enabled)
                return;

            this.enabled = true;

            laserCollider.Enable = true;
            boneTarget.position = origin.position;

            StartCoroutine(IE_ScaleLaser(laserShowTime, curveScaleUp));

            hitAudioTimer = 0;


            #region EFFECTS

            if (audioIdle != null)
                audioIdle.Play();
            if (audioInstant != null)
            {
                audioInstant.clip = clipShowHide;
                audioInstant.Play();
            }

            if (laserLight != null)
                laserLight.enabled = true;
            if (lightning_particle != null && lightning_particle.gameObject.activeSelf == true)
                lightning_particle.Play();
            if (smoke_particle != null && smoke_particle.gameObject.activeSelf == true)
                smoke_particle.Play();

            #endregion
        }

        /// <summary>
        /// Disable the laser.
        /// </summary>
        public void Disable()
        {
            if (!this.enabled)
                return;

            this.enabled = false;

            laserCollider.Enable = false;

            StartCoroutine(IE_ScaleLaser(laserHideTime, curveScaleDown));


            #region EFFECTS

            if (audioIdle != null)
                audioIdle.Stop();
            if (audioInstant != null)
            {
                audioInstant.clip = clipShowHide;
                audioInstant.Play();
            }

            if (laserLight != null)
                laserLight.enabled = false;

            if (lightning_particle != null && lightning_particle.gameObject.activeSelf == true)
                lightning_particle.Stop();

            if (smoke_particle != null && smoke_particle.gameObject.activeSelf == true)
                smoke_particle.Stop();

            if (contactLensFlare != null)
                contactLensFlare.enabled = false;

            #endregion
        }

        private void UpdateLaser()
        {
            if (!boneTarget)
                return;

            boneTarget.position = Vector3.Lerp(boneTarget.position, origin.position, lerpTime);
            if (Vector3.Distance(boneTarget.position, origin.position) > 0.1f)
            {
                reset = false;
                currentTime = 0;
            }

            if (!reset)
            {
                laserBone.right = laserBone.position - boneTarget.position;
                lerpTime = Time.deltaTime * laserFadeSpeed;
            }
            else
            {
                laserBone.localEulerAngles = new Vector3(0, 0, 269.7f);
            }

            currentTime += Time.deltaTime;
            if (currentTime > lerpTime - 0.1f)
                reset = true;
        }

        private void CreateTarget()
        {
            GameObject go = new GameObject("_laser_point");
            boneTarget = go.transform;
            boneTarget.position = origin.position;

            laserBone.localEulerAngles = origin.localEulerAngles;
            boneTarget.eulerAngles = origin.eulerAngles;
        }

        private void CheckHitAudioTime()
        {
            hitAudioTimer += Time.deltaTime;
            if (hitAudioTimer > hitSoundCooldown)
                if (Vector3.Distance(boneTarget.position, origin.position) > 0.7f)
                {
                    hitAudioTimer = 0;

                    if (audioInstant != null)
                    {
                        audioInstant.clip = clipMove;
                        audioInstant.Play();
                    }
                }
        }

        private IEnumerator IE_ScaleLaser(float duration, AnimationCurve curve)
        {
            float timer = 0f;
            while (timer < duration)
            {
                timer += Time.deltaTime;
                timer = Mathf.Clamp(timer, 0f, duration);
                float percent = timer / duration;
                float value = curve.Evaluate(percent);
                rigTrans.localScale = Vector3.one * value;

                yield return null;
            }
        }
    }

}