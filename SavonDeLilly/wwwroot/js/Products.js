"use strict"

// Éléments DOM
const cartCount = document.getElementById('cartCount');
const cartDetailsContainer = document.getElementById('cartDetailsContainer');
const cartIconLink = document.getElementById('cartIconLink');
const clearCartBtn = document.getElementById('clearCartBtn');
const closeCartBtn = document.getElementById('closeCartBtn');
const cartItemsList = document.getElementById('cartItemsList');
const cartTotalElement = document.getElementById('cartTotal');
const validateOrderBtn = document.getElementById('validateOrderBtn');
const productContainer = document.getElementById('product-container');
const searchInput = document.getElementById('searchInput');
const searchFilter = document.getElementById('searchFilter');
const searchForm = document.getElementById('searchForm');

// Constantes pour les catégories de produits
const categories = {
    "Lavande": "#E3E6F5",
    "Rosée": "#F6D5D5",
    "Vanille": "#F7E8C5",
};

// Récupération des produits du panier depuis localStorage
let cartProducts = JSON.parse(localStorage.getItem('cartProducts')) || [];

// Met à jour le compteur du panier
function updateCartCount() {
    const cartCountValue = cartProducts.reduce((sum, product) => sum + product.quantity, 0);
    cartCount.textContent = cartCountValue;
}

// Affiche les détails du panier
function showCartDetails() {
    cartItemsList.innerHTML = '';

    for (let i = 0; i < cartProducts.length; i++) {
        const listItem = document.createElement('li');
        const stockMessage = cartProducts[i].quantity >= cartProducts[i].stock ? ' (Stock épuisé)' : '';

        // Formatage du texte avec lien pour rediriger vers la page de la catégorie
        const productLink = document.createElement('a');
        productLink.href = `/${cartProducts[i].category}`;
        productLink.textContent = `${cartProducts[i].name} ${cartProducts[i].quantity} x ${cartProducts[i].price.toFixed(2)} € ${stockMessage}`;
        productLink.target = "_blank";
        listItem.appendChild(productLink);

        // Créer un conteneur pour les boutons + et - et le champ de saisie
        const quantityContainer = document.createElement('div');
        quantityContainer.style.display = 'inline-flex';
        quantityContainer.style.alignItems = 'center';
        quantityContainer.style.marginLeft = '15px';

        // Bouton pour diminuer la quantité
        const decreaseBtn = document.createElement('button');
        decreaseBtn.textContent = '-';
        decreaseBtn.style.marginRight = '15px';
        decreaseBtn.onclick = () => {
            if (cartProducts[i].quantity > 1) {
                cartProducts[i].quantity--;
                saveCart();
                showCartDetails();
            }
        };

        // Champ de saisie pour la quantité
        const quantityInput = document.createElement('input');
        quantityInput.type = 'number';
        quantityInput.value = cartProducts[i].quantity;
        quantityInput.min = 1;
        quantityInput.max = cartProducts[i].stock; // Updated to use the actual stock
        quantityInput.style.width = '50px';
        quantityInput.style.marginRight = '15px';

        // Événement pour mettre à jour la quantité
        quantityInput.onchange = () => {
            let newQuantity = parseInt(quantityInput.value);
            if (newQuantity < 1) newQuantity = 1;
            if (newQuantity > cartProducts[i].stock) {
                alert(`Vous ne pouvez pas commander plus de ${cartProducts[i].stock} unités de ce produit.`);
                newQuantity = cartProducts[i].stock;
            }
            cartProducts[i].quantity = newQuantity;
            saveCart();
            showCartDetails();
        };

        // Bouton pour augmenter la quantité
        const increaseBtn = document.createElement('button');
        increaseBtn.textContent = '+';
        increaseBtn.style.marginLeft = '15px';
        increaseBtn.onclick = () => {
            if (cartProducts[i].quantity < cartProducts[i].stock) {
                cartProducts[i].quantity++;
                saveCart();
                showCartDetails();
            } else {
                alert('Stock épuisé pour ce produit.');
            }
        };

        // Bouton de suppression
        const boutonSuppresionArticle = document.createElement('button');
        boutonSuppresionArticle.classList.add("btnSuppresion");
        boutonSuppresionArticle.onclick = () => removeFromCart(cartProducts[i].name);

        const image = document.createElement('img');
        image.src = '/images/corbeillerouge.jpg';
        image.style.width = "30px";
        image.style.height = "30px";
        boutonSuppresionArticle.appendChild(image);

        // Ajouter les éléments au conteneur
        quantityContainer.appendChild(decreaseBtn);
        quantityContainer.appendChild(quantityInput);
        quantityContainer.appendChild(increaseBtn);
        quantityContainer.appendChild(boutonSuppresionArticle);

        // Ajouter le conteneur de quantité au listItem
        listItem.appendChild(quantityContainer);
        cartItemsList.appendChild(listItem);
    }
    cartDetailsContainer.classList.toggle('show', cartProducts.length > 0);
    updateCartTotal();
    updateProductStockDisplay();
}

// Met à jour visuellement le stock des produits
function updateProductStockDisplay() {
    const products = Array.from(productContainer.children);
    products.forEach(product => {
        if (product) {
            const name = product.getAttribute('data-name');
            const stock = parseInt(product.getAttribute('data-stock'), 10);
            const cartProduct = cartProducts.find(p => p.name === name);
            const quantityInCart = cartProduct ? cartProduct.quantity : 0;
            const stockDisplay = product.querySelector('.card-stock');

            // Au lieu de remplacer tout le HTML, mise à jour du texte existant
            if (stockDisplay) {
                const stockSpan = stockDisplay.querySelector('span');
                if (stockSpan) {
                    if (stock <= 0) {
                        stockSpan.textContent = "Épuisé";
                        stockSpan.className = "stock-out";
                    } else if (quantityInCart >= stock) {
                        stockSpan.textContent = "Rupture de stock";
                        stockSpan.className = "stock-out";
                    } else {
                        stockSpan.textContent = `Stock restant : ${stock - quantityInCart}`;
                        stockSpan.className = "stock-in";
                    }
                }
            }
        }
    });
}

// Met à jour le total du panier
function updateCartTotal() {
    const total = cartProducts.reduce((sum, product) => sum + (product.price * product.quantity), 0);
    cartTotalElement.textContent = `${total.toFixed(2)} €`;
}

// Ajoute un produit au panier
function addToCart(name, price, stock, category) {
    const product = cartProducts.find(p => p.name === name);
    if (product) {
        if (product.quantity < stock) {
            product.quantity++;
        } else {
            alert('Stock épuisé pour ce produit.');
        }
    } else {
        cartProducts.push({ name, price, quantity: 1, stock, category });
    }
    saveCart();
    showCartDetails();
    updateCartTotal();
    alert(`${name} ajouté au panier !`);
}

// Supprime un produit du panier
function removeFromCart(name) {
    const index = cartProducts.findIndex(product => product.name === name);
    if (index !== -1) {
        cartProducts.splice(index, 1);
        saveCart();
        showCartDetails();
    }
}

// Sauvegarde le panier dans le localStorage
function saveCart() {
    localStorage.setItem('cartProducts', JSON.stringify(cartProducts));
    updateCartCount();
}

// Gère l'affichage/fermeture du panier
cartIconLink.addEventListener('click', event => {
    event.preventDefault();
    showCartDetails();
});

closeCartBtn.addEventListener('click', () => {
    cartDetailsContainer.classList.remove('show');
});

// Vider le panier
clearCartBtn.addEventListener('click', () => {
    if (confirm("Êtes-vous sûr de vouloir vider le panier ?")) {
        cartProducts = [];
        saveCart();
        showCartDetails();
    }
});

// Ajouter un produit depuis les boutons "Ajouter au panier"
document.body.addEventListener('click', event => {
    if (event.target.classList.contains('add-to-cart-btn')) {
        const name = event.target.getAttribute('data-product-name');
        const price = parseFloat(event.target.getAttribute('data-product-price').replace(',', '.'));
        const stock = parseInt(event.target.getAttribute('data-product-stock'), 10);
        const category = event.target.getAttribute('data-product-category');
        if (name && !isNaN(price) && !isNaN(stock)) {
            addToCart(name, price, stock, category);
        } else {
            alert("Données de produit invalides.");
        }
    }
});

// Valider la commande
validateOrderBtn.addEventListener('click', () => {
    const outOfStock = cartProducts.some(product => product.quantity > product.stock);
    if (outOfStock) {
        alert('Votre commande contient des articles en rupture de stock. Veuillez ajuster votre panier.');
        return;
    }
    fetch('/api/cart/save', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(cartProducts)
    })
        .then(response => {
            if (response.ok) {
                alert('Commande validée !');
                cartProducts = [];
                saveCart();
                showCartDetails();
                // Rediriger vers le paiement ou la confirmation
                window.location.href = '/confirmation';
            } else {
                alert('Erreur lors de la validation de la commande. Veuillez réessayer.');
            }
        })
        .catch(error => {
            console.error('Erreur:', error);
        });
});

// Afficher les détails du panier au chargement
showCartDetails();
updateCartCount();