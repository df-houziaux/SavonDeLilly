"use strict"

document.addEventListener("DOMContentLoaded", function () {
    // Gestion de la popup des produits
    let produitsButton = document.getElementById("produitsButton");
    let produitsPopup = document.getElementById("produitsPopup");

    if (produitsButton && produitsPopup) {
        // Ouvrir/Fermer la popup au clic
        produitsButton.addEventListener("click", function (event) {
            event.preventDefault(); // Empêche le lien de naviguer
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
    const panier = JSON.parse(localStorage.getItem("cartProducts")) || []; // Assurez-vous que le panier est un tableau

    const nombreArticle = panier.reduce((sum, product) => sum + product.quantity, 0);

    const cardCount = document.getElementById("cartCount");

    if (cardCount) {
        cardCount.innerText = nombreArticle;
    } else {
        console.error("Element avec ID 'cartCount' non trouvé !");
    }
}
