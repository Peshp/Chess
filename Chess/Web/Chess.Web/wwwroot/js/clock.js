const whiteClockDiv = document.getElementById('clock-white');
const blackClockDiv = document.getElementById('clock-black');
const whiteClockSpan = document.getElementById('clock-value-white');
const blackClockSpan = document.getElementById('clock-value-black');

if (!whiteClockDiv) console.error('Missing #clock-white in HTML');
if (!blackClockDiv) console.error('Missing #clock-black in HTML');
if (!whiteClockSpan) console.error('Missing #clock-value-white in HTML');
if (!blackClockSpan) console.error('Missing #clock-value-black in HTML');
if (!whiteClockDiv || !blackClockDiv || !whiteClockSpan || !blackClockSpan) return;

const clocks = {
    white: {
        minutes: parseInt(whiteClockDiv.dataset.minutes || '0', 10),
        increment: parseInt(whiteClockDiv.dataset.increment || '0', 10),
        seconds: 0,
        running: false,
        interval: null,
        display: whiteClockSpan
    },
    black: {
        minutes: parseInt(blackClockDiv.dataset.minutes || '0', 10),
        increment: parseInt(blackClockDiv.dataset.increment || '0', 10),
        seconds: 0,
        running: false,
        interval: null,
        display: blackClockSpan
    }
};

let currentTurn = 'white';

function updateClockDisplay(color) {
    const c = clocks[color];
    const m = c.minutes.toString().padStart(2, '0');
    const s = c.seconds.toString().padStart(2, '0');
    c.display.textContent = `${m}:${s}`;
}

function startClock(color) {
    const c = clocks[color];
    if (c.running) return;
    stopClock('white');
    stopClock('black');
    if (c.minutes === 0 && c.seconds === 0) return;
    c.running = true;
    c.interval = setInterval(() => {
        if (c.seconds === 0) {
            if (c.minutes === 0) {
                stopClock(color);
                return;
            }
            c.minutes--;
            c.seconds = 59;
        } else {
            c.seconds--;
        }
        updateClockDisplay(color);
    }, 1000);
}

function stopClock(color) {
    const c = clocks[color];
    if (!c.running) return;
    c.running = false;
    clearInterval(c.interval);
    c.interval = null;
}

function addIncrement(color) {
    const c = clocks[color];
    c.seconds += c.increment;
    while (c.seconds >= 60) {
        c.seconds -= 60;
        c.minutes++;
    }
    updateClockDisplay(color);
}

function onPlayerMove() {
    stopClock(currentTurn);
    addIncrement(currentTurn);
    currentTurn = currentTurn === 'white' ? 'black' : 'white';
    startClock(currentTurn);
}

updateClockDisplay('white');
updateClockDisplay('black');
startClock('white');