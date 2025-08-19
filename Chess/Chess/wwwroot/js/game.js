document.addEventListener('DOMContentLoaded', () => {
    const board = document.querySelector('.chess-board-container');
    const capturedDiv = document.querySelector('.captured-pieces');

    function bindDragEvents() {
        board.querySelectorAll('.figure-img').forEach(piece => {
            piece.setAttribute('draggable', true);
            piece.addEventListener('dragstart', (event) => {
                event.dataTransfer.setData("pieceId", event.target.id);
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
            board.appendChild(img);
        });

        bindDragEvents();

        if (capturedDiv) {
            capturedDiv.innerHTML = '<h4>Captured Pieces:</h4>';
            captured.forEach(f => {
                const img = document.createElement('img');
                img.src = `/images/pieces/${f.image}`;
                img.alt = f.name || '';
                img.className = 'captured-figure-img';
                capturedDiv.appendChild(img);
            });
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

        const response = await fetch('/Game/MakeMove', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                pieceId: pieceId.replace("piece-", ""),
                toX: gridX,
                toY: gridY
            })
        });

        const result = await response.json();

        if (result.success) {
            renderBoard(result.figures, result.captured);
        }
    });

    bindDragEvents();
});