using System;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [Serializable]
    public class MouseLook
    {
        public float XSensitivity = 2f;
        public float YSensitivity = 2f;
        public bool clampVerticalRotation = true;
        public float MinimumX = -90F;
        public float MaximumX = 90F;
        public bool smooth;
        public float smoothTime = 5f;
        public bool lockCursor = true;

        private Quaternion m_CameraTargetRot;
        private bool m_cursorIsLocked = true;

        private float yRot;
        private float xRot;
        private float angleX;

        //custom region settings:
        #region UserSettings

        public bool UsePDA;

        //private bool run_once;
        private bool Leaning;
        private bool LeanL;
        private bool LeanR;

        private Quaternion m_CharacterTargetRot;

        #endregion //end UserSettings

        public void Init(Transform character, Transform camera)
        {
            m_CharacterTargetRot = character.rotation;

            m_CameraTargetRot = camera.localRotation;
        }

        public void LookRotation(Transform character, Transform camera)
        {
            yRot = CrossPlatformInputManager.GetAxis("Mouse X") * XSensitivity;
            xRot = CrossPlatformInputManager.GetAxis("Mouse Y") * YSensitivity;

            if (!Leaning)//disable default character move if player is leaning left or right
                m_CharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
            m_CameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);


            //#user region settings#

            //check whether the player is leaning:
            if (Input.GetKeyUp(KeyCode.Q))
            {
                LeanL = false;
                if (!LeanR) { Leaning = false; }
            }
            else if (Input.GetKeyUp(KeyCode.E))
            {
                LeanR = false;
                if (!LeanL) { Leaning = false; }
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                Leaning = true;
                LeanL = true;
                LeanR = false;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                Leaning = true;
                LeanL = false;
                LeanR = true;
            }

            //prevent camera from entering inside the walls:
            if (Leaning)
            {
                if (LeanL)
                {
                    if (!Physics.Raycast(camera.transform.position, character.transform.TransformDirection(Vector3.forward), 0.5f) &&
                        !Physics.Raycast(camera.transform.position, character.transform.TransformDirection(Vector3.back), 0.5f))
                    {
                        m_CharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
                    }
                    else if (yRot < 0.0f && Physics.Raycast(camera.transform.position, character.transform.TransformDirection(Vector3.forward), 0.5f))
                    {
                        m_CharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
                    }
                    else if (yRot > 0.0f && Physics.Raycast(camera.transform.position, character.transform.TransformDirection(Vector3.back), 0.5f))
                    {
                        m_CharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
                    }
                }
                if (LeanR)
                {
                    if (!Physics.Raycast(camera.transform.position, character.transform.TransformDirection(Vector3.forward), 0.5f) &&
                        !Physics.Raycast(camera.transform.position, character.transform.TransformDirection(Vector3.back), 0.5f))
                    {
                        m_CharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
                    }
                    else if (yRot > 0.0f && Physics.Raycast(camera.transform.position, character.transform.TransformDirection(Vector3.forward), 0.5f))
                    {
                        m_CharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
                    }
                    else if (yRot < 0.0f && Physics.Raycast(camera.transform.position, character.transform.TransformDirection(Vector3.back), 0.5f))
                    {
                        m_CharacterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
                    }
                }
            }

            //#user region settings#


            if (clampVerticalRotation)
                m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot);

            if (smooth)
            {
                character.localRotation = Quaternion.Slerp(character.localRotation, m_CharacterTargetRot,
                    smoothTime * Time.deltaTime);
                camera.localRotation = Quaternion.Slerp(camera.localRotation, m_CameraTargetRot,
                    smoothTime * Time.deltaTime);
            }
            else
            {
                //character.localRotation = m_CharacterTargetRot;
                character.rotation = m_CharacterTargetRot;//USER!
                camera.localRotation = m_CameraTargetRot;
            }

            UpdateCursorLock();
        }

        public void SetCursorLock(bool value)
        {
            lockCursor = value;
            if (!lockCursor)
            {//we force unlock the cursor if the user disable the cursor locking helper
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        public void UpdateCursorLock()
        {
            //if the user set "lockCursor" we check & properly lock the cursos
            if (lockCursor)
                InternalLockUpdate();
        }

        private void InternalLockUpdate()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                m_cursorIsLocked = false;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                m_cursorIsLocked = true;
            }
            //UsePDA is custom feature:
            if (m_cursorIsLocked && !UsePDA)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else if (!m_cursorIsLocked && !UsePDA)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        Quaternion ClampRotationAroundXAxis(Quaternion q)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

            angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);

            q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

            return q;
        }
    }
}
