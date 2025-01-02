function toggleExpand(element) {
    element.classList.toggle('expanded');
}

function changeQuantity(itemId, change) {
    const quantityInput = document.getElementById('quantity-' + itemId);
    let currentQuantity = parseInt(quantityInput.value);
    const newQuantity = currentQuantity + change;

    if (newQuantity >= 1) {
        quantityInput.value = newQuantity;
    }
}

function handleAddToCart(itemId, name, price) {
    const quantityInput = document.getElementById('quantity-' + itemId);
    const quantity = parseInt(quantityInput.value);

    // Sepete ekleme işlemi cart.js'den çağırılıyor
    addToCart(itemId, name, price, quantity);
}

function goToCart() {
    // Sepet sayfasına yönlendirme
    window.location.href = "/QrCodeList/Cart"; // Sepet sayfanızın URL'sini buraya ekleyin
}