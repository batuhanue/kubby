# Küpik — Yol Haritası

İzometrik, fizik tabanlı puzzle-platformer. Unity 2022.3 LTS, 3D, ortografik izometrik kamera, Rigidbody fizik. **Öncelik: Android + dikey (portrait) ekran.**

---

## Asset Değerlendirmesi

Eldeki 4 paketten yalnızca biri 3D fizik planına doğrudan uyar:

| Paket | Tür | Karar |
|---|---|---|
| **Kenney Cube Pets** | 3D model (FBX/GLB/OBJ), 24 hayvan | ✅ **Kullan.** Karakter/hayvanlar için ideal. `animal-cat` → Küpik (maviye boyanır). Modeller statik → squash/stretch kodla yapılır. |
| **Kenney Isometric Blocks** | 2D PNG sprite | ⚠️ 3D fizik dünyasına girmez. Sadece renk/doku referansı veya UI ikonu. |
| **Kenney Isometric Miniature Bases** | 2D PNG sprite | ⚠️ Bu projeyle ilgisiz. Kullanılmıyor. |
| **Quaternius Universal Animation Library 2** | Humanoid FBX + animasyon | ⚠️ Yalnızca **humanoid** karaktere retarget olur. Cube-Pet kedi humanoid değil → şimdilik rafta. |

**Sonuç:** Blok dünyası 3D primitive küplerden üretilmeye devam eder (`KupikLevelBuilder`). Karakter olarak Cube-Pet kedi hedeflenir; ilk aşamada kapsül placeholder kullanılır.

---

## Teknik Kararlar (bu oturumda alındı)

- **Karakter:** Şimdilik kapsül placeholder. Cube-Pet kedi Faz 2'de bağlanır.
- **Render:** URP'ye geçiş (mobil için).
- **Kontrol:** Klavye (editör testi) + ekran joystick & zıpla butonu (Android). İkisi paralel çalışır.
- **Ekran:** Portrait'e kilitli (ProjectSettings güncellendi).

---

## Fazlar

### Faz 0 — Hazırlık ✔ (kısmen bu oturumda)
- [x] Portrait orientation kilidi
- [x] Mobil input sistemi + tek tık UI kurulumu
- [x] URP paketi manifest'e eklendi
- [ ] Cube Pets import + Küpik prefab
- [ ] (Ops.) Git LFS ile büyük binary asset yönetimi

### Faz 1 — Mobil temel (şu an burada)
- [x] Ekran joystick + zıpla butonu (`KupikInput`, `VirtualJoystick`, `TouchButton`)
- [x] Portrait kamera (yatay görüş sabitleme)
- [ ] Coyote-time + jump buffer cilası
- [ ] Squash/stretch (iniş/zıplama)

### Faz 2 — Çekirdek mekanikler
- [ ] İtilebilir kutu (Rigidbody, ağırlık)
- [ ] Buton → açılan kapı
- [ ] Hareketli platform (üstünde titremesiz taşıma)
- [ ] Yaylı ped
- [ ] Bitiş kapısı + checkpoint (düşünce geri dönme)

### Faz 3 — İlk bölüm & cila
- [ ] Bölüm akışı: başlangıç → boşluk → kutu → buton → kapı → platform → ped → çıkış
- [ ] Su/şelale, ağaç, çiçek detayları
- [ ] Ses + basit UI (bölüm sonu)

### Faz 4 — Android build
- [ ] APK, gerçek cihaz testi, dokunmatik his ayarı, performans

---

## Kurulum — Unity'de yapman gerekenler

### 1. Mobil kontrolleri sahneye ekle
Menüden: **Kupik > Setup > Create Mobile Controls (Portrait)**
Bu, Canvas + EventSystem + sol alt joystick + sağ alt zıpla butonunu otomatik kurar ve bağlar.

### 2. Sahne bağlantıları
- Oyuncu (kapsül) üzerinde **PlayerController** olmalı; `Ground Layer` alanında **Ground** layer'ı işaretli olmalı.
- Main Camera üzerinde **CameraFollow** olmalı ve `Target` = oyuncu atanmalı.
- Test: Editörde WASD + Space; Game view'ı **Portrait (1080x1920)** çözünürlüğe al, Simulator ile dokunmatiği dene.

### 3. URP'ye geçiş (paket zaten manifest'te)
1. Proje açılınca URP paketi kurulur.
2. **Assets > Create > Rendering > URP Asset (with Universal Renderer)** ile bir URP asset oluştur.
3. **Project Settings > Graphics** → Scriptable Render Pipeline Settings'e bu asset'i ata (ve Quality ayarlarına da).
4. **Edit > Rendering > Materials > Convert All Built-in Materials to URP** ile mevcut Standard materyalleri dönüştür (aksi halde pembe görünürler).

> Not: URP asset'i atanana kadar proje Built-in ile çalışmaya devam eder; bir şey bozulmaz.

---

## Kod Mimarisi (mevcut)

```
Assets/_Project/Scripts/
  Input/
    KupikInput.cs        # Klavye + dokunmatiği birleştiren tek giriş noktası
    VirtualJoystick.cs   # uGUI sanal joystick
    TouchButton.cs       # Dokunmatik aksiyon butonu (zıpla)
  Player/
    PlayerController.cs   # Rigidbody hareket + kameraya göre yön + zıplama
  Camera/
    CameraFollow.cs       # İzometrik takip + portrait yatay görüş sabitleme
  Editor/
    KubikSceneSetup.cs        # Prototip sahne kurucu
    Kubiklevelbuilder.cs      # Harita string'inden küp ada üretici
    KupikMobileUISetup.cs     # Tek tık mobil UI kurucu
```
