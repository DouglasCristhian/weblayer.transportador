using System;
using Android.Graphics;
using Java.IO;

namespace weblayer.transportador.android.pro.Helpers
{
    public class ByteHelper
    {
        public Android.Graphics.Bitmap ByteArrayToImage(byte[] byteArrayIn)
        {
            Android.Graphics.Bitmap bmp = BitmapFactory.DecodeByteArray(byteArrayIn, 0, byteArrayIn.Length);
            return bmp;
        }

        public byte[] imageToByteArray(Java.Net.URI imageIn, byte[] bytes)
        {
            Java.IO.File file = new Java.IO.File(imageIn);
            FileInputStream fis = new FileInputStream(file);

            ByteArrayOutputStream bos = new ByteArrayOutputStream();
            byte[] buf = new byte[1024];

            try
            {
                for (int readNum; (readNum = fis.Read(buf)) != -1;)
                {
                    bos.Write(buf, 0, readNum);
                }
            }
            catch (Exception)
            {
                throw new Exception();
            }

            bytes = bos.ToByteArray();
            return bytes;
        }
    }
}