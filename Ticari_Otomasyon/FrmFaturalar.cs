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

namespace Ticari_Otomasyon
{
    public partial class FrmFaturalar : Form
    {
        public FrmFaturalar()
        {
            InitializeComponent();
        }

        sqlbaglantisi bgl = new sqlbaglantisi();

        void listele()
        {
            SqlDataAdapter da = new SqlDataAdapter("Select * From TBL_FATURABILGI", bgl.baglanti());
            DataTable dt = new DataTable();
            da.Fill(dt);
            gridControl1.DataSource = dt;
        }

        void temizle() // işlem yapıldıktan sonra ınput yerlerını bosaltır temızler
        {
            Txtid.Text = "";
            TxtSeri.Text = "";
            TxtSıraNo.Text = "";
            MskTarih.Text = "";
            MskSaat.Text = "";
            TxtVergiDairesi.Text = "";
            TxtAlıcı.Text = "";
            cmbteslimeden.Text = "";
            TxtTeslimAlan.Text = "";

        }

        void personellistesi()
        {
            SqlCommand komut = new SqlCommand("select AD,SOYAD from tbl_personeller", bgl.baglanti());
            SqlDataReader dr = komut.ExecuteReader();
            while(dr.Read())
            {
                string fullName = dr["AD"].ToString() + " " + dr["SOYAD"].ToString();
                cmbteslimeden.Properties.Items.Add(fullName);
                
            }
            bgl.baglanti().Close();
        }

        private void FrmFaturalar_Load(object sender, EventArgs e)
        {
            
            listele();

            temizle();
            personellistesi();
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            if (TxtFaturaID.Text == "" )
            {
                SqlCommand komut = new SqlCommand("insert into TBL_FATURABILGI " +
                    "(SERI,SIRANO,TARIH,SAAT,VERGIDAIRE,ALICI,TESLIMEDEN,TESLIMALAN) VALUES (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8)", bgl.baglanti());
                komut.Parameters.AddWithValue("@p1", TxtSeri.Text);
                komut.Parameters.AddWithValue("@p2", TxtSıraNo.Text);
                komut.Parameters.AddWithValue("@p3", MskTarih.Text);
                komut.Parameters.AddWithValue("@p4", MskSaat.Text);
                komut.Parameters.AddWithValue("@p5", TxtVergiDairesi.Text);
                komut.Parameters.AddWithValue("@p6", TxtAlıcı.Text);
                komut.Parameters.AddWithValue("@p7", cmbteslimeden.Text);
                komut.Parameters.AddWithValue("@p8", TxtTeslimAlan.Text);
                komut.ExecuteNonQuery();
                bgl.baglanti().Close();
                MessageBox.Show("Fatura Bilgisi Sisteme Kaydedildi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                listele();
            }
            //Firma Carisi
            if (TxtFaturaID.Text != "" && comboBox1.Text == "Firma")
            {
                double miktar, tutar, fiyat;
                fiyat = Convert.ToDouble(TxtFiyat.Text);
                miktar = Convert.ToDouble(TxtMiktar.Text);
                tutar = miktar * fiyat;
                TxtTutar.Text = tutar.ToString();
                //miktar hesaplama yaptık burda

                SqlCommand komut2 = new SqlCommand("insert into TBL_FATURADETAY (URUNAD,MIKTAR,FIYAT,TUTAR,FATURAID)" +
                    " VALUES (@p1,@p2,@p3,@p4,@p5)", bgl.baglanti());
                komut2.Parameters.AddWithValue("@p1", TxtÜrünAd.Text);
                komut2.Parameters.AddWithValue("@p2", TxtMiktar.Text);
                komut2.Parameters.AddWithValue("@p3", decimal.Parse( TxtFiyat.Text));
                komut2.Parameters.AddWithValue("@p4", decimal.Parse( TxtTutar.Text));
                komut2.Parameters.AddWithValue("@p5", TxtFaturaID.Text);
                komut2.ExecuteNonQuery();
                bgl.baglanti().Close();

                //Hareket tablosuna verı gırısı
                SqlCommand komut3 = new SqlCommand("insert into tbl_fırmahareketler (Urunıd,adet,personel,fırma,fıyat,toplam,faturaıd,tarih)" +
                    " values (@h1,@h2,@h3,@h4,@h5,@h6,@h7,@h8)", bgl.baglanti());
                komut3.Parameters.AddWithValue("@h1", TxtÜrünID.Text);
                komut3.Parameters.AddWithValue("@h2", TxtMiktar.Text);
                komut3.Parameters.AddWithValue("@h3", txtpersonel.Text);
                komut3.Parameters.AddWithValue("@h4", TxtFırma.Text);
                komut3.Parameters.AddWithValue("@h5", decimal.Parse( TxtFiyat.Text));
                komut3.Parameters.AddWithValue("@h6", decimal.Parse( TxtTutar.Text));
                komut3.Parameters.AddWithValue("@h7", TxtFaturaID.Text);
                komut3.Parameters.AddWithValue("@h8", MskTarih.Text);
                komut3.ExecuteNonQuery();
                bgl.baglanti().Close();

                //Stok Ssayısını azaltma
                SqlCommand komut4 = new SqlCommand("update Tbl_urunler set adet= adet-@s1 where Id=@s2", bgl.baglanti());
                komut4.Parameters.AddWithValue("@s1", TxtMiktar.Text);
                komut4.Parameters.AddWithValue("@s2", TxtÜrünID.Text);
                komut4.ExecuteNonQuery();
                bgl.baglanti().Close();

                MessageBox.Show("Faturaya Ait Ürün Kaydedildi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                temizle();
            }
           
            // Müşteri Carisi
            if (TxtFaturaID.Text != "" && comboBox1.Text == "Müşteri")
            {
                double miktar, tutar, fiyat;
                fiyat = Convert.ToDouble(TxtFiyat.Text);
                miktar = Convert.ToDouble(TxtMiktar.Text);
                tutar = miktar * fiyat;
                TxtTutar.Text = tutar.ToString();
                //miktar hesaplama yaptık burda

              


                SqlCommand komut2 = new SqlCommand("insert into TBL_FATURADETAY (URUNAD,MIKTAR,FIYAT,TUTAR,FATURAID)" +
                    " VALUES (@p1,@p2,@p3,@p4,@p5)", bgl.baglanti());
                komut2.Parameters.AddWithValue("@p1", TxtÜrünAd.Text);
                komut2.Parameters.AddWithValue("@p2", TxtMiktar.Text);
                komut2.Parameters.AddWithValue("@p3", decimal.Parse(TxtFiyat.Text));
                komut2.Parameters.AddWithValue("@p4", decimal.Parse(TxtTutar.Text));
                komut2.Parameters.AddWithValue("@p5", TxtFaturaID.Text);
                komut2.ExecuteNonQuery();
                bgl.baglanti().Close();

                //Hareket tablosuna verı gırısı
                SqlCommand komut3 = new SqlCommand("insert into TBL_MUSTERIHAREKETLER (Urunıd,adet,personel,MUSTERI,fıyat,toplam,faturaıd,tarih)" +
                    " values (@h1,@h2,@h3,@h4,@h5,@h6,@h7,@h8)", bgl.baglanti());
                komut3.Parameters.AddWithValue("@h1", TxtÜrünID.Text);
                komut3.Parameters.AddWithValue("@h2", TxtMiktar.Text);
                komut3.Parameters.AddWithValue("@h3", txtpersonel.Text);
                komut3.Parameters.AddWithValue("@h4", TxtFırma.Text);
                komut3.Parameters.AddWithValue("@h5", decimal.Parse(TxtFiyat.Text));
                komut3.Parameters.AddWithValue("@h6", decimal.Parse(TxtTutar.Text));
                komut3.Parameters.AddWithValue("@h7", TxtFaturaID.Text);
                komut3.Parameters.AddWithValue("@h8", MskTarih.Text);
                komut3.ExecuteNonQuery();
                bgl.baglanti().Close();

                //Stok Ssayısını azaltma
                SqlCommand komut4 = new SqlCommand("update Tbl_urunler set adet= adet-@s1 where Id=@s2", bgl.baglanti());
                komut4.Parameters.AddWithValue("@s1", TxtMiktar.Text);
                komut4.Parameters.AddWithValue("@s2", TxtÜrünID.Text);
                komut4.ExecuteNonQuery();
                bgl.baglanti().Close();

                MessageBox.Show("Faturaya Ait Ürün Kaydedildi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                temizle();
            }
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle); // verileri boşuklara taşıdık
            if (dr != null)
            {
                Txtid.Text = dr["FATURABILGIID"].ToString();
                TxtSıraNo.Text = dr["SIRANO"].ToString();
                TxtSeri.Text = dr["SERI"].ToString();
                MskTarih.Text = dr["TARIH"].ToString();
                MskSaat.Text = dr["SAAT"].ToString();
                TxtVergiDairesi.Text = dr["VERGIDAIRE"].ToString();
                TxtAlıcı.Text = dr["ALICI"].ToString();
                cmbteslimeden.Text = dr["TESLIMEDEN"].ToString();
                TxtTeslimAlan.Text = dr["TESLIMALAN"].ToString();

            }
        }

        private void BtnSil_Click(object sender, EventArgs e)
        {
            DialogResult dg = MessageBox.Show("Silmek İstediğinize Emin misiniz?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dg == DialogResult.Yes)
            {
                SqlCommand cmd = new SqlCommand("Delete From TBL_FATURABILGI where FATURABILGIID=@p1", bgl.baglanti());
                cmd.Parameters.AddWithValue("@p1", Txtid.Text);
                cmd.ExecuteNonQuery();
                bgl.baglanti().Close();
                MessageBox.Show("Fatura Bilgisi Listeden Silindi", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                listele();
                temizle();
            }
        }

        private void BtnTemizle_Click(object sender, EventArgs e)
        {
            temizle();
        }

        private void BtnGüncelle_Click(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand("Update TBL_FATURABILGI set SERI=@p1,SIRANO=@p2,TARIH=@p3,SAAT=@p4,VERGIDAIRE=@p5,ALICI=@p6,TESLIMEDEN=@p7,TESLIMALAN=@p8 where FATURABILGIID=@p9", bgl.baglanti());

            komut.Parameters.AddWithValue("@p1", TxtSeri.Text);
            komut.Parameters.AddWithValue("@p2", TxtSıraNo.Text);
            komut.Parameters.AddWithValue("@p3", MskTarih.Text);
            komut.Parameters.AddWithValue("@p4", MskSaat.Text);
            komut.Parameters.AddWithValue("@p5", TxtVergiDairesi.Text);
            komut.Parameters.AddWithValue("@p6", TxtAlıcı.Text);
            komut.Parameters.AddWithValue("@p7", cmbteslimeden.Text);
            komut.Parameters.AddWithValue("@p8", TxtTeslimAlan.Text);
            komut.Parameters.AddWithValue("@p9", Txtid.Text);
            komut.ExecuteNonQuery();
            bgl.baglanti().Close();
            MessageBox.Show("Fatura Bilgisi Güncellendi", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            listele();
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            FrmFaturaUrunDetay fr = new FrmFaturaUrunDetay();
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);

            if(dr != null)
            {
                fr.id = dr["FATURABILGIID"].ToString();
            }
            fr.Show();
        }

        private void btnBul_Click(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand("select UrunAd,SatısFıyat from TBL_URUNLER where Id=@p1", bgl.baglanti());
            komut.Parameters.AddWithValue("@p1", TxtÜrünID.Text);
            SqlDataReader dr = komut.ExecuteReader();
            while(dr.Read())
            {
                TxtÜrünAd.Text = dr[0].ToString();
                TxtFiyat.Text = dr[1].ToString();
            }
            bgl.baglanti().Close();

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
    }
}
