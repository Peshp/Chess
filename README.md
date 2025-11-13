# Chess


A web chess application for playing and analyzing games. Built with ASP.NET Core 8, Entity Framework, SQL, Bootstrap and vanilla JS/CSS. The project is a work in progress — core gameplay and UI are implemented, with analysis and real-time play planned.

## Key ideas
- Play chess in the browser with a visual drag‑and‑drop board.
- Track move history and captured pieces.
- Configurable time controls (minutes + increment).
- Session-backed board state and server-side game logic in C#.
- Lightweight UI using Bootstrap and custom CSS/JS.

## Main features (implemented)
- Interactive chessboard rendering pieces and squares.
- Move submission and server-side validation.
- Move history panel and captured pieces panel (partial views).
- Basic clocks (minutes + increment) with client-side timer logic.
- Board and game state stored and updated via session + EF-backed models.

## Tech stack
- ASP.NET Core 8 (C#)
- Entity Framework Core
- SQL (database for games/users/history)
- Bootstrap (responsive UI)
- JavaScript/CSS for client-side interactions and clocks
- Server-side session for board state updates

## What's missing / Roadmap (high priority)
- Per-game details & analysis page (view and analyze finished games)
- Real-time play via SignalR (live two-player matches)
- Roles & authorization policies (players, admins)
- UI/UX polish and responsive design adjustments
- Persistent game playback / export (PGN or similar)
- Tests and CI workflow

## Contribution
- Open an issue for bugs, feature requests, or design proposals.
- Prefer small focused PRs that include tests or a clear manual test plan.
- Keep C# code style consistent with existing patterns; aim for small, reviewable changes.
- If working on real-time play, coordinate before large SignalR refactors.

## Notes for maintainers
- Board and clocks are currently updated with a mix of server responses and client-side JS; plan how authoritative timing/state will be handled when adding SignalR.
- Move validation and game rules live in engineService — be careful when refactoring to keep logic deterministic and testable.
- Consider separating UI partials and viewmodels for easier unit testing and reuse.

## License
This project is available under the MLT License. See LICENSE for details.

## Author
Peshp
