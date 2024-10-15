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
using System.Xml;

namespace Ticari_Otomasyon
{
    public partial class FrmAnaSayfa : Form
    {
        public FrmAnaSayfa()
        {
            InitializeComponent();
        }

        sqlbaglantisi bgl = new sqlbaglantisi();

        void stoklar()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("Select URUNAD,Sum(Adet) as 'ADET' From TBL_URUNLER group by " +
                "Urunad having Sum(adet) <= 20 order by sum(adet)", bgl.baglanti());
            da.Fill(dt);
            gridControlStoklar.DataSource = dt;
        }

        void ajanda()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("select top 10 TARIH,SAAT,BASLIK from TBL_NOTLAR  " +
                "order by Id desc ", bgl.baglanti());
            da.Fill(dt);
            gridControlAjanda.DataSource = dt;
        }

        void FirmaHareketleri()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("Exec FirmaHareket2", bgl.baglanti());
            da.Fill(dt);
            gridControlFirmaHareket.DataSource = dt;
        }

        void firmafihrist()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("select AD,TELEFON1 from TBL_FIRMALAR", bgl.baglanti());
            da.Fill(dt);
            gridControlFihrist.DataSource = dt;
        }

        void müsterifihrist()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("select AD,SOYAD,TELEFON from TBL_MUSTERILER", bgl.baglanti());
            da.Fill(dt);
            gridControlFihrist2.DataSource = dt;
        }


        void haberler()
        {
            XmlTextReader xmloku = new XmlTextReader("https://www.ntv.com.tr/ekonomi.rss");
            while (xmloku.Read())
            {
                if (xmloku.Name == "title")
                {
                    listBox1.Items.Add(xmloku.ReadString());
                    listBox1.Items.Add(""); // Bir satır boşluk eklendi
                }
            }
        }



        private void FrmAnaSayfa_Load(object sender, EventArgs e)
        {
            stoklar();
            ajanda();
            FirmaHareketleri();
            firmafihrist();
            müsterifihrist();
            webBrowser1.Navigate("https://www.tcmb.gov.tr/kurlar/today.xml");
            webBrowser2.Navigate("");
            haberler();
           
            
        }

        private void btnara_Click(object sender, EventArgs e)
        {
            webBrowser2.Navigate(textBox1.Text);
        }

        private void btngeri_Click(object sender, EventArgs e)
        {
            webBrowser2.GoBack();
        }

        private void btnileri_Click(object sender, EventArgs e)
        {
            webBrowser2.GoForward();
        }

        private void btnyenile_Click(object sender, EventArgs e)
        {
            webBrowser2.Refresh();
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {



        }
    }
}
