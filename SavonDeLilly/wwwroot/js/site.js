"use strict"

const searchForm = document.getElementById('searchForm');
const productContainer = document.getElementById('product-container');
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


searchForm.addEventListener('submit', function (e) {
    e.preventDefault();

    const searchInput = document.getElementById('searchInput');
    const searchFilter = document.getElementById('searchFilter');

    if (searchInput.value.trim() !== '') {
        localStorage.setItem('lastSearchQuery', searchInput.value);
        localStorage.setItem('lastSearchFilter', searchFilter.value);

        // Vérifiez que le localStorage est bien mis à jour avant de soumettre le formulaire
        console.log('Mise à jour localStorage:', localStorage.getItem('lastSearchQuery'), localStorage.getItem('lastSearchFilter'));

        // Soumettre le formulaire après un petit délai
        setTimeout(() => {
            this.submit();
        }, 200); // Délai réduit
    }
});

console.log(localStorage.getItem('lastSearchQuery'));
console.log(localStorage.getItem('lastSearchFilter'));

// Restaurer les derniers critères de recherche
if (localStorage.getItem('lastSearchQuery')) {
    searchInput.value = localStorage.getItem('lastSearchQuery');
}
if (localStorage.getItem('lastSearchFilter')) {
    const lastFilter = localStorage.getItem('lastSearchFilter');
    // S'assurer que la valeur existe dans le select
    const optionExists = Array.from(searchFilter.options).some(option => option.value === lastFilter);

    if (optionExists) {
        searchFilter.value = lastFilter;
    }
}


//Filtrer dynamiquement les produits affichés si nous sommes sur la page des résultats
if (productContainer) {
    searchFilter.addEventListener('change', function () {
        localStorage.setItem('lastSearchFilter', searchFilter.value);
        // Si nous sommes sur la page de résultats, soumettre automatiquement le formulaire
        if (window.location.href.includes('/Product/Search')) {
            searchForm.submit();
        }
    });
}