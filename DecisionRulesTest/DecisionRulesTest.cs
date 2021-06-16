using DecisionRules.Exceptions;
using NUnit.Framework;
using System.Collections.Generic;

namespace DecisionRules.Tests
{
    [TestFixture]
    class DecisionRulesTest
    {
        private static readonly string apiKey = "OzK7xjgI4NGU3MhDDax93kJysiFDDTmMzNPOyRhViMmDQAex77rj-d_aCeiyFqbD";

        private RequestOption requestOption;

        private RequestOption requestOptionForNoGeo;

        private DecisionRulesService service;

        private DecisionRulesService serviceForNoGeo;

        private static readonly string ruleId = "d34e2096-b9a5-086f-a3ce-512081241bd1";

        [SetUp]
        public void SetUp()
        {
            requestOption = new RequestOption(apiKey, "eu1");

            requestOptionForNoGeo = new RequestOption(apiKey);

            service = new DecisionRulesService(requestOption);

            serviceForNoGeo = new DecisionRulesService(requestOptionForNoGeo);
        }

        [Test]
        public void SolverTestWithStringInputFullRequest()
        {
            
            string jsonString = @"{""data"":{""day"": ""today""}}";
            List<OutputModel> response = service.Solve<List<OutputModel>>(ruleId, jsonString, "1");

            Assert.IsTrue(response[0].result == "Its wednesday my dudes ");
        }

        [Test]
        public void SolverTestWithGenericInputFullRequest()
        {
            InputModel inputData = new InputModel
            {
                data = new DataModel()
            };
            inputData.data.day = "today";

            List<OutputModel> response = service.Solve<InputModel, List<OutputModel>>(ruleId, inputData, "1");
        }

        [Test]
        public void SolverTestWithGenericInputWithoutVersion()
        {
            InputModel inputData = new InputModel
            {
                data = new DataModel()
            };
            inputData.data.day = "today";

            List<OutputModel> response = service.Solve<InputModel, List<OutputModel>>(ruleId, inputData);
        }

        [Test]
        public void SolverTestWithStringInputWithoutVersion()
        {
            string jsonString = @"{""data"":{""day"": ""today""}}";
            List<OutputModel> response = service.Solve<List<OutputModel>>(ruleId, jsonString);

            Assert.IsTrue(response[0].result == "Its wednesday my dudes ");
        }

        [Test]
        public void SolverTestWithGenericInputWithoutGeoloc()
        {
            InputModel inputData = new InputModel
            {
                data = new DataModel()
            };
            inputData.data.day = "today";

            List<OutputModel> response = serviceForNoGeo.Solve<InputModel, List<OutputModel>>(ruleId, inputData, "1");
        }

        [Test]
        public void SolverTestWithStringInputWithoutGeoloc()
        {
            string jsonString = @"{""data"":{""day"": ""today""}}";
            List<OutputModel> response = serviceForNoGeo.Solve<List<OutputModel>>(ruleId, jsonString, "1");

            Assert.IsTrue(response[0].result == "Its wednesday my dudes ");
        }

        [Test]
        public void SolverThrows401()
        {
            RequestOption unauthorizedRequestOptions = new RequestOption("WRONG!", "eu1");

            DecisionRulesService unauthorizedService = new DecisionRulesService(unauthorizedRequestOptions);

            InputModel inputData = new InputModel
            {
                data = new DataModel()
            };
            inputData.data.day = "today";

            Assert.Throws<NoUserException>(() => unauthorizedService.Solve<InputModel,List<OutputModel>>(ruleId, inputData, "1"));
        }
    }

    class InputModel
    {
        public DataModel data { get; set; }
    }

    class DataModel
    {
        public string day { get; set; }
    }

    class OutputModel
    {
        public string result { get; set; }
    }
}
