document.addEventListener("DOMContentLoaded", () => {
    const sections = document.querySelectorAll("section");
    const observer = new IntersectionObserver(
        (entries) => {
            entries.forEach((entry) => {
                if (entry.isIntersecting) {
                    entry.target.classList.add("visible");
                }
            });
        },
        { threshold: 0.1 }
    );
    sections.forEach((sec) => observer.observe(sec));
});

// Reveal-on-scroll с IntersectionObserver
(function () {
    const items = Array.from(document.querySelectorAll('.reveal'));
    if (!('IntersectionObserver' in window) || items.length === 0) {
        // Фолбэк без анимаций
        items.forEach(el => el.classList.add('visible'));
        return;
    }

    const io = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('visible');
                io.unobserve(entry.target); // анимируем один раз
            }
        });
    }, { root: null, rootMargin: '0px 0px -10% 0px', threshold: 0.15 });

    items.forEach(el => io.observe(el));
})();
