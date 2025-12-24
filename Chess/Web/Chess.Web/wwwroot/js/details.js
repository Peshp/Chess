document.addEventListener("DOMContentLoaded", () => {
    const squares = document.querySelectorAll("#chess-board .board-square"); // Select squares from `_GamePartial`
    const moveButtons = document.querySelectorAll(".move-button");

    squares.forEach(square => {
        square.addEventListener("click", () => {
            const toX = square.getAttribute("data-x");
            const toY = square.getAttribute("data-y");
            const pieceId = square.getAttribute("data-pieceId");

            // Update hidden inputs in the form
            document.getElementById("toX").value = toX;
            document.getElementById("toY").value = toY;
            document.getElementById("pieceId").value = pieceId;

            // Highlight the selected square
            squares.forEach(sq => sq.classList.remove("active"));
            square.classList.add("active");

            console.log(`Square clicked: toX=${toX}, toY=${toY}, pieceId=${pieceId}`);
        });
    });

    // Add event listeners to move buttons
    moveButtons.forEach(button => {
        button.addEventListener("click", () => {
            const toX = button.getAttribute("data-toX");
            const toY = button.getAttribute("data-toY");
            const pieceId = button.getAttribute("data-pieceId");

            // Update hidden inputs in the form
            document.getElementById("toX").value = toX;
            document.getElementById("toY").value = toY;
            document.getElementById("pieceId").value = pieceId;

            // Highlight the selected button
            moveButtons.forEach(btn => btn.classList.remove("active"));
            button.classList.add("active");

            console.log(`Move sent: toX=${toX}, toY=${toY}, pieceId=${pieceId}`);
        });
    });
});
