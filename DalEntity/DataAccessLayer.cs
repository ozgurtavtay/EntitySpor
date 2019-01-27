using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Data;

namespace DalEntity
{
    public class DataAccessLayer
    {
        
        SporOEntities et;

        public DataAccessLayer()
        {
            et = new SporOEntities();
        }

        public int LoginKontrol(Kullanici k)
        {
            int kayitSayisi = 0;
            try
            {
                var user = from kul in et.Kullanici where kul.Email == k.Email && kul.Sifre == k.Sifre select new { kul.Kid, kul.Email, kul.isLogin};

                if (user.ToList().Count() > 0)
                {
                    kayitSayisi = 1;
                    foreach (var item in user)
                    {

                        if (item.isLogin == 2)
                        {
                            kayitSayisi = -2;
                        }
                        else
                        {
                            try
                            {
                                et.IsLoginUser(item.Kid);
                                // update islogin = 2 
                            }
                            catch (Exception ex)
                            {
                                Hata(ex);
                            }
                            finally
                            {
                                kayitSayisi = item.Kid;
                            }
                        }

                    }
                }

                else
                {
                    HataVar("Bilgiler Hatali");
                }

            }
            catch (Exception ex)
            {
                Hata(ex);
            }

            return kayitSayisi;
        }

        public void Logout(int UserId)
        {
            try
            {
                var islogin = (from k in et.Kullanici where k.Kid == UserId select k).Single();
                islogin.isLogin = 1;
                et.SaveChanges();
            }
            catch (Exception ex)
            {
                Hata(ex);
            }
        }

        public int KullaniciKaydet(Kullanici k, int[] s)
        {
            int kayitSayisi = 0;
            try
            {
                List<Sporlar> sporlar = new List<Sporlar>();
                foreach (var item in s)
                {
                    var kullanicispor = from spor in et.Sporlar where spor.SporId == item select spor;
                    sporlar.Add(kullanicispor.Single());
                }
                k.Sporlar = sporlar;
                et.Kullanici.Add(k);
                kayitSayisi = et.SaveChanges();
            }
            catch (Exception ex)
            {
                Hata(ex);
            }

            return kayitSayisi;
        }

        public int EtkinlikAc(Etkinlik e)
        {
            int kayitSayisi = 0;
            try
            {
                et.Etkinlik.Add(e);
                kayitSayisi = et.SaveChanges();
            }
            catch (Exception ex)
            {
                Hata(ex);
            }
            return kayitSayisi;
        }

        public int MekanAc(Mekan m)
        {
            int kayitSayisi = 0;
            try
            {
                et.Mekan.Add(m);
                kayitSayisi = et.SaveChanges();
            }
            catch (Exception ex)
            {
                Hata(ex);
            }
            return kayitSayisi;
        }

        public List<Sporlar> SporGetir()
        {
            List<Sporlar> sporlar = null;
            try
            {
                sporlar = (from s in et.Sporlar select s).ToList();
                
                if (sporlar.Count > 0)
                {
                    return sporlar;
                }
                else
                {
                    HataVar("Kayitli spor yok");
                }
            }
            catch (Exception ex)
            {
                Hata(ex);
            }
            return sporlar;
        }

        public List<Iller> IlAl()
        {
            List<Iller> Iller = null;
            try
            {
                Iller = (from il in et.Iller select il).ToList();
            }
            catch (Exception ex)
            {
                Hata(ex);
            }
            return Iller;
        }

        public List<Ilceler> IlceAl(int sehirId)
        {
            List<Ilceler> Ilceler = null;
            try
            {
                Ilceler = (from ilce in et.Ilceler where ilce.Sehir == sehirId select ilce).ToList();
            }
            catch (Exception ex)
            {
                Hata(ex);
            }
            return Ilceler;
        }

        public List<EtkinlikAl_Result> EtkinlikAl()
        {
            List<EtkinlikAl_Result> etkinlikler = null;
            try
            {
                etkinlikler = et.EtkinlikAl().ToList();
            }
            catch (Exception ex)
            {
                Hata(ex);
            }
            return etkinlikler;
        }

        public List<EtkinlikTipi> EtkinlikTipAl()
        {
            List<EtkinlikTipi> eTipler = null;
            try
            {
                eTipler = (from e in et.EtkinlikTipi select e).ToList();
            }
            catch (Exception ex)
            {
                Hata(ex);
            }

            return eTipler;
        }

        public List<Mekan> MekanAl()
        {
            List<Mekan> mekanlar = null;
            try
            {
                mekanlar = (from m in et.Mekan select m).ToList();
            }
            catch (Exception ex)
            {
                Hata(ex);
            }
            return mekanlar;
        }

        public int Katil(int EtkinlikId, int UserId)
        {
            int kayitSayisi = 0;
            try
            {
                var katilan = (from k in et.Kullanici where k.Kid == UserId select k).SingleOrDefault();
                var etkinlik = (from e in et.Etkinlik where e.EtkinlikId == EtkinlikId select e).SingleOrDefault();
                etkinlik.Kullanici.Add(katilan);
                kayitSayisi = et.SaveChanges();
            }
            catch (Exception ex)
            {
                Hata(ex);
            }

            return kayitSayisi;
        }

        public int Cikis(int EtkinlikId, int UserId)
        {
            int kayitSayisi = 0;
            try
            {
                var katilan = (from k in et.Kullanici where k.Kid == UserId select k).SingleOrDefault();
                var etkinlik = (from e in et.Etkinlik where e.EtkinlikId == EtkinlikId select e).SingleOrDefault();
                etkinlik.Kullanici.Remove(katilan);
                kayitSayisi = et.SaveChanges();
            }
            catch (Exception ex)
            {
                Hata(ex);
            }

            return kayitSayisi;
        }

        public int EtkinlikKisiKontrol(int EtkinlikId, int UserId)
        {
            int ret = 0;

            try
            {
                var katilan = (from k in et.Kullanici where k.Kid == UserId select k).SingleOrDefault();
                var etkinlik = (from e in et.Etkinlik where e.EtkinlikId == EtkinlikId select e).SingleOrDefault();
                if (etkinlik.Kullanici.Contains(katilan))
                {
                    ret = 1;
                }
            }
            catch (Exception ex)
            {
                Hata(ex);
            }

            return ret;
        }

        public void Hata(Exception ex)
        {
            StackTrace stack = new StackTrace(true);
            foreach (StackFrame frame in stack.GetFrames())
            {
                if (!string.IsNullOrEmpty(frame.GetFileName()))
                {
                    HataLoglari htl = new HataLoglari();
                    htl.DosyaAdi = Path.GetFileName(frame.GetFileName());
                    htl.MethodAdi = frame.GetMethod().ToString();
                    htl.LineNumber = frame.GetFileLineNumber();
                    htl.ColumnNumber = frame.GetFileColumnNumber();
                    htl.Message = ex.Message.ToString();
                    l
                    et.HataLoglari.Add(htl);
                    et.SaveChanges();
                }
            }
        }//Bitti

        public void HataVar(string mesaj)
        {
            StackTrace stack = new StackTrace(true);
            foreach (StackFrame frame in stack.GetFrames())
            {
                if (!string.IsNullOrEmpty(frame.GetFileName()))
                {
                    HataLoglari htl = new HataLoglari();
                    htl.DosyaAdi = Path.GetFileName(frame.GetFileName());
                    htl.MethodAdi = frame.GetMethod().ToString();
                    htl.LineNumber = frame.GetFileLineNumber();
                    htl.ColumnNumber = frame.GetFileColumnNumber();
                    htl.Message = mesaj;

                    et.HataLoglari.Add(htl);
                    et.SaveChanges();
                }
            }
        }//Bitti

    }
}
