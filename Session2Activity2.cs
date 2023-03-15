using RestSharp;
using System.Net;

namespace Session2part2
{
    [TestClass]
    public class Session2Activity2
    {

        private static RestClient restClient;

        private static readonly string BaseURL = "https://petstore.swagger.io/v2/";

        private static readonly string UserEndpoint = "pet";

        private static string GetURL(string enpoint) => $"{BaseURL}{enpoint}";

        private static Uri GetURI(string endpoint) => new Uri(GetURL(endpoint));

        private readonly List<UserModel> cleanUpList = new List<UserModel>();

        [TestInitialize]
        public async Task TestInit()
        {
            restClient = new RestClient();
        }

        [TestCleanup]
        public async Task TestCleanUp()
        {
           foreach (var data in cleanUpList)
            {
                var restRequest = new RestRequest(GetURI($"{UserEndpoint}/{data.Id}"));
                var restResponse = await restClient.DeleteAsync(restRequest);
            }
        }

        [TestMethod]
        public async Task PostMethod1()
        {
            #region CreateUser
            //Create User
            var newPet = new UserModel()
            {
                Id = 1034,
                Name = "Milo",
                Status = "pending",
                Tags = new Category[] 
                { 
                    new Category { Id = 1034, Name = "Milo Tag" }
                
                },
                PhotoUrls = new string[] { "http://www.Milo.com" },
                Category = new Category()
                {
                    Id = 1034,
                    Name = "Cat"
                },
            };

            // Send Post Request
            var temp = GetURI(UserEndpoint);
            var postRestRequest = new RestRequest(GetURI(UserEndpoint)).AddJsonBody(newPet);
            var postRestResponse = await restClient.ExecutePostAsync(postRestRequest);
            
            //Verify POST request status code
            Assert.AreEqual(HttpStatusCode.OK, postRestResponse.StatusCode, "Status code is not equal to 200");
            #endregion

            #region GetUser
            var restRequest = new RestRequest(GetURI($"{UserEndpoint}/{newPet.Id}"), Method.Get);
            var restResponse = await restClient.ExecuteAsync<UserModel>(restRequest);
            #endregion

            #region Assertions
            Assert.AreEqual(HttpStatusCode.OK, restResponse.StatusCode, "Status code is not equal to 200");
            Assert.AreEqual(newPet.Id, restResponse.Data.Id, "Id did not match.");
            Assert.AreEqual(newPet.Name, restResponse.Data.Name, "Name did not match.");
            Assert.AreEqual(newPet.Category.Id, restResponse.Data.Category.Id, "Category did not match.");
            CollectionAssert.AreEqual(newPet.PhotoUrls, restResponse.Data.PhotoUrls, "PhotoUrls did not match.");
            Assert.AreEqual(newPet.Status, restResponse.Data.Status, "Status did not match.");

            #endregion

           #region CleanUp
            cleanUpList.Add(newPet);
           #endregion

        }






       }
 }
 