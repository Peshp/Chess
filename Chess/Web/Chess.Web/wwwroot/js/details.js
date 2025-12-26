document.addEventListener("DOMContentLoaded", function () {
    const nextBtn = document.getElementById('btnNextMove');
    const moveRows = document.querySelectorAll('.move-row');
    const progressBadge = document.getElementById('progressBadge');
    const form = document.getElementById('moveForm');

    const storageKey = 'chess_replay_step_@Model.BoardId';
    let currentIndex = parseInt(sessionStorage.getItem(storageKey) || '0');

    function refreshReplayUI() {
        moveRows.forEach((row, idx) => {
            if (idx === currentIndex) {
                row.classList.add('active-move');
                row.scrollIntoView({ behavior: 'smooth', block: 'center' });
            } else {
                row.classList.remove('active-move');
            }
        });

        progressBadge.innerText = `Step ${currentIndex} / ${moveRows.length}`;

        if (currentIndex >= moveRows.length) {
            nextBtn.disabled = true;
            nextBtn.className = "btn btn-outline-secondary btn-lg fw-bold text-uppercase w-100";
            nextBtn.innerHTML = '<i class="bi bi-check-all me-2"></i> Game Replayed';
        }
    }

    nextBtn.addEventListener('click', function () {
        if (currentIndex < moveRows.length) {
            const nextMoveData = moveRows[currentIndex];

            document.getElementById('toX').value = nextMoveData.getAttribute('data-tox');
            document.getElementById('toY').value = nextMoveData.getAttribute('data-toy');
            document.getElementById('figureId').value = nextMoveData.getAttribute('data-pieceid');

            sessionStorage.setItem(storageKey, currentIndex + 1);

            form.submit();
        }
    });

    refreshReplayUI();
});

function resetReplay() {
    sessionStorage.removeItem('chess_replay_step_@Model.BoardId');
    window.location.reload();
}