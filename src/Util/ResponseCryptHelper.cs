using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modoff.Util {
    internal static class ResponseCryptHelper {
        const string key = "56BB211B-CF06-48E1-9C1D-E40B5173D759";
        public static string EncryptResponse(string inData) {
            var responseBody = inData;
            var result = $"<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<string>{TripleDES.EncryptUnicode(responseBody, key)}</string>";
            return result;
        }

        public static string DecryptResponse(string inData) {
            var fieldValue = inData;
            string result = TripleDES.DecryptUnicode(fieldValue, key);
            return result;
        }
    }
}
