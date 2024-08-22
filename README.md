# Dice Royal
---
Merhaba,
17 Ağustos'ta planlamasına başladığım ve 18 Ağustos'ta geliştirmeye geçtiğim bu case çalışmasını sizlere sunmaktan mutluluk duyuyorum. Projeyi geliştirirken, sürdürülebilir bir mimari oluşturmaya özellikle dikkat ettim. Bu sayede, temel oynanış sistemi, farklı hedefler doğrultusunda esnek bir şekilde uyarlanabilir hale getirildi. Şimdi, dilerseniz proje hakkında detaylı bilgilere geçelim.

# Oynanış
---

https://github.com/user-attachments/assets/b937715e-b679-4b5f-9d29-e8f6c6167cd1

# Harita Oluşumu
---
Harita oluşturma sistemi için 3 farklı alternatif ürettim. Case çalışmamın içerisindeki sahnede bulunan "Table Manager" objesinin içerisindeki açılır listeden herhangi birisi seçilerek sahne oluşumu sağlanabilecektir.
Ayrıca bana verilen dokümanda belirtilen çizgi şeklindeki harita talebine ek olarak kare, dikdörtgen ya da zikzak şekillerinde sahne üretimi de mümkündür.

<p align="center">
<img width="507" alt="Table Manager" src="https://github.com/user-attachments/assets/90ea19af-b38f-4c85-9f19-2a23ac0f9307">
</p>

## Level Editor
JSON ile sahne oluşturmaya geçmeden önce bu proje için hazırladığım Level Editörümü size tanıtmak istiyorum. Görselde de görüldüğü üzere "OD Projects > Level Editor" yolu takip edilerek editör penceresinden istenilen harita tipi, istenilen boyutta oluşturulup içerisindeki hücreler istenilen envanter elemanları ile doldurulabilmektedir. Arzu edilirse bu editör ile farklı level tasarımları hazırlanıp level bazlı bir sistem de oluşturulabilir.

<table>
  <tr>
    <td><img width="556" alt="Line Level" src="https://github.com/user-attachments/assets/7c80f9c4-6030-4e32-86be-d2ea5e0dedd1"></td>
    <td><img width="556" alt="Square Level" src="https://github.com/user-attachments/assets/cb1f221b-57d7-47c7-87fe-5eaf1fcd88a8"></td>
  </tr>
</table>

## JSON
Hazırlanacak olan JSON dosyası ile sahnede istenilen hücreleri oluşturulabilir.

## Random
Bu özellik sayesinde stenilen hücre sayısı ve şeklinin içeriği randomize bir şekilde oluşturulabilir.

# Zar Mekaniği
---
Sahnemizdeki zar sistemi için fizik tabanlı bir simülasyon gerçekleştirdim. Bu zar sistemi hem kullanıcıya gerçek bir zar atma deneyimi yaşatmayı, hem de geliştirici ortamındaki ayarlamalar sayesinde istenilen zar değerlerinin ayarlanabilmesini amaçlamaktadır. Zar sayısını ve bu zarlar için istenilen değerleri kullanıcı arayüzündeki açılır pencereler aracılığıyla ayarlayabilmekteyiz.

<p align="center">
<img width="767" alt="Screenshot 2024-08-22 at 08 36 34" src="https://github.com/user-attachments/assets/f0a5e85b-6c16-4ddc-9b80-6c9a23252236">
</p>

# Proje Düzeni
---
Projenin genel yapısını sürdürülebilir ve olabildiğince karmaşıklıktan uzak olacak şekilde ayarladım. Klasör hiyerarşisini ve kod yapılarını buna göre düzenledim.

<p align="center">
<img width="293" alt="Screenshot 2024-08-22 at 08 38 31" src="https://github.com/user-attachments/assets/ab100159-740f-4f73-967b-281c78bd46d6">
</p>

# Teşekkür
Vaktinizi ayırıp, titizlikle hazırladığım bu çalışmayı inceleyeceğiniz için şimdiden teşekkür ederim. Sizlerle tanışmak için sabırsızlanıyorum. 
