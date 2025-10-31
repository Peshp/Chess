(function () {
    const whiteClockEl = document.getElementById('clock-white');
    const blackClockEl = document.getElementById('clock-black');
    const whiteValueEl = document.getElementById('clock-value-white');
    const blackValueEl = document.getElementById('clock-value-black');

    if (!whiteClockEl || !blackClockEl || !whiteValueEl || !blackValueEl) {
        console.error('Clock elements not found. Expected ids: clock-white, clock-black, clock-value-white, clock-value-black');
        return;
    }

    function parseMinutes(el) {
        const m = Number(el.getAttribute('data-minutes'));
        return Number.isFinite(m) ? Math.max(0, Math.floor(m)) : 0;
    }
    function parseIncrement(el) {
        const i = Number(el.getAttribute('data-increment'));
        return Number.isFinite(i) ? Math.max(0, Math.floor(i)) : 0;
    }

    let times = {
        white: parseMinutes(whiteClockEl) * 60,
        black: parseMinutes(blackClockEl) * 60
    };

    let increments = {
        white: parseIncrement(whiteClockEl),
        black: parseIncrement(blackClockEl)
    };

    let active = null;
    let timerId = null;
    let ended = false;

    function formatTime(totalSeconds) {
        totalSeconds = Math.max(0, Math.floor(totalSeconds));
        const m = Math.floor(totalSeconds / 60);
        const s = totalSeconds % 60;
        return String(m).padStart(2, '0') + ':' + String(s).padStart(2, '0');
    }

    function updateDisplays() {
        whiteValueEl.textContent = formatTime(times.white);
        blackValueEl.textContent = formatTime(times.black);
        whiteClockEl.classList.toggle('active', active === 'white' && !ended);
        blackClockEl.classList.toggle('active', active === 'black' && !ended);
    }

    function stopTimer() {
        if (timerId !== null) {
            clearInterval(timerId);
            timerId = null;
        }
    }

    function onFlag(player) {
        ended = true;
        stopTimer();
        updateDisplays();
        const ev = new CustomEvent('clock:flag', { detail: { player } });
        document.dispatchEvent(ev);
        console.warn(player + ' flagged (time\'s up).');
    }

    function startClockFor(player) {
        if (ended) return;
        if (player !== 'white' && player !== 'black') return;
        stopTimer();
        active = player;
        updateDisplays();

        timerId = setInterval(function () {
            times[player] -= 1;
            if (times[player] <= 0) {
                times[player] = 0;
                updateDisplays();
                onFlag(player);
                return;
            }
            updateDisplays();
            document.dispatchEvent(new CustomEvent('clock:tick', { detail: { player: player, remaining: times[player] } }));
        }, 1000);

        updateDisplays();
    }

    function startClock(side) {
        if (ended) return;
        const toStart = side === 'white' || side === 'black' ? side : (active || 'white');
        startClockFor(toStart);
    }

    function pauseClock() {
        stopTimer();
        updateDisplays();
    }

    function resetClocks() {
        stopTimer();
        ended = false;
        active = null;
        times.white = parseMinutes(whiteClockEl) * 60;
        times.black = parseMinutes(blackClockEl) * 60;
        increments.white = parseIncrement(whiteClockEl);
        increments.black = parseIncrement(blackClockEl);
        updateDisplays();
        document.dispatchEvent(new CustomEvent('clock:reset', { detail: { times: Object.assign({}, times), increments: Object.assign({}, increments) } }));
    }

    function moveMade(movedColor) {
        if (ended) return;
        const mover = (movedColor === 'white' || movedColor === 'black') ? movedColor : active || 'white';
        const opponent = mover === 'white' ? 'black' : 'white';
        times[mover] += increments[mover];
        stopTimer();
        if (times[opponent] <= 0) {
            onFlag(opponent);
            return;
        }
        startClockFor(opponent);
        document.dispatchEvent(new CustomEvent('clock:moveMade', { detail: { mover: mover, times: Object.assign({}, times) } }));
    }

    function setInitialMinutesFor(side, minutes) {
        if (side !== 'white' && side !== 'black') return;
        const m = Number(minutes);
        if (!Number.isFinite(m) || m < 0) return;
        const el = side === 'white' ? whiteClockEl : blackClockEl;
        el.setAttribute('data-minutes', String(Math.floor(m)));
    }
    function setIncrementFor(side, seconds) {
        if (side !== 'white' && side !== 'black') return;
        const s = Number(seconds);
        if (!Number.isFinite(s) || s < 0) return;
        const el = side === 'white' ? whiteClockEl : blackClockEl;
        el.setAttribute('data-increment', String(Math.floor(s)));
    }

    window.chessClock = {
        startClock: startClock,
        pauseClock: pauseClock,
        resetClocks: resetClocks,
        moveMade: moveMade,
        setInitialMinutesFor: setInitialMinutesFor,
        setIncrementFor: setIncrementFor,
        _state: function () { return { active: active, times: Object.assign({}, times), increments: Object.assign({}, increments), ended: ended }; }
    };

    window.moveMade = moveMade;

    document.addEventListener('keydown', function (e) {
        if (e.key && e.key.toLowerCase() === 'm') {
            e.preventDefault();
            moveMade();
        }
    });

    updateDisplays();
})();