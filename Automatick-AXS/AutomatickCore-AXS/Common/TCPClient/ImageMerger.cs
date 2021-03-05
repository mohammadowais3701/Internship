using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automatick.Core
{
    public class ImageMerger
    {
        static char[] alphabets = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p' };

        public static ImageMerger _imageMerger = null;

        public static ImageMerger ImageMergerInstance
        {
            get
            {
                if (_imageMerger == null)
                {
                    throw new Exception("Image Merger Object not created");
                }
                return _imageMerger;

            }
        }

        public static void createImageMerger()
        {
            _imageMerger = new ImageMerger();
        }

        public byte[] getMergeImage(byte[] captchaImagebytes, string textOnImage, out Bitmap outputImage)
        {
            try
            {
                Bitmap captchaImage = getCaptchaImage(captchaImagebytes);

                if (captchaImage != null)
                {
                    Bitmap imageWithText = CreateBitmapImage(textOnImage, captchaImage.Width);
                    captchaImage = WriteNumberMakeRectangle(captchaImage);
                    //--- create rectangle on loaded captcha image & write numbers on it
                    if (imageWithText != null && captchaImage != null)
                    {
                        outputImage = MergeTwoImages(imageWithText, captchaImage);
                        // string key=Automatick.Core.UniqueKey.getUniqueKey() ;
                        // outputImage.Save("bitmap" + key + ".jpeg", ImageFormat.Jpeg);
                        byte[] outputImagebytes = ToByteArray(outputImage, ImageFormat.Jpeg);
                        //  File.WriteAllBytes("bytes" + key + ".jpeg",outputImagebytes);
                        return outputImagebytes;
                    }
                }

                outputImage = null;
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                outputImage = null;
                return null;
            }

        }

        public static byte[] ToByteArray(Image image, ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, format);
                return ms.ToArray();
            }
        }

        public Bitmap getCaptchaImage(byte[] captchaImagebytes)
        {
            Bitmap captchaImage = null;

            try
            {
                using (MemoryStream ms = new MemoryStream(captchaImagebytes))
                {
                    captchaImage = new Bitmap(ms);
                }
            }
            catch
            {
                captchaImage = null;
            }

            return captchaImage;
        }

        #region For numbers handling
        private Bitmap CreateBitmapImage(string sImageText, int size)
        {
            string imageText = "Select all numbers of " + sImageText.ToUpper() + " below";

            Bitmap objBmpImage = new Bitmap(1, 1);

            int intWidth = 0;
            int intHeight = 0;

            // Create the Font object for the image text drawing.
            Font objFont = new Font("Arial", 13, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);

            // Create a graphics object to measure the text's width and height.
            Graphics objGraphics = Graphics.FromImage(objBmpImage);

            // This is where the bitmap size is determined.
            intWidth = size;//(int)objGraphics.MeasureString(sImageText, objFont).Width;
            intHeight = 25;//(int)objGraphics.MeasureString(sImageText, objFont).Height;

            // Create the bmpImage again with the correct size for the text and font.
            objBmpImage = new Bitmap(objBmpImage, new Size(intWidth, intHeight));

            // Add the colors to the new bitmap.
            objGraphics = Graphics.FromImage(objBmpImage);

            // Set Background color
            objGraphics.Clear(Color.White);
            objGraphics.SmoothingMode = SmoothingMode.AntiAlias;
            objGraphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            objGraphics.DrawString(imageText, objFont, new SolidBrush(Color.FromArgb(102, 102, 102)), 0, 0);
            objGraphics.Flush();

            return (objBmpImage);
        }
        #endregion

        #region Uncomment this to ask typer to enter alphabets
        //private Bitmap CreateBitmapImage(string sImageText, int size)
        //{
        //    string imageText = "Type all Alphabets of " + sImageText.ToUpper() + " below";

        //    Bitmap objBmpImage = new Bitmap(1, 1);

        //    int intWidth = 0;
        //    int intHeight = 0;

        //    // Create the Font object for the image text drawing.
        //    Font objFont = new Font("Arial", 13, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);

        //    // Create a graphics object to measure the text's width and height.
        //    Graphics objGraphics = Graphics.FromImage(objBmpImage);

        //    // This is where the bitmap size is determined.
        //    intWidth = size;//(int)objGraphics.MeasureString(sImageText, objFont).Width;
        //    intHeight = 25;//(int)objGraphics.MeasureString(sImageText, objFont).Height;

        //    // Create the bmpImage again with the correct size for the text and font.
        //    objBmpImage = new Bitmap(objBmpImage, new Size(intWidth, intHeight));

        //    // Add the colors to the new bitmap.
        //    objGraphics = Graphics.FromImage(objBmpImage);

        //    // Set Background color
        //    objGraphics.Clear(Color.White);
        //    objGraphics.SmoothingMode = SmoothingMode.AntiAlias;
        //    objGraphics.TextRenderingHint = TextRenderingHint.AntiAlias;
        //    objGraphics.DrawString(imageText, objFont, new SolidBrush(Color.FromArgb(102, 102, 102)), 0, 0);
        //    objGraphics.Flush();

        //    return (objBmpImage);
        //} 
        #endregion

        public Bitmap makelines(byte[] captchaImagebytes)
        {
            Bitmap captchaImage = getCaptchaImage(captchaImagebytes);

            try
            {
                if (captchaImage.Height > 300)
                {
                    using (Graphics graphicImage = Graphics.FromImage(captchaImage))
                    {
                        graphicImage.SmoothingMode = SmoothingMode.AntiAlias;

                        int[] array = { 0, 150, 300, 450 };
                        Pen opaquePen = new Pen(Color.FromArgb(255, 0, 0, 255), 1);

                        graphicImage.DrawLine(opaquePen, 0, 150, 600, 150);
                        graphicImage.DrawLine(opaquePen, 0, 300, 600, 300);
                        graphicImage.DrawLine(opaquePen, 0, 450, 600, 450);

                        graphicImage.DrawLine(opaquePen, 150, 0, 150, 600);
                        graphicImage.DrawLine(opaquePen, 300, 0, 300, 600);
                        graphicImage.DrawLine(opaquePen, 450, 0, 450, 600);
                    }
                }
            }
            catch
            { }

            return captchaImage;
        }

        public Bitmap WriteNumberMakeRectangle(Bitmap tempImage)
        {
            //Load the Image to be written on.
            Bitmap bitMapImage = tempImage;//new  System.Drawing.Bitmap(Server.MapPath("dallen.jpg"));

            System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Blue);

            //---make rectangle

            using (Graphics graphicImage = Graphics.FromImage(bitMapImage))
            {
                graphicImage.SmoothingMode = SmoothingMode.AntiAlias;

                if (bitMapImage.Height > 300)
                {
                    int[] array = { 0, 150, 300, 450 };
                    Pen opaquePen = new Pen(Color.FromArgb(255, 0, 0, 255), 1);
                    //Pen semiTransPen = new Pen(Color.FromArgb(128, 0, 0, 255), 1);

                    foreach (int i in array)
                    {
                        graphicImage.FillRectangle(myBrush, array[0], i, 32, 30);
                        graphicImage.FillRectangle(myBrush, array[1], i, 32, 30);
                        graphicImage.FillRectangle(myBrush, array[2], i, 32, 30);
                        graphicImage.FillRectangle(myBrush, array[3], i, 32, 30);
                    }

                    graphicImage.DrawLine(opaquePen, 0, 150, 600, 150);
                    graphicImage.DrawLine(opaquePen, 0, 300, 600, 300);
                    graphicImage.DrawLine(opaquePen, 0, 450, 600, 450);

                    graphicImage.DrawLine(opaquePen, 150, 0, 150, 600);
                    graphicImage.DrawLine(opaquePen, 300, 0, 300, 600);
                    graphicImage.DrawLine(opaquePen, 450, 0, 450, 600);

                }
                else
                {
                    int[] array = { 0, 100, 200 };

                    foreach (int i in array)
                    {
                        graphicImage.FillRectangle(myBrush, array[0], i, 20, 30);
                        graphicImage.FillRectangle(myBrush, array[1], i, 20, 30);
                        graphicImage.FillRectangle(myBrush, array[2], i, 20, 30);
                    }
                }
            }

            //-- write numbers
            myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Yellow);

            using (Graphics graphicImage = Graphics.FromImage(bitMapImage))
            {
                //Smooth graphics is nice.
                graphicImage.SmoothingMode = SmoothingMode.AntiAlias;
                //Write your text.

                #region to write alphabets on image
                //if (bitMapImage.Height > 300)
                //{
                //    int[] array = { 0, 150, 300, 450 };

                //    int number = 0;

                //    foreach (int i in array)
                //    {
                //        graphicImage.DrawString(alphabets[number++].ToString(), new Font("Arial", 20, FontStyle.Bold), myBrush, new Point(array[0], i));
                //        graphicImage.DrawString(alphabets[number++].ToString(), new Font("Arial", 20, FontStyle.Bold), myBrush, new Point(array[1], i));
                //        graphicImage.DrawString(alphabets[number++].ToString(), new Font("Arial", 20, FontStyle.Bold), myBrush, new Point(array[2], i));
                //        graphicImage.DrawString(alphabets[number++].ToString(), new Font("Arial", 20, FontStyle.Bold), myBrush, new Point(array[3], i));
                //    }
                //}
                //else
                //{
                //    int[] array = { 0, 100, 200 };

                //    int number = 0;

                //    foreach (int i in array)
                //    {
                //        graphicImage.DrawString(alphabets[number++].ToString(), new Font("Arial", 20, FontStyle.Bold), myBrush, new Point(array[0], i));
                //        graphicImage.DrawString(alphabets[number++].ToString(), new Font("Arial", 20, FontStyle.Bold), myBrush, new Point(array[1], i));
                //        graphicImage.DrawString(alphabets[number++].ToString(), new Font("Arial", 20, FontStyle.Bold), myBrush, new Point(array[2], i));
                //    } 
                //}
                #endregion

                #region To write numbers on image
                if (bitMapImage.Height > 300)
                {
                    int[] array = { 0, 150, 300, 450 };

                    int number = 0;

                    foreach (int i in array)
                    {
                        graphicImage.DrawString(alphabets[number++].ToString(), new Font("Arial", 20, FontStyle.Bold), myBrush, new Point(array[0], i));
                        graphicImage.DrawString(alphabets[number++].ToString(), new Font("Arial", 20, FontStyle.Bold), myBrush, new Point(array[1], i));
                        graphicImage.DrawString(alphabets[number++].ToString(), new Font("Arial", 20, FontStyle.Bold), myBrush, new Point(array[2], i));
                        graphicImage.DrawString(alphabets[number++].ToString(), new Font("Arial", 20, FontStyle.Bold), myBrush, new Point(array[3], i));
                    }
                }
                else
                {
                    int[] array = { 0, 100, 200 };

                    int number = 0;

                    foreach (int i in array)
                    {
                        graphicImage.DrawString((number += 1).ToString(), new Font("Arial", 20, FontStyle.Bold), myBrush, new Point(array[0], i));
                        graphicImage.DrawString((number += 1).ToString(), new Font("Arial", 20, FontStyle.Bold), myBrush, new Point(array[1], i));
                        graphicImage.DrawString((number += 1).ToString(), new Font("Arial", 20, FontStyle.Bold), myBrush, new Point(array[2], i));
                    }
                }
                #endregion

            }

            return bitMapImage;
        }

        #region number label
        ////public Bitmap WriteNumberMakeRectangle(Bitmap tempImage)
        ////{
        ////    //Load the Image to be written on.
        ////    Bitmap bitMapImage = tempImage;//new  System.Drawing.Bitmap(Server.MapPath("dallen.jpg"));

        ////    System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Blue);

        ////    //---make rectangle

        ////    using (Graphics graphicImage = Graphics.FromImage(bitMapImage))
        ////    {
        ////        graphicImage.SmoothingMode = SmoothingMode.AntiAlias;

        ////        if (bitMapImage.Height > 300)
        ////        {
        ////            int[] array = { 0, 150, 300, 450 };
        ////            Pen opaquePen = new Pen(Color.FromArgb(255, 0, 0, 255), 1);
        ////            //Pen semiTransPen = new Pen(Color.FromArgb(128, 0, 0, 255), 1);

        ////            foreach (int i in array)
        ////            {
        ////                graphicImage.FillRectangle(myBrush, array[0], i, 32, 30);
        ////                graphicImage.FillRectangle(myBrush, array[1], i, 32, 30);
        ////                graphicImage.FillRectangle(myBrush, array[2], i, 32, 30);
        ////                graphicImage.FillRectangle(myBrush, array[3], i, 32, 30);
        ////            }

        ////            graphicImage.DrawLine(opaquePen, 0, 150, 600, 150);
        ////            graphicImage.DrawLine(opaquePen, 0, 300, 600, 300);
        ////            graphicImage.DrawLine(opaquePen, 0, 450, 600, 450);

        ////            graphicImage.DrawLine(opaquePen, 150, 0, 150, 600);
        ////            graphicImage.DrawLine(opaquePen, 300, 0, 300, 600);
        ////            graphicImage.DrawLine(opaquePen, 450, 0, 450, 600);

        ////        }
        ////        else
        ////        {
        ////            int[] array = { 0, 100, 200 };

        ////            foreach (int i in array)
        ////            {
        ////                graphicImage.FillRectangle(myBrush, array[0], i, 20, 30);
        ////                graphicImage.FillRectangle(myBrush, array[1], i, 20, 30);
        ////                graphicImage.FillRectangle(myBrush, array[2], i, 20, 30);
        ////            }
        ////        }
        ////    }

        ////    //-- write numbers
        ////    myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Yellow);

        ////    using (Graphics graphicImage = Graphics.FromImage(bitMapImage))
        ////    {
        ////        //Smooth graphics is nice.
        ////        graphicImage.SmoothingMode = SmoothingMode.AntiAlias;
        ////        //Write your text.
        ////        if (bitMapImage.Height > 300)
        ////        {
        ////            int[] array = { 0, 150, 300, 450 };

        ////            int number = 0;

        ////            foreach (int i in array)
        ////            {
        ////                graphicImage.DrawString(number++.ToString(), new Font("Arial", 20, FontStyle.Bold), myBrush, new Point(array[0], i));
        ////                graphicImage.DrawString(number++.ToString(), new Font("Arial", 20, FontStyle.Bold), myBrush, new Point(array[1], i));
        ////                graphicImage.DrawString(number++.ToString(), new Font("Arial", 20, FontStyle.Bold), myBrush, new Point(array[2], i));
        ////                graphicImage.DrawString(number++.ToString(), new Font("Arial", 20, FontStyle.Bold), myBrush, new Point(array[3], i));
        ////            }
        ////        }
        ////        else
        ////        {
        ////            int[] array = { 0, 100, 200 };

        ////            int number = 0;

        ////            foreach (int i in array)
        ////            {
        ////                graphicImage.DrawString(number++.ToString(), new Font("Arial", 20, FontStyle.Bold), myBrush, new Point(array[0], i));
        ////                graphicImage.DrawString(number++.ToString(), new Font("Arial", 20, FontStyle.Bold), myBrush, new Point(array[1], i));
        ////                graphicImage.DrawString(number++.ToString(), new Font("Arial", 20, FontStyle.Bold), myBrush, new Point(array[2], i));
        ////            }
        ////        }

        ////    }

        ////    return bitMapImage;
        ////}
        #endregion

        public static Bitmap MergeTwoImages(Image firstImage, Image secondImage)
        {
            try
            {

                int outputImageWidth = firstImage.Width > secondImage.Width ? firstImage.Width : secondImage.Width;

                int outputImageHeight = firstImage.Height + secondImage.Height + 1;

                Bitmap outputImage = new Bitmap(outputImageWidth, outputImageHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                using (Graphics graphics = Graphics.FromImage(outputImage))
                {
                    graphics.DrawImage(firstImage, new Rectangle(new Point(), firstImage.Size),
                        new Rectangle(new Point(), firstImage.Size), GraphicsUnit.Pixel);
                    graphics.DrawImage(secondImage, new Rectangle(new Point(0, firstImage.Height + 1), secondImage.Size),
                        new Rectangle(new Point(), secondImage.Size), GraphicsUnit.Pixel);
                }

                return outputImage;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
    }
}