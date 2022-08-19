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

    public class Convert
    {
        #region Left_Right_Mid

        public static string Left(string text, int textLength)
        {
            string convertText;
            if (text.Length < textLength)
            {
                textLength = text.Length;
            }
            convertText = text.Substring(0, textLength);
            return convertText;
        }

        /// <summary>
        /// ������ ������ textLength ��°������ ���ڿ��� ��ȯ.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="textLength"></param>
        /// <returns></returns>
        public static string Right(string text, int textLength)
        {
            string convertText;
            if (text.Length < textLength)
            {
                textLength = text.Length;
            }
            convertText = text.Substring(text.Length - textLength, textLength);
            return convertText;
        }

        /// <summary>
        /// Mid text�� startpoint�ڿ��� nLength������ ���ڿ��� ��ȯ.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="startPoint"></param>
        /// <param name="nLength"> 0�̸� ������ ��� </param>
        /// <returns></returns>
        public static string Mid(string text, int startPoint, int nLength)
        {
            string sReturn;

            if (startPoint <= text.Length)
            {
                if ((startPoint + nLength) <= text.Length)
                {
                    if (nLength == 0)
                    {
                        int sLength = text.Length - startPoint;
                        sReturn = text.Substring(startPoint, sLength);
                    }
                    else
                    {
                        sReturn = text.Substring(startPoint, nLength);
                    }
                }
                else
                {
                    sReturn = text.Substring(startPoint);
                }
            }
            else
            {
                sReturn = string.Empty;
            }
            return sReturn;
        }

        #endregion

        #region ConvertToStrings
        /// <summary>
        /// List => String[]. �Ű����� <string> , <GameObject.Name>
        /// </summary>
        /// <param name="strList"></param>
        /// <returns></returns>
        public static string[] ConvertToStrings(List<string> strList)
        {
            if (strList.Count > 0)
            {
                string[] strs = new string[strList.Count];
                for (int i = 0; i < strList.Count; i++)
                {
                    strs[i] = strList[i];
                }
                return strs;
            }
            else
            {
                return null;
            }
        }

        public static string[] ConvertToStrings(List<GameObject> gameObjectList)
        {
            if (gameObjectList.Count > 0)
            {
                string[] strs = new string[gameObjectList.Count];
                for (int i = 0; i < gameObjectList.Count; i++)
                {
                    strs[i] = gameObjectList[i].name;
                }
                return strs;
            }
            else
            {
                return null;
            }
        }
        #endregion
    }
}