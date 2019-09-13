using BilkentKafe.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YazKafe
{
    public partial class SiparisForm : Form
    {
        KafeVeri db;
        Siparis siparis;
        BindingList<SiparisDetay> blSiparisDetaylar;

        public SiparisForm(KafeVeri kafeVeri, Siparis siparis)
        {
            db = kafeVeri;
            this.siparis = siparis;
            blSiparisDetaylar = new BindingList<SiparisDetay>(siparis.SiparisDetaylar);

            blSiparisDetaylar.ListChanged += BlSiparisDetaylar_ListChanged;

            InitializeComponent();

            Text = "Masa " + siparis.MasaNo;
            lblMasaNo.Text = string.Format("{0:00}", siparis.MasaNo);

            cboUrunler.DataSource = db.Urunler;
            dgvSiparisDetaylar.DataSource = blSiparisDetaylar;
            lblOdemetutari.Text = siparis.ToplamTutarTL;
        }

        private void BlSiparisDetaylar_ListChanged(object sender, ListChangedEventArgs e)
        {
            lblOdemetutari.Text = siparis.ToplamTutarTL;
        }

        private void btnSiparisDetayEkle_Click(object sender, EventArgs e)
        {
            Urun secili = (Urun)cboUrunler.SelectedItem;
            SiparisDetay sd = new SiparisDetay
            {
                UrunAd = secili.UrunAd,
                BirimFiyat = secili.BirimFiyat,
                Adet = (int)nudUrunAdet.Value
            };

            blSiparisDetaylar.Add(sd);


        }

        private void btnAnaSayfa_Click(object sender, EventArgs e)
        {
            //Pencereyi kapat
            Close();
        }

        private void btnSiparisIptal_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Bu siparişi iptal etmek istediğnize emin misini?", "Sipariş İptal Onayı", MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Yes)
            {
                db.MasayiKapat(siparis.MasaNo, SiparisDurum.Iptal);
                Close();
            }
        }

        private void btnOdemeAl_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show(siparis.ToplamTutarTL+ " tahsil edildiyse sipariş kapatılacaktır.Onaylıyor musunuz?",
                "Ödeme alındı onayı", MessageBoxButtons.YesNo,
               MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Yes)
            {
                db.MasayiKapat(siparis.MasaNo, SiparisDurum.Odendi);
                Close();
            }  
        }
    }
}
