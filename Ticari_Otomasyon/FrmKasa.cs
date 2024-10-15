using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.Charts;

namespace Ticari_Otomasyon
{
    public partial class FrmKasa : Form
    {
        public FrmKasa()
        {
            InitializeComponent();
        }

        sqlbaglantisi bgl = new sqlbaglantisi();

        void müsterihareket()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("execute MusteriHareketler", bgl.baglanti());
            da.Fill(dt);
            gridControl1.DataSource = dt;
        }
        void firmahareket()
        {
            DataTable dt2 = new DataTable();
            SqlDataAdapter da2 = new SqlDataAdapter("execute FirmaHareketler", bgl.baglanti());
            da2.Fill(dt2);
            gridControl3.DataSource = dt2;
        }
        void giderler()
        {
            DataTable dt3 = new DataTable();
            SqlDataAdapter da3 = new SqlDataAdapter("select * from TBL_GIDERLER", bgl.baglanti());
            da3.Fill(dt3);
            gridControl2.DataSource = dt3;
        }

        public string ad;
        private void FrmKasa_Load(object sender, EventArgs e)
        {
            LblAktifKullanıcı.Text = ad;// ANA MODULDE TANIMLADIGIMIZ AD'I BURDA LABELIMIZIN ICINE
                                        // YAZDIRDIK ANA MODULU KOPRU GIBI KULLANDIK KASA VE ADMIN PANELI ARASINDA 

            müsterihareket();
            firmahareket();
            giderler();

            //Toplam tutarı hesaplama//////////////////////////////////////////////////////////
            SqlCommand komut1 = new SqlCommand("Select sum(tutar) from TBL_FATURADETAY", bgl.baglanti());
            SqlDataReader dr1 = komut1.ExecuteReader();
            while (dr1.Read())
            {
                LblKasaToplam.Text = dr1[0].ToString() + " TL";
            }
            dr1.Close(); //DataReader'ı kapatmayı unutmayın
            bgl.baglanti().Close();


            //Son ayın faturaları////////////////////////////////////////////////////////////////
            SqlCommand komut2 = new SqlCommand("select (ELEKTRIK+ SU + DOGALGAZ+INTERNET+EKSTRA) from TBL_GIDERLER ORDER BY ID ASC", bgl.baglanti());
            SqlDataReader dr2 = komut2.ExecuteReader();
            while (dr2.Read())
            {
                LblÖdemeler.Text = dr2[0].ToString() + " TL";
            }
            dr2.Close(); //DataReader'ı kapatmayı unutmayın
            bgl.baglanti().Close();


            //Son ayın personel maaşları////////////////////////////////////////////////////////////
            SqlCommand komut3 = new SqlCommand("select MAASLAR from TBL_GIDERLER ORDER BY ID ASC ", bgl.baglanti());
            SqlDataReader dr3 = komut3.ExecuteReader();
            while (dr3.Read())
            {
                LblPersonelMaasları.Text = dr3[0].ToString() + " TL";
            }
            dr3.Close(); //DataReader'ı kapatmayı unutmayın
            bgl.baglanti().Close();


            //Toplam müşteri sayısı////////////////////////////////////////////////////////////
            SqlCommand komut4 = new SqlCommand("select count(*) from TBL_MUSTERILER", bgl.baglanti());
            SqlDataReader dr4 = komut4.ExecuteReader();
            while (dr4.Read())
            {
                LblMüsteriSayısı.Text = dr4[0].ToString() ;
            }
            dr4.Close(); //DataReader'ı kapatmayı unutmayın
            bgl.baglanti().Close();


            //Toplam firma sayısı////////////////////////////////////////////////////////////
            SqlCommand komut5 = new SqlCommand("select count(*) from TBL_FIRMALAR", bgl.baglanti());
            SqlDataReader dr5 = komut5.ExecuteReader();
            while (dr5.Read())
            {
                LblFirmaSayısı.Text = dr5[0].ToString();
            }
            dr5.Close(); //DataReader'ı kapatmayı unutmayın
            bgl.baglanti().Close();


            //Toplam firma şehir sayısı////////////////////////////////////////////////////////////
            SqlCommand komut6 = new SqlCommand("select count(Distinct(IL)) from TBL_FIRMALAR", bgl.baglanti());
            SqlDataReader dr6 = komut6.ExecuteReader();
            while (dr6.Read())
            {
                LblFirmaSehirSayısı.Text = dr6[0].ToString();
            }
            dr6.Close(); //DataReader'ı kapatmayı unutmayın
            bgl.baglanti().Close();


            //Toplam Müşteri şehir sayısı////////////////////////////////////////////////////////////
            SqlCommand komut7 = new SqlCommand("select count(Distinct(IL)) from TBL_MUSTERILER", bgl.baglanti());
            SqlDataReader dr7 = komut7.ExecuteReader();
            while (dr7.Read())
            {
                LblMüsteriSehirSayısı.Text = dr7[0].ToString();
            }
            dr7.Close(); //DataReader'ı kapatmayı unutmayın
            bgl.baglanti().Close();



            //Toplam personel sayısı////////////////////////////////////////////////////////////
            SqlCommand komut8 = new SqlCommand("select count(*) from TBL_PERSONELLER", bgl.baglanti());
            SqlDataReader dr8 = komut8.ExecuteReader();
            while (dr8.Read())
            {
                LblPersonelSayısı.Text = dr8[0].ToString();
            }
            dr8.Close(); //DataReader'ı kapatmayı unutmayın
            bgl.baglanti().Close();


            //Toplam ürün sayısı////////////////////////////////////////////////////////////
            SqlCommand komut9 = new SqlCommand("select sum(ADET) from TBL_URUNLER", bgl.baglanti());
            SqlDataReader dr9 = komut9.ExecuteReader();
            while (dr9.Read())
            {
                LblStokSayısı.Text = dr9[0].ToString();
            }
            dr9.Close(); //DataReader'ı kapatmayı unutmayın
            bgl.baglanti().Close();


        }

        int sayac = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            sayac++;

            //Elektrik
            if (sayac > 0 && sayac <= 5)
            {             
                groupControl12.Text = "Elektrik";
                chartControl1.Series["Aylar"].Points.Clear();
                SqlCommand komut10 = new SqlCommand("select top 4 Ay,Elektrık from TBL_GIDERLER order by ID desc", bgl.baglanti());
                SqlDataReader dr10 = komut10.ExecuteReader();
                while (dr10.Read())
                {
                    chartControl1.Series["Aylar"].Points.Add(new DevExpress.XtraCharts.SeriesPoint(dr10[0], dr10[1]));
                }
                bgl.baglanti().Close();
            }

            //Su
            if (sayac > 5 && sayac <= 10)
            {
                groupControl12.Text = "Su";
                chartControl1.Series["Aylar"].Points.Clear();               
                SqlCommand komut11 = new SqlCommand("select top 4 Ay,Su from TBL_GIDERLER order by ID desc", bgl.baglanti());
                SqlDataReader dr11 = komut11.ExecuteReader();
                while (dr11.Read())
                {
                    chartControl1.Series["Aylar"].Points.Add(new DevExpress.XtraCharts.SeriesPoint(dr11[0], dr11[1]));
                }
                bgl.baglanti().Close();
            }

            //dogalgaz
            if (sayac > 10 && sayac <= 15)
            {
                groupControl12.Text = "Dogalgaz";
                chartControl1.Series["Aylar"].Points.Clear();
                SqlCommand komut12 = new SqlCommand("select top 4 Ay,Dogalgaz from TBL_GIDERLER order by ID desc", bgl.baglanti());
                SqlDataReader dr12 = komut12.ExecuteReader();
                while (dr12.Read())
                {
                    chartControl1.Series["Aylar"].Points.Add(new DevExpress.XtraCharts.SeriesPoint(dr12[0], dr12[1]));
                }
                bgl.baglanti().Close();
            }

            //internet
            if (sayac > 15 && sayac <= 20)
            {
                groupControl12.Text = "İnternet";
                chartControl1.Series["Aylar"].Points.Clear();
                SqlCommand komut13 = new SqlCommand("select top 4 Ay,Internet from TBL_GIDERLER order by ID desc", bgl.baglanti());
                SqlDataReader dr13 = komut13.ExecuteReader();
                while (dr13.Read())
                {
                    chartControl1.Series["Aylar"].Points.Add(new DevExpress.XtraCharts.SeriesPoint(dr13[0], dr13[1]));
                }
                bgl.baglanti().Close();
            }

            //Ekstra
            if (sayac > 20 && sayac <= 25)
            {
                groupControl12.Text = "Ekstra";
                chartControl1.Series["Aylar"].Points.Clear();
                SqlCommand komut14 = new SqlCommand("select top 4 Ay,Ekstra from TBL_GIDERLER order by ID desc", bgl.baglanti());
                SqlDataReader dr14 = komut14.ExecuteReader();
                while (dr14.Read())
                {
                    chartControl1.Series["Aylar"].Points.Add(new DevExpress.XtraCharts.SeriesPoint(dr14[0], dr14[1]));
                }
                bgl.baglanti().Close();
            }
            if (sayac == 26)
            {
                sayac = 0;
            }
        }

        int sayac2;
        private void timer2_Tick(object sender, EventArgs e)
        {
            sayac2++;

            //Elektrik
            if (sayac2 > 0 && sayac2 <= 5)
            {
                groupControl13.Text = "Elektrik";
                chartControl2.Series["Aylar"].Points.Clear();
                SqlCommand komut15 = new SqlCommand("select top 4 Ay,Elektrık from TBL_GIDERLER order by ID desc", bgl.baglanti());
                SqlDataReader dr15 = komut15.ExecuteReader();
                while (dr15.Read())
                {
                    chartControl2.Series["Aylar"].Points.Add(new DevExpress.XtraCharts.SeriesPoint(dr15[0], dr15[1]));
                }
                bgl.baglanti().Close();
            }

            //Su
            if (sayac2 > 5 && sayac2 <= 10)
            {
                groupControl13.Text = "Su";
                chartControl2.Series["Aylar"].Points.Clear();
                SqlCommand komut16 = new SqlCommand("select top 4 Ay,Su from TBL_GIDERLER order by ID desc", bgl.baglanti());
                SqlDataReader dr16 = komut16.ExecuteReader();
                while (dr16.Read())
                {
                    chartControl2.Series["Aylar"].Points.Add(new DevExpress.XtraCharts.SeriesPoint(dr16[0], dr16[1]));
                }
                bgl.baglanti().Close();
            }

            //dogalgaz
            if (sayac2 > 10 && sayac2 <= 15)
            {
                groupControl13.Text = "Dogalgaz";
                chartControl2.Series["Aylar"].Points.Clear();
                SqlCommand komut17 = new SqlCommand("select top 4 Ay,Dogalgaz from TBL_GIDERLER order by ID desc", bgl.baglanti());
                SqlDataReader dr17 = komut17.ExecuteReader();
                while (dr17.Read())
                {
                    chartControl2.Series["Aylar"].Points.Add(new DevExpress.XtraCharts.SeriesPoint(dr17[0], dr17[1]));
                }
                bgl.baglanti().Close();
            }

            //internet
            if (sayac2 > 15 && sayac2 <= 20)
            {
                groupControl13.Text = "İnternet";
                chartControl2.Series["Aylar"].Points.Clear();
                SqlCommand komut18 = new SqlCommand("select top 4 Ay,Internet from TBL_GIDERLER order by ID desc", bgl.baglanti());
                SqlDataReader dr18 = komut18.ExecuteReader();
                while (dr18.Read())
                {
                    chartControl2.Series["Aylar"].Points.Add(new DevExpress.XtraCharts.SeriesPoint(dr18[0], dr18[1]));
                }
                bgl.baglanti().Close();
            }

            //Ekstra
            if (sayac2 > 20 && sayac2 <= 25)
            {
                groupControl13.Text = "Ekstra";
                chartControl2.Series["Aylar"].Points.Clear();
                SqlCommand komut19 = new SqlCommand("select top 4 Ay,Ekstra from TBL_GIDERLER order by ID desc", bgl.baglanti());
                SqlDataReader dr19 = komut19.ExecuteReader();
                while (dr19.Read())
                {
                    chartControl2.Series["Aylar"].Points.Add(new DevExpress.XtraCharts.SeriesPoint(dr19[0], dr19[1]));
                }
                bgl.baglanti().Close();
            }
            if (sayac2 == 26)
            {
                sayac2 = 0;
            }
        }
    }
}
