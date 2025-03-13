"use strict"

const productContainer1 = document.getElementById('product-container');

searchForm.addEventListener('submit', function (e) {
    e.preventDefault();

    const searchInput = document.getElementById('searchInput');
    const searchFilter = document.getElementById('searchFilter');

    if (searchInput.value.trim() !== '') {
        localStorage.setItem('lastSearchQuery', searchInput.value);
        localStorage.setItem('lastSearchFilter', searchFilter.value);

        // Soumettre le formulaire après un petit délai
        setTimeout(() => {
            this.submit();
        }, 200);
    }
});

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

// Filtrer dynamiquement les produits affichés si nous sommes sur la page des résultats
if (productContainer1) {
    searchFilter.addEventListener('change', function () {
        localStorage.setItem('lastSearchFilter', searchFilter.value);

        // Si nous sommes sur la page de résultats, soumettre automatiquement le formulaire
        if (window.location.href.includes('/Product/Search')) {
            searchForm.submit();
        }
    });
}