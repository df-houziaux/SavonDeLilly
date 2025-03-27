"use strict";

document.addEventListener("DOMContentLoaded", function () {
    // Gestion de la popup des produits
    let produitsButton = document.getElementById("produitsButton");
    let produitsPopup = document.getElementById("produitsPopup");

    if (produitsButton && produitsPopup) {
        // Ouvrir/Fermer la popup au clic
        produitsButton.addEventListener("click", function (event) {
            event.preventDefault();
            produitsPopup.classList.toggle("show");
        });

        // Fermer la popup si on clique ailleurs
        document.addEventListener("click", function (event) {
            if (!produitsButton.contains(event.target) && !produitsPopup.contains(event.target)) {
                produitsPopup.classList.remove("show");
            }
        });
    } else {
        console.error("produitsButton ou produitsPopup non trouvé !");
    }

    // 🛒 Vérifie si on est sur la page de confirmation
    if (window.location.pathname.includes("/Payment/Confirmation")) {
        console.log("🔄 Suppression du panier après paiement...");
        localStorage.removeItem("cartProducts");
    }

    // Récupérer le nombre d'articles dans le panier
    recupereNombreArticlesPanier();
});

// 🛒 Fonction pour mettre à jour le nombre d'articles dans le panier
function recupereNombreArticlesPanier() {
    const panier = JSON.parse(localStorage.getItem("cartProducts")) || [];
    const nombreArticle = panier.reduce((sum, product) => sum + product.quantity, 0);
    const cartCount = document.getElementById("cartCount");

    if (cartCount) {
        cartCount.innerText = nombreArticle;
    } else {
        console.error("❌ Élément avec ID 'cartCount' non trouvé !");
    }
}
