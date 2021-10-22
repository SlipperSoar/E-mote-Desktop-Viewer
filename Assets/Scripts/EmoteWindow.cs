using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;
using UnityEngine.UI;

public class EmoteWindow : MonoBehaviour
{
    #region inner

    /// <summary>
    /// 一共可选择三种样式
    /// </summary>
    public enum WindowStyle
    {
        /// <summary>
        /// 置顶
        /// </summary>
        WinTop,
        /// <summary>
        /// 置顶并且透明
        /// </summary>
        WinTopAlpha,
        /// <summary>
        /// 置顶透明并且可以穿透
        /// </summary>
        WinTopAlphaPenetrate
    }

    #endregion

    [SerializeField] private Material m_Material;
    [SerializeField] private WindowStyle style;

    float pressTime;
    bool mouseButtonDown;
    private IntPtr Handle => WinAPI.ThisWindowHandle;

    void Start()
    {
        mouseButtonDown = false;
        pressTime = 0f;
#if !UNITY_EDITOR && UNITY_STANDALONE_WIN
        switch (style)
        {
            case WindowStyle.WinTop:
                WinAPI.RemoveFrame();
                break;
            case WindowStyle.WinTopAlpha:
                WinAPI.RemoveFrameWithAlpha();
                break;
            case WindowStyle.WinTopAlphaPenetrate:
                WinAPI.RemoveFrameWithAlpha();
                WinAPI.PenetrateWindow();
                break;
        }
#endif
    }

    void Update()
    {
#if UNITY_STANDALONE_WIN
        if (Input.GetMouseButtonDown(0))
        {
            pressTime = 0f;
            mouseButtonDown = true;
            return;
        }
        if (mouseButtonDown && pressTime >= 0.01f)
        {
            //这样做为了区分界面上面其它需要滑动的操作
            WinAPI.ReleaseCapture();
            WinAPI.SendMessage(Handle, 0xA1, 0x02, 0);
            WinAPI.SendMessage(Handle, 0x0202, 0, 0);
        }
        if (mouseButtonDown)
        {
            pressTime += Time.deltaTime;
        }
        if (Input.GetMouseButtonUp(0))
        {
            pressTime = 0f;
            mouseButtonDown = false;
        }
#endif
    }

    // Pass the output of the camera to the custom material
    // for chroma replacement
    void OnRenderImage(RenderTexture from, RenderTexture to)
    {
        Graphics.Blit(from, to, m_Material);
    }
}
