document.addEventListener("DOMContentLoaded", function () {
    let carousel = new bootstrap.Carousel(document.getElementById('carouselExample'), {
        interval: 4000, // Force l'intervalle à 3 secondes
        ride: 'carousel'
    });
});