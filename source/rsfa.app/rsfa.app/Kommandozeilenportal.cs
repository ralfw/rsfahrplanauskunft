using System;
using System.Diagnostics;

namespace rsfa.app
{
    internal class Kommandozeilenportal
    {
        private readonly string[] _args;

        public Kommandozeilenportal(string[] args)
        {
            _args = args;
            Trace.WriteLine("Kommandozeile: " + string.Join(",", args));
        }

        public string Starthaltestellenname
        {
            get { return _args[0]; }
        }

        public string Zielhaltestellenname
        {
            get { return _args[1];  }
        }

        public DateTime Startzeit
        {
            get { return DateTime.Parse(_args[2]); }
        }

        public bool IstDebug
        {
            get
            {                 
                if (_args.Length < 4) return false;
                return _args[3].Equals("-d");
            }
        }

        public string Ausgabedatei
        {
            get
            {
                if (_args.Length < 5) return string.Empty;
                return _args[4];
            }
        }
    }
}