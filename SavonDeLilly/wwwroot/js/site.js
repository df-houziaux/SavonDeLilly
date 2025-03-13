"use strict";

if (typeof searchForm === "undefined") {
    const searchForm = document.getElementById('searchForm');
}

if (typeof productContainer === "undefined") {
    const productContainer = document.getElementById('product-container');
}

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

    // Récupérer le nombre d'articles dans le panier
    recupereNombreArticlesPanier();
});

function recupereNombreArticlesPanier() {
    const panier = JSON.parse(localStorage.getItem("cartProducts")) || [];
    const nombreArticle = panier.reduce((sum, product) => sum + product.quantity, 0)
    const cardCount = document.getElementById("cartCount");

    cardCount ? cardCount.innerText = nombreArticle : console.error("Element avec ID 'cartCount' non trouvé !");
}