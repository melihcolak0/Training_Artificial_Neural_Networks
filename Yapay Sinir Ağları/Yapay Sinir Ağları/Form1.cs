using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExcelDataReader;


namespace Yapay_Sinir_Ağları
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        // Melih Çolak (203302057) Yapay Sinir Ağlarının Eğitilmesi

        // Not: Hocam, Sistem teknik anlamda çalışmaktadır. Giriş-Gizli Katman Arasındaki Ağırlıkları değiştiremedim. Çünkü S(Hn) Listesindeki değerler çok büyük veya çok küçük olduğundan, değer-
        // lere sigmoid aktivasyon fonksiyonu uygulandığında sonuç çoğunlukla 1 veya 0 olmaktadır. Bu sonuçtan dolayı "Her h gizli katman birimi için Sk (Gizli birim)
        // Hata Terimi Hesaplanması" işlemindeki "z4 = z3 * (Convert.ToDouble(koleksiyon4[i13]) * (1 - (Convert.ToDouble(koleksiyon4[i13]))));" adımı sonucu z4 = 0 
        // çıkmaktadır. Bunun sonucunda da Giriş-Gizli Katman Arasındaki Ağırlıkların değerleri sabit kalmaktadır. 
    
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//

        string a;
        string b;
        string c;
        string d;
        string f;
        string g;
        string h;
        string j;
        string m;
        string l;
        string t3;
        double t;
        double t2;
        double u3;
        
        
        double u2;
        double y;
        double y2;
        double y3;
        double y4;
        //double y5;
        //double 76;
        //double y7;
        //double y8;
        //double y9;
        double K;
        double t4;
        double t5;
        double t6;
        double t7;
        double t8;
        double z1;
        double z2;
        double z3 = 0;
        double z4;
        double z5;
        double z6;
        double z7;
        double z8;
        double z9;
        double nrnsys; // Gizli Katmandaki Nöron Sayısı Değişkeni (Kullanıcıdan alınır.)
        int nrnsys_int;
        double egtmhz; // Eğitim Hızı Değişkeni (Kullanıcıdan alınır.)
        double epksys; // Epok Sayısı Değişkeni (Kullanıcıdan alınır.)
        double egtm; // Eğitim Oranı (%) (Kullanıcıdan alınır.)
        double vldtn; // Validasyon Oranı (%) (Kullanıcıdan alınır.)
        double test; // Test Oranı (%) (Kullanıcıdan alınır.)
        double toplam; // Toplam Oran (%)
        double agrlksys; // Ağırlık Sayısı Değişkeni
        int agrlksys_int;
        double G_stnsys; // Girişlerin Sütun Sayısı (Input Sayısı)
        int G_stnsys_int;
        double G_strsys; // Girişlerin Satır Sayısı
        int G_strsys_int;
        double C_stnsys; // Çıkışların Sütun Sayısı (Output Sayısı)
        int C_stnsys_int;
        double C_strsys; // Çıkışların Satır Sayısı
        int C_strsys_int;
        int bias_sys;
        int bias_grssys;
        int bias_ckssys;
        

        //------------------------------ Kullanıcının Seçtiği Excel Dosyasının Seçilmesi (Giriş Değerleri) --------------------------------------

        public void cboSheet_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = tableCollection[cboSheet.SelectedItem.ToString()];
            dataGridView3.DataSource = dt;
        }

        DataTableCollection tableCollection;

        public void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Excel 97 - 2003 Workbook|*.xls|Excel Workbook|*.xlsx" })
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtFilename.Text = openFileDialog.FileName;
                    using (var stream = File.Open(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
                    {
                        using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
                            {
                                ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
                            });
                            tableCollection = result.Tables;
                            cboSheet.Items.Clear();
                            foreach (DataTable table in tableCollection)
                                cboSheet.Items.Add(table.TableName);
                        }
                    }
                }
            }
        }

        //------------------------------ Kullanıcının Seçtiği Excel Dosyasının Seçilmesi (Çıkış Değerleri) --------------------------------------

        public void cboSheet2_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            DataTable dt2 = tableCollection2[cboSheet2.SelectedItem.ToString()];
            dataGridView1.DataSource = dt2;
        }
                
        DataTableCollection tableCollection2;

        public void btnBrowse2_Click_1(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Excel 97 - 2003 Workbook|*.xls|Excel Workbook|*.xlsx" })
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtFilename2.Text = openFileDialog.FileName;
                    using (var stream = File.Open(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
                    {
                        using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                        {
                            DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
                            {
                                ConfigureDataTable = (_) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
                            });
                            tableCollection2 = result.Tables;
                            cboSheet2.Items.Clear();
                            foreach (DataTable table in tableCollection2)
                                cboSheet2.Items.Add(table.TableName);
                        }
                    }
                }
            }
        }

        //------------------------------ Kullanıcının Girdiği Değerlerin Değişkenlere Aktarılması --------------------------------------

        private void button3_Click(object sender, EventArgs e)
        {
            a = textBox1.Text;
            nrnsys = Convert.ToDouble(a);
            nrnsys_int = Convert.ToInt16(a);
            
            b = textBox2.Text;
            egtmhz = Convert.ToDouble(b);
            
            c = textBox3.Text;
            epksys = Convert.ToDouble(c);
            
            t3 = textBox7.Text;
            K = Convert.ToDouble(t3);
            
            d = textBox4.Text;
            egtm = Convert.ToDouble(d);
            
            f = textBox5.Text;
            vldtn = Convert.ToDouble(f);
            
            g = textBox6.Text;
            test = Convert.ToDouble(g);
            
            toplam = egtm + vldtn + test;

            if (toplam != 100) // Eğitim, validasyon, test verilerinin toplamının 100 olması gerekir. Kontrol Kodu)
            {
                DialogResult result3 = MessageBox.Show("Eğitim, Validayon ve Test değerlerinin toplamı 100 olmalıdır. Lütfen değerleri kontrol ediniz!", "Değer Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (result3 == DialogResult.OK)
                {
                    textBox4.Clear();
                    textBox5.Clear();
                    textBox6.Clear();
                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Restart();
            Environment.Exit(0);
        }
        
        //------------------------------ Uygulamayı Kapatma Butonu --------------------------------------

        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult result1 = MessageBox.Show("Programı kapatmaya emin misin?", "Programdan Çıkış", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result1 == DialogResult.Yes)
            {
                this.Close();
            }
            else
            {

            }
        }

        //------------------------------ Eğitim Butonu --------------------------------------

        private void button1_Click(object sender, EventArgs e)
        {
            h = dataGridView3.ColumnCount.ToString();
            G_stnsys = Convert.ToDouble(h); // Girişlerin Sütun Sayısının G_stnsys değişkenine aktarılması.
            G_stnsys_int = Convert.ToInt16(h);

            j = dataGridView3.RowCount.ToString();
            G_strsys = Convert.ToDouble(j); // Girişlerin Satır Sayısının G_strsys değişkenine aktarılması.
            G_strsys_int = Convert.ToInt16(j);

            m = dataGridView1.ColumnCount.ToString();
            C_stnsys = Convert.ToDouble(m); // Çıkışların Sütun Sayısının C_stnsys değişkenine aktarılması.
            C_stnsys_int = Convert.ToInt16(m);

            l = dataGridView1.RowCount.ToString();
            C_strsys = Convert.ToDouble(l); // Çıkışların Satır Sayısının C_strsys değişkenine aktarılması.
            C_strsys_int = Convert.ToInt16(l);

            agrlksys = (G_stnsys * nrnsys) + (nrnsys * C_stnsys) + C_stnsys + nrnsys;
            agrlksys_int = Convert.ToInt16(agrlksys);

            

            //------------------------------ Ağırlıkların Listesi --------------------------------------

            ArrayList koleksiyon = new ArrayList();
            Random rnd = new Random();

            for (int i2 = 1; i2 <= agrlksys; i2++)
            {
                double sayi = -5 + (rnd.NextDouble() * 10);
                koleksiyon.Add(sayi);
            }

            foreach (var a in koleksiyon)
            {
                listBox1.Items.Add(a);
            }

            label20.Text = koleksiyon.Count.ToString();
            


            ArrayList koleksiyon2 = new ArrayList();

            DataTable dt = tableCollection[cboSheet.SelectedItem.ToString()];
            dataGridView3.DataSource = dt;

            bias_sys = Convert.ToInt16(nrnsys) + C_stnsys_int;
            bias_grssys = Convert.ToInt16(nrnsys);
            bias_ckssys = C_stnsys_int;

            for (int i8 = 0; i8 < epksys; i8++) //----------Genel EPOK Döngüsü------------------------------------------------------------
            {
                //------------------------------ Koleksiyon3 Sh1, Sh2, ..., Shn Sıralaması --------------------------------------
                

                ArrayList koleksiyon3 = new ArrayList();
                ArrayList koleksiyon4 = new ArrayList();
                ArrayList koleksiyon5 = new ArrayList();
                ArrayList koleksiyon6 = new ArrayList(); // S(On)
                ArrayList koleksiyon7 = new ArrayList(); // σ(Son) için // σ(S) = K * S;
                ArrayList koleksiyon8 = new ArrayList();
                ArrayList koleksiyon9 = new ArrayList();
                ArrayList koleksiyon10 = new ArrayList();
                ArrayList koleksiyon11 = new ArrayList();
                ArrayList koleksiyon12 = new ArrayList();

                koleksiyon2.Clear();
                koleksiyon3.Clear();
                koleksiyon4.Clear();
                koleksiyon5.Clear();
                koleksiyon6.Clear();
                koleksiyon7.Clear();
                koleksiyon8.Clear(); 
                koleksiyon9.Clear();
                koleksiyon10.Clear();
                koleksiyon11.Clear();
                koleksiyon12.Clear();

                listBox2.Items.Clear();
                listBox3.Items.Clear();
                listBox4.Items.Clear();
                listBox5.Items.Clear();
                listBox6.Items.Clear();
                listBox7.Items.Clear();
                listBox8.Items.Clear();
                listBox9.Items.Clear();
                listBox10.Items.Clear();
                listBox11.Items.Clear();
                listBox12.Items.Clear();

                for (int i2 = 0; i2 < nrnsys; i2++)
                {
                    double u = 0;

                    for (int i = 0; i < G_stnsys; i++)
                    {
                        t = Convert.ToDouble(dataGridView3.Rows[i8].Cells[i].Value) * Convert.ToDouble(koleksiyon[i + (i2 * G_stnsys_int)]);
                        koleksiyon2.Add(t);

                        u2 = Convert.ToDouble(koleksiyon2[i + (i2 * G_stnsys_int)]);
                        u = u + u2;
                    }

                    u = u + Convert.ToDouble(koleksiyon[(G_stnsys_int * nrnsys_int) + i2]);

                    koleksiyon3.Add(u);

                }

                foreach (var a in koleksiyon2)
                {
                    listBox2.Items.Add(a);
                }

                foreach (var a in koleksiyon3)
                {
                    listBox3.Items.Add(a);
                }



                //------------------------------ σ(Shn) Hesaplanması --------------------------------------


                // σ(S) = 1/(1+exp(-S));

                for (int i4 = 0; i4 < koleksiyon3.Count; i4++)
                {
                    t2 = Convert.ToDouble(koleksiyon3[i4]);
                    u3 = 1 / (1 + (Math.Exp((-1) * t2)));
                    koleksiyon4.Add(u3);
                }

                foreach (var a in koleksiyon4)
                {
                    listBox4.Items.Add(a);
                }


                //------------------------------ Koleksiyon6 So1, So2, ..., Son Sıralaması --------------------------------------

                for (int i5 = 0; i5 < C_stnsys; i5++)
                {
                    u2 = 0;

                    for (int i6 = 0; i6 < nrnsys; i6++)
                    {
                        y = Convert.ToDouble(koleksiyon4[i6]) * Convert.ToDouble(koleksiyon[agrlksys_int - nrnsys_int + i6]);
                        koleksiyon5.Add(y);

                        y2 = Convert.ToDouble(koleksiyon5[i5 + (i6 * C_stnsys_int)]);
                        u2 = u2 + y2;
                    }

                    u2 = u2 + Convert.ToDouble(koleksiyon[agrlksys_int - (nrnsys_int + C_stnsys_int)]);
                    koleksiyon6.Add(u2);
                }

                foreach (var a in koleksiyon5)
                {
                    listBox5.Items.Add(a);
                }

                foreach (var a in koleksiyon6)
                {
                    listBox6.Items.Add(a);
                }

                //------------------------------ σ(Son) Hesaplanması --------------------------------------

                for (int i7 = 0; i7 < koleksiyon6.Count; i7++)
                {
                    t4 = Convert.ToDouble(koleksiyon6[i7]);
                    t5 = K * t4;
                    koleksiyon7.Add(t5);
                }

                foreach (var a in koleksiyon7)
                {
                    listBox7.Items.Add(a);
                }


                //------------------------------ Her k çıkış birimi için Sk Hata Terimi Hesaplanması --------------------------------------
                // Sk= ok * (1-ok) * (tk-ok)

                for(int i10 = 0; i10 < C_stnsys; i10++)
                {
                    t6 = Convert.ToDouble(koleksiyon7[i10]); // (Ok)
                    t7 = Convert.ToDouble(dataGridView1.Rows[i8].Cells[i10].Value); // (tk)
                    t8 = t6 * (1 - t6) * (t7 - t6);
                    koleksiyon8.Add(t8);
                }

                foreach (var a in koleksiyon8)
                {
                    listBox8.Items.Add(a);
                }


                //------------------------------ Gizli katmandaki nöronlar ve çıkış katmanındaki nöronlar arasındaki ağırlıkların hesaplanması --------------------------------------

                for (int i17 = 0; i17 < C_stnsys; i17++)
                {
                    z8 = egtmhz * Convert.ToDouble(koleksiyon8[i17]) * 1;   // Delta Δ = n * Sk * 1 (2. Bias)
                    z9 = z8 + Convert.ToDouble(koleksiyon[agrlksys_int - (nrnsys_int + C_stnsys_int)]);
                    if (z9 < -5)
                    {
                        z9 = -5;
                    }
                    if (z9 > 5)
                    {
                        z9 = 5;
                    }
                    koleksiyon9.Add(z9);
                }
                
                for (int i11 = 0; i11 < nrnsys; i11++)
                {
                    for(int i12 = 0;  i12 < C_stnsys; i12++)
                    {
                        z1 = egtmhz * Convert.ToDouble(koleksiyon8[i12]) * Convert.ToDouble(koleksiyon4[i11]); // Delta Δ = n * Sk * σ(Shn)
                        z5 = z1 + Convert.ToDouble(koleksiyon[agrlksys_int - nrnsys_int + i11]);
                        if (z5 < -5)
                        {
                            z5 = -5;
                        }
                        if (z5 > 5)
                        {
                            z5 = 5;
                        }
                        koleksiyon9.Add(z5);
                    }
                }
                                
                foreach (var a in koleksiyon9)
                {
                    listBox9.Items.Add(a); // 2. Bias ve ağırlıklar sırasıyla
                }

                //------------------------------ Her h gizli katman birimi için Sk (Gizli birim) Hata Terimi Hesaplanması --------------------------------------

                for (int i13 = 0; i13 < nrnsys; i13++)
                {
                    for(int i14 = 0; i14 < C_stnsys; i14++)
                    {
                        z2 = (Convert.ToDouble(koleksiyon8[i14]) * Convert.ToDouble(koleksiyon[agrlksys_int - nrnsys_int + i13]));
                        z3 = z3 + z2;
                        
                    }

                    z4 = z3 * (Convert.ToDouble(koleksiyon4[i13]) * (1 - (Convert.ToDouble(koleksiyon4[i13]))));
                    koleksiyon10.Add(z4);

                }

                foreach (var a in koleksiyon10)
                {
                    listBox10.Items.Add(a);
                }

                //------------------------------ Gizli katmandaki nöronlar ve giriş katmanındaki nöronlar arasındaki ağırlıkların hesaplanması --------------------------------------

                for (int i15 = 0; i15 < G_stnsys_int; i15++)
                {
                    for (int i16 = 0; i16 < nrnsys; i16++)
                    {
                        z6 = egtmhz * (Convert.ToDouble(koleksiyon10[i16])) * (Convert.ToDouble(dataGridView3.Rows[i8].Cells[i15].Value)); // Delta Δ = n * SH * Xi
                        z7 = z6 + Convert.ToDouble(koleksiyon[i15 + (i16 * G_stnsys_int)]);
                        if(z7 < -5)
                        {
                            z7 = -5;
                        }
                        if(z7 > 5)
                        {
                            z7 = 5;
                        }
                        koleksiyon11.Add(z7);
                    }
                }

                for (int i18 = 0; i18 < nrnsys_int; i18++)
                {
                    y3 = egtmhz * Convert.ToDouble(koleksiyon10[i18]);  // 1. Bisas
                    y4 = y3 + Convert.ToDouble(koleksiyon[(G_stnsys_int * nrnsys_int) + i18]);
                    if (y4 < -5)
                    {
                        y4 = -5;
                    }
                    if (y4 > 5)
                    {
                        y4 = 5;
                    }
                    koleksiyon11.Add(y4);
                }

                foreach (var a in koleksiyon11)
                {
                    listBox11.Items.Add(a); // 1. Ağırlıklar ve 1. Bias Sırayla 
                }

                for(int i19 = 0; i19 < koleksiyon11.Count; i19++)
                {
                    koleksiyon12.Add(koleksiyon11[i19]);
                }

                for (int i20 = 0; i20 < koleksiyon9.Count; i20++)
                {
                    koleksiyon12.Add(koleksiyon9[i20]);
                }

                foreach (var a in koleksiyon12)
                {
                    listBox12.Items.Add(a); // Yeni Tüm Ağırlıklar 
                }

                koleksiyon = (ArrayList)koleksiyon12.Clone();

            }
        }

        
    }
}
