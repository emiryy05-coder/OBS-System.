# Öğrenci Bilgi Sistemi (Student Information System)

Bu proje, ASP.NET Core MVC mimarisi ve Entity Framework Core kullanılarak geliştirilmiş, akademik süreçleri dijitalleştirmeyi amaçlayan kapsamlı bir Öğrenci Bilgi Sistemi web uygulamasıdır. Proje, Admin (Yönetici) ve Öğrenci olmak üzere iki temel rol için özelleştirilmiş paneller ve işlevler sunmaktadır.

## Sistem Rolleri ve Özellikleri

### 1. Kimlik Doğrulama ve Giriş (Login)
* Admin ve Öğrenci rolleri için güvenli ve ayrıştırılmış giriş panelleri.(Admin geçici olarak öğrencilerle aynı tabloda yer aldı) 
* Kullanıcı rolüne göre dinamik olarak yönlendirilen yetkilendirilmiş sayfalar.

### 2. Öğrenci Paneli
* **Ders Notları:** Dönem içi vize, final ve bütünleme notlarının anlık takibi.
* **Ders Kaydı:** Dönemlik ders seçimi ve onay durumunun izlenmesi.
* **Transkript:** Akademik geçmişi, alınan dersleri, harf notlarını ve Genel Akademik Not Ortalamasını (GANO) gösteren detaylı döküm.

### 3. Yönetici (Admin) Paneli
* **Öğrenci Yönetimi:** Sisteme yeni öğrenci ekleme, mevcut bilgileri güncelleme ve silme (CRUD) işlemleri.
* **Ders Yönetimi:** Müfredatta yer alan dersleri tanımlama, içerik düzenleme ve yayından kaldırma (CRUD) işlemleri.
* **Ders Kaydı Onaylama:** Öğrencilerin dönemlik seçtiği derslerin kontrol edilmesi, onaylanması veya reddedilmesi süreçlerinin yönetimi.

## Kullanılan Teknolojiler

* **Arka Plan (Backend):** C#, .NET, ASP.NET Core MVC
* **Veri Erişimi (ORM):** Entity Framework Core (Code-First veya DB-First)
* **Veri Tabanı:** Microsoft SQL Server (MSSQL)
* **Ön Yüz (Frontend):** HTML5, CSS3, Bootstrap, Razor Pages (Dinamik Arayüz Yapısı)
* **Mimari Desen:** Model-View-Controller (MVC)

## Proje Yapısı ve Klasör Düzeni

* **WebApplication2/** - Ana uygulama katmanı.
* **Controllers/** - İstemci isteklerini işleyen, iş mantığını ve View-Model arasındaki veri akışını yöneten kontrolcüler.
* **Models/** - Veri tabanı tablolarını temsil eden Entity sınıfları, Context nesnesi ve View-Model (VM) yapıları.
* **Views/** - Kullanıcıların etkileşime girdiği, Razor söz dizimi ile zenginleştirilmiş arayüz dosyaları.
* **wwwroot/** - Uygulamanın ihtiyaç duyduğu CSS, JavaScript, kütüphaneler ve görsel bileşenler gibi statik dosyalar.

## Kurulum ve Çalıştırma

Projeyi yerel bilgisayarınızda ayağa kaldırmak için aşağıdaki adımları sırasıyla uygulayabilirsiniz:

### Gereksinimler
* Visual Studio 2022 (veya güncel bir .NET destekleyen IDE)
* .NET SDK (Projenin geliştirildiği sürüme uygun SDK)
* Microsoft SQL Server (Lokal veya Uzak Sunucu)

### Adımlar

1. Projeyi yerel bilgisayarınıza klonlayın:
   ```bash
   git clone https://github.com/kullanici-adi/WebApplication2.git
   ```

2. Proje dizinine giriş yapın:
   ```bash
   cd WebApplication2
   ```

3. `WebApplication2.sln` çözüm (solution) dosyasını Visual Studio ile açın.

4. Veri tabanı bağlantısını yapılandırın:
   `WebApplication2` projesinin içinde yer alan `appsettings.json` dosyasını açarak `ConnectionStrings` alanını kendi SQL Server bilgilerinize göre güncelleyin:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=OgrenciBilgiSistemiDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;"
   }
   ```

5. Veri tabanı tablolarını oluşturun:
   Visual Studio içerisinde **Package Manager Console** (Paket Yöneticisi Konsolu) ekranını açın ve aşağıdaki komutları sırasıyla çalıştırarak migration işlemlerini tamamlayın:
   ```bash
   Add-Migration InitialCreate
   Update-Database
   ```

6. Projeyi başlatın:
   Visual Studio üzerinde `F5` tuşuna basarak veya `Run` butonunu kullanarak projeyi tarayıcınızda test edebilirsiniz.

## Lisans

Bu proje MIT Lisansı ile lisanslanmıştır. Daha fazla bilgi için LICENSE dosyasını inceleyebilirsiniz.
