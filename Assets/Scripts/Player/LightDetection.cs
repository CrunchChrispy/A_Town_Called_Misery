using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LightDetection : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("The camera who scans for light.")]
    public Camera m_camLightScan;
    [Tooltip("Show the light value in the log.")]
    public bool m_bLogLightValue = false;
    [Tooltip("Time between light value updates (default = 0.1f).")]
    public float LightValue = 0.1f;

    public /*static*/ float s_fLightValue;

    private const int c_iTextureSize = 1;

    private Texture2D m_texLight;
    private RenderTexture m_texTemp;
    private Rect m_rectLight;
    private Color m_LightPixel;
    public TextMeshProUGUI text;
    private void Start()
    {
        StartLightDetection();
    }

    /// <summary>
    /// Prepare all needed variables and start the light detection coroutine.
    /// </summary>
    private void StartLightDetection()
    {
        m_texLight = new Texture2D(c_iTextureSize, c_iTextureSize, TextureFormat.RGB24, false);
        m_texTemp = new RenderTexture(c_iTextureSize, c_iTextureSize, 24);
        m_rectLight = new Rect(0f, 0f, c_iTextureSize, c_iTextureSize);

        StartCoroutine(LightDetectionUpdate(LightValue));
    }

    /// <summary>
    /// Updates the light value each x seconds.
    /// </summary>
    /// <param name="LightValue">Time in seconds between updates.</param>
    /// <returns></returns>
    private IEnumerator LightDetectionUpdate(float LightValue)
    {
        while (true)
        {
            //Set the target texture of the cam.
            m_camLightScan.targetTexture = m_texTemp;
            //Render into the set target texture.
            m_camLightScan.Render();

            //Set the target texture as the active rendered texture.
            RenderTexture.active = m_texTemp;
            //Read the active rendered texture.
            m_texLight.ReadPixels(m_rectLight, 0, 0);

            //Reset the active rendered texture.
            RenderTexture.active = null;
            //Reset the target texture of the cam.
            m_camLightScan.targetTexture = null;

            //Read the pixel in middle of the texture.
            m_LightPixel = m_texLight.GetPixel(c_iTextureSize / 2, c_iTextureSize / 2);

            //Calculate light value, based on color intensity (from 0f to 1f).
            s_fLightValue = (m_LightPixel.r + m_LightPixel.g + m_LightPixel.b) / 3f;

            if (m_bLogLightValue)
            {
                if (s_fLightValue <= .3f)
                {
                    text.text = "Hidden";
                }
                if (s_fLightValue >= .3f)
                {
                    text.text = "Visible";
                    //Color visibleColor = new Color(0.1f, 0.5f, 0.15f, 1.0f);
                    //text.Color = visibleColor;
                }

                Debug.Log("Light Value: " + s_fLightValue);
            }

            yield return new WaitForSeconds(LightValue);
        }
    }
}