﻿let cart = JSON.parse(localStorage.getItem("cart")) || [];

// Sepet sayacını güncelleme
function updateCartCount() {
    const cart = JSON.parse(localStorage.getItem('cart')) || [];
    const totalItems = cart.reduce((sum, item) => sum + item.Quantity, 0); // Tüm ürünlerin miktarlarını topla
    const cartCountElement = document.getElementById("cart-count");

    if (cartCountElement) {
        cartCountElement.innerText = totalItems; // Sepet toplam miktarını göster
    }
}

// Ürün ekleme
function addToCart(menuItemId, name, price) {
    let cart = JSON.parse(localStorage.getItem('cart')) || [];
    const existingItem = cart.find(item => item.MenuItemId === menuItemId);

    if (existingItem) {
        existingItem.Quantity++; // Mevcut ürünün miktarını artır
    } else {
        cart.push({ MenuItemId: menuItemId, Name: name, Price: price, Quantity: 1 });
    }

    localStorage.setItem('cart', JSON.stringify(cart));
    updateCartCount(); // İkonu güncelle
    alert('Ürün sepete eklendi!'); // Ürün eklenme mesajı
    loadCart();
}

// Sepet içeriğini localStorage'dan yükle ve göster
function loadCart() {
    let cart = JSON.parse(localStorage.getItem('cart')) || [];
    const cartContents = document.getElementById('cartContents');
    cartContents.innerHTML = '';

    if (cart.length === 0) {
        cartContents.innerHTML = '<p class="text-center text-muted">Sepetiniz boş!</p>';
        updateCartCount(); // İkonu sıfırla
        return;
    }

    cart.forEach((item, index) => {
        const itemElement = document.createElement('div');
        itemElement.classList.add('cart-item', 'mb-3', 'p-3', 'border', 'rounded');
        itemElement.innerHTML = `
            <div class="d-flex justify-content-between align-items-center">
                <div>
                    <strong>${item.Name}</strong><br>
                    <small>${item.Quantity} adet - ${(item.Price * item.Quantity).toFixed(2)}₺</small>
                </div>
                <button class="btn btn-danger btn-sm" onclick="removeFromCart(${index})">Çıkar</button>
            </div>
        `;
        cartContents.appendChild(itemElement);
    });

    updateCartCount(); // İkonu güncelle
}

// Sepetten ürün çıkarma
function removeFromCart(index) {
    let cart = JSON.parse(localStorage.getItem('cart')) || [];
    cart[index].Quantity--; // Miktarı azalt

    if (cart[index].Quantity === 0) {
        cart.splice(index, 1); // Ürün miktarı sıfırsa tamamen kaldır
    }

    localStorage.setItem('cart', JSON.stringify(cart));
    loadCart(); // Sepeti güncelle
    updateCartCount(); // İkonu güncelle
}

// Ödeme işlemini başlatma
function submitCart() {
    let cart = JSON.parse(localStorage.getItem('cart')) || [];
    if (cart.length === 0) {
        alert('Sepetiniz boş!');
        return;
    }

    console.log('Sepet gönderiliyor:', cart);

    fetch('/QrCodeList/Cart', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(cart)
    })
        .then(response => response.json())
        .then(data => {
            alert(data.message || 'Ödeme işlemi başarıyla tamamlandı.');
            localStorage.removeItem('cart'); // Sepeti temizle
            loadCart(); // Ekranı güncelle
            updateCartCount(); // İkonu sıfırla
        })
        .catch(error => {
            console.error('Hata:', error);
            alert('Bir hata oluştu. Lütfen tekrar deneyin.');
        });
}

// Sayfa yüklendiğinde işlemleri başlat
window.onload = function () {
    loadCart();
    updateCartCount(); // Sayfa yüklendiğinde ikon güncellenmeli
};