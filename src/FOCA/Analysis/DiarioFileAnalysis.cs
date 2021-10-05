using System;
using System.Threading;

namespace FOCA.Analysis
{
    public class DiarioFileAnalysis
    {
        public string FilePath { get; private set; }

        public int Retries { get; set; }

        public Action<DiarioFileAnalysis> Callback { get; set; }

        public DiarioSDKNet.Diario.Prediction Prediction { get; set; }

        public bool Completed { get; set; }

        public string Error { get; set; }

        public string Sha256Hash { get; set; }

        public CancellationToken CancelToken { get; private set; }

        public DiarioFileAnalysis(string file, Action<DiarioFileAnalysis> callback, CancellationToken token = default(CancellationToken))
        {
            if (String.IsNullOrWhiteSpace(file))
                throw new ArgumentNullException(nameof(file));

            if (callback == null)
                throw new ArgumentNullException(nameof(callback));

            this.FilePath = file;
            this.Callback = callback;
            this.Retries = 0;
            this.CancelToken = token;
            this.Completed = false;
        }
    }
}
