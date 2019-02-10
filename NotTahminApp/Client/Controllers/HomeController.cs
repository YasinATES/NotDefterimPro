using Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
namespace Client.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SingUp()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult UpdateLessons()
        {
            return View();
        }
        public ActionResult UpdateUsers()
        {
            return View();
        }

        public ActionResult UpdatePasswd()
        {
            return View();
        }
        public ActionResult TryUpdatePasswd()
        {
            return View();
        }
        public ActionResult ControlKayit(StudentsVM model)
        {
            return View();
        }
        public ActionResult UserList()
        {
            using (var myservice = new ServiceReference.Service1Client())
            {
                int id = 1;
                ViewBag.Ogrenciler = myservice.GetAllStudents(id);
            }
            return View();
        }
        public ActionResult LessonTable()
        {
            using (var myservice = new ServiceReference.Service1Client())
            {
                if((Convert.ToString(Session["UserName"]) == ""))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    int id = Convert.ToInt32(Session["UserId"].ToString());

                    ViewBag.Dersler = myservice.GetAllLesson(id);
                }
            }
            return View();  
        }
        [HttpPost]
        public ActionResult LessonTable(LessonVM model)
        {
            using (var MyService = new ServiceReference.Service1Client())
            {
                model.Adı = "Ders Adı Giriniz";
                model.GecmeNotu = 60;
                MyService.InsertLesson(new ServiceReference.Lesson { Id = model.Id, Adi = model.Adı, Büt = model.Büt, BütTahmin = model.BütTahmin, Final = model.Final, FinalDurum = model.FinalDurum, FinalTahmin = model.FinalTahmin, Vize = model.Vize, VizeDurum = model.VizeDurum, VizeTahmin = model.VizeTahmin,  GecmeNotu = model.GecmeNotu, GecerNot=model.GecerNot, StudentId = Convert.ToInt32(Session["UserId"].ToString())});
            }
            return RedirectToAction("LessonTable");
        }

        public ActionResult LogOff()
        {
        
            if (Session["UserName"] != null)
            {
                Session.RemoveAll();
            }
            return Redirect("Index");
        }
        [HttpPost]
        public ActionResult Login(StudentsVM swm)
        {
            try
            {
                var MyService = new ServiceReference.Service1Client();
                var result = MyService.Login(new ServiceReference.Student { KullaniciAdi = swm.KullaniciAdi, Sifre = swm.Sifre, AdSoyad = swm.AdSoyad, Oturum = swm.Oturum });
                if (result != null)
                {
                    Session["UserName"] = result.AdSoyad;
                    Session["UserId"] = Convert.ToString(result.Id);
                    if (result.Oturum)
                    {
                        Session["Oturum"] = "admin";
                    }
                    else
                    {
                        Session["Oturum"] = "user";
                    }
                    if (result.KullaniciAdi == "AdministratorYasinAteş")
                    {
                        Session["TamYetki"] = "var";
                    }
                    return Redirect("Index");
                }
                else
                {
                    TempData["KullaniciYok"] = "*Girdiğiniz Kullanıcı Adı ya da Parolası Yanlıştır.";
                    return View();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult SingUp(StudentsVM model)
        {

            string code = Request.QueryString["code"];



            if (model.KullaniciAdi == "AdministratorYasinAteş")
            {
                model.Oturum = true;
            }
            else
            {
                model.Oturum = false;
            }
            string eror;
            if (model.AdSoyad=="" || model.AdSoyad == null)
            {
                TempData["Eror"] = "AdSoyad ";
            }
            if (model.KullaniciAdi == "" || model.KullaniciAdi == null)
            {
                eror = Convert.ToString(TempData["Eror"]);
                TempData["Eror"] = eror + "KullanıcıAdı ";
            }
            if (model.Sifre == "" || model.Sifre == null)
            {
                eror = Convert.ToString(TempData["Eror"]);
                TempData["Eror"] = eror + "Şifre ";
            }
            if (model.EMail == "" || model.EMail == null)
            {
                eror = Convert.ToString(TempData["Eror"]);
                TempData["Eror"] = eror + "E-Mail ";
            }
            if (model.GizliSoruCevap == "" || model.GizliSoruCevap == null)
            {
                eror = Convert.ToString(TempData["Eror"]);
                TempData["Eror"] = eror + "GizliSoruCevabı";
            }
            if (Convert.ToString(TempData["Eror"]) == "")
            {
                if(model.KullaniciAdi.Length>=5 && model.Sifre.Length>=5 && model.GizliSoruCevap.Length>=5 && model.EMail.Length>=5 && model.AdSoyad.Length >= 5)
                {
                    using (var MyService = new ServiceReference.Service1Client())
                    {
                        var c = MyService.AccessControl(new ServiceReference.Student { KullaniciAdi = model.KullaniciAdi, EMail = model.EMail });                
                        if (c == null)
                        {
                            if (model.EMail.IndexOf("gmail.com") !=-1 | model.EMail.IndexOf("outlook.com") != -1 | model.EMail.IndexOf("yandex.com") != -1 | model.EMail.IndexOf("yahoo.com") != -1 | model.EMail.IndexOf("hotmail.com") != -1 | model.EMail.IndexOf("windowslive.com") != -1)
                            {
                                //Şifre oluşturma
                                char[] cr = "0123456789abcdefghijklmnoprstuvyzxw1234567890ABCDEFGHIJKLMNOPQRSTUCWXYZ".ToCharArray();
                                string result = string.Empty;
                                Random r = new Random();
                                for (int i = 0; i < 16; i++)
                                {
                                    result += cr[r.Next(0, cr.Length - 1)].ToString();
                                }
                                //Mail gönderimi
                                SmtpClient sc = new SmtpClient
                                {
                                    Port = 587,
                                    Host = "mail.notdefterim.pro",
                                    EnableSsl = false,
                                    Credentials = new NetworkCredential("info@notdefterim.pro", "393M6znreh.A")

                                };
                                MailMessage mail = new MailMessage
                                {
                                    From = new MailAddress("info@notdefterim.pro", "NotDefterimPro")
                                };
                                mail.To.Add(model.EMail);
                                mail.Subject = "Onay Kodu";
                                mail.IsBodyHtml = true;
                                mail.Body = "<div class='container'> <div style='background:white;'><h3 style='color:red'>Not Defterim Pro</h3>" + "<br/>Onay Kodunuz:" + result + "<p>Onay Kodunuz başarılı bir şekilde gönderilmiştir.<br/></br>Uygulamamızı kullandığınız için teşekkür eder,<br/> başarılarınızın devamını dileriz...<p/>" + "<p style='font - family:Helvetica, Arial, sans - serif; font - size:12px; line - height:16px; color:#aaaaaa;'>© 2018 - All Rights Reserved. </p><div/><div/>";
                                sc.Send(mail);
                                TempData["tutucu"] = 1;
                                TempData["result"] = result;



                                if (code == result) {

                                MyService.SignUp(new ServiceReference.Student { Id = model.Id, AdSoyad = model.AdSoyad, KullaniciAdi = model.KullaniciAdi, GizliSoru = model.GizliSoru, GizliSoruCevap = model.GizliSoruCevap, Sifre = model.Sifre, EMail = model.EMail, Oturum = model.Oturum });

                                }

                            }
                            else
                            {
                                TempData["Mail"] = "Gireceğiniz E-Posta Servisi 'gmail.com   outlook.com   yandex.com   yahoo.com   hotmail.com   windowslive.com' ve türevleri olmalıdır.";
                            }
                            
                        }
                        else
                        {
                            TempData["AyniUye"] = "*Kullanıcı Adı ya da E-Mail Daha Önce Alınmıştır.Lütfen Başka Bir Ad veya Mail Giriniz.";
                        }
                    }
                }
                else
                {
                    TempData["Uzunluk"] = "*Gireceğiniz her değer 5 karakterden uzun olmalıdır.";
                }
            }
            else
            {
                TempData["tutucu"] = 2;
            }
            TempData["temp"] = 1;
            return View();
        }

        [HttpPost]
        public ActionResult UpdatePasswd(StudentsVM model)
        {
           
            using (var MyService = new ServiceReference.Service1Client())
            {
                if (MyService.ChangePassword(new ServiceReference.Student { KullaniciAdi = model.KullaniciAdi, GizliSoru = model.GizliSoru, GizliSoruCevap = model.GizliSoruCevap, Sifre = model.Sifre, EMail = model.EMail }))
                {
                    ViewBag.Script = "swal('Şifre Güncellendi!', 'Yeni şifrenizi unutmayınız', 'success')";
                }
                else
                {
                    TempData["GuncellemeHata"] = "Bulunamıyor";
                    ViewBag.Script = "swal('Hatalı Giriş', 'Bilgileri yanlış ya da eksik girdiniz.', 'error')";
                }
            }
            return View();
        }
        [HttpPost]
        public ActionResult TryUpdatePasswd(StudentsVM model)
        {
            using (var MyService = new ServiceReference.Service1Client())
            {
                string sifre = MyService.TryChangePassword(new ServiceReference.Student { EMail = model.EMail });
                if (sifre!="")
                {
                    SmtpClient sc = new SmtpClient
                    {
                        Port = 587,
                        Host = "mail.notdefterim.pro",
                        EnableSsl = false,
                        Credentials = new NetworkCredential("info@notdefterim.pro", "393M6znreh.A")
                        
                    };
                    MailMessage mail = new MailMessage
                    {
                        From = new MailAddress("info@notdefterim.pro", "NotDefterimPro")
                    };
                    mail.To.Add(model.EMail);
                    mail.Subject = "Şifre İsteği";
                    mail.IsBodyHtml = true;
                    mail.Body = "<div class='container'> <div style='background:white;'><h3 style='color:red'>Not Defterim Pro</h3>" + "<br/>Şifreniz: " + sifre + "<p>Şifreniz başarılı bir şekilde gönderilmiştir.<br/></br>Uygulamamızı kullandığınız için teşekkür eder,<br/> başarılarınızın devamını dileriz...<p/>" + "<p style='font - family:Helvetica, Arial, sans - serif; font - size:12px; line - height:16px; color:#aaaaaa;'>© 2018 - All Rights Reserved. </p><div/><div/>";
                    sc.Send(mail);

                    ViewBag.Script = "swal('Şifre Maile Gönderildi!', 'Şifrenizi unutmayınız', 'success')";
                }
                else
                {
                    ViewBag.Script = "swal('Hatalı Giriş', 'Sistemde Böyle Bir Mail Kayıtlı Değil.', 'error')";
                }
            }
            return View();
        }
        [HttpPost]
        public ActionResult UpdateLessons(LessonVM model)
        {
            using (var MyService = new ServiceReference.Service1Client())
            {
                
                if (model.VizeTahmin <= model.Vize)
                    model.VizeDurum = "Başarılı";
                if (model.VizeTahmin > model.Vize)
                    model.VizeDurum = "Başarısız";
                if (model.FinalTahmin <= model.Final)
                    model.FinalDurum = "Başarılı";
                if (model.FinalTahmin > model.Final)
                    model.FinalDurum = "Başarısız";
                if (model.FinalTahmin == 0)
                    model.FinalDurum = "";
                if (model.Final == 0)
                    model.FinalDurum = "";
                if (model.VizeTahmin == 0)
                    model.VizeDurum = "";
                if (model.Vize == 0)
                    model.VizeDurum = "";
                if (model.Vize >= 0 & model.Vize <= 100 & model.VizeTahmin >= 0 & model.VizeTahmin <= 100 & model.Final >= 0 & model.Final <= 100 & model.FinalTahmin >= 0 & model.FinalTahmin <= 100 & model.Büt >= 0 & model.Büt <= 100 & model.BütTahmin >= 0 & model.BütTahmin <= 100 & model.GecmeNotu >= 35 & model.GecmeNotu <= 100)
                {
                    if (model.GecmeNotu == 60 & model.Vize!=0)
                    {
                        model.GecerNot= (model.GecmeNotu - (Convert.ToDouble(model.Vize) * 0.4)) / 0.6;
                        model.GecerNot = Math.Round(model.GecerNot, 2);
                        if (model.GecerNot <= 50)
                        {
                            model.GecerNot = 50;
                        }
                    }
                    else if(model.Vize!=0)
                    {
                        model.GecerNot =( model.GecmeNotu - (Convert.ToDouble(model.Vize) * 0.4)) / 0.6;
                        model.GecerNot = Math.Round(model.GecerNot, 2);
                        if (model.GecerNot <= 35)
                        {
                            model.GecerNot = 35;
                        }
                    }
                    MyService.UpdateLesson(new ServiceReference.Lesson { Id = model.Id, Adi = model.Adı, Büt = model.Büt, BütTahmin = model.BütTahmin, Final = model.Final, FinalDurum = model.FinalDurum, FinalTahmin = model.FinalTahmin, Vize = model.Vize, VizeDurum = model.VizeDurum, VizeTahmin = model.VizeTahmin, GecmeNotu = model.GecmeNotu, GecerNot = model.GecerNot }, Convert.ToInt32(Session["UserId"]));
                }

                    
                else
                    TempData["Aralik"] = "*Notların Puan Aralıkları 0 ile 100 arasında olmalıdır veya Geçer Not 35'ten bütük olmalıdır.";
            }
            return Json(true);
        }
        [HttpPost]
        public ActionResult UpdateUsers(StudentsVM model)
        {
            using (var MyService = new ServiceReference.Service1Client())
            {
                MyService.UpdateUser(new ServiceReference.Student { Id=model.Id, AdSoyad = model.AdSoyad, EMail = model.EMail, GizliSoru = model.GizliSoru, GizliSoruCevap = model.GizliSoruCevap, KullaniciAdi = model.KullaniciAdi, Oturum = model.Oturum, Sifre = model.Sifre });
            }
            return Json(true);
        }
        public ActionResult Sil(int id)
        {
            using (var MyService = new ServiceReference.Service1Client())
            {
                if(MyService.DeleteLesson(id, Convert.ToInt32(Session["UserId"])))
                {
                    return RedirectToAction("LessonTable");
                }
                else
                {
                    return RedirectToAction("LessonTable");
                }
            }
            
        }
        public ActionResult KullaniciSil (int id)
        {
            using (var MyService = new ServiceReference.Service1Client())
            {
                if (Convert.ToInt32(Session["UserId"]) != id)
                {
                    MyService.DeleteUser(id, Convert.ToInt32(Session["UserId"]));
                }
                return RedirectToAction("UserList");
            }
            
        }
    }
}