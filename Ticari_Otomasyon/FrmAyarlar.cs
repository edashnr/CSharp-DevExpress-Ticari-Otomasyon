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
    public partial class FrmAyarlar : Form
    {
        public FrmAyarlar()
        {
            InitializeComponent();
        }

        sqlbaglantisi bgl = new sqlbaglantisi();

        void temizle()
        {
            TxtId.Text = "";
            TxtKullanıcıAdı.Text = "";
            textAd.Text = "";
            textSoyAd.Text = "";
            TxtSıfre.Text = ""; 
        }

        void listele()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("select * from TBL_ADMIN", bgl.baglanti());
            da.Fill(dt);
            gridControl1.DataSource = dt;
        }

        private void FrmAyarlar_Load(object sender, EventArgs e)
        {
            listele();
            temizle();
        }


        

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if(dr != null)
            {
                TxtId.Text = dr["ID"].ToString();
                TxtKullanıcıAdı.Text = dr["KullaniciAd"].ToString();
                textAd.Text = dr["Ad"].ToString();
                textSoyAd.Text = dr["SoyAd"].ToString();
                TxtSıfre.Text = dr["Sifre"].ToString();
            }
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            // Kullanıcı adı kontrolü
            SqlCommand kontrolKomut = new SqlCommand("select count(*) from TBL_ADMIN where KullaniciAd=@p1", bgl.baglanti());
            kontrolKomut.Parameters.AddWithValue("@p1", TxtKullanıcıAdı.Text);
            int count = (int)kontrolKomut.ExecuteScalar();
            bgl.baglanti().Close();

            if (count > 0)
            {
                MessageBox.Show("Bu kullanıcı adı zaten mevcut!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Yeni kullanıcı ekleme işlemi
            SqlCommand komut = new SqlCommand("insert into TBL_ADMIN (KullaniciAd, Ad, SoyAd, Sifre) values (@p1,@p2,@p3,@p4)", bgl.baglanti());
            komut.Parameters.AddWithValue("@p1", TxtKullanıcıAdı.Text);
            komut.Parameters.AddWithValue("@p2", textAd.Text);
            komut.Parameters.AddWithValue("@p3", textSoyAd.Text);
            komut.Parameters.AddWithValue("@p4", TxtSıfre.Text);
            komut.ExecuteNonQuery();
            bgl.baglanti().Close();
            listele();
            temizle();
            MessageBox.Show("Admin Bilgileri Kaydedildi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void BtnGüncelle_Click(object sender, EventArgs e)
        {
            // Kullanıcı adı kontrolü (güncelleme)
            SqlCommand kontrolKomut = new SqlCommand("select count(*) from TBL_ADMIN where KullaniciAd=@p1 and ID != @p2", bgl.baglanti());
            kontrolKomut.Parameters.AddWithValue("@p1", TxtKullanıcıAdı.Text);
            kontrolKomut.Parameters.AddWithValue("@p2", TxtId.Text);
            int count = (int)kontrolKomut.ExecuteScalar();
            bgl.baglanti().Close();

            if (count > 0)
            {
                MessageBox.Show("Bu kullanıcı adı zaten mevcut!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Güncelleme işlemi
            SqlCommand komut = new SqlCommand("Update TBL_ADMIN set KullaniciAd=@p1,Ad=@p2,SoyAd=@p3,Sifre=@p4 where ID=@p5", bgl.baglanti());
            komut.Parameters.AddWithValue("@p1", TxtKullanıcıAdı.Text);
            komut.Parameters.AddWithValue("@p2", textAd.Text);
            komut.Parameters.AddWithValue("@p3", textSoyAd.Text);
            komut.Parameters.AddWithValue("@p4", TxtSıfre.Text);
            komut.Parameters.AddWithValue("@p5", TxtId.Text);
            komut.ExecuteNonQuery();
            bgl.baglanti().Close();
            listele();
            temizle();
            MessageBox.Show("Admin Bilgileri Güncellendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }


        private void BtnSil_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bu kaydı silmek istediğinizden emin misiniz?", "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                SqlCommand komut = new SqlCommand("delete from TBL_ADMIN where ID=@p1", bgl.baglanti());
                komut.Parameters.AddWithValue("@p1", TxtId.Text);
                komut.ExecuteNonQuery();
                bgl.baglanti().Close();
                listele();
                temizle();
                MessageBox.Show("Kayıt Silindi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnTemizle_Click(object sender, EventArgs e)
        {
            temizle();
        }

       
    }
}
