using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using NotTahminApp.Class;
using NotTahminApp.Models;

namespace NotTahminApp
{
    public class Service1 : IService1
    {
        public void SignUp(Student s)
        {
            Class.AppContext ac = new Class.AppContext();
            ac.Students.Add(s);
            ac.SaveChanges();
        }
        public StudentDTO Login(Student s)
        {
            Class.AppContext ac = new Class.AppContext();
            var c = (from per in ac.Students
                     where per.KullaniciAdi == s.KullaniciAdi
                     & per.Sifre == s.Sifre
                     select per).FirstOrDefault();
            if (c == null)
                return null;
            return new StudentDTO
            {
                AdSoyad = c.AdSoyad,
                EMail = c.EMail,
                GizliSoru = c.GizliSoru,
                GizliSoruCevap = c.GizliSoruCevap,
                Id = c.Id,
                KullaniciAdi = c.KullaniciAdi,
                Sifre = c.Sifre,
                Oturum = c.Oturum
            };
        }
        public StudentDTO AccessControl(Student s)
        {
           Class.AppContext ac = new Class.AppContext();
            var c = (from per in ac.Students
                     where per.KullaniciAdi == s.KullaniciAdi
                     | per.EMail==s.EMail
                     select per).FirstOrDefault();
            if (c == null)
                return null;
            else
            {
                StudentDTO stn = new StudentDTO
                {
                    EMail = c.EMail,
                    KullaniciAdi = c.KullaniciAdi
                };
                return stn;
            }
        }
        public bool ChangePassword(Student s)
        {
            Class.AppContext ac = new Class.AppContext();
            var c = (from per in ac.Students
                     where per.KullaniciAdi == s.KullaniciAdi
                     & per.GizliSoru == s.GizliSoru & per.GizliSoruCevap==s.GizliSoruCevap & per.EMail==s.EMail
                     select per).FirstOrDefault();
            if (c != null)
            {
                c.Sifre = s.Sifre;
                ac.SaveChanges();
                return true;
            }
            else
                return false;
        }
        public string TryChangePassword(Student s)
        {
            Class.AppContext ac = new Class.AppContext();
            var c = (from per in ac.Students
                     where per.EMail == s.EMail
                     select per).FirstOrDefault();
            if (c != null)
            {
                return c.Sifre;
            }
            else
                return "";
        }
        public void InsertLesson(Lesson ls)
        {
            Class.AppContext ac = new Class.AppContext();
            var c = (from per in ac.Students
                     where per.Id == ls.StudentId
                     select per).FirstOrDefault();
            if (c.DersSayisi <= 29)
            {
                c.DersSayisi++;
                ac.Lessons.Add(ls);
                ac.SaveChanges();
            }
        }
        public List<LessonDTO> GetAllLesson(int id)
        {
            Class.AppContext ac = new Class.AppContext();
            return ac.Lessons.Where(x=>x.StudentId==id).Select(x => new LessonDTO
            {
                Id = x.Id,
                Adı = x.Adi,
                VizeTahmin = x.VizeTahmin,
                Vize = x.Vize,
                VizeDurum = x.VizeDurum,
                FinalTahmin = x.FinalTahmin,
                Final = x.Final,
                FinalDurum = x.FinalDurum,
                BütTahmin = x.BütTahmin,
                Büt = x.Büt,
                StudentId=id,
                GecmeNotu=x.GecmeNotu,
                GecerNot=x.GecerNot
            }).ToList();
        }
        public List<StudentDTO> GetAllStudents(int id)
        {
            Class.AppContext ac = new Class.AppContext();
            return ac.Students.Where(x => x.Id >= 0).Select(x => new StudentDTO
            {
                Id = x.Id,
                AdSoyad = x.AdSoyad,
                KullaniciAdi = x.KullaniciAdi,
                Sifre = x.Sifre,
                EMail = x.EMail,
                GizliSoru = x.GizliSoru,
                GizliSoruCevap = x.GizliSoruCevap,
                Oturum = x.Oturum
            }).ToList();

        }
        public void UpdateLesson(Lesson ls , int uid)
        {
            Class.AppContext ac = new Class.AppContext();
            var c = (from less in ac.Lessons
                     where less.Id == ls.Id & less.StudentId == uid
                     select less).FirstOrDefault();
            if (c != null)
            {
                c.Adi = ls.Adi;
                c.VizeTahmin = ls.VizeTahmin;
                c.Vize = ls.Vize;
                c.VizeDurum = ls.VizeDurum;
                c.FinalTahmin = ls.FinalTahmin;
                c.Final = ls.Final;
                c.FinalDurum = ls.FinalDurum;
                c.BütTahmin = ls.BütTahmin;
                c.Büt = ls.Büt;
                c.GecmeNotu = ls.GecmeNotu;
                c.GecerNot = ls.GecerNot;
                ac.SaveChanges();
            }
        }
        public void UpdateUser(Student user)
        {
            Class.AppContext ac = new Class.AppContext();
            var c = (from std in ac.Students
                     where std.Id == user.Id
                     select std).First();
            c.AdSoyad = user.AdSoyad; ;
            c.EMail = user.EMail;
            c.GizliSoru = user.GizliSoru;
            c.GizliSoruCevap = user.GizliSoruCevap;
            c.KullaniciAdi = user.KullaniciAdi;
            c.Oturum = user.Oturum;
            c.Sifre = user.Sifre;
            ac.SaveChanges();
        }
        public bool DeleteLesson(int Id, int uid)
        {
            Class.AppContext ac = new Class.AppContext();
            var c = (from less in ac.Lessons
                     where less.Id == Id & less.StudentId==uid
                     select less).FirstOrDefault();
            if (c != null)
            {
                ac.Lessons.Remove(c);
                ac.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
            
        }
        public void DeleteUser(int Id, int uid)
        {
            Class.AppContext ac = new Class.AppContext();
            var c = (from std in ac.Students
                     where std.Id == Id
                     select std).First();
            if (c.Id != 1)
            {
                ac.Students.Remove(c);
                ac.SaveChanges();
            }
        }
    }
}
