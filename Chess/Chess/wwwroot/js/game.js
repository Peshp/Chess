document.addEventListener('DOMContentLoaded', () => {
    const board = document.querySelector('.chess-board-container');

    document.querySelectorAll('.figure-img').forEach(piece => {
        piece.setAttribute('draggable', true);

        piece.addEventListener('dragstart', (event) => {
            event.dataTransfer.setData("pieceId", event.target.id);
            event.dataTransfer.setData("startX", event.target.style.left);
            event.dataTransfer.setData("startY", event.target.style.top);
        });
    });

    board.addEventListener('dragover', (event) => {
        event.preventDefault();
    });

    board.addEventListener('drop', async (event) => {
        event.preventDefault();

        const pieceId = event.dataTransfer.getData("pieceId");
        const piece = document.getElementById(pieceId);
        if (!piece) return;

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
            piece.style.left = `${gridX * 12.5}%`;
            piece.style.top = `${gridY * 12.5}%`;
        }
    });
});