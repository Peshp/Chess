// Prevent double initialization when the script is included more than once
if (window.__chessGameInitialized) {
    // already initialized
} else {
    window.__chessGameInitialized = true;

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
        // track server-side current turn in lower-case ('white'|'black')
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
                if (this[color]?.interval) {
                    clearInterval(this[color].interval);
                    this[color].interval = null;
                }
            },

            handleMove(moverColor) {
                // moverColor is the color that made the move (white/black)
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

                if (!response.ok) {
                    console.error('Move request failed:', response.status, response.statusText);
                    return;
                }
                const data = await response.json();

                if (data.success) {
                    renderBoard(data.figures, data.captured);
                    renderMoveHistory(data.moveHistory);

                    // Server sends currentTurn as "White" or "Black" (capitalized) — map to lower-case
                    const serverCurrentTurn = data.currentTurn ? data.currentTurn.toLowerCase() : null;
                    // moverColor is the player who just moved (the opposite of the server's currentTurn)
                    let moverColor;
                    if (serverCurrentTurn === 'white' || serverCurrentTurn === 'black') {
                        moverColor = serverCurrentTurn === 'white' ? 'black' : 'white';
                        // apply clock changes for the mover, and then set local currentTurn to server value
                        clockManager.handleMove(moverColor);
                        currentTurn = serverCurrentTurn;
                    } else {
                        // fallback: assume local currentTurn was mover
                        clockManager.handleMove(currentTurn);
                        currentTurn = currentTurn === 'white' ? 'black' : 'white';
                    }

                    if (data.gameOver) {
                        clockManager.stop('white');
                        clockManager.stop('black');
                        setTimeout(() => window.location.href = '/Game/EndGame', 1000);
                    }
                } else {
                    // If move not allowed by server, clear selection UI
                    clearSelection();
                }
            } catch (err) {
                console.error('Move processing error:', err);
            }
        }

        function renderBoard(figures, captured) {
            if (!elements.board) return;

            // Remove existing piece images
            elements.board.querySelectorAll('.figure-img').forEach(i => i.remove());

            // Add new pieces from server JSON (figures have id, x, y, image, color)
            (figures || []).forEach(f => {
                const img = document.createElement('img');
                img.id = `piece-${f.id}`;
                img.className = 'figure-img';
                img.src = `/images/pieces/${f.image}`;
                img.style.left = `${f.x}%`;
                img.style.top = `${f.y}%`;
                img.dataset.color = f.color;
                img.setAttribute('draggable', 'true');
                elements.board.appendChild(img);
            });

            if (elements.captured) {
                elements.captured.innerHTML = (captured || []).map(pc =>
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
            if (!elements.board) return;

            // safely iterate over pieces
            elements.board.querySelectorAll('.figure-img').forEach(img => {
                img.setAttribute('draggable', 'true');
                img.onclick = (e) => {
                    elements.board.querySelectorAll('.selected').forEach(el => el.classList.remove('selected'));
                    selectedPieceId = e.currentTarget.id;
                    e.currentTarget.classList.add('selected');
                };
                img.ondragstart = (e) => {
                    try {
                        e.dataTransfer.setData('pieceId', e.currentTarget.id);
                    } catch (err) {
                        // some browsers require explicit data type
                        e.dataTransfer.setData('text/plain', e.currentTarget.id);
                    }
                };
            });
        }

        function clearSelection() {
            if (!elements.board) return;
            elements.board.querySelectorAll('.selected').forEach(el => el.classList.remove('selected'));
            selectedPieceId = null;
        }

        // wire up board squares (guard with optional chaining)
        elements.board?.querySelectorAll('.board-square')?.forEach(sq => {
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
            const id = e.dataTransfer.getData('pieceId') || e.dataTransfer.getData('text/plain');
            if (!id) return;
            const rect = elements.board.getBoundingClientRect();
            const x = Math.floor((e.clientX - rect.left) / (rect.width / 8));
            const y = Math.floor((e.clientY - rect.top) / (rect.height / 8));
            await tryMove(id.replace('piece-', ''), x, y);
        });

        // initialize displays and start clocks
        clockManager.updateDisplay('white');
        clockManager.updateDisplay('black');
        clockManager.start('white');
        rebindEvents();
    });
}