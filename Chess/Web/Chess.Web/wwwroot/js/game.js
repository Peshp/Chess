if (window.__chessGameInitialized) {
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
                div: document.getElementById('clock-white'), // Fixed selector from your snippet if needed
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
                if (this[color]?.interval) {
                    clearInterval(this[color].interval);
                    this[color].interval = null;
                }
            },

            handleMove(moverColor) {
                this.stop(moverColor);
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

                // Check for pawn promotion trigger
                if (data.needsPromotion) {
                    // Find the pawn to check color for dynamic icons
                    const pawn = data.figures.find(f => f.id === data.pieceId);
                    showPromotionModal(data.pieceId, data.figures, pawn ? pawn.color : 'White');
                    return;
                }

                if (data.success) {
                    updateBoardState(data);
                } else {
                    clearSelection();
                }
            } catch (err) {
                console.error('Move processing error:', err);
            }
        }

        function updateBoardState(data) {
            renderBoard(data.figures, data.captured);
            renderMoveHistory(data.moveHistory);

            const serverCurrentTurn = data.currentTurn ? data.currentTurn.toLowerCase() : null;
            let moverColor;
            if (serverCurrentTurn === 'white' || serverCurrentTurn === 'black') {
                moverColor = serverCurrentTurn === 'white' ? 'black' : 'white';
                clockManager.handleMove(moverColor);
                currentTurn = serverCurrentTurn;
            } else {
                clockManager.handleMove(currentTurn);
                currentTurn = currentTurn === 'white' ? 'black' : 'white';
            }

            if (data.gameOver) {
                clockManager.stop('white');
                clockManager.stop('black');
                setTimeout(() => window.location.href = '/Game/EndGame', 1000);
            }
        }

        function showPromotionModal(pieceId, currentFigures, color) {
            const modal = document.getElementById('promotionModal');
            const optionsContainer = document.getElementById('promotionOptions');
            if (!modal || !optionsContainer) return;

            renderBoard(currentFigures, null);

            const pieces = ['Queen', 'Rook', 'Bishop', 'Night'];
            const prefix = color.toLowerCase() === 'white' ? 'w' : 'b';

            optionsContainer.innerHTML = pieces.map(p => `
                <div class="promotion-piece" data-piece="${p}" style="cursor:pointer; padding:10px;">
                    <img src="/images/pieces/${prefix}${p[0].toLowerCase()}.png" width="50" height="50" />
                </div>
            `).join('');

            modal.style.display = 'flex';

            optionsContainer.querySelectorAll('.promotion-piece').forEach(piece => {
                piece.onclick = async () => {
                    const promoteTo = piece.dataset.piece;
                    modal.style.display = 'none';
                    await promotePawn(pieceId, promoteTo);
                };
            });
        }

        async function promotePawn(pieceId, promoteTo) {
            try {
                const response = await fetch('/Game/PromotePawn', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify({
                        pieceId: parseInt(pieceId),
                        promoteTo: promoteTo 
                    })
                });

                if (!response.ok) {
                    console.error('Server returned error:', response.status);
                    return;
                }

                const data = await response.json();
                if (data.success) {
                    updateBoardState(data);
                }
            } catch (err) {
                console.error('Promotion network error:', err);
            }
        }

        function renderBoard(figures, captured) {
            if (!elements.board) return;

            elements.board.querySelectorAll('.figure-img').forEach(i => i.remove());

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

            if (captured !== null && elements.captured) {
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

        elements.board?.querySelectorAll('.board-square')?.forEach(sq => {
            sq.onclick = async () => {
                if (!selectedPieceId) return;
                const x = parseInt(sq.dataset.x);
                const y = parseInt(sq.dataset.y);
                const idToMove = selectedPieceId.replace('piece-', '');
                selectedPieceId = null; // Clear before move to prevent double clicks
                await tryMove(idToMove, x, y);
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

        clockManager.updateDisplay('white');
        clockManager.updateDisplay('black');
        clockManager.start('white');
        rebindEvents();
    });
}