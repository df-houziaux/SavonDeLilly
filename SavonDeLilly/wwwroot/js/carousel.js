﻿document.addEventListener("DOMContentLoaded", () => {
    const slides = document.querySelectorAll(".slides img");
    let currentIndex = 0;

    if (slides.length > 0) {

        slides[currentIndex].classList.add("active");


        function nextSlide() {

            slides[currentIndex].classList.remove("active");


            currentIndex = (currentIndex + 1) % slides.length;


            slides[currentIndex].classList.add("active");
        }

        setInterval(nextSlide, 3000);
    }
});
