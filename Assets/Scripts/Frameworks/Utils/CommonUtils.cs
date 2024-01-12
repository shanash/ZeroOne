using System;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace FluffyDuck.Util
{
    public enum TOAST_BOX_LENGTH
    {
        SHORT = 0,
        LONG
    }

    public static class CommonUtils
    {
        public static bool IsExistInstalledPkg(string pkg_name)
        {
#if UNITY_ANDROID && !UNITY_EDITOR_WIN
        AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject pkgMng = ca.Call<AndroidJavaObject>("getPackageManager");
        AndroidJavaObject appList = pkgMng.Call<AndroidJavaObject>("getInstalledPackages", 0);
        for (int i = 0; i < appList.Call<int>("size"); i++)
        {
            AndroidJavaObject appInfo = appList.Call<AndroidJavaObject>("get", i);

            if (appInfo.Get<string>("packageName") == pkg_name)
            {
                return true;
            }
        }
#endif
            return false;
        }

        public static void CallPackage(string pkg_name)
        {
#if UNITY_ANDROID && !UNITY_EDITOR_WIN
        AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject pkgMng = ca.Call<AndroidJavaObject>("getPackageManager");
        AndroidJavaObject intent = pkgMng.Call<AndroidJavaObject>("getLaunchIntentForPackage", pkg_name);
        ca.Call("startActivity", intent);
#endif
        }

        /// <summary>
        /// subject : "Alchemist"
        /// body : "Game Link + Description"
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        public static void SharedPackage(string subject, string body)
        {
#if UNITY_ANDROID && !UNITY_EDITOR_WIN
        //Refernece of AndroidJavaClass class for intent
        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        //Refernece of AndroidJavaObject class for intent
        AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
        //call setAction method of the Intent object created
        intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
        //set the type of sharing that is happening
        intentObject.Call<AndroidJavaObject>("setType", "text/plain");
        //add data to be passed to the other activity i.e., the data to be sent
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), subject);
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), body);
        //get the current activity
        AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
        //start the activity by sending the intent data
        currentActivity.Call("startActivity", intentObject);
#endif
        }

        public static string RandomUniqueId(int length)
        {
            const string str_pool = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789abcdefghijkmnpqrstuvwxyz";
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                sb.Append(str_pool[UnityEngine.Random.Range(0, str_pool.Length)]);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 한글 영문 체크
        /// </summary>
        /// <param name="en"></param>
        /// <param name="kr"></param>
        /// <returns></returns>
        public static string GetLanguageString(string en, string kr)
        {
            if (Application.systemLanguage == SystemLanguage.Korean)
            {
                return kr;
            }
            else
            {
                return en;
            }
        }

        /// <summary>
        /// 인터넷이 연결되지 않은 상태인지 체크
        /// </summary>
        /// <returns></returns>
        public static bool IsNetworkDisonnection()
        {
            return Application.internetReachability == NetworkReachability.NotReachable;
        }

        public static bool IsNetworkConnectedWifi()
        {
            return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork;
        }

#if UNITY_ANDROID && !UNITY_EDITOR_WIN
    static AndroidJavaClass UnityPlayer = null;
    static AndroidJavaObject ToastBox = null;
#endif

        /// <summary>
        /// TOAST BOX Show
        /// </summary>
        /// <param name="message"></param>
        /// <param name="length"></param>
        public static void ShowToast(string message, TOAST_BOX_LENGTH length)
        {
#if UNITY_ANDROID && !UNITY_EDITOR_WIN
        if (UnityPlayer == null)
        {
            UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        }
        
        AndroidJavaObject currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");

        currentActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
        {
            //  message
            AndroidJavaObject javaMsg = new AndroidJavaObject("java.lang.String", message);

            //  msg show time length
            string msg_legnth = string.Empty;
            if (length == TOAST_BOX_LENGTH.SHORT)
            {
                msg_legnth = "LENGTH_SHORT";
            }
            else if (length == TOAST_BOX_LENGTH.LONG)
            {
                msg_legnth = "LENGTH_LONG";
            }
            else
            {
                msg_legnth = "LENGTH_SHORT";
            }
            //  toast box
            AndroidJavaClass javaToast = new AndroidJavaClass("android.widget.Toast");
            ToastBox = javaToast.CallStatic<AndroidJavaObject>("makeText", context, javaMsg, javaToast.GetStatic<int>(msg_legnth));

            ToastBox.Call("show");
        }));
#else
            Debug.Log(message);
#endif
        }

        /// <summary>
        /// TOAST BOX Hide
        /// </summary>
        public static void HideToast()
        {
#if UNITY_ANDROID && !UNITY_EDITOR_WIN
        if (ToastBox == null)
        {
            return;
        }

        AndroidJavaObject currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        currentActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
        {
            if (ToastBox != null)
            {
                ToastBox.Call("cancel");
            }
        }));
#endif
        }

        public static string ToRGBHex(this Color c)
        {
            return string.Format("#{0:X2}{1:X2}{2:X2}", c.r.ToByte(), c.g.ToByte(), c.b.ToByte());
        }

        /// <summary>
        /// 어플 강제 종료. 
        /// GPGS등 게임내에서 사용하고 있는 프로세스까지 모두 종료.
        /// 화면이 Black out 되듯 종료된다.
        /// Assets/Plugins/Android/quit_helper.aar 파일이 존재해야 함
        /// </summary>
        public static void ApplicationForceQuit()
        {
#if UNITY_ANDROID && !UNITY_EDITOR_WIN
        AndroidJavaClass ajc = new AndroidJavaClass("com.lancekun.quit_helper.AN_QuitHelper");
        AndroidJavaObject UnityInstance = ajc.CallStatic<AndroidJavaObject>("Instance");
        UnityInstance.Call("AN_Exit");
#else
            Application.Quit();
#endif
        }


        /// <summary>
        /// 어플 강제 종료
        /// GPGS등 게임내에서 사용하고 있는 프로세스까지 모두 종료.
        /// 홈키를 누르듯 자연스럽게 종료됨
        /// </summary>
        public static void ApplicationMoveTaskToBackQuit()
        {
#if UNITY_ANDROID && !UNITY_EDITOR_WIN
        using (AndroidJavaClass javaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            AndroidJavaObject unityActivity = javaClass.GetStatic<AndroidJavaObject>("currentActivity");
            unityActivity.Call<bool>("moveTaskToBack", true);
            unityActivity.Call("finish");
        }
#else
            Application.Quit();
#endif
        }

        /// <summary>
        /// 진동 - 1초
        /// </summary>
        public static void Vibrator()
        {
#if UNITY_ANDROID && !UNITY_EDITOR_WIN
            Handheld.Vibrate();
#endif
        }

        /// <summary>
        /// 진동 - 원하는 시간만큼
        /// </summary>
        /// <param name="milliseconds"></param>
        public static void Vibrator(long milliseconds)
        {
#if UNITY_ANDROID && !UNITY_EDITOR_WIN
        AndroidJavaClass AndroidPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject AndroidcurrentActivity = AndroidPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject AndroidVibrator = AndroidcurrentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
        AndroidVibrator.Call("vibrate", milliseconds);
#endif
        }

        /// <summary>
        /// 진동 - 원하는 패턴으로 원하는 횟수만큼
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="repeat"></param>
        public static void Vibrator(long[] pattern, int repeat)
        {

#if UNITY_ANDROID && !UNITY_EDITOR_WIN
        AndroidJavaClass AndroidPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject AndroidcurrentActivity = AndroidPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject AndroidVibrator = AndroidcurrentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
        AndroidVibrator.Call("vibrate", pattern, repeat);
#endif
        }

        /// <summary>
        /// 진동 취소
        /// </summary>
        public static void VibratorCancel()
        {

#if UNITY_ANDROID && !UNITY_EDITOR_WIN
        AndroidJavaClass AndroidPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject AndroidcurrentActivity = AndroidPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject AndroidVibrator = AndroidcurrentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
        AndroidVibrator.Call("cancel");
#endif
        }

        /// <summary>
        /// 문자열 이름으로 클래스 생성하기(테스트 필요)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T CreateClassByString<T>(string name, params object[] args)
        {
            System.Type _type = System.Type.GetType(name);
            object obj = System.Activator.CreateInstance(_type, args);
            T c = (T)obj;
            return c;
        }

        public static T CopyComponent<T>(T original, GameObject destination) where T : Component
        {
            System.Type type = original.GetType();
            var dst = destination.GetComponent(type) as T;
            if (!dst) dst = destination.AddComponent(type) as T;
            var fields = type.GetFields();
            foreach (var field in fields)
            {
                if (field.IsStatic) continue;
                field.SetValue(dst, field.GetValue(original));
            }
            var props = type.GetProperties();
            foreach (var prop in props)
            {
                if (!prop.CanWrite || !prop.CanWrite || prop.Name == "name") continue;
                prop.SetValue(dst, prop.GetValue(original, null), null);
            }
            return dst as T;
        }

        /// <summary>
        /// Addressable Asset 에서 리소스 가져오기.
        /// 호출하는 쪽에서 async / await을 사용해야 함
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static async Task<T> GetResourceFromAddressableAsset<T>(string path)
        {
            var handle = Addressables.LoadAssetAsync<T>(path);
            return await handle.Task;
        }

        /// <summary>
        /// Addressable Asset에서 리소스 가져오기
        /// 호출하는 쪽에서 async 사용할 필요 없이 콜백으로 사용 가능
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="callback"></param>
        public static async void GetResourceFromAddressableAsset<T>(string path, System.Action<T> callback)
        {
            var handle = Addressables.LoadAssetAsync<T>(path);
            var result = await handle.Task;
            callback?.Invoke(result);
        }

        public static class Math
        {
            /// <summary>
            /// 각 구하기
            /// </summary>
            /// <param name="center"></param>
            /// <param name="target"></param>
            /// <returns></returns>
            public static float Angle(Vector2 center, Vector2 target)
            {
                //Vector2 v = target - center;
                //Vector2 v = center - target;
                //float angle = (Mathf.Atan2(v.y, v.x) * 180.0f / Mathf.PI) + 180.0f;
                //return angle;

                Vector2 v2 = target - center;
                float angle = Mathf.Atan2(v2.y, v2.x) * Mathf.Rad2Deg;
                return angle;
            }

            /// <summary>
            /// 각으로 벡터 구하기
            /// </summary>
            /// <param name="angle"></param>
            /// <returns></returns>
            public static Vector2 AngleToVector(float angle)
            {
                float radian = angle * Mathf.Deg2Rad;
                return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
            }

            /// <summary>
            /// 두 벡터간 코사인
            /// </summary>
            /// <param name="a"></param>
            /// <param name="b"></param>
            /// <returns></returns>
            public static float Cos(Vector2 a, Vector2 b)
            {
                float dotProduct = Vector2.Dot(a, b);
                float magnitudeA = a.magnitude;
                float magnitudeB = b.magnitude;

                return dotProduct / (magnitudeA * magnitudeB);
            }

            public static float Sin(Vector2 a, Vector2 b)
            {
                float angleDegrees = Vector2.Angle(a, b);
                float angleRadians = angleDegrees * Mathf.Deg2Rad;
                return Mathf.Sin(angleRadians);
            }
        }
    }
}
