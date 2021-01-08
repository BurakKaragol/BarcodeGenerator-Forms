using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IronBarCode;

namespace barcodeGeneratorDeneme
{
    public partial class Form1 : Form
    {
        public enum barkodTipi //tum barkod tiplerini tutmak icin bir enumarator olusturduk
        {
            code128,
            code39,
            code93,
            ean13,
            ean8,
            upcA,
            upcE,
            msi,
            itf14,
            qrCode
        }

        barkodTipi barkodtip; //barkodun tipini tutan bir degisken tanimladik
        string icerik = "12345678901234567890"; //barkodumuzun icerigi
        string gecici = ""; //barkod uzunlugu fazla olursa yedek verinin tutulacagi yer
        int basamak = 1; //icerigin uzunlugunu tutar ve barkodlarin alan kontrolunu bu degisken ile kontrol ederiz
        int baslangic = 0; //qr kod debugging icin cizgilerin baslangic konumu baslangic konumu
        int carpan = 8; //qr kod debugging icin cizgilerin aralarindaki dot cinsinden mesafe
        bool debugMod = false; //qr kod ustune kareli tablo cizer
        bool uyar = false; //yazinin kirmiziya donmesi ve uyari mesajinin gosterilmesini kontrol eder
        int maxX = 300; //secilen tipteki barkodun maksimum genisligi
        int maxY = 100; //secilen tipteki barkodun maksimum yuksekligi
        //string path = @"D:\WORK\printer\monitoring\kodciktisi\barkod.png"; //dosyayi kaydedecegimiz adres
        Image img1; //olusturdugumuz barkodu resim olarak tutup ekrana o sekilde yazdiriyoruz

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true; //flickering sorununu azaltmak icin
            comboBox1.DataSource = Enum.GetValues(typeof(barkodTipi)); //enumerator icerisindeki tipleri comboboxa ekler
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            gecici = icerik;
            textBox1.Text = gecici;
            basamak = icerik.Length;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            float carp = 1; //buyutec orani
            e.Graphics.ScaleTransform(carp, carp); //buyutec
            e.Graphics.FillRectangle(Brushes.LightGray, new Rectangle(0, 0, 56 * 8, 40 * 8)); //etiketi temsil etmesi icin
            secme(e);
            if (debugMod)// eger debug modu aktif ise
            {
                for (int i = 0; i < 34; i++) // 34 er tane
                {
                    e.Graphics.DrawLine(Pens.Red, new Point(baslangic, baslangic + i * carpan), 
                                                  new Point(baslangic + 260, baslangic + i * carpan)); //yatay
                    e.Graphics.DrawString(i.ToString(), DefaultFont, Brushes.Red,
                                                  new Point((baslangic / 2) + 270, (baslangic / 2) + i * carpan));
                    e.Graphics.DrawLine(Pens.Red, new Point(baslangic + i * carpan, baslangic),
                                                  new Point(baslangic + i * carpan, baslangic + 260)); //ve dikey 
                    e.Graphics.DrawString(i.ToString(), DefaultFont, Brushes.Red,
                                                  new Point((baslangic / 2) + (i * carpan), (baslangic / 2) + 270));
                } //cizgi ciz
            }
            e.Graphics.ResetTransform(); //buyutme oranini sifirla
        }
        
        public void basamakAyarla()
        {
            switch (barkodtip) //tum tipler icinden
            {
                case barkodTipi.code128:
                    break;
                case barkodTipi.code39:
                    break;
                case barkodTipi.code93:
                    break;
                case barkodTipi.ean13:
                    gecici = icerik.Substring(0, 12);
                    basamak = gecici.Length;
                    break;
                case barkodTipi.ean8:
                    gecici = icerik.Substring(0, 7);
                    basamak = gecici.Length;
                    break;
                case barkodTipi.upcA:
                    gecici = icerik.Substring(0, 11);
                    basamak = gecici.Length;
                    break;
                case barkodTipi.upcE:
                    gecici = icerik.Substring(0, 6);
                    basamak = gecici.Length;
                    break;
                case barkodTipi.msi:
                    break;
                case barkodTipi.itf14:
                    gecici = icerik.Substring(0, 14);
                    basamak = gecici.Length;
                    break;
                case barkodTipi.qrCode:
                    break;
            }
        }

        public void secme(PaintEventArgs e)
        {
            switch (barkodtip) //tum tipler icinden
            {
                case barkodTipi.code128: //secileni
                    label5.Text = "CODE 128";
                    label5.ForeColor = Color.Black;
                    label6.Visible = false;
                    uyar = false;
                    img1 = BarcodeWriter.CreateBarcode(gecici, BarcodeWriterEncoding.Code128, maxX, maxY).ToImage(); //olustur
                    e.Graphics.DrawImage(img1, new PointF(10f, 10f)); //ve ciz
                    break;
                case barkodTipi.code39:
                    label5.Text = "CODE 39";
                    label5.ForeColor = Color.Black;
                    label6.Visible = false;
                    uyar = false;
                    img1 = BarcodeWriter.CreateBarcode(gecici, BarcodeWriterEncoding.Code39, maxX, maxY).ToImage();
                    e.Graphics.DrawImage(img1, new PointF(10f, 10f));
                    break;
                case barkodTipi.code93:
                    label5.Text = "CODE 93";
                    label5.ForeColor = Color.Black;
                    label6.Visible = false;
                    uyar = false;
                    img1 = BarcodeWriter.CreateBarcode(gecici, BarcodeWriterEncoding.Code93, maxX, maxY).ToImage();
                    e.Graphics.DrawImage(img1, new PointF(10f, 10f));
                    break;
                case barkodTipi.ean13:
                    label5.Text = "EAN 13";
                    label5.ForeColor = Color.Black;
                    label6.Visible = false;
                    if (basamak > 12)
                    {
                        uyar = true;
                        //MessageBox.Show("EAN 13 tipi etikeler maksimum 12 basamaktan olusabilir. Sadece ilk 12 basamak aliniyor..");
                        basamakAyarla();
                    }
                    if (uyar)
                    {
                        label6.Text = "tipi etikeler maksimum 12 basamaktan olusabilir. Sadece ilk 12 basamak aliniyor..";
                        label5.ForeColor = Color.Red;
                        label6.ForeColor = Color.Red;
                        label6.Visible = true;
                    }
                    e.Graphics.ScaleTransform(0.8f, 0.75f);
                    img1 = BarcodeWriter.CreateBarcode(gecici, BarcodeWriterEncoding.EAN13, maxX, maxY).ToImage();
                    e.Graphics.DrawImage(img1, new PointF(10f, 10f));
                    break;
                case barkodTipi.ean8:
                    label5.Text = "EAN 8";
                    label5.ForeColor = Color.Black;
                    label6.Visible = false;
                    if (basamak > 7)
                    {
                        uyar = true;
                        //MessageBox.Show("EAN 8 tipi etikeler maksimum 7 basamaktan olusabilir. Sadece ilk 7 basamak aliniyor..");
                        basamakAyarla();
                    }
                    if (uyar)
                    {
                        label6.Text = "tipi etikeler maksimum 7 basamaktan olusabilir. Sadece ilk 7 basamak aliniyor..";
                        label5.ForeColor = Color.Red;
                        label6.ForeColor = Color.Red;
                        label6.Visible = true;
                    }
                    img1 = BarcodeWriter.CreateBarcode(gecici, BarcodeWriterEncoding.EAN8, maxX, maxY).ToImage();
                    e.Graphics.DrawImage(img1, new PointF(10f, 10f));
                    break;
                case barkodTipi.upcA:
                    label5.Text = "UPC-A";
                    label5.ForeColor = Color.Black;
                    label6.Visible = false;
                    if (basamak > 11)
                    {
                        uyar = true;
                        //MessageBox.Show("UPC-A tipi etikeler maksimum 11 basamaktan olusabilir. Sadece ilk 11 basamak aliniyor..");
                        basamakAyarla();
                    }
                    if (uyar)
                    {
                        label6.Text = "tipi etikeler maksimum 11 basamaktan olusabilir. Sadece ilk 11 basamak aliniyor..";
                        label5.ForeColor = Color.Red;
                        label6.ForeColor = Color.Red;
                        label6.Visible = true;
                    }
                    img1 = BarcodeWriter.CreateBarcode(gecici, BarcodeWriterEncoding.UPCA, maxX, maxY).ToImage();
                    e.Graphics.DrawImage(img1, new PointF(10f, 10f));
                    break;
                case barkodTipi.upcE:
                    label5.Text = "UPC-E";
                    label5.ForeColor = Color.Black;
                    label6.Visible = false;
                    if (basamak > 6)
                    {
                        uyar = true;
                        //MessageBox.Show("UPC-E tipi etikeler maksimum 6 basamaktan olusabilir. Sadece ilk 6 basamak aliniyor..");
                        basamakAyarla();
                    }
                    if (uyar)
                    {
                        label6.Text = "tipi etikeler maksimum 6 basamaktan olusabilir. Sadece ilk 6 basamak aliniyor..";
                        label5.ForeColor = Color.Red;
                        label6.ForeColor = Color.Red;
                        label6.Visible = true;
                    }
                    img1 = BarcodeWriter.CreateBarcode(gecici, BarcodeWriterEncoding.UPCE, maxX, maxY).ToImage();
                    e.Graphics.DrawImage(img1, new PointF(10f, 10f));
                    break;
                case barkodTipi.msi:
                    label5.Text = "MSI";
                    label5.ForeColor = Color.Black;
                    label6.Visible = false;
                    uyar = false;
                    img1 = BarcodeWriter.CreateBarcode(gecici, BarcodeWriterEncoding.MSI, maxX, maxY).ToImage();
                    e.Graphics.DrawImage(img1, new PointF(10f, 10f));
                    break;
                case barkodTipi.itf14:
                    label5.Text = "ITF 14";
                    label5.ForeColor = Color.Black;
                    label6.Visible = false;
                    if (basamak > 14)
                    {
                        uyar = true;
                        //MessageBox.Show("ITF 14 tipi etikeler maksimum 14 basamaktan olusabilir. Sadece ilk 14 basamak aliniyor..");
                        basamakAyarla();
                    }
                    if (uyar)
                    {
                        label6.Text = "tipi etikeler maksimum 14 basamaktan olusabilir. Sadece ilk 14 basamak aliniyor..";
                        label5.ForeColor = Color.Red;
                        label6.ForeColor = Color.Red;
                        label6.Visible = true;
                    }
                    img1 = BarcodeWriter.CreateBarcode(gecici, BarcodeWriterEncoding.ITF, maxX, maxY).ToImage();
                    e.Graphics.DrawImage(img1, new PointF(10f, 10f));
                    break;
                case barkodTipi.qrCode: //qr kod icin ayri komutu kullanarak tolerans ayari yapabiliyoruz
                    label5.Text = "QR CODE";
                    label5.ForeColor = Color.Black;
                    label6.Visible = false;
                    uyar = false;
                    img1 = QRCodeWriter.CreateQrCode(gecici, maxX, QRCodeWriter.QrErrorCorrectionLevel.Low).ToImage();
                    //img1 = BarcodeWriter.CreateBarcode(icerik, BarcodeWriterEncoding.QRCode, maxX, maxY).ToImage();
                    e.Graphics.DrawImage(img1, new PointF(10f, 10f));
                    break;
            }
            label3.Text = img1.Width.ToString(); //resimin genislik 
            label4.Text = img1.Height.ToString(); //ve yuksekligini ekranda sayisall olarak goster
        }

        //debug component
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            carpan = trackBar1.Value;
            maxX = trackBar1.Value * 10;
            label1.Text = carpan.ToString();
            this.Invalidate();
        }

        //debug component
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            baslangic = trackBar2.Value;
            if (trackBar2.Value > 1)
            {
                maxY = trackBar2.Value * 10;
            }
            label2.Text = baslangic.ToString();
            this.Invalidate();
        }

        //debug component
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            debugMod = checkBox1.Checked;
            this.Invalidate();
        }

        //barkod tipini secmemizi saglar
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            basamak = icerik.Length;
            gecici = icerik;
            barkodtip = (barkodTipi)comboBox1.SelectedItem;
            uyar = false;
            this.Invalidate();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "" || textBox1.Text != null)
            {
                icerik = textBox1.Text;
                gecici = icerik;
                this.Invalidate();
            }
        }
    }
}
