document.addEventListener('DOMContentLoaded', () => {
    const board = document.getElementById('chess-board');
    const capturedDiv = document.getElementById('captured-pieces');
    const moveListDiv = document.getElementById('move-history-list');
    let selectedPieceId = null;

    async function tryMove(pieceId, toX, toY) {
        const res = await fetch('/Game/MakeMove', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ pieceId, toX, toY })
        });
        const data = await res.json();
        if (!data.success) return;

        renderBoard(data.figures, data.captured);
        renderMoveHistory(data.moveHistory);
        onPlayerMove();

        if (data.gameOver) {
            await fetch('/Game/EndGame', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' }
            });
            window.location.href = '/Game/EndGame';
        }
    }

    function bindDragEvents() {
        if (!board) return;
        board.querySelectorAll('.figure-img').forEach(img => {
            img.draggable = true;
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
        board.querySelectorAll('.figure-img').forEach(i => i.remove());
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
            img.dataset.color = f.color;
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

    function renderMoveHistory(list) {
        if (!moveListDiv) return;
        if (!list || list.length === 0) {
            moveListDiv.innerHTML = `<div class="text-muted">No moves yet.</div>`;
            return;
        }
        let html = `<div class="table-responsive">
          <table class="table table-sm align-middle mb-0">
            <thead><tr><th class="text-start" style="width:45%">White</th><th class="text-end" style="width:45%">Black</th></tr></thead>
          <tbody>`;
        for (let i = 0; i < list.length; i += 2) {
            html += `<tr>
              <td class="text-start">${list[i] ? `<img src="/images/pieces/${list[i].figureImage}" style="width:20px;height:20px;vertical-align:middle;margin-right:2px;">${list[i].coordinate}` : ''}</td>
              <td class="text-end" >${list[i + 1] ? `<img src="/images/pieces/${list[i + 1].figureImage}" style="width:20px;height:20px;vertical-align:middle;margin-left:2px;">${list[i + 1].coordinate}` : ''}</td>
            </tr>`;
        }
        html += '</tbody></table></div>';
        moveListDiv.innerHTML = html;
    }

    if (board) {
        board.addEventListener('dragover', e => e.preventDefault());
        board.addEventListener('drop', async e => {
            e.preventDefault();
            const pieceId = e.dataTransfer.getData('pieceId');
            const rect = board.getBoundingClientRect();
            const x = Math.floor((e.clientX - rect.left) / (rect.width / 8));
            const y = Math.floor((e.clientY - rect.top) / (rect.height / 8));
            await tryMove(pieceId.replace('piece-', ''), x, y);
        });
        bindDragEvents();
        bindSquareClickEvents();
    }
});