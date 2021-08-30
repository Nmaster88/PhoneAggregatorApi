using System.Collections.Generic;

namespace AggregatorAPI.Interfaces.Services
{
    public interface IPrefixesService
    {
        /// <summary>
        /// Reads prefixes from a textfile,
        /// they need to be sepparated by newline
        /// </summary>
        /// <param name="filename">optional parameter for name of the textfile</param>
        void ReadFromTextFile(string filename = "");

        /// <summary>
        /// Gets phone prefixes
        /// </summary>
        /// <returns>List of phone prefixes</returns>
        List<string> GetPhonePrefixes();
    }
}
