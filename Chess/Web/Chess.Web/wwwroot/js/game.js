document.addEventListener('DOMContentLoaded', () => {
    // Clocks
    const whiteClockDiv = document.getElementById('clock-white');
    const blackClockDiv = document.getElementById('clock-black');
    const whiteClockSpan = document.getElementById('clock-value-white');
    const blackClockSpan = document.getElementById('clock-value-black');
    if (!whiteClockDiv) console.error('Missing #clock-white in HTML');
    if (!blackClockDiv) console.error('Missing #clock-black in HTML');
    if (!whiteClockSpan) console.error('Missing #clock-value-white in HTML');
    if (!blackClockSpan) console.error('Missing #clock-value-black in HTML');
    if (!whiteClockDiv || !blackClockDiv || !whiteClockSpan || !blackClockSpan) return;

    // Store time in seconds to avoid minute/second roll bugs
    const clocks = {
        white: {
            secondsRemaining: parseInt(whiteClockDiv.dataset.minutes || '0', 10) * 60,
            incrementSeconds: parseInt(whiteClockDiv.dataset.increment || '0', 10),
            running: false,
            interval: null,
            display: whiteClockSpan
        },
        black: {
            secondsRemaining: parseInt(blackClockDiv.dataset.minutes || '0', 10) * 60,
            incrementSeconds: parseInt(blackClockDiv.dataset.increment || '0', 10),
            running: false,
            interval: null,
            display: blackClockSpan
        }
    };

    let currentTurn = 'white';

    function formatTime(totalSeconds) {
        totalSeconds = Math.max(0, Math.floor(totalSeconds));
        const m = Math.floor(totalSeconds / 60).toString().padStart(2, '0');
        const s = (totalSeconds % 60).toString().padStart(2, '0');
        return `${m}:${s}`;
    }

    function updateClockDisplay(color) {
        const c = clocks[color];
        c.display.textContent = formatTime(c.secondsRemaining);
    }

    function stopClock(color) {
        const c = clocks[color];
        if (c.interval) {
            clearInterval(c.interval);
            c.interval = null;
        }
        c.running = false;
    }

    function startClock(color) {
        const c = clocks[color];
        const other = color === 'white' ? 'black' : 'white';
        // Ensure no other intervals are running
        stopClock(other);
        stopClock(color);
        if (c.secondsRemaining <= 0) {
            c.running = false;
            updateClockDisplay(color);
            return;
        }
        c.running = true;
        updateClockDisplay(color);
        c.interval = setInterval(() => {
            c.secondsRemaining -= 1;
            if (c.secondsRemaining <= 0) {
                c.secondsRemaining = 0;
                updateClockDisplay(color);
                stopClock(color);
                // time out handling can be added here (e.g., notify server)
                return;
            }
            updateClockDisplay(color);
        }, 1000);
    }

    function addIncrement(color) {
        const c = clocks[color];
        if (!c || !Number.isFinite(c.incrementSeconds) || c.incrementSeconds === 0) return;
        c.secondsRemaining += c.incrementSeconds;
        updateClockDisplay(color);
    }

    function onPlayerMove() {
        // Apply increment to the player who just moved, stop their clock, then switch and start opponent
        const mover = currentTurn;
        stopClock(mover);
        addIncrement(mover);
        currentTurn = mover === 'white' ? 'black' : 'white';
        startClock(currentTurn);
    }

    // Initialize clock displays and start side to move
    updateClockDisplay('white');
    updateClockDisplay('black');
    startClock(currentTurn);

    // Board / moves
    const board = document.getElementById('chess-board');
    const capturedDiv = document.getElementById('captured-pieces');
    const moveListDiv = document.getElementById('move-history-list');
    let selectedPieceId = null;

    async function tryMove(pieceId, toX, toY) {
        try {
            const res = await fetch('/Game/MakeMove', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ pieceId, toX, toY })
            });
            if (!res.ok) return;
            const data = await res.json();
            if (!data.success) return;
            renderBoard(data.figures, data.captured);
            renderMoveHistory(data.moveHistory);
            onPlayerMove();
            if (data.gameOver) {
                await fetch('/Game/EndGame', { method: 'POST', headers: { 'Content-Type': 'application/json' } });
                window.location.href = '/Game/EndGame';
            }
        } catch (err) {
            console.error('Move error', err);
        }
    }

    function bindDragEvents() {
        if (!board) return;
        board.querySelectorAll('.figure-img').forEach(img => {
            img.setAttribute('draggable', 'true');
            img.addEventListener('dragstart', e => {
                e.dataTransfer.setData('pieceId', e.target.id);
            });
            img.addEventListener('click', e => {
                board.querySelectorAll('.figure-img.selected').forEach(p => p.classList.remove('selected'));
                selectedPieceId = e.target.id;
                e.target.classList.add('selected');
            });
        });
    }

    function bindSquareClickEvents() {
        if (!board) return;
        board.querySelectorAll('.board-square').forEach(sq => {
            sq.addEventListener('click', async e => {
                if (!selectedPieceId) return;
                const x = parseInt(sq.dataset.x, 10);
                const y = parseInt(sq.dataset.y, 10);
                await tryMove(selectedPieceId.replace('piece-', ''), x, y);
                board.querySelectorAll('.figure-img.selected').forEach(p => p.classList.remove('selected'));
                selectedPieceId = null;
            });
        });
    }

    function renderBoard(figures, captured) {
        if (!board) return;
        // Remove existing piece images
        board.querySelectorAll('.figure-img').forEach(i => i.remove());
        if (Array.isArray(figures)) {
            figures.forEach(f => {
                const img = document.createElement('img');
                img.id = `piece-${f.id ?? f.pieceId ?? (f.name ?? Math.random().toString(36).slice(2))}`;
                img.className = 'figure-img';
                img.src = `/images/pieces/${f.image}`;
                img.style.left = `${f.x}%`;
                img.style.top = `${f.y}%`;
                img.style.position = 'absolute';
                img.dataset.color = f.color || '';
                if (typeof f.gridX !== 'undefined') img.dataset.x = String(f.gridX);
                if (typeof f.gridY !== 'undefined') img.dataset.y = String(f.gridY);
                board.appendChild(img);
            });
        }
        if (capturedDiv) {
            capturedDiv.innerHTML = Array.isArray(captured) && captured.length > 0
                ? captured.map(pc => `<img src="/images/pieces/${pc.image}" class="captured-piece" alt="${pc.name || ''}" style="width:20px;height:20px;margin:2px">`).join('')
                : '';
        }
        // Re-bind events for newly created pieces
        bindDragEvents();
        bindSquareClickEvents();
    }

    function renderMoveHistory(list) {
        if (!moveListDiv) return;
        if (!list || list.length === 0) {
            moveListDiv.innerHTML = '<div class="text-muted">No moves yet.</div>';
            return;
        }
        let html = '<div class="table-responsive"><table class="table table-sm align-middle mb-0"><thead><tr><th class="text-start" style="width:45%">White</th><th class="text-end" style="width:45%">Black</th></tr></thead><tbody>';
        for (let i = 0; i < list.length; i += 2) {
            const w = list[i];
            const b = list[i + 1];
            html += `<tr><td class="text-start">${w ? `<img src="/images/pieces/${w.figureImage}" style="width:20px;height:20px;vertical-align:middle;margin-right:2px;">${w.coordinate}` : ''}</td><td class="text-end">${b ? `<img src="/images/pieces/${b.figureImage}" style="width:20px;height:20px;vertical-align:middle;margin-left:2px;">${b.coordinate}` : ''}</td></tr>`;
        }
        html += '</tbody></table></div>';
        moveListDiv.innerHTML = html;
    }

    // Drag & drop on board
    if (board) {
        board.addEventListener('dragover', e => e.preventDefault());
        board.addEventListener('drop', async e => {
            e.preventDefault();
            const pieceId = e.dataTransfer.getData('pieceId');
            if (!pieceId) return;
            const rect = board.getBoundingClientRect();
            const x = Math.floor((e.clientX - rect.left) / (rect.width / 8));
            const y = Math.floor((e.clientY - rect.top) / (rect.height / 8));
            await tryMove((pieceId || '').replace('piece-', ''), x, y);
        });
        bindDragEvents();
        bindSquareClickEvents();
    }
});