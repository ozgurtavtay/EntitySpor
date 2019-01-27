using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DalEntity;

namespace BLL
{
    public class BusinessLogic
    {
        DataAccessLayer da;

        public BusinessLogic()
        {
            da = new DataAccessLayer();
        }

        private bool EmailKontrol(string email)
        {
            return Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        public int LoginKontrol(string email, string sifre)
        {
            int ret = 0;

            Kullanici k = new Kullanici();


            bool kontrol = EmailKontrol(email);
            if (kontrol && !string.IsNullOrEmpty(sifre))
            {
                k.Email = email;
                k.Sifre = sifre;

                ret = da.LoginKontrol(k); // kullanici id geri döner
            }
            else
            {
                da.HataVar("Uygun Email değil!!!");
                ret = -1;
            }

            return ret;
        }

        public void Logout(int Userid)
        {
            da.Logout(Userid);
        }

        public int KullaniciKaydet(string ad, string soyad, string email, string telefon, string sifre, string ilce, DateTime dogumtarihi, int cinsiyet, int[] spor)
        {
            int kayitSayisi;
            Kullanici k = new Kullanici();
            bool kontrol = EmailKontrol(email);

            if (!string.IsNullOrEmpty(ad) && !string.IsNullOrEmpty(soyad) && kontrol)
            {
                k.Ad = ad;
                k.Soyad = soyad;
                k.Email = email;
                k.Telefon = telefon;
                k.Sifre = sifre;
                k.Ilce = ilce;
                k.DogumTarihi = dogumtarihi;
                k.Cinsiyet = cinsiyet;

                kayitSayisi = da.KullaniciKaydet(k, spor);
            }

            else
            {
                kayitSayisi = -1;
            }

            return kayitSayisi;
        }

        public int EtkinlikAc(string etkinlikAd, int tipId, int mekanId, DateTime etkinlikTarihi, int kontenjan, int sporId)
        {
            int kayitSayisi;
            Etkinlik e = new Etkinlik();
            if (!string.IsNullOrEmpty(etkinlikAd) && !string.IsNullOrEmpty(etkinlikTarihi.ToString()))
            {
                e.EtkinlikAdi = etkinlikAd;
                e.TipId = tipId;
                e.MekanID = mekanId;
                e.EtkinlikTarihi = etkinlikTarihi;
                e.Kontenjan = kontenjan;
                e.Sid = sporId;
                e.isActive = 1;
                kayitSayisi = da.EtkinlikAc(e);
            }

            else
            {
                kayitSayisi = -1;
            }

            return kayitSayisi;
        }

        public int MekanAc(string mekanAdi, int ilceId)
        {
            int kayitSayisi;
            Mekan m = new Mekan();
            if (!string.IsNullOrEmpty(mekanAdi))
            {
                m.MekanAdi = mekanAdi;

                m.IlceId = ilceId;
                kayitSayisi = da.MekanAc(m);
            }
            else
            {
                kayitSayisi = -1;
            }

            return kayitSayisi;
        }

        public List<Sporlar> SporAl()
        {
            var spor = da.SporGetir();
            return spor;
        }

        public List<Iller> Iller()
        {
            var il = da.IlAl();
            return il;
        }

        public List<Ilceler> Ilceler(int sehir)
        {
            var ilce = da.IlceAl(sehir);
            return ilce;
        }

        public List<EtkinlikAl_Result> EtkinlikAl()
        {
            var etkinlik = da.EtkinlikAl();
            return etkinlik;
        }
        public List<EtkinlikTipi> EtkinlikTipAl()
        {
            var etip = da.EtkinlikTipAl();
            return etip;
        }
        public List<Mekan> MekanAl()
        {
            var mekan = da.MekanAl();
            return mekan;
        }

        public int Katil(int eid, int kid)
        {
            return da.Katil(eid, kid);
        }

        public int Cikis(int eid, int kid)
        {
            return da.Cikis(eid, kid);
        }

        public int EtkinlikKisiKontrol(int EtkinlikId, int UserId)
        {
            int ret = 0;

            ret = da.EtkinlikKisiKontrol(EtkinlikId, UserId);

            return ret;
        }
    }
}
