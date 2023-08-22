using ANK13SuperMarket.Context;
using ANK13SuperMarket.Entities;
using ANK13SuperMarket.Models;
using Microsoft.AspNetCore.Mvc;

namespace ANK13SuperMarket.Controllers
{
    public class MarketController : Controller
    {
        private readonly MarketDbContext _db;
        private readonly Product _product = new Product();

        

        public MarketController(MarketDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()  //Index, stokta olanları getirir !
        {
            //Bütün ürünlerin listelendiği yer
            //Bütün ürünleri getir ve görünüme gönder.
          
            ViewBag.StockDurumu=false;
            return View(_db.Products.Where(u => u.IsInStock).ToList());
        }

        public IActionResult StoktaOlmayanlar()
        {
            return View();
        }

        public IActionResult StoktaOlmayanlariGetir()  //stokta olmayanları getirir !
        {
            //Bütün ürünlerin listelendiği yer
            //Bütün ürünleri getir ve görünüme gönder.
            ViewBag.StockDurumu = true;
            return View("Index", _db.Products.Where(u => !u.IsInStock).ToList());
          
        }







        [HttpGet]
        public IActionResult Ekle()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult Ekle(ProductViewModel pvm)
        //{
        //    if (!ModelState.IsValid) return View();  //Tokat

        //    _product.Name = pvm.Name;
        //    _product.Price = pvm.Price;
        //    _product.IsInStock = pvm.IsInStock;
        //    var existingProduct = _db.Products.FirstOrDefault(p => p.Name == _product.Name);

        //    if (existingProduct != null)
        //    {
        //        TempData["mesaj"] = "Bu ürün zaten var!";
        //        return View();
        //    }
        //    if (pvm.Image != null)  // Eğer resim varsa adını Product class objesine ata. wwwroot/img ye resmi kaydet.
        //    {
        //        _product.ImageName = pvm.Image.FileName;
        //        var konum = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", pvm.Image.FileName);
        //        var akisOrtami = new FileStream(konum, FileMode.Create);
        //        pvm.Image.CopyTo(akisOrtami);
        //        akisOrtami.Close();
        //    }

        //    _db.Products.Add(_product);
        //    _db.SaveChanges();
        //    return RedirectToAction("Index");
        //}
        [HttpPost]
        public IActionResult Ekle(ProductViewModel pvm)
        {
            if (!ModelState.IsValid) return View();  // Validation hatası

            if (pvm.Price <= 0)
            {
                TempData["mesaj"] = "Ürün fiyatı sıfır veya negatif olamaz!";
                return View();
            }

            var existingProduct = _db.Products.FirstOrDefault(p => p.Name == pvm.Name);

            if (existingProduct != null)
            {
                TempData["mesaj"] = "Bu ürün zaten var!";
                return View();
            }

            try
            {
                // Yeni bir ürün oluştur
                Product newProduct = new Product
                {
                    Name = pvm.Name,
                    Price = pvm.Price,
                    IsInStock = pvm.IsInStock
                };

                if (pvm.Image != null)  // Eğer resim varsa adını Product nesnesine ata ve kaydet
                {
                    newProduct.ImageName = pvm.Image.FileName;
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", pvm.Image.FileName);
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        pvm.Image.CopyTo(stream);
                    }
                }

                _db.Products.Add(newProduct);
                _db.SaveChanges();

                TempData["mesaj"] = "Ürün başarıyla eklendi.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["mesaj"] = "Hata Oluştu: " + ex.Message;
                return View();
            }
        }








        [HttpGet]
        public IActionResult Guncelle(int id)
        {
            return View(_db.Products.Find(id));
        }

        //[HttpPost]
        //public IActionResult Guncelle(Product urun)
        //{
        //    //Bu action ekleme işlemi için gerekli o yüzden POST
        //    //DB'ye ekle

        //    var existingProduct = _db.Products.FirstOrDefault(p => p.Name == _product.Name);

        //    if (existingProduct != null)
        //    {
        //        TempData["mesaj"] = "Bu ürün zaten var!";
        //        return View();
        //    }

        //    _db.Products.Update(urun);
        //    _db.SaveChanges();

        //    //Ana listeye git
        //    return RedirectToAction("Index");
        //}

        [HttpPost]
        public IActionResult Guncelle(Product urun)
        {
            if (!ModelState.IsValid) return View();  // Validation hatası

            if (urun.Price <= 0)
            {
                TempData["mesaj"] = "Ürün fiyatı sıfır veya negatif olamaz!";
                return View();
            }

            var existingProduct = _db.Products.FirstOrDefault(p => p.Name == urun.Name && p.Id != urun.Id);

            if (existingProduct != null)
            {
                TempData["mesaj"] = "Bu ürün zaten var!";
                return View();
            }

            try
            {
                // Eğer veritabanında güncellenmesi gereken ürünü bulamazsanız hata döndürebilirsiniz.
                var dbUrun = _db.Products.FirstOrDefault(p => p.Id == urun.Id);
                if (dbUrun == null)
                {
                    TempData["mesaj"] = "Ürün bulunamadı!";
                    return View();
                }

                // Güncelleme işlemleri
                dbUrun.Name = urun.Name;
                dbUrun.Price = urun.Price;
                dbUrun.IsInStock = urun.IsInStock;

                _db.Products.Update(dbUrun);
                _db.SaveChanges();

                TempData["mesaj"] = "Ürün başarıyla güncellendi.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["mesaj"] = "Hata Oluştu: " + ex.Message;
                return View();
            }
        }









        public IActionResult Sil(int id)
        {
            //Önce gelen id ye ait olan ürünü bul.
            //Sonra da onu silme view'una aktar.

            return View(_db.Products.Find(id));

        }

        [HttpPost]
        public IActionResult Sil(Product urun)
        {
            //şimdi bu ürünü sil
            _db.Products.Remove(urun);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult IkinciDereceDenklem()
        {
            return View();
        }
        [HttpPost]
        public IActionResult IkinciDereceDenklem(double a, double b, double c)
        {
            if (a == 0)
            {
                TempData["mesaj"] = "Denklemin birinci terimi sıfır olamaz.";
                return View();
            }

            double delta = b * b - 4 * a * c;
            if (delta < 0)
            {
                TempData["mesaj"] = "Denklemin reel kökü yok.";
                return View();
            }
            else if (delta == 0)
            {
                double kok = -b / (2 * a);
                TempData["mesaj"] = $"Denklemin çift kökü: {kok}";
                return View();
            }
            else
            {
                double kok1 = (-b + Math.Sqrt(delta)) / (2 * a);
                double kok2 = (-b - Math.Sqrt(delta)) / (2 * a);
                TempData["mesaj"] = $"Denklemin birinci kökü: {kok1}, İkinci kökü: {kok2}";
                return View();
            }
        }

    }
}
