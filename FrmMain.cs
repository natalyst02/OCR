using System;
using System.Collections.Generic;

using System.Drawing;

using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;

using Emgu.CV.UI;
using Emgu.Util;

using Emgu.CV.CvEnum;
using System.IO;

using tesseract;


namespace OCRteam
{
    public class mainCalculate
    {
        private static int count = 0;
        private static string _caption = "OCR Team";
        public static int IdentifyContours(
          Bitmap colorImage,
          int thresholdValue,
          bool invert,
          out Bitmap processedGray,
          out Bitmap processedColor,
          out List<Rectangle> list)
        {
            List<Rectangle> rectangleList = new List<Rectangle>();
            Image<Gray, byte> src = new Image<Gray, byte>(colorImage);
            Image<Gray, byte> image1 = new Image<Gray, byte>(src.Width, src.Height);
            Image<Bgr, byte> image2 = new Image<Bgr, byte>(colorImage);
            double num1 = mainCalculate.cout_avg_new(src);
            if (num1 == 0.0)
                num1 = src.GetAverage().Intensity;
            double num2 = 0.1;
            double num3 = 10.0;
            Rectangle[] array = new Rectangle[9];
            Image<Bgr, byte> image3 = new Image<Bgr, byte>(colorImage);
            Image<Gray, byte> image4 = src;
            Image<Gray, byte> image5 = image1;
            int num4 = 0;
            for (int index1 = 2; index1 < 10; ++index1)
            {
                for (double num5 = num2; num5 <= num3; num5 += 0.1)
                {
                    Image<Bgr, byte> img = new Image<Bgr, byte>(colorImage);
                    Image<Gray, byte> image6 = image1;
                    rectangleList.Clear();
                    int num6 = 0;
                    double intensity = num1 / num5;
                    Image<Gray, byte> image7 = src.ThresholdBinary(new Gray(intensity), new Gray((double)byte.MaxValue));
                    using (MemStorage stor = new MemStorage())
                    {
                        for (Contour<Point> contour = image7.FindContours(CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE, RETR_TYPE.CV_RETR_LIST, stor); contour != null; contour = contour.HNext)
                        {
                            Rectangle boundingRectangle = contour.BoundingRectangle;
                            CvInvoke.cvDrawContours((IntPtr)(UnmanagedObject)img, (IntPtr)(Seq<Point>)contour, new MCvScalar((double)byte.MaxValue, (double)byte.MaxValue, 0.0), new MCvScalar(0.0), -1, 1, LINE_TYPE.EIGHT_CONNECTED, new Point(0, 0));
                            double num7 = (double)boundingRectangle.Width / (double)boundingRectangle.Height;
                            if (boundingRectangle.Width > 20 && boundingRectangle.Width < 150 && boundingRectangle.Height > 80 && boundingRectangle.Height < 150 && num7 > 0.2 && num7 < 1.1)
                            {
                                ++num6;
                                CvInvoke.cvDrawContours((IntPtr)(UnmanagedObject)img, (IntPtr)(Seq<Point>)contour, new MCvScalar(0.0, (double)byte.MaxValue, (double)byte.MaxValue), new MCvScalar((double)byte.MaxValue), -1, 3, LINE_TYPE.EIGHT_CONNECTED, new Point(0, 0));
                                img.Draw(contour.BoundingRectangle, new Bgr(0.0, (double)byte.MaxValue, 0.0), 2);
                                image6.Draw((Seq<Point>)contour, new Gray((double)byte.MaxValue), -1);
                                rectangleList.Add(contour.BoundingRectangle);
                            }
                        }
                    }
                    double num8 = 0.0;
                    double num9 = 0.0;
                    Rectangle rectangle;
                    for (int index2 = 0; index2 < num6; ++index2)
                    {
                        double num10 = num8;
                        rectangle = rectangleList[index2];
                        double height1 = (double)rectangle.Height;
                        num8 = num10 + height1;
                        for (int index3 = index2 + 1; index3 < num6; ++index3)
                        {
                            rectangle = rectangleList[index3];
                            int x1 = rectangle.X;
                            rectangle = rectangleList[index2];
                            int x2 = rectangle.X;
                            rectangle = rectangleList[index2];
                            int width1 = rectangle.Width;
                            int num11 = x2 + width1;
                            int num12;
                            if (x1 < num11)
                            {
                                rectangle = rectangleList[index3];
                                int x3 = rectangle.X;
                                rectangle = rectangleList[index2];
                                int x4 = rectangle.X;
                                if (x3 > x4)
                                {
                                    rectangle = rectangleList[index3];
                                    int y1 = rectangle.Y;
                                    rectangle = rectangleList[index2];
                                    int y2 = rectangle.Y;
                                    rectangle = rectangleList[index2];
                                    int width2 = rectangle.Width;
                                    int num13 = y2 + width2;
                                    if (y1 < num13)
                                    {
                                        rectangle = rectangleList[index3];
                                        int y3 = rectangle.Y;
                                        rectangle = rectangleList[index2];
                                        int y4 = rectangle.Y;
                                        num12 = y3 <= y4 ? 1 : 0;
                                        goto label_21;
                                    }
                                    else
                                    {
                                        num12 = 1;
                                        goto label_21;
                                    }
                                }
                            }
                            num12 = 1;
                        label_21:
                            if (num12 == 0)
                            {
                                rectangleList.RemoveAt(index3);
                                --num6;
                                --index3;
                            }
                            else
                            {
                                rectangle = rectangleList[index2];
                                int x5 = rectangle.X;
                                rectangle = rectangleList[index3];
                                int x6 = rectangle.X;
                                rectangle = rectangleList[index3];
                                int width3 = rectangle.Width;
                                int num14 = x6 + width3;
                                int num15;
                                if (x5 < num14)
                                {
                                    rectangle = rectangleList[index2];
                                    int x7 = rectangle.X;
                                    rectangle = rectangleList[index3];
                                    int x8 = rectangle.X;
                                    if (x7 > x8)
                                    {
                                        rectangle = rectangleList[index2];
                                        int y5 = rectangle.Y;
                                        rectangle = rectangleList[index3];
                                        int y6 = rectangle.Y;
                                        rectangle = rectangleList[index3];
                                        int width4 = rectangle.Width;
                                        int num16 = y6 + width4;
                                        if (y5 < num16)
                                        {
                                            rectangle = rectangleList[index2];
                                            int y7 = rectangle.Y;
                                            rectangle = rectangleList[index3];
                                            int y8 = rectangle.Y;
                                            num15 = y7 <= y8 ? 1 : 0;
                                            goto label_29;
                                        }
                                        else
                                        {
                                            num15 = 1;
                                            goto label_29;
                                        }
                                    }
                                }
                                num15 = 1;
                            label_29:
                                if (num15 == 0)
                                {
                                    double num17 = num8;
                                    rectangle = rectangleList[index2];
                                    double height2 = (double)rectangle.Height;
                                    num8 = num17 - height2;
                                    rectangleList.RemoveAt(index2);
                                    --num6;
                                    --index2;
                                    break;
                                }
                            }
                        }
                    }
                    double num18 = num8 / (double)num6;
                    for (int index4 = 0; index4 < num6; ++index4)
                    {
                        double num19 = num9;
                        double num20 = num18;
                        rectangle = rectangleList[index4];
                        double height = (double)rectangle.Height;
                        double num21 = Math.Abs(num20 - height);
                        num9 = num19 + num21;
                    }
                    if (num6 <= 8 && num6 > 1 && num6 > num4 && num9 <= (double)(num6 * index1))
                    {
                        rectangleList.CopyTo(array);
                        num4 = num6;
                        image3 = img;
                        image5 = image6;
                        image4 = image7;
                    }
                }
                if (num4 == 8)
                    break;
            }
            mainCalculate.count = num4;
            Image<Gray, byte> image8 = image4;
            Image<Bgr, byte> image9 = image3;
            rectangleList.Clear();
            for (int index = 0; index < array.Length; ++index)
            {
                if (array[index].Height != 0)
                    rectangleList.Add(array[index]);
            }
            processedColor = image9.ToBitmap();
            processedGray = image8.ToBitmap();
            list = rectangleList;
            return mainCalculate.count;
        }

        private static double cout_avg(Image<Gray, byte> src)
        {
            double num = 0.0;
            List<Rectangle> rectangleList = new List<Rectangle>();
            Image<Gray, byte> dst = new Image<Gray, byte>(src.Width, src.Height);
            CvInvoke.cvAdaptiveThreshold((IntPtr)(UnmanagedObject)src, (IntPtr)(UnmanagedObject)dst, (double)byte.MaxValue, ADAPTIVE_THRESHOLD_TYPE.CV_ADAPTIVE_THRESH_MEAN_C, THRESH.CV_THRESH_BINARY, 21, 2.0);
            Image<Gray, byte> image1 = dst.Dilate(3).Erode(3);
            using (MemStorage stor = new MemStorage())
            {
                for (Contour<Point> contour = image1.FindContours(CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE, RETR_TYPE.CV_RETR_LIST, stor); contour != null; contour = contour.HNext)
                {
                    Rectangle boundingRectangle = contour.BoundingRectangle;
                    if (boundingRectangle.Width > 50 && boundingRectangle.Width < 150 && boundingRectangle.Height > 80 && boundingRectangle.Height < 150)
                        rectangleList.Add(boundingRectangle);
                }
            }
            for (int index = 0; index < rectangleList.Count; ++index)
            {
                Bitmap bitmap = src.ToBitmap();
                Image<Gray, byte> image2 = new Image<Gray, byte>(bitmap.Clone(rectangleList[index], bitmap.PixelFormat));
                num += image2.GetAverage().Intensity / (double)rectangleList.Count;
            }
            return num;
        }

        private static double cout_avg_new(Image<Gray, byte> src)
        {
            double num1 = 0.0;
            List<Rectangle> rectangleList = new List<Rectangle>();
            Image<Gray, byte> dst = new Image<Gray, byte>(src.Width, src.Height);
            CvInvoke.cvAdaptiveThreshold((IntPtr)(UnmanagedObject)src, (IntPtr)(UnmanagedObject)dst, (double)byte.MaxValue, ADAPTIVE_THRESHOLD_TYPE.CV_ADAPTIVE_THRESH_MEAN_C, THRESH.CV_THRESH_BINARY, 21, 2.0);
            Image<Gray, byte> image1 = dst.Dilate(3).Erode(3);
            using (MemStorage stor = new MemStorage())
            {
                for (Contour<Point> contour = image1.FindContours(CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE, RETR_TYPE.CV_RETR_LIST, stor); contour != null; contour = contour.HNext)
                {
                    Rectangle boundingRectangle = contour.BoundingRectangle;
                    if (boundingRectangle.Width > 50 && boundingRectangle.Width < 150 && boundingRectangle.Height > 80 && boundingRectangle.Height < 150)
                        rectangleList.Add(boundingRectangle);
                }
            }
            try
            {
                for (int index1 = 0; index1 < rectangleList.Count; ++index1)
                {
                    Bitmap bitmap = src.ToBitmap();
                    Image<Gray, byte> image2 = new Image<Gray, byte>(bitmap.Clone(rectangleList[index1], bitmap.PixelFormat));
                    int num2 = 128;
                    int num3;
                    do
                    {
                        num3 = num2;
                        int num4 = 0;
                        int num5 = 0;
                        int num6 = 0;
                        int num7 = 0;
                        for (int index2 = 0; index2 < image2.Rows; ++index2)
                        {
                            for (int index3 = 0; index3 < image2.Cols; ++index3)
                            {
                                int num8 = (int)image2.Data[index2, index3, 0];
                                if (num8 <= num3)
                                {
                                    ++num4;
                                    num6 += num8;
                                }
                                else
                                {
                                    ++num5;
                                    num7 += num8;
                                }
                            }
                        }
                        num2 = (num6 / num4 + num7 / num5) / 2;
                    }
                    while (num3 - num2 > 1 || num2 - num3 > 1);
                    num1 += (double)num2 / (double)rectangleList.Count;
                }
            }
            catch (Exception ex)
            {
                int num9 = (int)MessageBox.Show("Lỗi: " + ex.Message, mainCalculate._caption);
            }
            return num1;
        }

        private Image<Gray, byte> search(
          double thr,
          Image<Gray, byte> grayImage,
          double min,
          double max,
          out List<Rectangle> list_out,
          out int count,
          Image<Bgr, byte> color,
          out Image<Bgr, byte> color_out,
          Image<Gray, byte> bi,
          out Image<Gray, byte> bi_out)
        {
            List<Rectangle> rectangleList1 = (List<Rectangle>)null;
            List<Rectangle> rectangleList2 = (List<Rectangle>)null;
            Image<Bgr, byte> img = color;
            Image<Gray, byte> image1 = grayImage;
            Image<Gray, byte> image2 = bi;
            int num1 = 0;
            int num2 = 0;
            for (double num3 = min; num3 <= max; num3 += 0.1)
            {
                double intensity = thr / num3;
                image1 = grayImage.ThresholdBinary(new Gray(intensity), new Gray((double)byte.MaxValue));
                using (MemStorage stor = new MemStorage())
                {
                    for (Contour<Point> contour = image1.FindContours(CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE, RETR_TYPE.CV_RETR_LIST, stor); contour != null; contour = contour.HNext)
                    {
                        Rectangle boundingRectangle = contour.BoundingRectangle;
                        CvInvoke.cvDrawContours((IntPtr)(UnmanagedObject)img, (IntPtr)(Seq<Point>)contour, new MCvScalar((double)byte.MaxValue, (double)byte.MaxValue, 0.0), new MCvScalar(0.0), -1, 1, LINE_TYPE.EIGHT_CONNECTED, new Point(0, 0));
                        if (boundingRectangle.Width > 20 && boundingRectangle.Width < 150 && boundingRectangle.Height > 80 && boundingRectangle.Height < 150)
                        {
                            ++num1;
                            CvInvoke.cvDrawContours((IntPtr)(UnmanagedObject)img, (IntPtr)(Seq<Point>)contour, new MCvScalar(0.0, (double)byte.MaxValue, (double)byte.MaxValue), new MCvScalar((double)byte.MaxValue), -1, 3, LINE_TYPE.EIGHT_CONNECTED, new Point(0, 0));
                            img.Draw(contour.BoundingRectangle, new Bgr(0.0, (double)byte.MaxValue, 0.0), 2);
                            image2.Draw((Seq<Point>)contour, new Gray((double)byte.MaxValue), -1);
                            rectangleList1.Add(contour.BoundingRectangle);
                        }
                    }
                    for (int index1 = 0; index1 < num1; ++index1)
                    {
                        for (int index2 = index1 + 1; index2 < num1; ++index2)
                        {
                            int x1 = rectangleList1[index2].X;
                            int x2 = rectangleList1[index1].X;
                            Rectangle rectangle = rectangleList1[index1];
                            int width1 = rectangle.Width;
                            int num4 = x2 + width1;
                            int num5;
                            if (x1 < num4)
                            {
                                rectangle = rectangleList1[index2];
                                int x3 = rectangle.X;
                                rectangle = rectangleList1[index1];
                                int x4 = rectangle.X;
                                if (x3 > x4)
                                {
                                    rectangle = rectangleList1[index2];
                                    int y1 = rectangle.Y;
                                    rectangle = rectangleList1[index1];
                                    int y2 = rectangle.Y;
                                    rectangle = rectangleList1[index1];
                                    int width2 = rectangle.Width;
                                    int num6 = y2 + width2;
                                    if (y1 < num6)
                                    {
                                        rectangle = rectangleList1[index2];
                                        int y3 = rectangle.Y;
                                        rectangle = rectangleList1[index1];
                                        int y4 = rectangle.Y;
                                        num5 = y3 <= y4 ? 1 : 0;
                                        goto label_15;
                                    }
                                    else
                                    {
                                        num5 = 1;
                                        goto label_15;
                                    }
                                }
                            }
                            num5 = 1;
                        label_15:
                            if (num5 == 0)
                            {
                                rectangleList1.RemoveAt(index2);
                                --num1;
                                --index2;
                            }
                            else
                            {
                                rectangle = rectangleList1[index1];
                                int x5 = rectangle.X;
                                rectangle = rectangleList1[index2];
                                int x6 = rectangle.X;
                                rectangle = rectangleList1[index2];
                                int width3 = rectangle.Width;
                                int num7 = x6 + width3;
                                int num8;
                                if (x5 < num7)
                                {
                                    rectangle = rectangleList1[index1];
                                    int x7 = rectangle.X;
                                    rectangle = rectangleList1[index2];
                                    int x8 = rectangle.X;
                                    if (x7 > x8)
                                    {
                                        rectangle = rectangleList1[index1];
                                        int y5 = rectangle.Y;
                                        rectangle = rectangleList1[index2];
                                        int y6 = rectangle.Y;
                                        rectangle = rectangleList1[index2];
                                        int width4 = rectangle.Width;
                                        int num9 = y6 + width4;
                                        if (y5 < num9)
                                        {
                                            rectangle = rectangleList1[index1];
                                            int y7 = rectangle.Y;
                                            rectangle = rectangleList1[index2];
                                            int y8 = rectangle.Y;
                                            num8 = y7 <= y8 ? 1 : 0;
                                            goto label_23;
                                        }
                                        else
                                        {
                                            num8 = 1;
                                            goto label_23;
                                        }
                                    }
                                }
                                num8 = 1;
                            label_23:
                                if (num8 == 0)
                                {
                                    rectangleList1.RemoveAt(index1);
                                    --num1;
                                    --index1;
                                    break;
                                }
                            }
                        }
                    }
                }
                if (num1 <= 8 && num1 > num2)
                {
                    rectangleList2 = rectangleList1;
                    num2 = num1;
                    if (num1 == 8)
                    {
                        color_out = img;
                        bi_out = image2;
                        list_out = rectangleList2;
                        count = num2;
                        return image1;
                    }
                }
            }
            color_out = img;
            bi_out = image2;
            list_out = rectangleList2;
            count = num2;
            return image1;
        }

        public static string Ocr(
          Bitmap image_s,
          bool isFull,
          TesseractProcessor full_tesseract,
          TesseractProcessor num_tesseract,
          TesseractProcessor ch_tesseract,
          bool isNum = false)
        {
            Image<Gray, byte> arr = new Image<Gray, byte>(image_s);
            while (true)
            {
                if ((double)CvInvoke.cvCountNonZero((IntPtr)(UnmanagedObject)arr) / (double)(arr.Width * arr.Height) <= 0.5)
                    arr = arr.Dilate(2);
                else
                    break;
            }
            Bitmap bitmap = arr.ToBitmap();
            TesseractProcessor tesseractProcessor = !isFull ? (!isNum ? ch_tesseract : num_tesseract) : full_tesseract;
            int num = 0;
            tesseractProcessor.Clear();
            tesseractProcessor.ClearAdaptiveClassifier();
            string str = tesseractProcessor.Apply((Image)bitmap);
            while (str.Length > 3)
            {
                bitmap = new Image<Gray, byte>(bitmap).Erode(2).ToBitmap();
                tesseractProcessor.Clear();
                tesseractProcessor.ClearAdaptiveClassifier();
                str = tesseractProcessor.Apply((Image)bitmap);
                ++num;
                if (num > 10)
                {
                    str = "";
                    break;
                }
            }
            return str;
        }

        public static void FindLicensePlate(
          Bitmap image,
          PictureBox pictureBox1,
          ImageBox imageBox1,
          List<Image<Bgr, byte>> PlateImagesList,
          Panel panel1)
        {
            Image<Bgr, byte> image1 = new Image<Bgr, byte>(image);
            using (Image<Gray, byte> image2 = new Image<Gray, byte>(image))
            {
                MCvAvgComp[] mcvAvgCompArray = image2.DetectHaarCascade(new HaarCascade(Application.StartupPath + "\\output-hv-33-x25.xml"), 1.1, 8, HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(0, 0))[0];
                if (mcvAvgCompArray.Length == 0)
                {
                    int num = (int)MessageBox.Show("Không thể nhận dạng được biển số xe này!", mainCalculate._caption);
                }
                foreach (MCvAvgComp mcvAvgComp in mcvAvgCompArray)
                {
                    Image<Bgr, byte> image3 = image1.Copy();
                    image3.ROI = mcvAvgComp.rect;
                    image1.Draw(mcvAvgComp.rect, new Bgr(Color.Blue), 2);
                    PlateImagesList.Add(image3.Resize(500, 500, INTER.CV_INTER_CUBIC, true));
                    pictureBox1.Image = (Image)image3.ToBitmap();
                    pictureBox1.Update();
                }
                Image<Bgr, byte> image4 = new Image<Bgr, byte>(image.Size);
                Image<Bgr, byte> image5 = image1.Resize(imageBox1.Width, imageBox1.Height, INTER.CV_INTER_NN);
                imageBox1.Image = (IImage)image5;
                Label label1 = new Label();
                label1.Location = new Point(0, panel1.Height - 15);
                label1.ForeColor = Color.Red;
                Label label2 = label1;
                label2.Text = "";
                label2.Size = new Size(350, 24);
                panel1.Controls.Add((Control)label2);
            }
        }
    }
    public partial class FrmMain : Form
    {
        
        public FrmMain()
        {
            InitializeComponent();
        }

        #region định nghĩa
        List<Image<Bgr, Byte>> PlateImagesList = new List<Image<Bgr, byte>>();
        List<string> PlateTextList = new List<string>();
        List<Rectangle> listRect = new List<Rectangle>();
        PictureBox[] box = new PictureBox[12];

        public TesseractProcessor full_tesseract = null;
        public TesseractProcessor ch_tesseract = null;
        public TesseractProcessor num_tesseract = null;
        private string m_path = Application.StartupPath + @"\data\";
        private List<string> lstimages = new List<string>();
        private const string m_lang = "eng";
		private string _Caption = "OCR";
        #endregion

        private void FrmMain_Load(object sender, EventArgs e)
        {
            full_tesseract = new TesseractProcessor();
            bool succeed = full_tesseract.Init(m_path, m_lang, 3);            
            if (!succeed)
            {
                MessageBox.Show("Lỗi thư viện Tesseract. Chương trình cần kết thúc.",_Caption);
                Application.Exit();
            }
            full_tesseract.SetVariable("tessedit_char_whitelist", "ACDFHKLMNPRSTVXY1234567890").ToString();

            ch_tesseract = new TesseractProcessor();
            succeed = ch_tesseract.Init(m_path, m_lang, 3);
            if (!succeed)
            {
				MessageBox.Show("Lỗi thư viện Tesseract. Chương trình cần kết thúc.",_Caption);
                Application.Exit();
            }
            ch_tesseract.SetVariable("tessedit_char_whitelist", "ACDEFHKLMNPRSTUVXY").ToString();

            num_tesseract = new TesseractProcessor();
            succeed = num_tesseract.Init(m_path, m_lang, 3);
            if (!succeed)
            {
				MessageBox.Show("Lỗi thư viện Tesseract. Chương trình cần kết thúc.",_Caption);
                Application.Exit();
            }
            num_tesseract.SetVariable("tessedit_char_whitelist", "1234567890").ToString();

            System.Environment.CurrentDirectory = System.IO.Path.GetFullPath(m_path);
            for (int i = 0; i < box.Length; i++)
            {
                box[i] = new PictureBox();
            }
            string folder = Application.StartupPath + "\\ImageTest";
            foreach (string fileName in Directory.GetFiles(folder, "*.bmp", SearchOption.TopDirectoryOnly))
            {
                lstimages.Add(Path.GetFullPath(fileName));
            }
            foreach (string fileName in Directory.GetFiles(folder, "*.jpg", SearchOption.TopDirectoryOnly))
            {
                lstimages.Add(Path.GetFullPath(fileName));
            }
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Image (*.bmp; *.jpg; *.jpeg; *.png) |*.bmp; *.jpg; *.jpeg; *.png|All files (*.*)|*.*||";
            dlg.InitialDirectory = Application.StartupPath + "\\ImageTest";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
				 {
					 return;
				 }
            string startupPath = dlg.FileName;
          
            ProcessImage(startupPath);
            if (PlateImagesList.Count != 0)
            {
                Image<Bgr, byte> src = new Image<Bgr, byte>(PlateImagesList[0].ToBitmap());
                Bitmap grayframe;
                Bitmap color;
				int c = mainCalculate.IdentifyContours(src.ToBitmap(), 50, false, out grayframe, out color, out listRect);
                pictureBox1.Image = color;
                pictureBox2.Image = grayframe;
                textBox2.Text = c.ToString();
                Image<Gray, byte> dst = new Image<Gray, byte>(grayframe);
                grayframe = dst.ToBitmap();
                string zz ="";

                // lọc và sắp xếp số
                List<Bitmap> bmp = new List<Bitmap>();
                List<int> erode = new List<int>();
                List<Rectangle> up = new List<Rectangle>();
                List<Rectangle> dow = new List<Rectangle>();
                int up_y = 0, dow_y = 0;
                bool flag_up = false;

                int di = 0;

                if (listRect == null) return;

                for (int i = 0; i < listRect.Count; i++)
                {
                    Bitmap ch = grayframe.Clone(listRect[i], grayframe.PixelFormat);
                    int cou = 0;
                    full_tesseract.Clear();
                    full_tesseract.ClearAdaptiveClassifier();
                    string temp = full_tesseract.Apply(ch);
                    while (temp.Length > 3)
                    {
                        Image<Gray, byte> temp2 = new Image<Gray, byte>(ch);
                        temp2 = temp2.Erode(2);
                        ch = temp2.ToBitmap();
                        full_tesseract.Clear();
                        full_tesseract.ClearAdaptiveClassifier();
                        temp = full_tesseract.Apply(ch);
                        cou++;
                        if (cou > 10)
                        {
                            listRect.RemoveAt(i);
                            i--;
                            di = 0;
                            break;
                        }
                        di = cou;
                    }
                }

                for (int i = 0; i < listRect.Count; i++)
                {
                    for (int j = i; j < listRect.Count; j++)
                    {
                        if (listRect[i].Y > listRect[j].Y + 100)
                        {
                            flag_up = true;
                            up_y = listRect[j].Y;
                            dow_y = listRect[i].Y;
                            break;
                        }
                        else if (listRect[j].Y > listRect[i].Y + 100)
                        {
                            flag_up = true;
                            up_y = listRect[i].Y;
                            dow_y = listRect[j].Y;
                            break;
                        }
                        if (flag_up == true) break;
                    }
                }

                for (int i = 0; i < listRect.Count; i++)
                {
                    if (listRect[i].Y < up_y + 50 && listRect[i].Y > up_y - 50)
                    {
                        up.Add(listRect[i]);
                    }
                    else if (listRect[i].Y < dow_y + 50 && listRect[i].Y > dow_y - 50)
                    {
                        dow.Add(listRect[i]);
                    }
                }

                if (flag_up == false) dow = listRect;

                for (int i = 0; i < up.Count; i++)
                {
                    for (int j = i; j < up.Count; j++)
                    {
                        if (up[i].X > up[j].X)
                        {
                            Rectangle w = up[i];
                            up[i] = up[j];
                            up[j] = w;
                        }
                    }
                }
                for (int i = 0; i < dow.Count; i++)
                {
                    for (int j = i; j < dow.Count; j++)
                    {
                        if (dow[i].X > dow[j].X)
                        {
                            Rectangle w = dow[i];
                            dow[i] = dow[j];
                            dow[j] = w;
                        }
                    }
                }

                int x = 0;
                int c_x = 0;

                for(int i = 0; i<up.Count ;i++)
                {
                    Bitmap ch = grayframe.Clone(up[i],grayframe.PixelFormat);
                    Bitmap o = ch;
                    string temp;
                    if (i < 2)
                    {
						temp = mainCalculate.Ocr(ch, false, full_tesseract, num_tesseract, ch_tesseract, true); // nhan dien so
                    }
                    else
                    {
						temp = mainCalculate.Ocr(ch, false, full_tesseract, num_tesseract, ch_tesseract, false); // nhan dien chu
                    }

                    zz += temp;
                    box[i].Location = new Point(x + i*50, 0);
                    box[i].Size = new Size(50, 100);
                    box[i].SizeMode = PictureBoxSizeMode.StretchImage;
                    box[i].Image = ch;
					panel1.Controls.Add(box[i]);
                    c_x++;
                }
                zz += "\r\n";
                for (int i = 0; i < dow.Count; i++)
                {
                    Bitmap ch = grayframe.Clone(dow[i],grayframe.PixelFormat);
                    string temp = mainCalculate.Ocr(ch,false,full_tesseract,num_tesseract,ch_tesseract,true); // nhan dien so

                    zz += temp;
                    box[i+c_x].Location = new Point(x + i * 50, 100);
                    box[i+c_x].Size = new Size(50, 100);
                    box[i+c_x].SizeMode = PictureBoxSizeMode.StretchImage;
                    box[i+c_x].Image = ch;
					panel1.Controls.Add(box[i + c_x]);
                }
                textBox1.Text = zz;

            }
            
        }

		public void ProcessImage(string urlImage)
		{
			PlateImagesList.Clear();
			PlateTextList.Clear();
			panel1.Controls.Clear();
			Bitmap img = new Bitmap(urlImage);
			pictureBox2.Image = null;
			pictureBox1.Image = img;
			pictureBox1.Update();
			mainCalculate.FindLicensePlate(img, pictureBox1, imageBox1, PlateImagesList,panel1);
		}

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start(linkLabel1.Text);
		}
    }

}
