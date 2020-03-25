using System;
using System.IO;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.AutoML;
using Microsoft.ML.Data;
using Newtonsoft.Json;
using ml.net.dao;

namespace ml.net
{
    class Program
    {
        static void Main(string[] args)
        {
            // ************
            // Set run duration, override with optional args
            uint trainingDurationInSeconds = 60;
            if (args != null && args.Length > 0 && uint.TryParse(args[0], out trainingDurationInSeconds))
            {
                trainingDurationInSeconds = (uint)Math.Max(trainingDurationInSeconds, 30);
                Console.WriteLine($"Overriding training time, setting for {trainingDurationInSeconds} seconds.");
            }

            // ************
            // ml.net Context Creation
            var mlContext = new MLContext();

            // ************
            // Load data from CSVs
            var trainData = mlContext.Data.LoadFromTextFile(path: "../../data/optdigits-train.csv",
                                    columns : new[] 
                                    {
                                        new TextLoader.Column(nameof(InputData.PixelValues), DataKind.Single, 0, 63),
                                        new TextLoader.Column(nameof(InputData.Number), DataKind.Single, 64)
                                    },
                                    hasHeader : false,
                                    separatorChar : ','
                                    );
            var testData = mlContext.Data.LoadFromTextFile(path: "../../data/optdigits-test.csv",
                                    columns: new[]
                                    {
                                        new TextLoader.Column(nameof(InputData.PixelValues), DataKind.Single, 0, 63),
                                        new TextLoader.Column(nameof(InputData.Number), DataKind.Single, 64)
                                    },
                                    hasHeader: false,
                                    separatorChar: ','
                                    );

            // ************
            // This is the magic sauce! This does it all!
            ExperimentResult<MulticlassClassificationMetrics> experimentResult = mlContext.Auto()
                .CreateMulticlassClassificationExperiment(trainingDurationInSeconds)
                .Execute(trainData, nameof(InputData.Number));

            // ************
            // Let's get the real performance of our hero model
            var heroModel = experimentResult.BestRun;

            var predictions = heroModel.Model.Transform(testData);
            var metrics = mlContext.MulticlassClassification.Evaluate(predictions, nameof(InputData.Number));

            // ************
            // Serialize hero model to binary (savable to disk, database, deployable to mobile app or hostable via REST service)
            byte[] binaryModel;
            using (var memoryStream = new MemoryStream())
            {
                mlContext.Model.Save(heroModel.Model, null, memoryStream);
                binaryModel = memoryStream.ToArray();
            }

            // ************
            // Let's look at stuff

            // Binary
            Console.WriteLine($"Best model binary to save for use later out of {experimentResult.RunDetails.Count()} trained models:");
            Console.WriteLine($"  Trainer Name: {heroModel.TrainerName}");
            Console.WriteLine($"  Serialized Binary: {BitConverter.ToString(binaryModel).Substring(0, 33)}...");

            // Hero model training metrics
            Console.WriteLine(" ");
            Console.WriteLine("*********************");
            Console.WriteLine("Hero Model Training Metrics:");
            Console.WriteLine(JsonConvert.SerializeObject(heroModel.ValidationMetrics, Formatting.Indented).Substring(0, 600));

            // Hero model evaluation metrics
            Console.WriteLine(" ");
            Console.WriteLine("*********************");
            Console.WriteLine("Hero Model Evaluation Metrics:");
            Console.WriteLine(JsonConvert.SerializeObject(metrics, Formatting.Indented).Substring(0, 600));

            // ************
            // Let's deserialize our binary and use it!
            var newPredictionMlContext = new MLContext();
            ITransformer predictionModel;
            using (var stream = new MemoryStream(binaryModel))
            {
                predictionModel = newPredictionMlContext.Model.Load(stream, out _);
            }

            // ************
            // This is now our super-smart engine that can read handwriting!
            var predictionEngine = newPredictionMlContext.Model.CreatePredictionEngine<InputData, OutputData>(predictionModel);

            // ************
            // Let's test it. Here we go!
            var predictionScenarios = new InputData[]{SampleMNISTData.MNIST1,SampleMNISTData.MNIST2};
            Console.WriteLine(" ");
            Console.WriteLine("*********************");
            foreach (var input in predictionScenarios)
            {
                var predictionResult = predictionEngine.Predict(input);

                Console.WriteLine($"Predicted probability:       zero:  {predictionResult.Score[0]:0.####}");
                Console.WriteLine($"                             one :  {predictionResult.Score[1]:0.####}");
                Console.WriteLine($"                             two:   {predictionResult.Score[2]:0.####}");
                Console.WriteLine($"                             three: {predictionResult.Score[3]:0.####}");
                Console.WriteLine($"                             four:  {predictionResult.Score[4]:0.####}");
                Console.WriteLine($"                             five:  {predictionResult.Score[5]:0.####}");
                Console.WriteLine($"                             six:   {predictionResult.Score[6]:0.####}");
                Console.WriteLine($"                             seven: {predictionResult.Score[7]:0.####}");
                Console.WriteLine($"                             eight: {predictionResult.Score[8]:0.####}");
                Console.WriteLine($"                             nine:  {predictionResult.Score[9]:0.####}");
                Console.WriteLine();
            }
        }
    }
}
