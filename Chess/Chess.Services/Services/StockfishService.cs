namespace Chess.Services.Services
{
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Chess.Services.Services.Contracts;

    using Microsoft.Extensions.Hosting;

    public class StockfishService : IStockfishService, IDisposable
    {
        private readonly Process _stockfishProcess;
        private readonly StreamWriter _input;
        private readonly StreamReader _output;
        private readonly object _lock = new object();

        public StockfishService(IHostEnvironment env)
        {
            string path = Path.Combine(env.ContentRootPath, "stockfish.exe");

            _stockfishProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = path,
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

            while (_output.ReadLine() != "readyok") { }
        }

        public async Task<string> GetBestMoveAsync(string fen, int skillLevel)
        {
            await _input.WriteLineAsync("stop");
            await _input.WriteLineAsync($"setoption name Skill Level value {skillLevel}");
            await _input.WriteLineAsync($"position fen {fen}");
            await _input.WriteLineAsync("go depth 10");
            await _input.FlushAsync();

            string line;
            while ((line = await _output.ReadLineAsync()) != null)
            {
                if (line.StartsWith("bestmove"))
                {
                    return line.Split(' ')[1];
                }
            }

            return string.Empty;
        }

        public void Dispose()
        {
            _input.WriteLine("quit");
            _stockfishProcess.Close();
        }
    }
}
