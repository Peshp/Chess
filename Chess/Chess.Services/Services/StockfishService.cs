namespace Chess.Services.Services
{
    using System;
    using System.Diagnostics;
    using System.IO;

    using Chess.Services.Services.Contracts;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;

    public class StockfishService : IStockfishService, IDisposable
    {
        private readonly Process _stockfishProcess;
        private readonly StreamWriter _input;
        private readonly StreamReader _output;

        public StockfishService(IHostEnvironment env)
        {
            string _path = Path.Combine(env.ContentRootPath, "stockfish.exe");

            _stockfishProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = _path,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            _stockfishProcess.Start();
            _input = _stockfishProcess.StandardInput;
            _output = _stockfishProcess.StandardOutput;

            _input.WriteLine("uci");
            _input.WriteLine("isready");
            while (_output.ReadLine() != "readyok");

        }

        public async Task<string> GetBestMoveAsync(string fen, int skillLevel = 1)
        {
            await _input.WriteLineAsync("stop");
            await _input.FlushAsync();

            await _input.WriteLineAsync($"setoption name Skill Level value {skillLevel}");
            await _input.WriteLineAsync($"position fen {fen}");

            await _input.WriteLineAsync("go movetime 300");

            string bestMove = string.Empty;
            while (true)
            {
                var line = await _output.ReadLineAsync();
                if (line != null && line.StartsWith("bestmove"))
                {
                    bestMove = line.Split(' ')[1];
                    break;
                }
            }

            return bestMove;
        }

        public void Dispose()
        {
            _input.WriteLine("quit");
            _stockfishProcess.Dispose();
        }
    }
}
