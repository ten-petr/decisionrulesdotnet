using System;
using System.Collections.Generic;
using DecisionRules;
using DemoApp.Model;
using DemoApp.Model.Out;

namespace DemoApp
{
    class Program
    {
        private static readonly string API_KEY = "OzK7xjgI4NGU3MhDDax93kJysiFDDTmMzNPOyRhViMmDQAex77rj-d_aCeiyFqbD";
        private static readonly string GEO_LOC = "eu1";
        static void Main(string[] args)
        {

            Console.WriteLine("Generic usage demo: (full request)");
            GenericTypeDemo();

            Console.WriteLine("JSON string usage demo: (full request)");
            JsonStringDemo();

            Console.WriteLine("Generic usage demo: (minimal request)");
            GenericWithoutGeoAndVersion();

            Console.WriteLine("JSON string usage demo: (minimal request)");
            JsonStringDemoWithoutGeoAndVersion();
        }

        private static void GenericTypeDemo() 
        {
            RequestOption requestOption = new RequestOption(API_KEY, GEO_LOC);

            DecisionRulesService service = new DecisionRulesService(requestOption);

            InputModel inputModel = new InputModel
            {
                data = new InputData
                {
                    day = "today"
                }
            };

            string ruleId = "d34e2096-b9a5-086f-a3ce-512081241bd1";

            List<ResultModel> result = service.Solve<InputModel, List<ResultModel>>(ruleId, inputModel, "1");

            Console.WriteLine(result[0].result);
        }

        private static void JsonStringDemo()
        {
            RequestOption requestOption = new RequestOption(API_KEY, GEO_LOC);

            DecisionRulesService service = new DecisionRulesService(requestOption);

            string input = @"{""data"":{""day"":""today""}}";

            string ruleId = "d34e2096-b9a5-086f-a3ce-512081241bd1";

            List<ResultModel> result = service.Solve<List<ResultModel>>(ruleId, input, "1");

            Console.WriteLine(result[0].result);
        }

        private static void GenericWithoutGeoAndVersion()
        {
            RequestOption requestOption = new RequestOption(API_KEY);

            DecisionRulesService service = new DecisionRulesService(requestOption);

            InputModel inputModel = new InputModel
            {
                data = new InputData
                {
                    day = "today"
                }
            };

            string ruleId = "d34e2096-b9a5-086f-a3ce-512081241bd1";

            List<ResultModel> result = service.Solve<InputModel, List<ResultModel>>(ruleId, inputModel);

            Console.WriteLine(result[0].result);
        }

        private static void JsonStringDemoWithoutGeoAndVersion()
        {
            RequestOption requestOption = new RequestOption(API_KEY);

            DecisionRulesService service = new DecisionRulesService(requestOption);

            string input = @"{""data"":{""day"":""today""}}";

            string ruleId = "d34e2096-b9a5-086f-a3ce-512081241bd1";

            List<ResultModel> result = service.Solve<List<ResultModel>>(ruleId, input);

            Console.WriteLine(result[0].result);
        }
    }

}
