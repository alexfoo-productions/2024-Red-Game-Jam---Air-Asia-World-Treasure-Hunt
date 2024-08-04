using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Library
{
    public class WebglUtils : MonoBehaviour
    {
        static bool isMobile;

#if !UNITY_EDITOR && UNITY_WEBGL
    [System.Runtime.InteropServices.DllImport("__Internal")]
    static extern bool IsMobile();
#endif

        public static bool CheckIfMobile()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
       isMobile = IsMobile();
       return isMobile;
#endif
            return false;
        }
    }

    /**
     **  CheckIfMobileForUnityWebgl.jslib 

        mergeInto(LibraryManager.library, 
        {
            IsMobile: function () 
            {
                var ua = window.navigator.userAgent.toLowerCase();
                var mobilePattern = /android|iphone|ipad|ipod/i;

                return ua.search(mobilePattern) !== -1 || (ua.indexOf("macintosh") !== -1 && "ontouchend" in document);
            },
        });
     * */
}