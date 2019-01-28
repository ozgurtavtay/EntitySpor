using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BLL;

namespace SporOrganizasyon
{
    public partial class Etkinlik : Form
    {
        BusinessLogic bl;
        public Etkinlik()
        {
            InitializeComponent();
            bl = new BusinessLogic();
        }

        private void Etkinlik_Load(object sender, EventArgs e)
        {
            comboBoxTip.DisplayMember = "Tip";
            comboBoxTip.ValueMember = "TipId";
            comboBoxTip.DataSource = bl.EtkinlikTipAl();

            comboBoxMekan.DisplayMember = "MekanAdi";
            comboBoxMekan.ValueMember = "Mid";
            comboBoxMekan.DataSource = bl.MekanAl();

            comboBoxSpor.DisplayMember = "SporAdi";
            comboBoxSpor.ValueMember = "SporId";
            comboBoxSpor.DataSource = bl.SporAl();
        }

        private void btnAc_Click(object sender, EventArgs e)
        {
            int k = bl.EtkinlikAc(txtEtkinlik.Text, int.Parse(comboBoxTip.SelectedValue.ToString()), int.Parse(comboBoxMekan.SelectedValue.ToString()),Convert.ToDateTime(maskedTextBoxTrh.Text), int.Parse(txtKontenjan.Text),int.Parse(comboBoxSpor.SelectedValue.ToString()));

            if (k>0)
            {
                MessageBox.Show("Etkinlik Oluştu");
            }
            else
            {
                MessageBox.Show("Etkinlik Oluşamadı");
            }
        }

        private void comboBoxTip_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(comboBoxTip.SelectedValue.ToString()) == 3)
            {
                txtKontenjan.Text = 2.ToString();
            }
            else
            {
                txtKontenjan.Text = null;
            }
        }
    }
}
