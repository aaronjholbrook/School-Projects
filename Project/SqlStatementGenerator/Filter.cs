using System;
using System.Collections.Generic;
using System.Text;

namespace SqlStatementGenerator
{
    class Filter
    {
        List<string> filterList = new List<string>();
        public List<string> CategoryAddition(string category)
        {
            filterList.Clear();
            return filterList;
        }
    }
}
