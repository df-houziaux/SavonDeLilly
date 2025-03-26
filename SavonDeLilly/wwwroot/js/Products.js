"use strict";

document.addEventListener("DOMContentLoaded", function () {
    const addToCartButtons = document.querySelectorAll('.add-to-cart-btn');
    const cartMessageElement = document.getElementById('cart-message');

    // Vérification que des boutons existent
    if (addToCartButtons.length === 0) {
        console.error("Aucun bouton 'Ajouter au panier' trouvé !");
        return; // Sortir si aucun bouton n'est trouvé
    }

    addToCartButtons.forEach(button => {
        button.addEventListener('click', () => {
            const name = button.getAttribute('data-product-name');
            const price = parseFloat(button.getAttribute('data-product-price'));
            const stock = parseInt(button.getAttribute('data-product-stock'), 10);

            console.log(`Ajout du produit : ${name}, Prix : ${price}, Stock : ${stock}`); // Debug

            if (name && !isNaN(price) && !isNaN(stock)) {
                addToCart(name, price, stock);
            } else {
                console.error("Données de produit invalides.");
            }
        });
    });

    function showCartMessage(message) {
        if (cartMessageElement) {
            cartMessageElement.textContent = message;
            cartMessageElement.style.display = "block";
            cartMessageElement.style.opacity = "1";

            // Faire défiler jusqu'au message
            cartMessageElement.scrollIntoView({ behavior: "smooth", block: "center" });

            // Faire disparaître le message après 3 secondes
            setTimeout(() => {
                cartMessageElement.style.opacity = "0";
                setTimeout(() => {
                    cartMessageElement.style.display = "none";
                }, 500);
            }, 3000);
        } else {
            console.error("L'élément de message du panier est introuvable !");
        }
    }

    function addToCart(name, price, stock) {
        let cartProducts = JSON.parse(localStorage.getItem('cartProducts')) || [];
        const product = cartProducts.find(p => p.name === name);
        let message = "";

        if (product) {
            if (product.quantity < stock) {
                product.quantity++;
                message = `${name} a été ajouté au panier !`;
            } else {
                message = `Stock épuisé pour ${name}.`;
            }
        } else {
            cartProducts.push({ name, price, quantity: 1, stock });
            message = `${name} a été ajouté au panier !`;
        }

        localStorage.setItem('cartProducts', JSON.stringify(cartProducts));
        showCartMessage(message);
    }
});
