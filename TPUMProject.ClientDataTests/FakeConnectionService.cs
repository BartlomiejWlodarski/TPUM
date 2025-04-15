using ClientData;
using ClientData.Abstract;
using ConnectionAPI;
using System.Text.Json.Nodes;

namespace TPUMProject.ClientDataTests
{
    internal class FakeConnectionService : IConnectionService
    {
        public event Action<string>? Logger;
        public event Action? OnConnectStateChanged;
        public event Action<string>? OnMessage;
        public event Action? OnError;
        public event Action? OnDisconnect;

        public Task Connect(Uri uri)
        {
            throw new NotImplementedException();
        }

        public Task Disconnect()
        {
            throw new NotImplementedException();
        }

        public bool IsConnected()
        {
            return true;
        }

        public async Task SendAsync(string message)
        {
            if (serializer.GetCommandHeader(message) == FakeServerStatics.GetBooksCommandHeader)
            {
                FakeAllBooksUpdateResponse response = new FakeAllBooksUpdateResponse();
                response.Books = [new FakeBookDTO { Id = nextId, Title = "TestTitle", Author = "TestAuthor", Price = 20, Recommended = false, Genre = 1 }];
                nextId++;
                OnMessage?.Invoke(serializer.Serialize(response));
            }
            else if (serializer.GetCommandHeader(message) == FakeServerStatics.SellBookCommandHeader)
            {
                SellBookCommand sellItemCommand = serializer.Deserialize<SellBookCommand>(message);
                lastBoughtId = sellItemCommand.BookID;

                FakeTransactionResultResponse response = new FakeTransactionResultResponse {BookID = lastBoughtId, Username = "TestUser", ResultCode = 0 };
                OnMessage?.Invoke(serializer.Serialize(response));
            }

            await Task.Delay(0);
        }

        private Serializer serializer = Serializer.Create();
        public int lastBoughtId;
        public int nextId = 5;

        public void MockUpdateAllBooks(FakeBookDTO[] books)
        {
            FakeAllBooksUpdateResponse response = new FakeAllBooksUpdateResponse();
            response.Header = FakeServerStatics.AllBooksUpdateResponseHeader;
            response.Books = books;
            OnMessage?.Invoke(serializer.Serialize(response));
        }

        public void MockUpdateUser(FakeUserDTO user)
        {
            FakeUserChangedResponse response = new FakeUserChangedResponse();
            response.User = user;
            OnMessage?.Invoke(serializer.Serialize(response));
        }
    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "11.0.0.0 (Newtonsoft.Json v13.0.0.0)")]
    public partial class FakeBookDTO
    {
        [Newtonsoft.Json.JsonProperty("Id", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int Id { get; set; }

        [Newtonsoft.Json.JsonProperty("Title", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Title { get; set; }

        [Newtonsoft.Json.JsonProperty("Author", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Author { get; set; }

        [Newtonsoft.Json.JsonProperty("Price", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public decimal Price { get; set; }

        [Newtonsoft.Json.JsonProperty("Recommended", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool Recommended { get; set; }

        [Newtonsoft.Json.JsonProperty("Genre", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int Genre { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "11.0.0.0 (Newtonsoft.Json v13.0.0.0)")]
    public abstract partial class FakeServerResponse
    {
        [Newtonsoft.Json.JsonProperty("Header", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Header { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "11.0.0.0 (Newtonsoft.Json v13.0.0.0)")]
    public partial class FakeAllBooksUpdateResponse : FakeServerResponse
    {
        [Newtonsoft.Json.JsonProperty("Books", Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.ICollection<FakeBookDTO> Books { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "11.0.0.0 (Newtonsoft.Json v13.0.0.0)")]
    public partial class FakeBookChangedResponse : FakeServerResponse
    {
        [Newtonsoft.Json.JsonProperty("book", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public FakeBookDTO Book { get; set; }

        [Newtonsoft.Json.JsonProperty("changeType", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int ChangeType { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "11.0.0.0 (Newtonsoft.Json v13.0.0.0)")]
    public partial class FakeTransactionResultResponse : FakeServerResponse
    {
        [Newtonsoft.Json.JsonProperty("BookID", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int BookID { get; set; }

        [Newtonsoft.Json.JsonProperty("Username", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Username { get; set; }

        [Newtonsoft.Json.JsonProperty("ResultCode", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int ResultCode { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "11.0.0.0 (Newtonsoft.Json v13.0.0.0)")]
    public partial class FakeUserDTO
    {
        [Newtonsoft.Json.JsonProperty("Username", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Username { get; set; }

        [Newtonsoft.Json.JsonProperty("Balance", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public decimal Balance { get; set; }

        [Newtonsoft.Json.JsonProperty("Books", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public System.Collections.Generic.ICollection<FakeBookDTO> Books { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "11.0.0.0 (Newtonsoft.Json v13.0.0.0)")]
    public partial class FakeUserChangedResponse : FakeServerResponse
    {
        [Newtonsoft.Json.JsonProperty("User", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public FakeUserDTO User { get; set; }


    }

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "11.0.0.0 (Newtonsoft.Json v13.0.0.0)")]
    public partial class FakeNewsletterUpdateResponse : FakeServerResponse
    {
        [Newtonsoft.Json.JsonProperty("Number", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int Number { get; set; }

    }

    internal static class FakeServerStatics
    {
        public static readonly string GetBooksCommandHeader = "GetBooks";
        public static readonly string SellBookCommandHeader = "SellBook";
        public static readonly string GetUserCommandHeader = "GetUser";
        public static readonly string SubscribeToNewsletterUpdatesHeader = "SubscribeToNewsletterUpdates";

        public static readonly string AllBooksUpdateResponseHeader = "AllBooksUpdate";
        public static readonly string BookChangedResponseHeader = "BookChanged";
        public static readonly string TransactionResultResponseHeader = "TransactactionResult";
        public static readonly string UserChangedResponseHeader = "UserChanged";
        public static readonly string NewsletterSend = "NewsletterSend";
    }

}
