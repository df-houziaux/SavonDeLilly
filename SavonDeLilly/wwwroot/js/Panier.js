"use strict";

// Attendre que le DOM soit complètement chargé
document.addEventListener("DOMContentLoaded", function () {
    const cartItemsList = document.getElementById('cartItemsList');
    const cartTotalElement = document.getElementById('cartTotal');
    const clearCartBtn = document.getElementById('clearCartBtn');
    const validateOrderBtn = document.getElementById('validateOrderBtn');
    const paypalButtonContainer = document.getElementById('paypal-button-container');

    let cartProducts = JSON.parse(localStorage.getItem('cartProducts')) || [];

    function loadCart() {
        const cartTableBody = cartItemsList.querySelector('tbody');
        cartTableBody.innerHTML = '';
        let total = 0;

        if (cartProducts.length === 0) {
            cartTableBody.innerHTML = '<tr><td colspan="5">Votre panier est vide.</td></tr>';
            cartTotalElement.textContent = "0.00 €";
            return;
        }

        cartProducts.forEach((item, index) => {
            const itemTotal = item.price * item.quantity;
            total += itemTotal;

            const row = document.createElement('tr');
            row.innerHTML = `
                <td>${item.name}</td>
                <td>${item.price.toFixed(2)} €</td>
                <td>
                    <input type="number" value="${item.quantity}" min="1" max="${item.stock}" data-index="${index}">
                </td>
                <td>${itemTotal.toFixed(2)} €</td>
                <td>
                    <img src="/images/corbeillerouge.jpg" alt="Supprimer" class="remove-btn" data-index="${index}" style="cursor: pointer; width: 30px; height: 30px;">
                </td>
            `;
            cartTableBody.appendChild(row);
        });

        cartTotalElement.textContent = total.toFixed(2) + ' €';
        addEventListeners();
    }

    function addEventListeners() {
        const quantityInputs = cartItemsList.querySelectorAll('input[type="number"]');
        quantityInputs.forEach(input => {
            input.addEventListener('change', event => {
                const index = event.target.getAttribute('data-index');
                updateQuantity(index, event.target.value);
            });
        });

        const removeButtons = cartItemsList.querySelectorAll('.remove-btn');
        removeButtons.forEach(button => {
            button.addEventListener('click', () => {
                const index = button.getAttribute('data-index');
                removeFromCart(index);
            });
        });
    }

    function updateQuantity(index, newQuantity) {
        cartProducts[index].quantity = Math.max(1, Math.min(cartProducts[index].stock, parseInt(newQuantity)));
        saveCart();
        loadCart();
    }

    function removeFromCart(index) {
        cartProducts.splice(index, 1);
        saveCart();
        loadCart();
    }

    function saveCart() {
        localStorage.setItem('cartProducts', JSON.stringify(cartProducts));
    }

    clearCartBtn.addEventListener('click', () => {
        if (confirm("Êtes-vous sûr de vouloir vider le panier ?")) {
            cartProducts = [];
            saveCart();
            loadCart();
        }
    });

    validateOrderBtn.addEventListener('click', () => {
        const paymentMethod = document.querySelector('input[name="paymentMethod"]:checked');
        if (!paymentMethod) {
            alert("Veuillez choisir un mode de paiement.");
            return;
        }

        if (paymentMethod.value === "paypal") {
            paypalButtonContainer.style.display = "block";
            loadPayPalButton(cartTotalElement.textContent.replace(' €', ''));
        } else {
            window.location.href = "/Payment/ProcessPayment?method=cash";
        }
    });

    function loadPayPalButton(totalAmount) {
        paypal.Buttons({
            createOrder: function (data, actions) {
                return actions.order.create({
                    purchase_units: [{
                        amount: { value: totalAmount }
                    }]
                });
            },
            onApprove: function (data, actions) {
                return actions.order.capture().then(function (details) {
                    alert('Paiement réussi par ' + details.payer.name.given_name);
                    window.location.href = "/Payment/Confirmation";
                });
            },
            onError: function (err) {
                console.error('Paiement PayPal échoué', err);
                alert('Une erreur est survenue lors du paiement.');
            }
        }).render('#paypal-button-container');
    }

    loadCart();
});
