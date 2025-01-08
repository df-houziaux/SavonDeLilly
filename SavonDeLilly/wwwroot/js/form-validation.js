﻿// Affiche un message pour indiquer que le script est chargé
console.log("Script de validation chargé");

document.addEventListener('DOMContentLoaded', function () {
    const form = document.getElementById("contactForm");
    const nomInput = document.getElementById('nom');
    const emailInput = document.getElementById('email');
    const telephoneInput = document.getElementById('telephone');
    const messageInput = document.getElementById('message');

    // Regular expressions for validation
    const regexNom = /^(?![A-Z]{1}$)([A-ZÀ-ÿ][a-zà-ÿ]+(?: [A-ZÀ-ÿ][a-zà-ÿ]+)*)$/;
    const regexEmail = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    const regexTelephone = /^(0[1-9][ .]?[0-9]{2}[ .]?[0-9]{2}[ .]?[0-9]{2}[ .]?[0-9]{2})$/;

    // Function to display error messages
    const showError = (elementId, message) => {
        document.getElementById(elementId).textContent = message;
    };

    // Validation functions
    const validateNom = () => {
        const nom = nomInput.value.trim();
        if (regexNom.test(nom)) {
            showError('nomError', '');
            return true;
        }
        showError('nomError', 'Veuillez entrer un nom valide (ex: Dupont Nicola).');
        return false;
    };

    const validateEmail = () => {
        const email = emailInput.value.trim();
        if (regexEmail.test(email)) {
            showError('emailError', '');
            return true;
        }
        showError('emailError', 'Veuillez entrer un email valide.');
        return false;
    };

    const validateTelephone = () => {
        const telephone = telephoneInput.value.trim();
        if (regexTelephone.test(telephone)) {
            showError('telephoneError', '');
            return true;
        }
        showError('telephoneError', 'Veuillez entrer un numéro de téléphone valide (ex: 03.12.12.09).');
        return false;
    };

    const validateMessage = () => {
        const message = messageInput.value.trim();
        const hasValidContent = /\b[A-Za-zÀ-ÿ]{2,}\b/.test(message);
        if (message.length > 10 && hasValidContent) {
            showError('messageError', '');
            return true;
        } else {
            if (message.length <= 10) {
                showError('messageError', 'Le message doit contenir au moins 10 caractères.');
            } else {
                showError('messageError', 'Le message doit contenir au moins un mot valide.');
            }
            return false;
        }
    };

    // Validation complète du formulaire
    form.addEventListener('submit', function (event) {
        event.preventDefault();
        const isNomValid = validateNom();
        const isEmailValid = validateEmail();
        const isTelephoneValid = validateTelephone();
        const isMessageValid = validateMessage();

        if (isNomValid && isEmailValid && isTelephoneValid && isMessageValid) {
            console.log('Formulaire validé');
            form.submit();
        }
    });

    // Écouteurs pour valider au fur et à mesure que l'utilisateur tape
    nomInput.addEventListener('input', validateNom);
    emailInput.addEventListener('input', validateEmail);
    telephoneInput.addEventListener('input', validateTelephone);
    messageInput.addEventListener('input', validateMessage);
});
