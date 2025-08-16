document.addEventListener('DOMContentLoaded', () => {
    const board = document.querySelector('.chess-board-container');
    if (!board) {
        console.error("Chess board container not found.");
        return;
    }

    // Parse the board model from the page
    const boardViewModel = JSON.parse(document.getElementById('board-data').textContent);

    // Enable drag for each piece
    document.querySelectorAll('.figure-img').forEach(piece => {
        piece.setAttribute('draggable', true);

        piece.addEventListener('dragstart', (event) => {
            event.dataTransfer.setData("pieceId", event.target.id);
        });
    });

    // Allow board to accept drops
    board.addEventListener('dragover', (event) => {
        event.preventDefault();
    });

    board.addEventListener('drop', async (event) => {
        event.preventDefault();

        const pieceId = event.dataTransfer.getData("pieceId");
        if (!pieceId) {
            console.error("No pieceId found in drop event.");
            return;
        }

        const piece = document.getElementById(pieceId);
        if (!piece) {
            console.error("Piece not found for id:", pieceId);
            return;
        }

        // Get board square
        const boardRect = board.getBoundingClientRect();
        const x = event.clientX - boardRect.left;
        const y = event.clientY - boardRect.top;

        const gridX = Math.floor(x / (boardRect.width / 8));
        const gridY = Math.floor(y / (boardRect.height / 8));

        // Prepare move request object
        const moveRequest = {
            pieceId: pieceId.replace("piece-", ""),
            toX: gridX,
            toY: gridY,
            board: boardViewModel // send full model to server
        };

        try {
            const response = await fetch('/Game/MakeMove', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(moveRequest)
            });

            if (!response.ok) {
                console.error("Move request failed:", response.status);
                return;
            }

            const result = await response.json();

            if (result.success) {
                piece.style.left = `${gridX * 12.5}%`;
                piece.style.top = `${gridY * 12.5}%`;
            } 
        } catch (err) {
        }
    });
});