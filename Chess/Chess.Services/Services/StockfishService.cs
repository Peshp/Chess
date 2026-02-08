namespace Chess.Services.Services
{
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Channels;
    using System.Threading.Tasks;

    using Chess.Services.Services.Contracts;

    using Microsoft.Extensions.Hosting;

    public record StockfishRequest(string Fen, TaskCompletionSource<string> ResultSource);

    public class StockfishService : BackgroundService
    {
        private readonly Channel<StockfishRequest> _channel = Channel.CreateUnbounded<StockfishRequest>();

        public async Task<string> GetBestMoveAsync(string fen)
        {
            var tcs = new TaskCompletionSource<string>();
            await _channel.Writer.WriteAsync(new StockfishRequest(fen, tcs));

            // This line waits until the BackgroundService calls SetResult()
            return await tcs.Task;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var process = StartStockfish();

            await foreach (var request in _channel.Reader.ReadAllAsync(stoppingToken))
            {
                await process.StandardInput.WriteLineAsync($"position fen {request.Fen}");
                await process.StandardInput.WriteLineAsync("go movetime 1000");

                string? line;
                while ((line = await process.StandardOutput.ReadLineAsync(stoppingToken)) != null)
                {
                    if (line.StartsWith("bestmove"))
                    {
                        string move = line.Split(' ')[1];
                        // Fulfill the "claim check" - the Controller wakes up now
                        request.ResultSource.SetResult(move);
                        break;
                    }
                }
            }
        }

        private Process StartStockfish() 
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "stockfish.exe",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,

                WorkingDirectory = AppContext.BaseDirectory
            };

            Process? process = Process.Start(startInfo);

            if (process is null)
            {
                throw new InvalidOperationException("Failed to start the Stockfish.");
            }

            process.StandardInput.WriteLine("uci");

            process.StandardInput.WriteLine("setoption name Threads value 1");
            process.StandardInput.WriteLine("setoption name Hash value 32");
            process.StandardInput.WriteLine("isready");

            return process;
        }
    }
}
