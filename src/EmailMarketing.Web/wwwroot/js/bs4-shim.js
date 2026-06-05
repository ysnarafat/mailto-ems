/**
 * Bootstrap 4 → Bootstrap 5 attribute shim.
 * Translates data-toggle, data-dismiss, data-target, etc.
 * before Bootstrap 5 auto-initialises components.
 * Load this BEFORE bootstrap.bundle.min.js.
 */
(function () {
    var attrMap = {
        'data-toggle':  'data-bs-toggle',
        'data-dismiss': 'data-bs-dismiss',
        'data-target':  'data-bs-target',
        'data-parent':  'data-bs-parent',
        'data-slide':   'data-bs-slide',
        'data-slide-to': 'data-bs-slide-to',
        'data-ride':    'data-bs-ride',
        'data-spy':     'data-bs-spy',
        'data-offset':  'data-bs-offset',
        'data-content': 'data-bs-content',
        'data-placement': 'data-bs-placement',
        'data-trigger': 'data-bs-trigger',
        'data-container': 'data-bs-container',
    };

    function translateAttrs() {
        Object.keys(attrMap).forEach(function (old) {
            var next = attrMap[old];
            document.querySelectorAll('[' + old + ']').forEach(function (el) {
                if (!el.hasAttribute(next)) {
                    el.setAttribute(next, el.getAttribute(old));
                }
                el.removeAttribute(old);
            });
        });

        // dropdown-menu-right → dropdown-menu-end
        document.querySelectorAll('.dropdown-menu-right').forEach(function (el) {
            el.classList.add('dropdown-menu-end');
            el.classList.remove('dropdown-menu-right');
        });

        // float-right → float-end
        document.querySelectorAll('.float-right').forEach(function (el) {
            el.classList.add('float-end');
            el.classList.remove('float-right');
        });
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', translateAttrs);
    } else {
        translateAttrs();
    }
})();
