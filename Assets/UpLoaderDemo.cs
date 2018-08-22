using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace U3DC.Extents
{
    public class UpLoaderDemo : MonoBehaviour
    {
        private Camera CutFrameCamer;
        private Rect _canvas;
        private readonly string m_URL = "http://www.u3dc.com/appupload/upload.php";

        private void Awake()
        {
            CutFrameCamer = Camera.main;
        }

        private string GetInfo()
        {

            var systemInfo = "\tTitle:Current System Back Info：\nDeviceModel：" + SystemInfo.deviceModel + "\nDeviceName：" + SystemInfo.deviceName + "\nDeviceType：" + SystemInfo.deviceType +
                         "\nDeviceUniqueIdentifier：" + SystemInfo.deviceUniqueIdentifier + "\nGraphicsDeviceID：" + SystemInfo.graphicsDeviceID +
                         "\nGraphicsDeviceName：" + SystemInfo.graphicsDeviceName + "\nGraphicsDeviceVendor：" + SystemInfo.graphicsDeviceVendor +
                         "\nGraphicsDeviceVendorID:" + SystemInfo.graphicsDeviceVendorID + "\nGraphicsDeviceVersion:" + SystemInfo.graphicsDeviceVersion +
                         "\nGraphicsMemorySize（M）：" + SystemInfo.graphicsMemorySize +
                         "\nGraphicsShaderLevel：" + SystemInfo.graphicsShaderLevel + "\nMaxTextureSize：" + SystemInfo.maxTextureSize +
                         "\nnpotSupport：" + SystemInfo.npotSupport + "\nOperatingSystem：" + SystemInfo.operatingSystem +
                         "\nProcessorCount：" + SystemInfo.processorCount + "\nProcessorType：" + SystemInfo.processorType +
                         "\nsupportedRenderTargetCount：" + SystemInfo.supportedRenderTargetCount + "\nsupports3DTextures：" + SystemInfo.supports3DTextures +
                         "\nsupportsAccelerometer：" + SystemInfo.supportsAccelerometer + "\nsupportsComputeShaders：" + SystemInfo.supportsComputeShaders +
                         "\nsupportsGyroscope：" + SystemInfo.supportsGyroscope + "\nsupportsImageEffects：" + SystemInfo.supportsImageEffects +
                         "\nsupportsInstancing：" + SystemInfo.supportsInstancing + "\nsupportsLocationService：" + SystemInfo.supportsLocationService +
                          "\nsupportsRenderToCubemap：" + SystemInfo.supportsRenderToCubemap +
                         "\nsupportsShadows：" + SystemInfo.supportsShadows + "\nsupportsSparseTextures：" + SystemInfo.supportsSparseTextures +
                         "\nsupportsVibration：" + SystemInfo.supportsVibration + "\nSystemMemorySize：" + SystemInfo.systemMemorySize;
            return systemInfo;

        }



        public void UpLoadLogInfo()
        {
            SaveStringToBinary(GetInfo());
            StartCoroutine(UploadLogFile(m_URL));
        }

        public void UpLoadSreenShot()
        {
            _canvas.Set(0, 0, Screen.width, Screen.height); //设置画布大小等于当前屏幕的宽和高。
            CaptureScreen(CutFrameCamer, _canvas);

        }


        /// <summary>
        /// 截图上传到服务器
        /// </summary>
        /// <param name="c"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public void CaptureScreen(Camera c, Rect r)
        {
            //捕抓摄像机图像并转换成字符数组
            var rt = new RenderTexture((int)r.width, (int)r.height, 0);
            c.targetTexture = rt;
            c.Render();

            RenderTexture.active = rt;
            var screenShot = new Texture2D((int)r.width, (int)r.height, TextureFormat.RGB24, false);
            screenShot.ReadPixels(r, 0, 0);
            screenShot.Apply();

            c.targetTexture = null;
            RenderTexture.active = null;
            GameObject.Destroy(rt);

            StartCoroutine(UploadTexture(screenShot, m_URL));

        }

        private IEnumerator UploadTexture(Texture2D screenShot, string Url)
        {
            //var encoder = new JPGEncoder(screenShot, 20);//质量1~100
            //encoder.doEncoding();
            //while (!encoder.isDone)
            //    yield return null;
            //var bytes = encoder.GetBytes();

            var bytes = screenShot.EncodeToJPG();
            var form = new WWWForm();
            // form.AddBinaryData("file", bytes); //把图片流上传
            form.AddBinaryData("file", bytes, "screenShot.jpg", "image/jpeg");
            var www = new WWW(Url, form);
            yield return www;
            if (www.error == null)
                Debug.Log("upload done :" + www.text);
            else
                Debug.Log("Error during upload: " + www.error);
            StartCoroutine(PostData(www)); //启动子线程
            Destroy(screenShot); //销毁
        }

        /// <summary>
        /// 上传log到服务器
        /// </summary>
        /// <param name="str"></param>
        private void SaveStringToBinary(string str)
        {
            //序列化过程（将Save对象转换为字节流）
            //创建Save对象并保存当前游戏状态
            //创建一个二进制格式化程序
            var bf = new BinaryFormatter();
            //创建一个文件流
            var fileStream = File.Create(Application.streamingAssetsPath + "/log.txt");
            //用二进制格式化程序的序列化方法来序列化Save对象,参数：创建的文件流和需要序列化的对象
            bf.Serialize(fileStream, str);
            //关闭流
            fileStream.Close();

            //如果文件存在，则显示保存成功
            if (File.Exists(Application.streamingAssetsPath + "/log.txt"))
            {
                Debug.Log("保存成功~");
            }
        }

        IEnumerator UploadLogFile(string url)
        {
            var localFile = new WWW(Application.streamingAssetsPath + "/log.txt");
            yield return localFile;
            if (localFile.error == null)
                Debug.Log("Loaded file successfully");
            else
            {
                Debug.Log("Open file error: " + localFile.error);
                yield break; // stop the coroutine here
            }
            var postForm = new WWWForm();
            // version 1 上传一个二进制文件 .dat
            postForm.AddBinaryData("file", localFile.bytes);
            // version 2
            //上传一个原格式的文件 源文件后缀
            //postForm.AddBinaryData("file", localFile.bytes,"log.txt", "text/plain");
            var upload = new WWW(url, postForm);
            yield return upload;
            if (upload.error == null)
                Debug.Log("upload done :" + upload.text);
            else
                Debug.Log("Error during upload: " + upload.error);
        }

        IEnumerator PostData(WWW www)
        {
            yield return www;
            Debug.Log(www.text); //输出服务器返回结果。  
        }


    }
}