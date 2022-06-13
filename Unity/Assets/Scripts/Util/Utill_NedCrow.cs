using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NedCrow
{

    public class Escape
    {
        public static void AppQuit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
            Application.OpenURL("https://google.com");
#else
            Application.Quit();
#endif
        }
    }
}