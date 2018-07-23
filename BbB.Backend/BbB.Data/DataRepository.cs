using System;
using System.Collections.Generic;
using System.Text;

namespace BbB.Data
{
    public class DataRepository
    {
        private readonly BbBContext bbBContext;

        public DataRepository(BbBContext input)
        {
            bbBContext = input ?? throw new ArgumentException(nameof(input));
        }


    }
}
