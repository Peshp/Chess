document.addEventListener('DOMContentLoaded', () => {
    const elements = {
        board: document.getElementById('chess-board'),
        captured: document.getElementById('captured-pieces'),
        history: document.getElementById('move-history-list'),
        whiteClock: {
            div: document.getElementById('clock-white'),
            span: document.getElementById('clock-value-white')
        },
        blackClock: {
            div: document.getElementById('clock-black'),
            span: document.getElementById('clock-value-black')
        }
    };

    let selectedPieceId = null;
    let currentTurn = 'white';

    const clockManager = {
        white: {
            seconds: parseInt(elements.whiteClock.div?.dataset.minutes || '10') * 60,
            increment: parseInt(elements.whiteClock.div?.dataset.increment || '0'),
            interval: null
        },
        black: {
            seconds: parseInt(elements.blackClock.div?.dataset.minutes || '10') * 60,
            increment: parseInt(elements.blackClock.div?.dataset.increment || '0'),
            interval: null
        },

        format(totalSeconds) {
            const time = Math.max(0, Math.floor(totalSeconds));
            const m = Math.floor(time / 60).toString().padStart(2, '0');
            const s = (time % 60).toString().padStart(2, '0');
            return `${m}:${s}`;
        },

        updateDisplay(color) {
            const data = this[color];
            const span = color === 'white' ? elements.whiteClock.span : elements.blackClock.span;
            if (span) span.textContent = this.format(data.seconds);
        },

        start(color) {
            this.stop('white');
            this.stop('black');

            const data = this[color];
            if (data.seconds <= 0) return;

            let lastTick = Date.now();
            data.interval = setInterval(() => {
                const now = Date.now();
                const delta = (now - lastTick) / 1000;
                lastTick = now;

                data.seconds -= delta;
                if (data.seconds <= 0) {
                    data.seconds = 0;
                    this.stop(color);
                    alert(`${color.toUpperCase()} ran out of time!`);
                }
                this.updateDisplay(color);
            }, 200);
        },

        stop(color) {
            if (this[color].interval) {
                clearInterval(this[color].interval);
                this[color].interval = null;
            }
        },

        handleMove(moverColor) {
            this.stop(moverColor);
            // Increment logic: only if time hasn't run out
            if (this[moverColor].seconds > 0) {
                this[moverColor].seconds += this[moverColor].increment;
            }
            this.updateDisplay(moverColor);

            currentTurn = (moverColor === 'white') ? 'black' : 'white';
            this.start(currentTurn);
        }
    };

    async function tryMove(pieceId, toX, toY) {
        try {
            const response = await fetch('/Game/MakeMove', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ pieceId: parseInt(pieceId), toX, toY })
            });

            if (!response.ok) return;
            const data = await response.json();

            if (data.success) {
                renderBoard(data.figures, data.captured);
                renderMoveHistory(data.moveHistory);

                // Clock handles turn switch only on success
                clockManager.handleMove(currentTurn);

                if (data.gameOver) {
                    clockManager.stop('white');
                    clockManager.stop('black');
                    setTimeout(() => window.location.href = '/Game/EndGame', 1000);
                }
            }
        } catch (err) {
            console.error('Move processing error:', err);
        }
    }

    function renderBoard(figures, captured) {
        if (!elements.board) return;

        elements.board.querySelectorAll('.figure-img').forEach(i => i.remove());

        figures.forEach(f => {
            const img = document.createElement('img');
            img.id = `piece-${f.id}`;
            img.className = 'figure-img';
            img.src = `/images/pieces/${f.image}`;
            img.style.left = `${f.x}%`;
            img.style.top = `${f.y}%`;
            img.dataset.color = f.color;
            elements.board.appendChild(img);
        });

        if (elements.captured) {
            elements.captured.innerHTML = captured.map(pc =>
                `<img src="/images/pieces/${pc.image}" class="captured-piece" style="width:25px; margin:2px">`
            ).join('');
        }
        rebindEvents();
    }

    function renderMoveHistory(list) {
        if (!elements.history || !list) return;
        let html = '<table class="table table-sm"><tbody>';
        for (let i = 0; i < list.length; i += 2) {
            const w = list[i];
            const b = list[i + 1];
            html += `<tr>
                <td>${w ? `<img src="/images/pieces/${w.figureImage}" width="20"> ${w.coordinate}` : ''}</td>
                <td>${b ? `<img src="/images/pieces/${b.figureImage}" width="20"> ${b.coordinate}` : ''}</td>
            </tr>`;
        }
        elements.history.innerHTML = html + '</tbody></table>';
    }

    function rebindEvents() {
        elements.board.querySelectorAll('.figure-img').forEach(img => {
            img.setAttribute('draggable', 'true');
            img.onclick = (e) => {
                elements.board.querySelectorAll('.selected').forEach(el => el.classList.remove('selected'));
                selectedPieceId = e.target.id;
                e.target.classList.add('selected');
            };
            img.ondragstart = (e) => e.dataTransfer.setData('pieceId', e.target.id);
        });
    }

    elements.board?.querySelectorAll('.board-square').forEach(sq => {
        sq.onclick = async () => {
            if (!selectedPieceId) return;
            const x = parseInt(sq.dataset.x);
            const y = parseInt(sq.dataset.y);
            await tryMove(selectedPieceId.replace('piece-', ''), x, y);
            selectedPieceId = null;
        };
    });

    elements.board?.addEventListener('dragover', e => e.preventDefault());
    elements.board?.addEventListener('drop', async e => {
        e.preventDefault();
        const id = e.dataTransfer.getData('pieceId');
        const rect = elements.board.getBoundingClientRect();
        const x = Math.floor((e.clientX - rect.left) / (rect.width / 8));
        const y = Math.floor((e.clientY - rect.top) / (rect.height / 8));
        await tryMove(id.replace('piece-', ''), x, y);
    });

    clockManager.updateDisplay('white');
    clockManager.updateDisplay('black');
    clockManager.start('white');
    rebindEvents();
});