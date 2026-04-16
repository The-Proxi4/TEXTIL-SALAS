const demoProducts = [
  { name: 'Toalla de Mano Blanca', qty: 100, price: 70 },
  { name: 'Toalla de Piso Blanca', qty: 50, price: 85 },
  { name: 'Bata de Baño Blanca', qty: 25, price: 100 }
];

const detail = document.getElementById('quote-detail');
const totalEl = document.getElementById('quote-total');

if (detail && totalEl) {
  let total = 0;
  detail.innerHTML = demoProducts.map(item => {
    const subtotal = item.qty * item.price;
    total += subtotal;
    return `
      <div class="quote-row">
        <strong>${item.name}</strong><br>
        Cantidad: ${item.qty}<br>
        Precio referencial: S/${item.price.toFixed(2)}<br>
        Subtotal: S/${subtotal.toFixed(2)}
      </div>`;
  }).join('');
  totalEl.textContent = `S/ ${total.toFixed(2)}`;
}
