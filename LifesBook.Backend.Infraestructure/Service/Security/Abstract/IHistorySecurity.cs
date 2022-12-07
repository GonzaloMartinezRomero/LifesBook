using LifesBook.Backend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifesBook.Backend.Infraestructure.Service.Security.Abstract
{
    public interface IHistorySecurity
    {
        public string GenerateHash(string password);

        internal string Encrypt(string password, string text);

        internal string Decrypt(string password, string text);
    }
}
