using System;
using System.Net.Mail;
using System.Windows.Forms;

namespace Ticari_Otomasyon
{
    public partial class FrmMail : Form
    {
        public FrmMail()
        {
            InitializeComponent();
        }

        public string mail;
        private void FrmMail_Load(object sender, EventArgs e)
        {
            TxtMailAdres.Text = mail;
        }

        private void BtnGönder_Click(object sender, EventArgs e)
        {
            try
            {
                MailMessage mesajim = new MailMessage();
                SmtpClient istemci = new SmtpClient();
                //simple mail transfer protocol
                istemci.Credentials = new System.Net.NetworkCredential("edanursahiner@zohomail.com", "Edanur.66");
                //kimlik bilgisi
                istemci.Port = 587;
                istemci.Host = "smtp.zoho.com";
                istemci.EnableSsl = true;
                //sunucu ile istemci arasındakı verılerı dogru adrese gıdene kadar sıfreleme yapar
                mesajim.To.Add(TxtMailAdres.Text);
                mesajim.From = new MailAddress("edanursahiner@zohomail.com");
                mesajim.Subject = TxtKonu.Text;
                mesajim.Body = TxtMesaj.Text;
                istemci.Send(mesajim);

                DialogResult result = MessageBox.Show("Mail gönderildi.", "Bilgi", MessageBoxButtons.OK);
                if (result == DialogResult.OK)
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Mail gönderirken bir hata oluştu: " + ex.Message);
            }
        }
    }
}
