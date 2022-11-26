using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FOV3D
{
    [ExecuteInEditMode]
    public class FieldOfView3D : MonoBehaviour
    {
        #region Variables
        [Range(0f, 50f)] public float viewRadius = 5f;
        [Range(0f, 180f)] public float viewAngle = 30f;
        [Range(1f, 2500f)] public int viewResolution = 1000;

        public LayerMask layerMask = 1;
        public enum DetectionType
        {
            Raycast,
            Linecast,
            Spherecast
        }
        public DetectionType detectionType;

        [Range(0.1f, 1f)] public float sphereCastRadius = 0.1f;

        public List<GameObject> seenObjects = new List<GameObject>();

        public List<GameObject> targetObjects = new List<GameObject>();
        public UnityEvent onTargetSeen;
        public UnityEvent onTargetLost;

        [SerializeField] private bool detectionActive = true;

        private bool m_goldenRatio = true;
        [Range(0f, 2f)] private float turnFraction;
        private float power = 1;

        private FOVVisualizer fovV;

        [HideInInspector] public List<Vector3> m_directions = new List<Vector3>();
        [HideInInspector] public List<Vector3> m_point = new List<Vector3>();
        [HideInInspector] public List<Vector3> spherePoints = new List<Vector3>();
        [HideInInspector] public List<int> hitIndexs = new List<int>();
        [HideInInspector] public List<int> missIndexs = new List<int>();
        [HideInInspector] public int hitIndexCount;
        [HideInInspector] public int missIndexCount;
        private float goldenRatio = (1 + Mathf.Sqrt(5)) / 2;
        private bool tempbool = false;
        #endregion
        private void Awake()
        {
            m_directions = new List<Vector3>(viewResolution);
            seenObjects = new List<GameObject>();
            m_point = new List<Vector3>();
            tempbool = false;
        }
        private void Update()
        {
            if (m_directions.Count != viewResolution)
                StartCoroutine(ListSetup(m_directions, viewResolution));

            if (m_goldenRatio) turnFraction = goldenRatio;
            float angleIncrement = Mathf.PI * 2 * turnFraction;
            float radians = viewAngle * Mathf.Deg2Rad;
            float c = -1 * Mathf.Cos(radians) + 1;

            Vector3 rot = transform.rotation.eulerAngles;
            rot = new Vector3(rot.x + 270, rot.y, 180);
            Quaternion myRotation = Quaternion.Euler(rot);

            for (int i = 0; i < viewResolution; i++)
            {
                float t = (float)i / viewResolution;
                float inclination = Mathf.Acos(1 - c * t);
                inclination = Mathf.Pow(inclination, power);
                float azimuth = angleIncrement * i;

                float x = Mathf.Sin(inclination) * Mathf.Cos(azimuth) * viewRadius;
                float y = Mathf.Sin(inclination) * Mathf.Sin(azimuth) * viewRadius;
                float z = Mathf.Cos(inclination) * viewRadius;

                Vector3 endPoint = new Vector3(x, z, y);
                endPoint = myRotation * endPoint;

                m_directions[i] = endPoint;

                if (detectionActive)
                {
                    RaycastHit hit;
                    Vector3 dir = (m_directions[i]).normalized;

                    if (i == 0)
                    {
                        hitIndexCount = 0;
                        missIndexCount = 0;
                        hitIndexs.Clear();
                        missIndexs.Clear();
                        spherePoints.Clear();
                    }                    
                    switch (detectionType)
                    {
                        case DetectionType.Raycast:
                            if (Physics.Raycast(transform.position, dir, out hit, viewRadius, layerMask))
                                Detection(hit, i);
                            break;
                        case DetectionType.Linecast:
                            if (Physics.Linecast(transform.position, endPoint + transform.position, out hit, layerMask))
                                Detection(hit, i);
                            break;
                        case DetectionType.Spherecast:
                            if (Physics.SphereCast(transform.position, sphereCastRadius, dir, out hit, viewRadius - sphereCastRadius, layerMask))
                            {                                
                                hitIndexs.Add(new int());
                                hitIndexs[hitIndexCount] = i;
                                spherePoints.Add(new Vector3());
                                spherePoints[hitIndexCount] = SphereCase(hit, i);
                                hitIndexCount++;
                                Detection(hit, i);
                            }
                            else
                            {
                                missIndexs.Add(new int());
                                missIndexs[missIndexCount] = i;
                                missIndexCount++;
                            }                                                          
                            break;
                    }
                    if ((ValidateVisualizer()) && (fovV.viewAllRaycastLines)) fovV.DrawRaycastLines(i);
                }
            }
        }

        private Vector3 SphereCase(RaycastHit hit, int i)
        {
            Vector3 midPoint = new Vector3();
            if (seenObjects != null)
            {
                Ray r = new Ray(transform.position, m_directions[i]);
                Vector3 a = transform.position;
                Vector3 b = hit.point;
                Vector3 c = r.GetPoint(viewRadius - sphereCastRadius);

                float v1 = Vector3.Dot((c - a), (c - a));
                float v2 = Vector3.Dot((b - a), (c - a));
                float t = v2 / v1;

                midPoint = (a + t * (c - a));       
                return midPoint;
            }
            return midPoint;
        }
        private void Detection(RaycastHit hit, int i)
        {
            m_directions[i] = hit.point - transform.position;
            GameObject viewObj = hit.collider.gameObject;

            if (!seenObjects.Contains(viewObj))
            {
                seenObjects.Add(viewObj);
                m_point.Add(hit.point);
            }
            else
            {
                if (seenObjects.Count() > 0)
                {
                    int index = seenObjects.IndexOf(viewObj);
                    if (Vector3.Distance(transform.position, hit.point) < viewRadius)
                        m_point[index] = hit.point;
                }
            }
            if (targetObjects.Contains(viewObj))
            {
                if (!tempbool)
                {
                    tempbool = true;
                    StartCoroutine(OnTargetEventTrigger(viewObj));
                }
            }
            if ((ValidateVisualizer()) && (fovV.viewSeenObjectLines)) fovV.DrawObjectLines();
        }
        private void FixedUpdate()
        {
            if (seenObjects.Count > 0)
            {
                for (int j = 0; j < seenObjects.Count; j++)
                {
                    Collider collider = seenObjects[j].GetComponent<Collider>();
                    if (!CheckPointInsideCone(m_point[j], transform.position, transform.forward, viewAngle, viewRadius))
                    {
                        RemoveFromSight(j);
                        break;
                    }
                    else if (!CheckPointInsideCone(collider.bounds.max, transform.position, transform.forward, viewAngle, viewRadius))
                    {
                        if (collider.bounds.SqrDistance(m_point[j]) > .01f)
                        {
                            RemoveFromSight(j);
                            break;
                        }
                    }

                    if (!CheckObstruction(seenObjects[j], m_point[j]))
                        RemoveFromSight(j);
                }
            }
        }
        private void RemoveFromSight(int j)
        {
            if (j >= 0 && j < seenObjects.Count())
                seenObjects.RemoveAt(j);
            if (j >= 0 && j < m_point.Count())
                m_point.RemoveAt(j);
            if (detectionType == DetectionType.Spherecast)
                if (j >= 0 && j < spherePoints.Count())
                    spherePoints.RemoveAt(j);
        }
        private IEnumerator ListSetup(List<Vector3> list, int viewResolution)
        {
            while (list.Count != viewResolution)
            {
                if (list.Count < viewResolution)
                    list.Add(new Vector3());
                if (list.Count > viewResolution)
                    list.RemoveAt(list.Count - 1);
            }
            yield return null;
        }
        bool CheckObstruction(GameObject og, Vector3 point)
        {
            Vector3 dir = (point - transform.position);
            float dis = Vector3.Distance(transform.position, point);
            RaycastHit hitCheck;
            if (Physics.Linecast(transform.position, point, out hitCheck, layerMask))
            {
                GameObject g = hitCheck.collider.gameObject;
                if (og != g)
                    return false;
                else
                    return true;
            }
            else if (Physics.Raycast(transform.position, dir, out hitCheck, dis, layerMask))
            {
                GameObject g = hitCheck.collider.gameObject;
                if (og != g)
                    return false;
                else
                    return true;
            }
            else if (Physics.SphereCast(transform.position, .01f, dir, out hitCheck, dis, layerMask))
            {
                GameObject g = hitCheck.collider.gameObject;
                if (og != g)
                    return false;
                else
                    return true;
            }
            return false;
        }
        bool CheckPointInsideCone(Vector3 point, Vector3 coneOrigin, Vector3 coneDirection, float maxAngle, float maxDistance)
        {
            var distanceToConeOrigin = (point - coneOrigin).magnitude;
            if (distanceToConeOrigin < maxDistance)
            {
                var pointDirection = point - coneOrigin;
                var angle = Vector3.Angle(coneDirection, pointDirection);
                if (angle < maxAngle)
                    return true;
            }
            return false;
        }
        private IEnumerator OnTargetEventTrigger(GameObject target)
        {
            onTargetSeen.Invoke();
            yield return new WaitUntil(() => (!seenObjects.Contains(target)));
            tempbool = false;
            onTargetLost.Invoke();
        }
        private bool ValidateVisualizer()
        {
            if (this.gameObject.TryGetComponent(out FOVVisualizer f))
            {
                fovV = f;
                return true;
            }
            else
                return false;
        }
    }
}