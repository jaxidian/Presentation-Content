using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace ml.net.dao
{
    class OutputData
    {
        [ColumnName("Score")]
        public float[] Score;
    }
}
