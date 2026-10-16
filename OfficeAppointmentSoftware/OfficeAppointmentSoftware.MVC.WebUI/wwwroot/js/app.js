// Shared UI behaviour for all pages.
(function () {
    'use strict';

    // ---- Theme (light / dark). The initial theme is applied by an inline script in <head> to avoid a flash.
    function storedTheme() {
        try { return localStorage.getItem('theme'); } catch (e) { return null; }
    }
    function applyTheme(theme) {
        document.documentElement.setAttribute('data-bs-theme', theme);
        document.querySelectorAll('[data-theme-icon]').forEach(function (icon) {
            icon.className = theme === 'dark' ? 'bi bi-sun' : 'bi bi-moon-stars';
        });
    }
    document.addEventListener('click', function (e) {
        var toggle = e.target.closest('[data-theme-toggle]');
        if (!toggle) return;
        var next = document.documentElement.getAttribute('data-bs-theme') === 'dark' ? 'light' : 'dark';
        try { localStorage.setItem('theme', next); } catch (err) { /* private mode */ }
        applyTheme(next);
    });
    applyTheme(document.documentElement.getAttribute('data-bs-theme') || storedTheme() || 'light');

    // ---- Confirmation for destructive forms: <form data-confirm="..." data-confirm-title="...">
    var confirmModalEl = document.getElementById('confirmModal');
    var pendingForm = null;
    if (confirmModalEl) {
        var confirmModal = new bootstrap.Modal(confirmModalEl);
        document.addEventListener('submit', function (e) {
            var form = e.target;
            if (!form.matches('form[data-confirm]') || form.dataset.confirmed === 'true') return;
            e.preventDefault();
            pendingForm = form;
            confirmModalEl.querySelector('[data-confirm-title]').textContent = form.dataset.confirmTitle || 'Emin misiniz?';
            confirmModalEl.querySelector('[data-confirm-text]').textContent = form.dataset.confirm;
            confirmModal.show();
        });
        confirmModalEl.querySelector('[data-confirm-ok]').addEventListener('click', function () {
            if (!pendingForm) return;
            pendingForm.dataset.confirmed = 'true';
            confirmModal.hide();
            pendingForm.requestSubmit ? pendingForm.requestSubmit() : pendingForm.submit();
        });
    }

    // ---- Client-side table filter: <input data-table-search="#tableId">
    document.querySelectorAll('[data-table-search]').forEach(function (input) {
        var table = document.querySelector(input.dataset.tableSearch);
        if (!table) return;
        var empty = document.querySelector(input.dataset.emptyTarget || '#noResults');
        input.addEventListener('input', function () {
            var term = input.value.trim().toLocaleLowerCase('tr-TR');
            var visible = 0;
            table.querySelectorAll('tbody tr').forEach(function (row) {
                var match = row.textContent.toLocaleLowerCase('tr-TR').indexOf(term) !== -1;
                row.classList.toggle('d-none', !match);
                if (match) visible++;
            });
            if (empty) empty.classList.toggle('d-none', visible !== 0);
        });
    });

    // ---- Status toasts rendered by _StatusMessages
    document.querySelectorAll('.toast[data-autoshow]').forEach(function (el) {
        new bootstrap.Toast(el, { delay: 6000 }).show();
    });

    // ---- Image preview: <input type="file" data-preview="#img">
    document.querySelectorAll('input[type=file][data-preview]').forEach(function (input) {
        input.addEventListener('change', function () {
            var img = document.querySelector(input.dataset.preview);
            if (!img || !input.files || !input.files[0]) return;
            img.src = URL.createObjectURL(input.files[0]);
            img.classList.remove('d-none');
            var placeholder = document.querySelector(input.dataset.previewPlaceholder);
            if (placeholder) placeholder.classList.add('d-none');
        });
    });

    // ---- Character counter: <textarea data-counter="#id" maxlength="450">
    document.querySelectorAll('[data-counter]').forEach(function (field) {
        var out = document.querySelector(field.dataset.counter);
        if (!out) return;
        var update = function () { out.textContent = field.value.length + ' / ' + field.maxLength; };
        field.addEventListener('input', update);
        update();
    });

    // ---- Landing navbar becomes solid after scrolling
    var nav = document.querySelector('.landing-nav');
    if (nav) {
        var onScroll = function () { nav.classList.toggle('scrolled', window.scrollY > 40); };
        window.addEventListener('scroll', onScroll, { passive: true });
        onScroll();
    }
})();
