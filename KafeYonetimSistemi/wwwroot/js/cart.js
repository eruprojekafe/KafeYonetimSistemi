// Sepeti localStorage'dan almak için yardımcı bir fonksiyon
function getCart() {
    return JSON.parse(localStorage.getItem("cart")) || [];
}

// Sepeti localStorage'da güncellemek için yardımcı bir fonksiyon
function setCart(cart) {
    localStorage.setItem('cart', JSON.stringify(cart));
}

// Sepet sayacını güncelleme
function updateCartCount() {
    const cart = getCart();
    const totalItems = cart.reduce((sum, item) => sum + item.Quantity, 0); // Tüm ürünlerin miktarlarını topla
    const cartCountElement = document.getElementById("cart-count");

    if (cartCountElement) {
        cartCountElement.innerText = totalItems; // Sepet toplam miktarını göster
    }
}

// Ürün ekleme
function addToCart(itemId, name, price, quantity) {
    const cart = JSON.parse(localStorage.getItem("cart")) || [];
    const existingItem = cart.find(item => item.MenuItemId === itemId);

    if (existingItem) {
        existingItem.Quantity += quantity; // Mevcut ürün miktarını güncelle
    } else {
        cart.push({
            MenuItemId: itemId,
            Name: name,
            Price: price,
            Quantity: quantity // Yeni ürün ekle
        });
    }

    localStorage.setItem("cart", JSON.stringify(cart));
    alert('${ quantity } adet ${ name } sepete eklendi!');
    updateCartCount(); // Sepet simgesindeki sayıyı güncelle
}

// Sepet içeriğini localStorage'dan yükle ve göster
function loadCart() {
    const cart = getCart();
    const cartContents = document.getElementById('cartContents');

    // Eğer 'cartContents' öğesi yoksa fonksiyonu çalıştırmamalıyız
    if (!cartContents) {
        console.error("cartContents öğesi bulunamadı.");
        return;
    }

    cartContents.innerHTML = '';

    if (cart.length === 0) {
        cartContents.innerHTML = '<p class="text-center text-muted">Sepetiniz boş!</p>';
        updateCartCount(); // İkonu sıfırla
        togglePaymentButton(); // Ödeme Yap butonunu güncelle
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
    togglePaymentButton(); // Ödeme Yap butonunu güncelle
}

// Sepetten ürün çıkarma
function removeFromCart(index) {
    const cart = getCart();
    cart[index].Quantity--; // Miktarı azalt

    if (cart[index].Quantity === 0) {
        cart.splice(index, 1); // Ürün miktarı sıfırsa tamamen kaldır
    }

    setCart(cart); // Sepeti güncelle
    loadCart(); // Sepeti güncelle
    updateCartCount(); // İkonu güncelle
}
// Ödeme Yap butonunu gizle/göster
function togglePaymentButton() {
    const cart = getCart();
    const paymentButton = document.getElementById('paymentButton');

    if (!paymentButton) {
        console.error("paymentButton öğesi bulunamadı.");
        return; // Hata alıyorsanız, fonksiyonu sonlandır
    }

    if (cart.length === 0) {
        paymentButton.style.display = 'none'; // Sepet boşsa gizle
    } else {
        paymentButton.style.display = 'inline-block'; // Sepette ürün varsa göster
    }
}

// Ödeme işlemini başlatma
function submitCart() {
    const cart = getCart(); // Sepeti al
    if (!cart || cart.length === 0) {
        alert('Sepetiniz boş!');
        return;
    }

    // Toplam tutarı hesapla
    const totalAmount = cart.reduce((sum, item) => {
        const price = parseFloat(item.Price);
        const quantity = parseInt(item.Quantity, 10);

        if (isNaN(price) || isNaN(quantity)) {
            console.error(`Geçersiz fiyat veya miktar: Fiyat=${item.Price}, Miktar=${item.Quantity}`);
            return sum;
        }

        return sum + price * quantity;
    }, 0);

    // Toplam tutarı HTML'ye yaz
    const totalAmountElement = document.getElementById('totalAmount');
    if (totalAmountElement) {
        totalAmountElement.innerText = totalAmount.toFixed(2); // Ondalık format
    } else {
        console.error('Toplam tutar elemanı bulunamadı.');
    }

    // Masa numarasını al
    const tableNumber = "@(Model.TableNumber)";

    // URL oluştur ve yönlendir
    const queryString = new URLSearchParams({
        totalAmount: totalAmount.toFixed(2),
        cartItems: JSON.stringify(cart),
        tableNumber: tableNumber
    }).toString();

    window.location.href = `/QrCodeList/Buy?${queryString}`;
}

// Sayfa yüklendiğinde işlemleri başlat
window.addEventListener('DOMContentLoaded', function () {
    loadCart();
    updateCartCount(); // Sayfa yüklendiğinde ikon güncellenmeli
    togglePaymentButton();
});