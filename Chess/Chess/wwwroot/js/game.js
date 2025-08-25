document.addEventListener('DOMContentLoaded', () => {
    const board = document.querySelector('.chess-board-container');
    const capturedDiv = document.querySelector('.captured-pieces');
    let selectedPieceId = null; 

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

                    const response = await fetch('/Game/MakeMove', {
                        method: 'POST',
                        headers: { 'Content-Type': 'application/json' },
                        body: JSON.stringify({
                            pieceId: selectedPieceId.replace("piece-", ""),
                            toX: gridX,
                            toY: gridY
                        })
                    });

                    const result = await response.json();

                    if (result.success) {
                        renderBoard(result.figures, result.captured);
                    }

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
    bindSquareClickEvents();
});