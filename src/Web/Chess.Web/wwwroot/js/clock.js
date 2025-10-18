document.addEventListener('DOMContentLoaded', function () {
    const clockSpan = document.getElementById('clock-value');
    let minutes = parseInt(clockSpan.getAttribute('data-minutes'), 10) || 0;
    let increment = parseInt(clockSpan.getAttribute('data-increment'), 10) || 0;

    function updateClockDisplay() {
        const minStr = minutes.toString().padStart(2, '0');
        const incStr = increment.toString().padStart(2, '0');
        clockSpan.textContent = `${minStr}:${incStr}`;
    }

    function tickClock() {
        increment++;
        if (increment >= 60) {
            minutes++;
            increment = 0;
        }
        updateClockDisplay();
    }

    updateClockDisplay();
    setInterval(tickClock, 1000);
});