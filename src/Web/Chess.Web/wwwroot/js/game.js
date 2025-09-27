document.addEventListener('DOMContentLoaded', () => {
    const board = document.getElementById('chess-board');
    const capturedDiv = document.getElementById('captured-pieces');
    const moveListDiv = document.getElementById('move-history-list');
    let selectedPieceId = null;

    async function tryMove(pieceId, toX, toY) {
        const response = await fetch('/Game/MakeMove', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                pieceId: pieceId,
                toX: toX,
                toY: toY
            })
        });

        const result = await response.json();

        if (result.success) {
            renderBoard(result.figures, result.captured);
            renderMoveHistory(result.moveHistory);

            if (result.gameOver) {
                await fetch('/Game/EndGame', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' }
                });
            }
        }
    }

    function bindDragEvents() {
        board.querySelectorAll('.figure-img').forEach(piece => {
            piece.setAttribute('draggable', true);
            piece.addEventListener('dragstart', (event) => {
                event.dataTransfer.setData("pieceId", event.target.id);
            });
            piece.addEventListener('click', (event) => {
                board.querySelectorAll('.figure-img.selected').forEach(p => p.classList.remove('selected'));
                selectedPieceId = event.target.id;
                event.target.classList.add('selected');
            });
        });
    }

    function bindSquareClickEvents() {
        board.querySelectorAll('.board-square').forEach(square => {
            square.addEventListener('click', async (event) => {
                if (selectedPieceId) {
                    const gridX = parseInt(square.getAttribute('data-x'));
                    const gridY = parseInt(square.getAttribute('data-y'));
                    await tryMove(selectedPieceId.replace("piece-", ""), gridX, gridY);
                    board.querySelectorAll('.figure-img.selected').forEach(p => p.classList.remove('selected'));
                    selectedPieceId = null;
                }
            });
        });
    }

    function renderBoard(figures, captured) {
        board.querySelectorAll('.figure-img').forEach(img => img.remove());
        figures.forEach(f => {
            const img = document.createElement('img');
            img.src = `/images/pieces/${f.image}`;
            img.alt = f.name || '';
            img.id = `piece-${f.id}`;
            img.className = 'figure-img';
            img.draggable = true;
            img.style.left = `${f.x}%`;
            img.style.top = `${f.y}%`;
            img.style.position = 'absolute';
            board.appendChild(img);
        });

        bindDragEvents();
        bindSquareClickEvents();

        if (capturedDiv) {
            capturedDiv.innerHTML = '';
            captured.forEach(f => {
                const img = document.createElement('img');
                img.src = `/images/pieces/${f.image}`;
                img.alt = f.name || '';
                img.className = 'captured-figure-img';
                capturedDiv.appendChild(img);
            });
        }
    }

    function renderMoveHistory(moveHistory) {
        if (moveHistory && moveHistory.length > 0) {
            let html = `
                <div class="table-responsive">
                  <table class="table table-sm align-middle mb-0">
                    <thead>
                      <tr>
                        <th class="text-start" style="width:45%">White</th>
                        <th class="text-end" style="width:45%">Black</th>
                      </tr>
                    </thead>
                    <tbody>
            `;
            for (let i = 0; i < moveHistory.length; i += 2) {
                html += `<tr>
                  <td class="text-start">
                    ${moveHistory[i]
                        ? `<img src="/images/pieces/${moveHistory[i].figureImage}" alt="" style="width:20px;height:20px;vertical-align:middle;margin-right:2px;">
                           ${moveHistory[i].coordinate}`
                        : ''}
                  </td>
                  <td class="text-end">
                    ${moveHistory[i + 1]
                        ? `<img src="/images/pieces/${moveHistory[i + 1].figureImage}" alt="" style="width:20px;height:20px;vertical-align:middle;margin-left:2px;">
                           ${moveHistory[i + 1].coordinate}`
                        : ''}
                  </td>
                </tr>`;
            }
            html += `</tbody></table></div>`;
            moveListDiv.innerHTML = html;
        } else {
            moveListDiv.innerHTML = `<div class="text-muted">No moves yet.</div>`;
        }
    }

    board.addEventListener('dragover', event => event.preventDefault());

    board.addEventListener('drop', async event => {
        event.preventDefault();

        const pieceId = event.dataTransfer.getData("pieceId");
        const boardRect = board.getBoundingClientRect();
        const x = event.clientX - boardRect.left;
        const y = event.clientY - boardRect.top;

        const gridX = Math.floor(x / (boardRect.width / 8));
        const gridY = Math.floor(y / (boardRect.height / 8));

        await tryMove(pieceId.replace("piece-", ""), gridX, gridY);
    });

    bindDragEvents();
    bindSquareClickEvents();
});