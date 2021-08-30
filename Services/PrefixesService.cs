using AggregatorAPI.Interfaces.Services;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;

namespace AggregatorAPI.Services
{
    public class PrefixesService : IPrefixesService
    {
        private List<string> _phonePrefixes { get; set; }

        private readonly AppSettings _config;

        private readonly IFileSystem _fileSystem;

        public PrefixesService(IOptions<AppSettings> config, IFileSystem fileSystem)
        {
            _config = config?.Value ?? throw new ArgumentNullException();
            _fileSystem = fileSystem ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Reads prefixes from a textfile,
        /// they need to be sepparated by newline
        /// </summary>
        /// <param name="fileName">optional parameter for name of the textfile</param>
        public void ReadFromTextFile(string fileName = "")
        {
            _phonePrefixes = new List<string>();

            if (string.IsNullOrEmpty(fileName))
            {
                fileName = _config.PrefixesFileName;
            }

            string path = Path.Combine(_fileSystem.Directory.GetCurrentDirectory(), $"Files{Path.DirectorySeparatorChar}Prefixes{Path.DirectorySeparatorChar}", fileName);

            if (!_fileSystem.File.Exists(path))
            {
                throw new FileNotFoundException();
            }

            using (StreamReader sr = _fileSystem.File.OpenText(path))
            {
                string s = String.Empty;
                lock (_phonePrefixes)
                {
                    while ((s = sr.ReadLine()) != null)
                    {
                        _phonePrefixes.Add(s);
                    }
                }
            }
        }

        /// <summary>
        /// Gets phone prefixes
        /// </summary>
        /// <returns>List of phone prefixes</returns>
        public List<string> GetPhonePrefixes()
        {
            return _phonePrefixes;
        }
    }
}
